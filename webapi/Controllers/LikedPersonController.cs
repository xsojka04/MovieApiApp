using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using ViewModel.Model;
using webapi.Service;
using webapi.Extensions;


namespace webapi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class LikedPersonController : ControllerBase
{
    private readonly LikedPersonService _likedPersonService;

    public LikedPersonController(LikedPersonService likedPersonService)
    {
        _likedPersonService = likedPersonService;
    }

    [HttpGet]
    [Authorize]
    public IEnumerable<LikedPersonVM> Get()
    {
        return _likedPersonService.All(User.GetUserId());
    }

    [HttpPut]
    [Authorize]
    public void Put(LikedPersonVM likedPersonVM)
    {
        _likedPersonService.Add(User.GetUserId(), likedPersonVM);
    }


    [HttpDelete]
    [Authorize]
    public void Delete(int id)
    {
        _likedPersonService.Delete(User.GetUserId(), id);
    }
}
