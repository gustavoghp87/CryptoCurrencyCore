using CryptoCurrency.Controllers.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using Services.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class NodeController : ControllerBase, INodeController
    {
        private static INodeService _nodeService;
        public NodeController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpGet]
        public IActionResult GetNodes()
        {
            List<Node> nodes = _nodeService.GetAll();
            //if (nodes != null && nodes.Count > 0)
            //{
            //    var nodesArray = new string[100];
            //    foreach (Node node in nodes)
            //    {
            //        nodesArray.
            //    }
            //}
            return Ok(nodes);
        }

        [HttpPost]
        public IActionResult AddNode()
        {
            var hashtable = new Hashtable
            {
                { "node", "https://hashes.com/" }
            };
            var dictionary = new Dictionary<string, Node>
            {
                { "node", new Node() }
            };
            try
            {
                string url = Request.Headers["Referer"].ToString();
                Console.WriteLine("URL: ***************************************: " + url);
                if (url + "/" == Program.DomainName) return Ok();
                if (!url.StartsWith("https://") && !url.StartsWith("http://")) return BadRequest();
                CleanUrl.Clean(ref url);
                Node newNode = new() {
                    Address = new Uri(url)
                };
                bool response = _nodeService.RegisterOne(newNode);
                if (!response) return StatusCode(500);
                return Ok("Done: " + newNode);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return BadRequest();
            }
        }
    }
}