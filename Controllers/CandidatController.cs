
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backoffice_ANCFCC.Models;


namespace ANCFCC_BACKOFFICE_PROJECT.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]

    public class CandidatController : Controller
    {


        private readonly DbAncfccContext _DB_ANCFCC;
        private object? candidats;

        public CandidatController(DbAncfccContext dbAncfccContext)
        {
            _DB_ANCFCC = dbAncfccContext;
        }
        [HttpGet("Search/{idConcours}")]
        public ActionResult<IEnumerable<Candidat>> SearchCandidats(int? idConcours, float? note, int? anneeExperience)
        {
            try
            {
                if (note.HasValue && (note < 0 || note > 20))
                {
                    throw new ArgumentException("The Note must be between 0 and 20.");
                }
                if (anneeExperience.HasValue && anneeExperience < 0 || anneeExperience > 28) //45(Max pour travailler) -18(min pour travailler) 
                {
                    throw new ArgumentException("Sorry, I think you are wrong!.");
                }

                var query = _DB_ANCFCC.Candidats.AsQueryable();

                if (idConcours.HasValue)
                {
                    // Add filtering by idConcours if needed
                }

                if (note.HasValue)
                {
                    query = query.Where(c => c.Candidatures.Any(ca => ca.Diplomes.Any(d => d.Note == note)));
                }

                if (anneeExperience.HasValue)
                {
                    query = query.Where(c => c.Candidatures.Any(ca => ca.Experiences.Any(e => e.NombreAnnee == anneeExperience)));
                }

                // Select the properties to include in the response
                var candidats = query.Select(c => new
                {
                    c.Candidatures.FirstOrDefault().Reference,
                    c.Nom,
                    c.Prenom,
                    c.Email,
                    c.Telephone1,
                });

                return Ok(candidats);
            }
            catch (ArgumentException ex)
            {
                // Handle the argument exception 
                return BadRequest(ex.Message);
            }


        }
        [HttpGet("SearchNoteInferieureA13")]
        public async Task<IActionResult> GetDiplomesByNote()
        {
            var diplomeDtos = await _DB_ANCFCC.Diplomes
                .Include(d => d.Candidature)
                .Where(d => d.Note < 18) // filter by Note inferior to 13
                .Select(d => new
                {


                    CandidatureReference = d.Candidature.Reference, // include the Candidature reference in the response
                    d.Candidature.Candidats.Prenom,
                    d.Candidature.Candidats.PrenomArabe,
                    d.Candidature.Candidats.NomArabe,
                    d.Candidature.Candidats.Telephone1,
                    d.Candidature.Candidats.Email,

                    // d.Etablissement,
                    // d.OptionDiplome,
                    //d.Anne,
                    //d.Note,
                    // d.EstSimilaire,
                    // d.AuMaroc,

                })
                .ToListAsync();

            return Ok(diplomeDtos);
        }

       /* [HttpGet("Search/noteInferieureA13")]
        public async Task<IActionResult> GetCandidaturesByDiplomeNote()
        {
            var candidatures = await _DB_ANCFCC.Candidatures
                .Include(c => c.Diplomes)
                .Include(c => c.Candidats)
                .Where(c => c.Diplomes.Any(d => d.Note < 13))
                .ToListAsync();

            var candidatureDtos = candidatures.Select(c => new
            {
                //Reference = c.Reference,
                Nom = c.Candidats.Nom,
                Prenom = c.Candidats.Prenom,
                Email = c.Candidats.Email,
                Telephone1 = c.Candidats.Telephone1,
                Diplomes = c.Diplomes.Select(d => new
                {
                    Note = d.Note
                })
            });

            return Ok(candidatureDtos);
        }
*/

        [HttpDelete("Id")]

        public ActionResult<IEnumerable<Candidat>> DeleteCandidat(int? Id)
        {

            var Candidat = _DB_ANCFCC.Candidats.Find(Id);
            if (Candidat == null)
            {
                return NotFound();
            }


            try
            {

                _DB_ANCFCC.Candidats.Remove(Candidat);
                _DB_ANCFCC.SaveChanges();
                return Ok(new { message = "This candidat has been deleted successfully." });
            }
            catch (DbUpdateException ex)
            {
                // Log the inner exception details

                Console.WriteLine($"Stack trace: {ex.InnerException.StackTrace}");
                return StatusCode(500, "An error occurred while deleting the candidat. Please try again later.");
            }
        }

        [HttpGet("VoirDetailsPerso/{id}")] // apres avoir clique sur  voir le detailpesonnelle 
        public async Task<IActionResult> DetailsPersoCandidature(int id)
        {
            var candidature = await _DB_ANCFCC.Candidatures
                .Include(c => c.Candidats)
                .FirstOrDefaultAsync(c => c.CandidatsId == id);

            if (candidature == null)
            {
                return NotFound();
            }

            var informationsPersonnelles = new
            {
                Nom = candidature.Candidats.Nom,
                Prenom = candidature.Candidats.Prenom,
                Cin = candidature.Candidats.Cin,
                DateNaissance = candidature.Candidats.DateNaissance,
                Email = candidature.Candidats.Email,
                Telephone1 = candidature.Candidats.Telephone1,
                Telephone2 = candidature.Candidats.Telephone2,
                Adresse = candidature.Candidats.Adresse,
                AdresseArabe = candidature.Candidats.AdresseArabe,
                CodePostal = candidature.Candidats.CodePostal,
                Genre = candidature.Candidats.Genre,
                SituationMatrimoniale = candidature.Candidats.Matrimoniale,
                EstHandicape = candidature.Candidats.EstHandicape,
                EstFonctionnaire = candidature.Candidats.EstFonctionnaire,
                AvoirQualiteResistant = candidature.Candidats.AvoirQualiteResistant,
                AvoirQualiteMilitaire = candidature.Candidats.AvoirQualiteMilitaire,
                AvoirQualitePupille = candidature.Candidats.AvoirQualitePupille,
                AvoirQualiteComBattant = candidature.Candidats.AvoirQualiteComBattant
            };

            return Ok(informationsPersonnelles);
        }
    }
}



