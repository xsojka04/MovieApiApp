using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Model;
using webapi.Service;
using webapi.Extensions;

namespace webapi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class LikedMovieController : ControllerBase
{
    private readonly LikedMovieService _likedMovieService;

    public LikedMovieController(LikedMovieService likedMovieService)
    {
        _likedMovieService = likedMovieService;
    }

    [HttpGet]
    [Authorize]
    public IEnumerable<LikedMovieVM> Get()
    {
        return _likedMovieService.All(User.GetUserId());
    }

    [HttpPut]
    [Authorize]
    public void Put(LikedMovieVM likedMovieVM)
    {
        Console.Write(likedMovieVM.MovieId);
        _likedMovieService.Add(User.GetUserId(), likedMovieVM);
    }


    [HttpDelete]
    [Authorize]
    public void Delete(int id)
    {
        _likedMovieService.Delete(User.GetUserId(), id);
    }
}
