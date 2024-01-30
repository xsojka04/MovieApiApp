using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using ViewModel.Model;
using webapi.Service;
using Microsoft.AspNetCore.WebUtilities;
//For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TmdbController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync(string url, string? query = null)
        {
            var uri = $"https://api.themoviedb.org/3/{url}";
            if (query != null)
            {
                var dictQuery = new Dictionary<string, string?>()
                {
                    ["query"] = query,
                };
                uri = QueryHelpers.AddQueryString(uri, dictQuery);
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "******");

            try
            {
                var data = await client.GetStringAsync(uri);
                return Ok(data);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
