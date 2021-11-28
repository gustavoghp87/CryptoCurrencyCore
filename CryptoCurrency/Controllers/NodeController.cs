using CryptoCurrency.Controllers.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using Services.Nodes;
using System;

namespace CryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class NodeController : ControllerBase, INodeController
    {
        private readonly INodeService _nodeService;
        public NodeController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpGet("node")]
        public IActionResult GetNodes()
        {
            return Ok(_nodeService.GetAll());
        }

        [HttpPost("node")]
        public IActionResult AddNode()
        {
            string url = Request.Headers["Referer"].ToString();
            Console.WriteLine("URL: ***************************************: " + url);
            //if (url + "/" == Program.AppUrl) return Ok();
            if (!url.StartsWith("https://") && !url.StartsWith("http://")) return BadRequest();
            CleanUrl.Clean(ref url);
            Node newNode = new() {
                Address = new Uri(url)
            };
            _nodeService.RegisterOne(newNode);
            return Ok(newNode);
        }
    }
}