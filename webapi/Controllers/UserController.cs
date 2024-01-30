using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Model;
using webapi.Extensions;
using webapi.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<UserVM> Get()
        {
            return _userService.AllUsers();
        }

        [HttpPost]
        public IActionResult Token([FromBody] UserVM userVM)
        {
            try
            {
                return Ok(_userService.Token(userVM));
            } catch
            {
                return BadRequest("Špatné přihlašovací údaje");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult TokenRefresh()
        {
            try
            {
                return Ok(_userService.TokenRefresh(User.GetUserId()));
            }
            catch
            {
                return BadRequest("Nepodařilo se aktualizovat token");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SignInAsync([FromBody] UserVM userVM)
        {
            try
            {
                await _userService.SignInAsync(userVM);
                return Ok();
            }
            catch
            {
                return BadRequest("Registrace se nezdařila. Uřivatelské jméno je již zabrané.");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(_userService.GetSignedInUser(this.User));
        }
    }
}
