using Backoffice_ANCFCC.Models;
using System.Security.Claims;

namespace Backoffice_ANCFCC.Services.ClaimsHelper
{
    public class ClaimsHelper : IClaimsHelper 
    {
        private readonly ILogger<ClaimsHelper> _logger;

        public ClaimsHelper(ILogger<ClaimsHelper> logger)
        {
            _logger = logger; 
        }
        public User GetUserClaim(ClaimsPrincipal user)
        {
            try
            {
                var nameClaim = user.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                var prenomClaim = user.Claims.FirstOrDefault(c => c.Type == "prenom")?.Value;
                var uniqueCodeClaim = user.Claims.FirstOrDefault(c => c.Type == "uniqueUserCode")?.Value;
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                
                if (int.TryParse(userIdClaim, out int userId))
                {
                    return new User
                    {
                        Nom = nameClaim,
                        Prenom = prenomClaim,
                        UserUniqueCode = uniqueCodeClaim,
                        UserId = userId
                    };
                }
                else
                {
                    return null; 
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user claims."); 
                return null;  
            }

        }

    }
}
