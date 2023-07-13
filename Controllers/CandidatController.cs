
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

                }

                if (note.HasValue)
                {
                    query = query.Where(c => c.Candidatures.Any(ca => ca.Diplomes.Any(d => d.Note == note)));
                }

                if (anneeExperience.HasValue)
                {
                    query = query.Where(c => c.Candidatures.Any(ca => ca.Experiences.Any(e => e.NombreAnnee == anneeExperience)));
                }


                var candidats = query.ToList();
                return Ok(candidats);
            }

            catch (ArgumentException ex)
            {
                // Handle the argument exception 
                return BadRequest(ex.Message);
            }

        }


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
    }
}



