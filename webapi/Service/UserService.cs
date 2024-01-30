using Data;
using Data.Model;
using System.Security.Claims;
using ViewModel.Model;
using webapi.Extensions;

namespace webapi.Service;
public class UserService
{
    private readonly WapContext _dbContext;
    private readonly AuthService _authService;

    public UserService(WapContext context, AuthService authService)
    {
        _dbContext = context;
        _authService = authService;
    }

    public IEnumerable<UserVM> AllUsers()
    {
        var users = _dbContext.Users.ToList();
        var viewModels = new List<UserVM>();
        foreach (var user in users)
        {
            viewModels.Add(new UserVM(user));
        }
        return viewModels;
    }

    public async Task SignInAsync(UserVM userVM)
    {
        await _authService.SignInAsync(userVM);
    }

    public UserVM? GetSignedInUser(ClaimsPrincipal claim)
    {
        var userId = claim.GetUserId();
        var user = _dbContext.Users.First(x => x.Id == userId);
        return new UserVM(user);
    }

    internal string Token(UserVM userVM)
    {
        return _authService.Token(userVM);
    }

    internal string TokenRefresh(int userId)
    {
        return _authService.Token(userId);
    }
}
