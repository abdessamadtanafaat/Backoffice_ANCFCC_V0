using Backoffice_ANCFCC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backoffice_ANCFCC.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly DbAncfccContext _DB_ANCFCC;

        public ExperienceController(DbAncfccContext DbAncfccContext)
        {
            _DB_ANCFCC = DbAncfccContext;
        }



        [HttpGet("experiences/{reference}")]
        public async Task<IActionResult> GetDiplomesByReference(string reference)
        {
            var candidature = await _DB_ANCFCC.Diplomes
                .Include(c => c.Diplomes)
                .FirstOrDefaultAsync(c => c.Reference == reference);

            if (candidature == null)
            {
                return NotFound();
            }

            var experienceDtos = ((IEnumerable<Diplome>)candidature.Diplomes).Select(e => new
            {
                e.Id,
                e.CandidatureId,
                e.Nom,
                e.Etablissement,
                e.OptionDiplome,
                e.Anne,
                e.Note,
                e.EstSimilaire,
                e.AuMaroc,
                e.AutreDiplome
            });

            return Ok(experienceDtos);
        }
    }
    }

