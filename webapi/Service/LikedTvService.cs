using Data;
using Data.Model;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ViewModel.Model;
using webapi.Controllers;

namespace webapi.Service;
public class LikedMovieService
{
    private readonly WapContext _dbContext;

    public LikedMovieService(WapContext context, AuthService authService)
    {
        _dbContext = context;
    }

    public IEnumerable<LikedMovieVM> All(int userId)
    {
        var likedMovies = _dbContext.LikedMovies.Where(x => x.UserRefId == userId).ToList();
        var viewModels = new List<LikedMovieVM>();
        foreach (var likedMovie in likedMovies)
        {
            viewModels.Add(new LikedMovieVM(likedMovie));
        }
        return viewModels;
    }

    public void Add(int userId, LikedMovieVM likedMovieVM)
    {
        var likedMovie = new LikedMovie { MovieId = likedMovieVM.MovieId, User = _dbContext.Users.First(x => x.Id == userId) };
        _dbContext.LikedMovies.Add(likedMovie);
        _dbContext.SaveChanges();
    }

    public void Delete(int userId, int likedMovieId)
    {
        var likedMovies = _dbContext.LikedMovies.Where(x => x.UserRefId == userId && x.MovieId == likedMovieId.ToString());
        foreach (var likedMovie in likedMovies)
        {
            _dbContext.LikedMovies.Remove(likedMovie);
        }
        _dbContext.SaveChanges();
    }
}
