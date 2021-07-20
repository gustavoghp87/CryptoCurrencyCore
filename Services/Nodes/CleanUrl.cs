using System;

namespace Services.Nodes
{
    public class CleanUrl
    {
        public static void Clean(ref string url)
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
    }
}