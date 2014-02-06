using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebGenerator
{
    class Utility
    {
        public static string GetFilename(string path)
        {
            return path.Split('\\').Last().Split('.').First();
        }

        public static Dictionary<string, string> LoadMap(string filename)
        {
            var result = new Dictionary<string, string>();
            foreach(string s in File.ReadAllLines(filename))
            {
                var parts = s.Split('=');
                if(parts.Length == 2) result.Add(parts[0], parts[1]);
            }
            return result;
        }
    }
}
