using BlockchainAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BlockchainAPI.Services.Nodes
{
    public static class RegisterMeService
    {
        public static void Send(Node[] nodes)
        {
            int counter = 0;
            if (nodes == null || nodes.Length == 0) return;
            foreach (Node node in nodes)
            {
                //var response = await new HttpClient().GetAsync(node.ToString() + "register");
                //if (response.IsSuccessStatusCode) counter++;
            }
            Console.WriteLine(counter);
        }
    }
}
