using Microsoft.AspNetCore.Mvc;
using Backoffice_ANCFCC.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ANCFCC_BACKOFFICE.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CandidatureController : ControllerBase
    {
        private readonly DbAncfccContext _DB_ANCFCC;

        public CandidatureController(DbAncfccContext DbAncfccContext)
        {
            _DB_ANCFCC = DbAncfccContext;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCandidature(int id, Candidature candidature)
        {
            //var candidature = await _DB_ANCFCC.Candidatures.FindAsync(id);

            if (id != candidature.Id)
            {
                return BadRequest();
            }
            //candidature.StatutFormation = candidature.StatutFormation;


            _DB_ANCFCC.Entry(candidature).State = EntityState.Modified;

            try
            {
                await _DB_ANCFCC.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //var candidatureExists = await _DB_ANCFCC.Candidatures.FindAsync(id);
                //if (candidatureExists == null) 
                if (!CandidatureAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
            //return Ok();

        }

        private bool CandidatureAvailable(int id)
        {
            throw new NotImplementedException();
        }

        private bool CandidatureAvailable(int? id)
        {
            return _DB_ANCFCC.Candidatures.Any(x => x.Id == id);
        }
    }
}