using cryptoCurrency.Models;
using cryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace cryptoCurrency.Controllers
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
        private void CleanUrl(ref string url)
        {
            try
            {
                int first = url.StartsWith("https://") ? 8 : 7;
                int length = url.Length;
                string sub = url.Substring(first, length-first);
                int limit;
                if (sub.IndexOf("/") != -1) limit = first + sub.IndexOf("/");
                else limit = length;
                url = url.Substring(0, limit);
            }
            catch (Exception e) { Console.Write(e.Message); }
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
            CleanUrl(ref url);
            Node newNode = new();
            newNode.Address = new Uri(url);
            _nodeService.RegisterOne(newNode);
            return Ok(newNode);
        }
    }
}