using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backoffice_ANCFCC.Models;
using Microsoft.AspNetCore.Authorization;
using Backoffice_ANCFCC.Services.ClaimsHelper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using Serilog;

namespace ANCFCC_BACKOFFICE_PROJECT.Controllers
{
    [ApiController]
    [Route("concours")]
    [ApiVersion("1.0")]
    public class ConcoursController : Controller
    {


        private readonly DbAncfccContext _DB_ANCFCC;
        private readonly IClaimsHelper _claimsHelper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ConcoursController> _logger;

        public ConcoursController(DbAncfccContext dbAncfccContext, IMemoryCache cache, ILogger<ConcoursController> logger)
        {
            _DB_ANCFCC = dbAncfccContext;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve the list of concours.
        /// </summary>
        /// <remarks>
        /// This endpoint returns the list of concours available. It first attempts to retrieve
        /// the list from the cache. If not found in the cache, it fetches the list from the database,
        /// caches it for 30 days, and returns the result.
        /// </remarks>
        /// <returns>
        ///   <response code="200">Returns the list of concours.</response>
        ///   <response code="500">If an error occurs while processing the request.</response>
        /// </returns>
        [HttpGet("list")]
        [Authorize(Roles = "Admin,Agent")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        public async Task<ActionResult<IQueryable<Concour>>> GetConcoursList()
        {
            try
            {
                if (!_cache.TryGetValue("ConcoursList", out List<Concour> concoursNames))

                {
                    Log.Information("ConcoursList from the database.");
                    concoursNames = await _DB_ANCFCC.Concours.ToListAsync();
                    var cacheEntyOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
                    };

                    _cache.Set("ConcoursList", concoursNames, cacheEntyOptions);
                }
                else
                {
                    Log.Information("ConcoursList From the cache.");
                }
                return Ok(concoursNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing this request");
                return StatusCode(500, "Internal sever error"); 

            
            }
        }

    }
}



