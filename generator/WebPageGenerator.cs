using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebGenerator
{
    class WebPageGenerator
    {
        void ProcessDirectory(string path)
        {
            foreach(string file in Directory.GetFiles(path, "*.html"))
            {
                ProcessFile(file);
            }
        }

        void ProcessFile(string file)
        {
            string[] lines = File.ReadAllLines(file);
        }
    }
}
