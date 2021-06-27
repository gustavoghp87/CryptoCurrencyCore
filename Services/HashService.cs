using System.Linq;

namespace cryptoCurrency.Services
{
    public class HashService
    {
        public static string GetHigher(string hash1, string hash2)
        {
            string[] hashes = new string[] { hash1, hash2 };
            string[] ordered = (from x in hashes orderby x select x).ToArray();
            return ordered.ElementAt(0);
        }
    }
}
