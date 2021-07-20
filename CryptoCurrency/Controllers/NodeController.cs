using Models;
using Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using Services.Nodes;

namespace CryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class NodeController : ControllerBase
    {
        private INodeService _nodeService;
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
            if (!url.StartsWith("https://") && !url.StartsWith("http://")) return BadRequest();
            CleanUrl.Clean(ref url);
            Node newNode = new();
            newNode.Address = new Uri(url);
            _nodeService.RegisterOne(newNode);
            return Ok(newNode);
        }
    }
}