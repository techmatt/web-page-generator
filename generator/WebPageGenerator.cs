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
        Dictionary<string, Dictionary<string, string>> authors = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> papers = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> bibs = new Dictionary<string, string>();
        Dictionary<string, string> abstracts = new Dictionary<string, string>();
        string paperHeader, paperFooter;
        
        public void Generate(string inputDir, string outputDir)
        {
            LoadAuthors(inputDir + "authors");
            LoadPapers(inputDir + "papers");
            LoadBibs(inputDir + "bibs");
            LoadAbstracts(inputDir + "abstracts");

            paperHeader = File.ReadAllText(inputDir + "paperHeader.txt");
            paperFooter = File.ReadAllText(inputDir + "paperFooter.txt");

            foreach(string paper in papers.Keys)
            {
                ProcessPaper(paper, outputDir);
            }
        }

        

        void LoadAuthors(string path)
        {
            foreach (string file in Directory.GetFiles(path, "*.txt"))
            {
                authors.Add(Utility.GetFilename(file), Utility.LoadMap(file));
            }
        }

        void LoadPapers(string path)
        {
            foreach(string file in Directory.GetFiles(path, "*.txt"))
            {
                papers.Add(Utility.GetFilename(file), Utility.LoadMap(file));
            }
        }

        void LoadBibs(string path)
        {
            foreach (string file in Directory.GetFiles(path, "*.txt"))
            {
                bibs.Add(Utility.GetFilename(file), File.ReadAllText(file));
            }
        }

        void LoadAbstracts(string path)
        {
            foreach (string file in Directory.GetFiles(path, "*.txt"))
            {
                abstracts.Add(Utility.GetFilename(file), File.ReadAllText(file));
            }
        }

        void ProcessPaper(string paper, string outputDir)
        {
            var p = papers[paper];
            string title = p["title"];

            List<string> lines = new List<string>();
            lines.Add(paperHeader.Replace("<title></title>","<title>" + title + "</title>"));
            lines.Add(paperFooter);

            System.IO.File.WriteAllLines(outputDir + paper + ".html", lines);
        }
    }
}
