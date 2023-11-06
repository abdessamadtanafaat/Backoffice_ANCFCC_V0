using Backoffice_ANCFCC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backoffice_ANCFCC.Controllers
{
    public class CommonQuery:ControllerBase
    {
        public static IQueryable<Candidat> BuildCommonQuery(DbAncfccContext dbAncfcc, int? idconcours, float? note, int? anneeExperience)
        {
            var query = dbAncfcc.Candidats.AsQueryable();

            if (idconcours.HasValue)
            {
                if (!dbAncfcc.Concours.Any(c => c.Id == idconcours))
                {
                    return null; // Handle this condition in the calling method.
                }

                query = query.Where(c => c.Candidatures.Any(ca => ca.ConcoursId == idconcours));

                if (note.HasValue)
                {
                    query = query.Where(c => c.Candidatures.Any(ca => ca.Diplomes.Any(d => d.Note == note)));
                }

                if (anneeExperience.HasValue)
                {
                    query = query.Where(c => c.Candidatures.Any(ca => ca.Experiences.Any(e => e.NombreAnnee == anneeExperience)));
                }
            }

            return query;
        }

        public  ActionResult ValidateParameters(float? note, int? anneeExperience)
        {
            if (note.HasValue && (note < 0 || note > 20))
            {
                
                return BadRequest("La note doit être entre 0 et 20.");
            }

            if (anneeExperience.HasValue && (anneeExperience < 0 || anneeExperience > 28))
            {
                return BadRequest("Valeur invalide pour l'expérience.");
            }
            return null;
        }

        public static async Task<bool> IsCandidatureTreatedAgent(DbAncfccContext dbAncfcc, string reference)
        {
            var candidature = await dbAncfcc.Candidatures.FirstOrDefaultAsync(c => c.Reference == reference);
            return (candidature.IsTraitedByAgent == true);
        }

        public static async Task<bool> IsCandidatureTreatedAdmin(DbAncfccContext dbAncfcc, string reference)
        {
            var candidature = await dbAncfcc.Candidatures.FirstOrDefaultAsync(c => c.Reference == reference);
            return (candidature.IsTraitedByAdmin == true);
        }

        public static bool CandidatureAvailable(DbAncfccContext dbAncfcc, string? Reference) =>
        dbAncfcc.Candidatures.Any(x => x.Reference == Reference);

        public static void LockCandidature(DbAncfccContext dbAncfcc, string reference)
        {
            var candidatureToUpdate = dbAncfcc.Candidatures.FirstOrDefault(c => c.Reference == reference);
            if (candidatureToUpdate != null)
            {
                candidatureToUpdate.Verrouille = true;
                dbAncfcc.SaveChanges();
            }
        }
        public static bool IsCandidatureLocked(DbAncfccContext dbAncfcc, string reference)
        {
            var candidatureToUpdate = dbAncfcc.Candidatures.FirstOrDefault(c => c.Reference == reference && c.Verrouille);
            return candidatureToUpdate != null;
        }

    }
}
