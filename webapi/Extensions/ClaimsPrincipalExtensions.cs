using System.Security.Claims;
using webapi.Service;

namespace webapi.Extensions;

public static class ClaimsPrincipalExtensions
{
  public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
  {
        return Int32.Parse(claimsPrincipal.FindFirstValue(AuthService.USER_ID_CLAIM_ID) ?? throw new Exception());
  }
}
