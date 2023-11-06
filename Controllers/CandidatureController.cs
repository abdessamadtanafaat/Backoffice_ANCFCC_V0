using Backoffice_ANCFCC.Controllers;
using Backoffice_ANCFCC.Models;
using Backoffice_ANCFCC.Services.ClaimsHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ANCFCC_BACKOFFICE_PROJECT.Controllers
{
    [ApiController]
    [Route("candidatures")]
    [ApiVersion("1.0")]

    public class CandidatureController : Controller
    {
        private readonly DbAncfccContext _DB_ANCFCC;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IClaimsHelper _claimsHelper;
        public CandidatureController(DbAncfccContext dbAncfccContext, ILogger<AuthenticationController> logger, IClaimsHelper claimsHelper)
        {
            _DB_ANCFCC = dbAncfccContext;
            _logger = logger;
            _claimsHelper = claimsHelper;

        }


        /// <summary>
        /// Get the details of a candidature by its reference.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves the details of a candidature identified by its reference. 
        /// It checks if the candidature is locked by another agent and handles various statuses.
        /// </remarks>
        /// <param name="reference">The reference of the candidature to retrieve.</param>
        /// <returns>
        ///   <response code="200">Returns the details of the candidature.</response>
        ///   <response code="400">If the candidature is locked by another agent.</response>
        ///   <response code="404">If the candidature is not found.</response>
        ///   <response code="500">If an error occurs while processing the request.</response>
        /// </returns>
        [HttpGet("details/{reference}")]
        [Authorize(Roles = "Admin,Agent")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        [ProducesResponseType(typeof(string), 404)] // Response type for 404 Not found

        public async Task<IActionResult> GetCandidatureDetails(string reference)
        {
            try
            {
                ClaimsPrincipal user = HttpContext.User;
                var userInfo = _claimsHelper.GetUserClaim(user);

                if (CommonQuery.IsCandidatureLocked(_DB_ANCFCC, reference))
                {
                    return BadRequest(new { message = "Candidature is locked by another agent." });
                }

                if (!CommonQuery.CandidatureAvailable(_DB_ANCFCC, reference))
                {
                    return NotFound();
                }

                CommonQuery.LockCandidature(_DB_ANCFCC, reference);

                var candidature = await _DB_ANCFCC.Candidatures
                .Include(c => c.Candidats)
                .Include(c => c.Documents)
                .Include(c => c.Experiences)
                .Include(c => c.Diplomes)
                .Select(c => new
                {
                    Reference = c.Reference,
                    c.StatutInfoPerso,
                    c.CommentaireInfoPerso,
                    c.StatutDoc,
                    c.CommentaireDoc,
                    c.StatutExp,
                    c.CommentaireExp,
                    c.StatutFormation,
                    c.CommentaireFormation,
                    c.StatusAgent,
                    Candidats = new
                    {
                        c.Candidats.Adresse,
                        c.Candidats.AdresseArabe,
                        c.Candidats.AvoirQualiteResistant,
                        c.Candidats.Cin,
                        c.Candidats.CodePostal,
                        c.Candidats.DateNaissance,
                        c.Candidats.Email,
                        c.Candidats.EstHandicape,
                        c.Candidats.Genre,
                        c.Candidats.Matrimoniale,
                        c.Candidats.Nom,
                        c.Candidats.NomArabe,
                        c.Candidats.Prenom,
                        c.Candidats.PrenomArabe,
                        c.Candidats.Telephone1,
                        c.Candidats.Telephone2,
                        c.Candidats.Ville,
                        c.Candidats.EstFonctionnaire,
                        c.Candidats.AvoirQualiteMilitaire,
                        c.Candidats.AvoirQualitePupille,
                        c.Candidats.AvoirQualiteComBattant
                    },
                    Documents = c.Documents.Select(d => new { d.Chemin, d.Nom }),
                    Experiences = c.Experiences.Select(e => new { e.DomaineActivite, e.NombreAnnee, e.Poste }),
                    Diplomes = c.Diplomes.Select(d => new
                    {
                        d.Anne,
                        d.AuMaroc,
                        d.AutreDiplome,
                        d.CandidatureId,
                        d.EstSimilaire,
                        d.Etablissement,
                        d.Nom,
                        d.Note,
                        d.OptionDiplome
                    })
                })
    .FirstOrDefaultAsync(c => c.Reference == reference);

                if (candidature == null)
                {
                    return NotFound();
                }

                _logger.LogInformation($"the {userInfo.Role} {userInfo.UserUniqueCode} successfully see the {candidature}.");
                return Ok(candidature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving statistics.");
                return StatusCode(500, "An error occurred.");
            }


        }



        /// <summary>
        ///  Get statistics on candidatures based on their status.
        /// </summary>
        /// <remarks>
        /// 
        /// GET /candidatures/statistiques/{status}
        /// 
        /// Use 0 : rejected candidatures.
        /// 
        /// Use 1 : accepted candidatures.
        /// 
        /// </remarks>
        /// 
        /// <param name="status">The status of applications to filter by (0 for rejected, 1 for accepted).</param>
        /// <returns>A response containing statistics for the specified application status.
        ///   <response code="200">Returns statistics for the specified application status.</response>
        ///   <response code="400">If the provided status value is invalid.</response>
        ///   <response code="404">If no statistics are available.</response>
        ///   <response code="500">If an error occurs while retrieving statistics.</response>
        /// </returns>

        [HttpGet("statistiques/{status}")]
        [Authorize(Roles = "Admin,Agent")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        [ProducesResponseType(typeof(string), 404)] // Response type for 404 Not found


        public IActionResult GetStatistiques(int? status)
        {
            ClaimsPrincipal user = HttpContext.User;
            var userInfo = _claimsHelper.GetUserClaim(user);

            try
            {
                if (status != null && (status < 0 || status > 1))
                {
                    return BadRequest("\"Invalid status value. Use 0 for rejected and 1 for accepted");
                }

                var statistiques = _DB_ANCFCC.Concours
                    .Select(c => new
                    {
                        ConcoursId = c.Id,
                        NomConcours = c.Nom,
                        NombreCandidatures = c.Candidatures
                            .Count(ca => status.HasValue
                                ? (status == 1 && ca.StatusGlobal == 1 && ca.StatusAgent != null) // Candidature acceptée Definitivement
                                : (status == 0 && ca.StatusGlobal == 2 && ca.StatusAgent != null)) // Candidature refussée définitivement
                    }).ToList();

                if (statistiques.Count == 0)
                {
                    return NotFound("No statistics available.");
                }

                string liste = status == 1 ? "Accepted applications" : "Rejected applications";

                _logger.LogInformation($"the {userInfo.Role} {userInfo.UserUniqueCode} see successfully the statistiques.");
                return Ok(new { message = $"Statistics for the list of { liste}, statistiques"});
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving statistics.");
                return StatusCode(500, "An error occurred.");
            }
        }

        /// <summary>
        ///  Get statistics on the numbers of candidatures.
        /// </summary>
        /// <remarks>
        /// 
        /// GET /candidatures/statistiques/nombres
        /// 
        /// The response include the name of the profile (concours) and the nombre of the validated , rejected, untreted candidtures.
        /// 
        /// </remarks>
        /// 
        /// <returns>
        ///   <response code="200">Returns statistics for concours profiles.</response>
        ///   <response code="500">If an error occurs while retrieving statistics.</response>
        /// </returns>
        /// 
        [HttpGet("statistiques/nombres")]
        [Authorize(Roles = "Admin,Agent")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error

        public IActionResult GetStatisticsByProfile()
        {
            ClaimsPrincipal user = HttpContext.User;
            var userInfo = _claimsHelper.GetUserClaim(user);

            try
            {
                var profileStats = _DB_ANCFCC.Concours
                    .Select(c => new
                    {
                        Profile = c.Nom,
                        Validated_Candidatures = c.Candidatures.Count(ca => ca.StatusAgent != null && ca.StatusGlobal == 1),
                        Rejected_Candidatures = c.Candidatures.Count(ca => ca.StatusAgent == null && ca.StatusGlobal == 2),
                        Untreated_Candidatures = c.Candidatures.Count(ca => ca.StatusAgent == null && ca.StatusGlobal == null)
                    })
                    .ToList();

                _logger.LogInformation($" the {userInfo.Role} {userInfo.UserUniqueCode} see the nombre of the candidatures.");
                return Ok(profileStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error.");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }


}




