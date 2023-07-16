using Microsoft.AspNetCore.Mvc;
using Backoffice_ANCFCC.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ANCFCC_BACKOFFICE.Controllers
{
    [ApiController]
    [Route("/api")]
    public class CandidatureController : ControllerBase
    {
        private readonly DbAncfccContext _DB_ANCFCC;

        public CandidatureController(DbAncfccContext DbAncfccContext)
        {
            _DB_ANCFCC = DbAncfccContext;
        }


 
        [HttpGet("/VoirInfoPerso")]
        public async Task<IActionResult> ListeCandidatures() //page 1 qui s'affiche directement dans la plateforme 
        {
            var candidatures = await _DB_ANCFCC.Candidatures
                .Include(c => c.Candidats)
                .Select(c => new {
                    Nom = c.Candidats.Nom,
                    Prenom = c.Candidats.Prenom,
                    Reference = c.Reference,
                    Cin = c.Candidats.Cin,
                    DatePostulation = c.DatePostulation
                })
                .ToListAsync();

            return Ok(candidatures);
        }
        [HttpPut("validerInfoPersoParAgent/{Reference}")]
        public async Task<IActionResult> ValiderLesInformationsPersonnelles(String Reference, [FromBody] Candidature candidature)
        {
            var candidatureExistante = await _DB_ANCFCC.Candidatures
                .FirstOrDefaultAsync(c => c.Reference == Reference);

            if (candidatureExistante == null)
            {
                return NotFound();
            }

            candidatureExistante.StatutInfoPerso = candidature.StatutInfoPerso;
            candidatureExistante.CommentaireInfoPerso = candidature.CommentaireInfoPerso;
            candidatureExistante.DateModification = DateTime.Now;

            await _DB_ANCFCC.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("ValiderToutesLesinfosParAgent_Recap/{Reference}")]
        public async Task<ActionResult> UpdateCandidature(String Reference, [FromBody] Candidature candidature)
        {
            var candidatureToUpdate = await _DB_ANCFCC.Candidatures.FirstOrDefaultAsync(c => c.Reference == Reference);

            if (candidatureToUpdate == null)
            {
                return NotFound();
            }
            // CE CODE PERMET DE MISE AJOUR JUST LES CHAMPS SUIVANTES ET PAS LES AUTRES MEME SI VOUS ESSAYER !
            candidatureToUpdate.StatutDoc = candidature.StatutDoc;
            candidatureToUpdate.CommentaireDoc = candidature.CommentaireDoc;
            candidatureToUpdate.StatutExp = candidature.StatutExp;
            candidatureToUpdate.CommentaireExp = candidature.CommentaireExp;
            candidatureToUpdate.StatutInfoPerso = candidature.StatutInfoPerso;
            candidatureToUpdate.CommentaireInfoPerso = candidature.CommentaireInfoPerso;
            candidatureToUpdate.StatusAgent = candidature.StatusAgent; //supprimerValidationStatusAGENT 
            candidatureToUpdate.CommentaireFormation = candidature.CommentaireFormation;
            candidatureToUpdate.StatutFormation = candidature.StatutFormation;
            candidatureToUpdate.DateModification = DateTime.Now;
            //var agentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //écupérer l'ID de l'agent à partir des informations de connexion,

            //candidatureToUpdate.AgentId = agentId;

            try
            {
                await _DB_ANCFCC.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //var candidatureExists = await _DB_ANCFCC.Candidatures.FindAsync(id);
                //if (candidatureExists == null) 
                if (!CandidatureAvailable(Reference))
                {
                    return NotFound();


                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
            return Ok(new { message = "DONE" });

        }

        private bool CandidatureAvailable(int Reference)
        {
            throw new NotImplementedException();
        }

        private bool CandidatureAvailable(String? Reference)
        {
            return _DB_ANCFCC.Candidatures.Any(x => x.Reference == Reference);
        }
    }


}

