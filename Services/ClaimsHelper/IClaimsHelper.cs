using Backoffice_ANCFCC.Models;
using System.Security.Claims;

namespace Backoffice_ANCFCC.Services.ClaimsHelper
{
    public interface IClaimsHelper
    {
        public User GetUserClaim(ClaimsPrincipal user);






    }
}
