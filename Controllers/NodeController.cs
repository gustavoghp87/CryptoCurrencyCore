using cryptoCurrency.Models;
using cryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
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

        [HttpGet("node")]
        public IActionResult GetNodes()
        {
            return Ok(_nodeService.GetAll());
        }

        [HttpPost("node")]
        public IActionResult AddNode(Node newNode)
        {
            // TODO: take from url and not from body
            RequestHeaders header = Request.GetTypedHeaders();
            Uri uriReferer = header.Referer;
            Console.WriteLine(uriReferer);
            //
            if (newNode.Address == null || newNode.Address.ToString() == "") return BadRequest();
            while (newNode.Address.ToString().EndsWith('/'))
                newNode.Address = new Uri(newNode.Address.ToString().Substring(0, -1));
            _nodeService.RegisterOne(newNode);
            return Ok();
        }
    }
}