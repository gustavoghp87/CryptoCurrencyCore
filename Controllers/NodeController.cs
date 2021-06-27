using cryptoCurrency.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace cryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class NodeController : ControllerBase
    {

        [HttpPost("node")]
        public IActionResult AddNode(Node newNode)
        {
            
            return Ok();
        }

        [HttpPost("node/registry")]
        public async Task<IActionResult> Register()
        {
            var httpClient = new HttpClient();
            var stringPayload = JsonConvert.SerializeObject(new { Ip = "http://localhost:5000" });
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            Console.WriteLine(httpContent);
            var httpResponse = await httpClient.PostAsync("http://localhost:10000/registry", httpContent);
            if (!httpResponse.IsSuccessStatusCode) return BadRequest();
            return Ok("Register");
            //if (httpResponse.Content != null)
            //{
            //    var responseContent = await httpResponse.Content.ReadAsStringAsync();
            //    Console.WriteLine(responseContent);
            //}

        }
    }
}