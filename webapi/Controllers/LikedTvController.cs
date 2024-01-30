using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Model;
using webapi.Service;
using webapi.Extensions;

namespace webapi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class LikedTvController : ControllerBase
{
    private readonly LikedTvService _likedTvService;

    public LikedTvController(LikedTvService likedTvService)
    {
        _likedTvService = likedTvService;
    }

    [HttpGet]
    [Authorize]
    public IEnumerable<LikedTvVM> Get()
    {
        return _likedTvService.All(User.GetUserId());
    }

    [HttpPut]
    [Authorize]
    public void Put(LikedTvVM likedTvVM)
    {
        Console.Write(likedTvVM.TvId);
        _likedTvService.Add(User.GetUserId(), likedTvVM);
    }

    [HttpDelete]
    [Authorize]
    public void Delete(int id)
    {
        _likedTvService.Delete(User.GetUserId(), id);
    }
}
