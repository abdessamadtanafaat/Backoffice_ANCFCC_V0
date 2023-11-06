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
    [Route("admin/candidature")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]

    public class AdminController : ControllerBase
    {
        private readonly DbAncfccContext _DB_ANCFCC;
        private readonly ILogger<AdminController> _logger;
        private readonly IClaimsHelper _claimsHelper; 

        private static readonly SemaphoreSlim AdminLock = new SemaphoreSlim(1,1);

        public AdminController(DbAncfccContext DbAncfccContext, ILogger<AdminController> logger, IClaimsHelper claimsHelper)
        {
            _DB_ANCFCC = DbAncfccContext;
            _logger = logger;
            _claimsHelper = claimsHelper;

        }
        /// <summary>
        /// Get processed candidature for a specific concours.(filtred) 
        /// </summary>
        /// <remarks>
        /// 
        /// GET /admin/candidature/Processed/{idconcours}
        /// 
        /// The admin filter the candidature existed. 
        /// 
        /// The admin trait the candidature sent by the agents , gonna add the final decision about the candidature. 
        /// 
        /// The id concours is required.
        /// 
        /// The note and anneExperience are optionals.
        /// 
        /// Sample request: 
        /// 
        /// GET "/admin/candidature/Processed/{idconcours}"
        /// 
        /// {
        /// 
        ///    "reference": "R463284",
        ///    
        ///    "statusGlobal": 0,
        ///    
        ///    "message": "A revoir stp"
        ///    
        ///    }
        ///    
        ///    for the statusGlobal : Chose 0 or 1 or 2 : 
        ///    
        ///    0 : the candidature is sent to the agent to see it again , my be there is mistake .
        ///    
        ///    1 : the candidature is accepted by the admin . 
        ///    
        ///    2 : the candidature is refused by the admin . 
        /// 
        /// </remarks>
        ///<param name="anneeExperience"></param>
        ///<param name="idconcours"></param>
        ///<param name="note"></param>
        /// <response code="200">Returns the list of candidates sorted by profile.</response>
        /// <response code="204">No content found for the specified criteria.</response>
        /// <response code="400">Bad request if the provided parameters are invalid.</response>
        /// <response code="404">Not found if the concours ID does not exist.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpGet("Processed/{idconcours}")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        [ProducesResponseType(typeof(string), 204)] // Response type for 204 no content 
        [ProducesResponseType(typeof(string), 404)] // Response type for 404 not found
        public  ActionResult<IQueryable<List<CandidaturePersoDTO>>> GetCandidatsTriésParProfil(int? idconcours, float? note, int? anneeExperience)
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
                        .Where(c => c.Candidatures.Any(ca => ca.TreatedByAgent != null && ca.TreatedByAdmin == null && ca.StatusAgent != null && ca.Verrouille == false))
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

                return Ok(new
                {

                    message = $"Liste de candidatures correspondant aux critères :" + $"Id concours:{idconcours}" + $"Note : {note}" + $"Experience  : {anneeExperience}.",
                    candidats

                });

            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "An error.");
                return StatusCode(500, "internal server problem.");
            }
        }



        /// <summary>
        /// Approve a candidature
        /// </summary>
        /// <remarks>
        /// The admin approve a candidature by accepted or refused . 
        /// 
        /// PUT /admin/candidature/approve
        /// 
        /// The statusGlobal accept 0 , 1 , 2 : 
        /// 
        /// 0 : candidature to be reviewed by the agent. 
        /// 
        /// 1 : candidature accepted. 
        /// 
        /// 2 : canddature refused.
        /// Sample request : 
        /// 
        /// {
        /// 
        /// "reference": "R463284",
        /// 
        /// "statusGlobal": 0,
        /// 
        ///"message": "A revoir "
        ///
        /// }
        /// 
        /// the message is optionnal. if it's not provided the system provid automatically a commentaire .
        /// </remarks>
        /// <param name="candidature"></param>
        /// <returns>The updated candidature status.</returns>
        /// <response code="200">Candidature updated successfully.</response>
        /// <response code="204">No content found for the specified candidature.</response>
        /// <response code="400">Bad request if the provided data is invalid.</response>
        /// <response code="404">Not found if the candidature does not exist.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpPut("approve")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        [ProducesResponseType(typeof(string), 404)] // Response type for 404 not found
        [ProducesResponseType(typeof(string), 204)] // Response type for 204 no content 

        public async Task<ActionResult> UpdateCandidatureAdmin([FromBody] Candidature candidature)
        {
            ClaimsPrincipal user = HttpContext.User;
            var userInfo = _claimsHelper.GetUserClaim(user);

            if (await CommonQuery.IsCandidatureTreatedAdmin(_DB_ANCFCC, candidature.Reference))
            {
                return BadRequest(new { message = $"La candidature {candidature.Reference} est déjà traitée par un admin" });
            }

            try
            {

                if (!await CommonQuery.IsCandidatureTreatedAgent(_DB_ANCFCC, candidature.Reference))
                {
                    return BadRequest(new { message = $"La candidature {candidature.Reference} n'est pas encore traitée par un agent" });
                }

                var candidatureToUpdate = await _DB_ANCFCC.Candidatures.FirstOrDefaultAsync(c => c.Reference == candidature.Reference);

                if (candidatureToUpdate == null)
                    return NotFound();

                if (candidature.StatusGlobal == null)
                    return BadRequest("Your status is required.");

                candidatureToUpdate.UserId = userInfo.UserId;
                candidatureToUpdate.TreatedByAdmin = userInfo.Prenom;
                candidatureToUpdate.AdminId = userInfo.UserUniqueCode;
                candidatureToUpdate.StatusGlobal = candidature.StatusGlobal;
                candidatureToUpdate.Message = candidature.Message;
                candidatureToUpdate.DateModification = DateTime.Now;
                candidatureToUpdate.IsTraitedByAdmin = true; 

                if (candidatureToUpdate.StatusGlobal.HasValue && (candidatureToUpdate.StatusGlobal.Value < 0 || candidatureToUpdate.StatusGlobal.Value > 2))
                {
                    return BadRequest("La valeur de StatusGlobal est invalide");
                }
                if (string.IsNullOrEmpty(candidatureToUpdate.Message))
                {
                    candidatureToUpdate.Message = "Aucun commentaire de l'admin n'a été fourni.";
                }


                if (candidature.StatusGlobal == 0)
                {
                    candidatureToUpdate.AssignedTo = candidatureToUpdate.AgentId;
                    candidatureToUpdate.IsTraitedByAgent = false; 
                }

                candidatureToUpdate.Verrouille = false;
                await _DB_ANCFCC.SaveChangesAsync();


                _logger.LogInformation($"the candidature {candidature.Reference} successfully Updated by the {userInfo.Role} {userInfo.UserUniqueCode}.");

                switch (candidature.StatusGlobal.Value)
                {
                    case 0:
                        return Ok(new { message = $"Dossier {candidature.Reference} à revoir par l'agent {userInfo.UserUniqueCode}" });
                    case 1:
                        return Ok(new { message = $"Dossier {candidature.Reference} validé définitivement par l'admin {userInfo.UserUniqueCode}" });
                    case 2:
                        return Ok(new { message = $"Dossier {candidature.Reference} refusé définitivement par l'admin {userInfo.UserUniqueCode}" });
                }

                return Ok(new { message = $"Candidature traitée avec succès par l'admin {candidature.AdminId}" });

            }

            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "An error.");
                return StatusCode(500, "Internal server error");
            }

        }

        /// <summary>
        /// Get candidatures filtered by admin status.
        /// </summary>
        /// <remarks>
        /// 
        /// GET /admin/candidature/mycandidatures/{statusAdmin}
        /// 
        /// The admin see his approved candidatures : 
        /// 
        /// The status Admin is : 1 or 2. 
        /// 
        /// 1 :  for accepted candidatures
        /// 
        /// 2 : for refused candidatures 
        /// </remarks>
        /// <param name="statusAdmin"></param>
        /// <returns>The list of candidatures with the specified status.</returns>
        /// <response code="200">Candidatures retrieved successfully.</response>
        /// <response code="204">No content found for the specified status.</response>
        /// <response code="400">Bad request if the provided status is invalid.</response>
        /// <response code="404">Not found if the admin's candidatures are not found.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
        [HttpGet("mycandidatures/{statusAdmin}")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        [ProducesResponseType(typeof(string), 204)] // Response type for 204 No content
        [ProducesResponseType(typeof(string), 404)] // Response type for 404 not found

        public ActionResult<IQueryable<List<CandidaturePersoDTO>>> GetCandidaturesByStatus(int statusAdmin)
        {
            ClaimsPrincipal user = HttpContext.User;
            var userInfo = _claimsHelper.GetUserClaim(user);

            try
            {
                var candidaturesQuery = _DB_ANCFCC.Candidatures
                    .Where(c => c.AdminId == userInfo.UserUniqueCode);

                if (statusAdmin == 1)
                {
                    candidaturesQuery = candidaturesQuery.Where(c => c.StatusGlobal == 1);

                }
                else if (statusAdmin == 2)
                {
                    candidaturesQuery = candidaturesQuery.Where(c => c.StatusGlobal == 2);

                }
                else
                {
                    return BadRequest("Invalid status value. Use 2 for refused or 1 for accepted.");
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
                    AnneeExperience = c.Candidats.Candidatures.SelectMany(ca => ca.Experiences).FirstOrDefault(e => e.NombreAnnee.HasValue).NombreAnnee
                });

                if (!candidatures.Any())
                {
                    return NoContent();
                }
                string message = statusAdmin == 1
                ? $"Liste des candidatures acceptées par l'admin {userInfo.Prenom} {userInfo.Nom}"
                : $"Liste des candidatures refusées par l'admin {userInfo.Prenom} {userInfo.Nom}";

                _logger.LogInformation($"l'agent {userInfo.UserUniqueCode} a vu :{candidatures} ");

                return Ok(new { message, candidatures }
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
        /// <returns>The list of unprocessed candidatures assigned to the current admin.</returns>
        /// <response code="200">Candidatures retrieved successfully.</response>
        /// <response code="204">No content found for unprocessed candidatures.</response>
        /// <response code="400">Bad request if an unexpected error occurs.</response>
        /// <response code="500">Internal server error if an unexpected error occurs.</response>
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
                var candidatures = _DB_ANCFCC.Candidatures
                 .Where(c => c.AssignedToAdmin == userInfo.UserUniqueCode)
                 .Select(c => new CandidatureValidationDTO
                 {
                     Reference = c.Reference,
                     TreatedByAgent = c.TreatedByAgent,
                     Status = c.StatusAgent == 0 ? "non validé" : "validé",
                     Date_de_Modification = c.DateModification,
                     TreatedByAdmin = c.TreatedByAdmin,
                     Message = c.Message,
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


