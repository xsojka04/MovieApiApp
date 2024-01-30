using Microsoft.AspNetCore.Mvc;
//For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImdbController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> RatingsAsync(string imdbid)
        {
            HttpClient client = new HttpClient();
            var uri = $"https://imdb-api.com/en/API/Ratings/****/{imdbid}";

            try
            {
                var data = await client.GetStringAsync(uri);
                return Ok(data);
            } catch
            {
                return NotFound();
            }
        }
    }
}
