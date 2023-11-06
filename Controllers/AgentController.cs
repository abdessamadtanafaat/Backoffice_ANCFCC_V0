using Backoffice_ANCFCC.Controllers;
using Backoffice_ANCFCC.Models;
using Backoffice_ANCFCC.Services.ClaimsHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ANCFCC_BACKOFFICE.Controllers
{
    [ApiController]
    [Route("agent/candidature")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Agent")]
    public class AgentController : ControllerBase
    {
        private readonly DbAncfccContext _DB_ANCFCC;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IClaimsHelper _claimsHelper;

        public AgentController(DbAncfccContext DbAncfccContext, ILogger<AuthenticationController> logger, IClaimsHelper claimsHelper)
        {
            _DB_ANCFCC = DbAncfccContext;
            _logger = logger;
            _claimsHelper = claimsHelper;
        }


        /// <summary>
        /// Get unprocessed candidatures for a specific concours. (filtred)
        /// </summary>
        /// <remarks>
        /// 
        /// GET /agent/candidature/unprocessed/{idconcours}
        /// 
        /// The agent filter the list of the candidatures to trait a candidature.
        /// 
        /// The id concours is required . 
        /// 
        /// The note and anneExperience are optional.
        /// 
        /// </remarks>>
        /// <param name="idconcours"></param>
        /// <param name="note"></param>
        /// <param name="anneeExperience"></param>
        /// <returns>The list of unprocessed candidatures matching the specified criteria.</returns>
        /// <response code="200">Candidatures retrieved successfully.</response>
        /// <response code="204">No content found for unprocessed candidatures.</response>
        /// <response code="400">Bad request if an unexpected error occurs or invalid parameters are provided.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>

        [HttpGet("unprocessed/{idconcours}")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        [ProducesResponseType(typeof(string), 204)] // Response type for 204 No content
        [ProducesResponseType(typeof(string), 404)] // Response type for 404 not found
        public ActionResult<List<CandidaturePersoDTO>> GetCandidatureNonTraitee(int? idconcours, float? note, int? anneeExperience)

        {
            ClaimsPrincipal user = HttpContext.User;
            var userInfo = _claimsHelper.GetUserClaim(user);

            try
            {
                CommonQuery validateParameters = new CommonQuery();

                var validationError = validateParameters.ValidateParameters(note, anneeExperience);

                if (validationError != null)
                {
                    return validationError;
                }

                var query = CommonQuery.BuildCommonQuery(_DB_ANCFCC, idconcours, note, anneeExperience);

                if (query == null)
                {
                    return BadRequest("l'ID de concours est obligatoire ou n'existe pas.");
                }

                var candidats = query
                        .Where(c => c.Candidatures.Any(ca => ca.StatusAgent == null && ca.UserId == null && ca.StatusGlobal == null && ca.Verrouille == false))
                        .Select(c => new CandidaturePersoDTO
                        {
                            Reference = c.Candidatures.FirstOrDefault().Reference,
                            Nom = c.Nom,
                            Prenom = c.Prenom,
                            CIN = c.Cin,
                            Telephone = c.Telephone1,
                            Diplome = c.Candidatures.SelectMany(ca => ca.Diplomes).FirstOrDefault(d => d.Note.HasValue).Nom,
                            Note = c.Candidatures.SelectMany(ca => ca.Diplomes).FirstOrDefault(d => d.Note.HasValue).Note,
                            AnneeExperience = c.Candidatures.SelectMany(ca => ca.Experiences).FirstOrDefault(e => e.NombreAnnee.HasValue).NombreAnnee
                        });

                    if (!candidats.Any())
                    {
                        return NoContent();
                    }

                    _logger.LogInformation($"liste de candidature est ouvrit par {userInfo.UserUniqueCode}.");

                    // Returning JSON DATA
                    return Ok(new {

                        message = $"Liste de candidatures correspondant aux critères {idconcours} Note : {note} Experience : {anneeExperience}.",
                        candidats

                    });

                
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "An error from the external server.");
                 return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Validate a candidature
        /// </summary>
        /// <remarks>
        /// 
        /// PUT /agent/candidature/validate
        /// 
        /// The agent chose a candidature : 
        /// 
        /// Sample request : 
        /// 
        /// {
        /// 
        /// "reference": "R463284",
        /// 
        /// "statutDoc": 0,
        /// 
        /// "statutExp": 0,  
        /// 
        /// "statutInfoPerso": 0, 
        /// 
        /// "statutFormation": 0,
        /// 
        /// "commentaireInfoPerso": "string",
        /// 
        /// "commentaireFormation": "string",
        /// 
        /// "commentaireDoc": "string",
        /// 
        /// "commentaireExp": "string",
        /// 
        /// "statusAgent": 1
        /// 
        /// }
        /// 
        /// NOTE : THE COMMENTS ARE OPTIONAL. if it's not provided the systme generate an auto comments . 
        /// </remarks>
        /// <param name="candidature"></param>
        /// <response code="200">OK if the candidature is updated successfully.</response>
        /// <response code="204">No content found</response>
        /// <response code="400">Bad Request if the candidature is already processed, required fields are missing, or an invalid status is provided.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        /// <response code="404">Not Found if the candidature does not exist.</response>
        /// <exception cref="Exception"></exception>
        [HttpPut("validate")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        [ProducesResponseType(typeof(string), 204)] // Response type for 204 No content
        [ProducesResponseType(typeof(string), 404)] // Response type for 404 not found
        public async Task<ActionResult> UpdateCandidatureInfos([FromBody] Candidature candidature)
        {

            // Access claims-related data using the _claimsHelper service

            ClaimsPrincipal user = HttpContext.User;

            var userInfo = _claimsHelper.GetUserClaim(user);

            if (await CommonQuery.IsCandidatureTreatedAgent(_DB_ANCFCC, candidature.Reference))
            {
                return BadRequest(new { message = "La candidature est déjà traitée" });
            }
            else

            {
                try
                {
                    var candidatureToUpdate = await _DB_ANCFCC.Candidatures.FirstOrDefaultAsync(c => c.Reference == candidature.Reference);

                    if (candidatureToUpdate == null)
                        return NotFound();

                    if (candidature.StatusAgent == null)
                        return BadRequest("Your status is required.");

                    candidatureToUpdate.StatutDoc = candidature.StatutDoc;
                    candidatureToUpdate.CommentaireDoc = candidature.CommentaireDoc;
                    candidatureToUpdate.StatutExp = candidature.StatutExp;
                    candidatureToUpdate.CommentaireExp = candidature.CommentaireExp;
                    candidatureToUpdate.StatutInfoPerso = candidature.StatutInfoPerso;
                    candidatureToUpdate.CommentaireInfoPerso = candidature.CommentaireInfoPerso;
                    candidatureToUpdate.CommentaireFormation = candidature.CommentaireFormation;
                    candidatureToUpdate.StatutFormation = candidature.StatutFormation;
                    candidatureToUpdate.StatusAgent = candidature.StatusAgent;
                    candidatureToUpdate.DateModification = DateTime.Now;
                    candidatureToUpdate.TreatedByAgent = userInfo.Prenom;
                    candidatureToUpdate.AgentId = userInfo.UserUniqueCode;
                    candidatureToUpdate.UserId = userInfo.UserId;
                    candidatureToUpdate.IsTraitedByAgent = true; 
                    // in case the candidature is retourned by the admin , and the agent want to updated once more
                    //candidatureToUpdate.AdminId = null;
                    //candidatureToUpdate.TreatedByAdmin = null;
                    //candidatureToUpdate.StatusGlobal = null;
                    //candidatureToUpdate.Message = null;
                    //candidatureToUpdate.AssignedTo = null;
                    if (candidatureToUpdate.AssignedTo != null)
                    { 
                    candidatureToUpdate.AssignedToAdmin = candidatureToUpdate.AdminId;
                    candidatureToUpdate.IsTraitedByAdmin = false; 
                    
                    }

                    string[] commentaireFields = new string[] { "CommentaireDoc", "CommentaireExp", "CommentaireInfoPerso", "CommentaireFormation" };

                    foreach (var field in commentaireFields)
                    {
                        var commentaireValue = (string)candidatureToUpdate.GetType().GetProperty(field).GetValue(candidatureToUpdate);
                        if (string.IsNullOrEmpty(commentaireValue))
                        {

                            candidatureToUpdate.GetType().GetProperty(field).SetValue(candidatureToUpdate, "Aucun commentaire de l'agent n'a été fourni.");
                        }

                    }

                    if (candidatureToUpdate.StatusAgent.HasValue && (candidatureToUpdate.StatusAgent.Value < 0 || candidatureToUpdate.StatusAgent.Value > 1))
                        throw new Exception("La valeur de Status Agent est invalide, choisir 1:valide ou 0: non valide");

                    candidatureToUpdate.Verrouille = false; 
                    await _DB_ANCFCC.SaveChangesAsync();

                    _logger.LogInformation($"the candidature {candidature.Reference} successfully Updated by {userInfo.UserUniqueCode}.");

                    // Returning JSON data as an anonymous object
                    return Ok(new
                    {
                        message = $"Candidature de référence {candidature.Reference} traitée avec succès par l'agent {userInfo.Nom} {userInfo.Prenom} ({userInfo.UserUniqueCode}).",
                        status = candidature.StatusAgent.Value == 0 ? "non validé" : "validé",
                    });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest(new { message = "Concurrency Exception Occurred. optimistic concurrency exceptions." });                    
                }
            }
        }

        /// <summary>
        ///  Get candidatures filtred by agent status.
        /// </summary>
        /// <remarks>
        /// 
        /// GET /agent/candidature/mycandidatures/{statusAgent}
        /// 
        /// Use 0 : refused candidatures. 
        /// 
        /// Use 1 : accepted candidatures. 
        /// 
        /// 
        /// </remarks>>
        /// 
        /// <param name="statusAgent"></param>
        /// <response code="200">OK if the candidatures are retrieved successfully.</response>
        /// <response code="204">No Content if no candidatures match the specified criteria.</response>
        /// <response code="400">Bad Request if an invalid status value is provided</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        /// <response code="404">Not Found if the user's candidatures are not found.</response>
        [HttpGet("mycandidatures/{statusAgent}")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        [ProducesResponseType(typeof(string), 204)] // Response type for 204 No content
        [ProducesResponseType(typeof(string), 404)] // Response type for 404 not found
        public ActionResult<IQueryable<List<CandidaturePersoDTO>>> GetMyCandidaturesAcceptee(int statusAgent)
        {
            ClaimsPrincipal user = HttpContext.User;
            var userInfo = _claimsHelper.GetUserClaim(user);

            try
            {
                var candidaturesQuery = _DB_ANCFCC.Candidatures
                    .Where(c => c.AgentId == userInfo.UserUniqueCode);

                if (statusAgent == 0)
                {
                    candidaturesQuery = candidaturesQuery.Where(c => c.StatusAgent == 0);
                }
                else if (statusAgent == 1)
                {
                    candidaturesQuery = candidaturesQuery.Where(c => c.StatusAgent == 1);
                }
                else
                {
                    return BadRequest("Invalid status value. Use 0 for refused or 1 for accepted.");
                }

                    var candidatures = candidaturesQuery
                    .Select(c => new CandidaturePersoDTO
                    {
                        Reference = c.Reference,
                        Nom = c.Candidats.Nom,
                        Prenom = c.Candidats.Prenom,
                        CIN = c.Candidats.Cin,
                        Telephone = c.Candidats.Telephone1,
                        Diplome = c.Candidats.Candidatures.SelectMany(ca => ca.Diplomes).FirstOrDefault(d => d.Note.HasValue).Nom,
                        Note = c.Candidats.Candidatures.SelectMany(ca => ca.Diplomes).FirstOrDefault(d => d.Note.HasValue).Note,
                        AnneeExperience = c.Candidats.Candidatures.SelectMany(ca=>ca.Experiences).FirstOrDefault(e=>e.NombreAnnee.HasValue).NombreAnnee
                    });

                if (!candidatures.Any())
                {
                    return NoContent();
                }
                //message = "Liste des candidatures avec le statut : 1 (accepté par l'agent)";

                string message = statusAgent == 1
                            ? $"Liste des candidatures acceptées  par l'agent {userInfo.Prenom} {userInfo.Nom}:"
                            : $"Liste des candidatures refusées par l'agent {userInfo.Prenom} {userInfo.Nom}:";

                _logger.LogInformation($"l'agent {userInfo.UserUniqueCode} a vu :{candidatures} ");

                return Ok (new {message,candidatures }
                );
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        ///  Get unprocessed candidatures for review by an agent . 
        /// </summary>
        /// <remarks>
        /// 
        /// GET /agent/candidature/review-unprocessed
        /// 
        /// The agent review the candidatures sent by the admin , maybe you have do an error in the prcess of traiting this candidature or something similar.
        /// 
        /// </remarks>
        /// 
        /// <returns></returns>
        /// <response code="200">OK if the unprocessed candidatures are retrieved successfully..</response>
        /// <response code="204">No Content if no unprocessed candidatures are found.</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        /// <response code="404">Not Found if the user's unprocessed candidatures are not found.</response>
        [HttpGet("review-unprocessed")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        [ProducesResponseType(typeof(string), 204)] // Response type for 204 No content
        [ProducesResponseType(typeof(string), 404)] // Response type for 404 not found
        public async Task<ActionResult<List<CandidatureValidationDTO>>> GetMyCandidatures()
        {

            ClaimsPrincipal user = HttpContext.User;
            var userInfo = _claimsHelper.GetUserClaim(user);

            try
            {
                var candidatures =  _DB_ANCFCC.Candidatures
                 .Where(c => c.AssignedTo == userInfo.UserUniqueCode && c.AssignedToAdmin ==null)
                 .Select(c => new CandidatureValidationDTO
                 {
            Reference = c.Reference,
            TreatedByAgent = c.TreatedByAgent,
            TreatedByAdmin = c.TreatedByAdmin,
            Status = c.StatusAgent == 0 ? "non validé" : "validé",
            Message = c.Message,
            Date_de_Modification = c.DateModification
                 })
                 .ToList();


            return candidatures;
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "An error occurred.");
               return StatusCode(500, "Internal server error");
             }
}

    }
}





