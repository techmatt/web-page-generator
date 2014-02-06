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
        Dictionary<string, string[]> bibs = new Dictionary<string, string[]>();
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
                bibs.Add(Utility.GetFilename(file), File.ReadAllLines(file));
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
            string conference = p["conference"];
            string teaser = p["teaser"];
            string pdf = p["pdf"];
            string abstractText = abstracts[paper];
            string[] bib = bibs[paper];

            List<string> lines = new List<string>();
            lines.Add(paperHeader.Replace("<title></title>","<title>" + title + "</title>"));

            lines.Add("<div id=\"mainD\">");
            lines.Add("<div id=\"contentD\">");
            lines.Add("<div id=\"headerD\">");
            lines.Add("<h1>" + title + "</h1>");
            lines.Add("<div id=\"authorsD\">");

            List<string> affiliations = new List<string>();
            
            affiliations.Add("Stanford University");
            foreach(string author in p["authors"].Split(','))
            {
                var a = authors[author];
                string affiliation = a["affiliation"];
                if(!affiliations.Contains(affiliation))
                {
                    affiliations.Add(affiliation);
                }
            }

            foreach(string author in p["authors"].Split(','))
            {
                lines.Add("<span>");
                var a = authors[author];
                string website = a["website"];
                string name = a["name"];
                string affiliation = a["affiliation"];
                lines.Add("<a href=\"" + website + "\">" + name + "</a>");

                if(affiliations.Count > 1)
                {
                    int affiliationIndex = affiliations.IndexOf(affiliation) + 1;
                    lines.Add("<sup>" + affiliationIndex + "</sup>");
                }
                lines.Add("</span>");
            }

            lines.Add("</div>");

            lines.Add("<div id=\"affiliationsD\">");

            foreach(string affiliation in affiliations)
            {
                lines.Add("<span>");
                int affiliationIndex = affiliations.IndexOf(affiliation) + 1;
                lines.Add("<sup>" + affiliationIndex + "</sup>");
                lines.Add(affiliation);
                lines.Add("</span>");
            }
            
            lines.Add("</div>");

            lines.Add("<div id=\"centerpieceD\">");
            lines.Add("<img id=\"teaserD\" src=\"" + teaser + "\"/>");
            lines.Add("<span>" + conference + "</span>");
            lines.Add("</div>");

            lines.Add("<div id=\"gutsD\">");
            lines.Add("<div id=\"abstractD\">");
            lines.Add("<h2>Abstract</h2>");
            lines.Add("<br/>");
            lines.Add("<p>");
            lines.Add(abstractText);
            lines.Add("</p>");
            lines.Add("</div>");

            lines.Add("<div id=\"extrasD\">");
            lines.Add("<h2>Extras</h2>");
            lines.Add("<br/>");
            lines.Add("<p class=\"item\">");
            lines.Add("Paper: <a href=\"" + pdf + "\"><img src=\"Images/pdf-icon.png\"/>PDF</a>");
            lines.Add("</p>");

            if(p.ContainsKey("supplemental"))
            {
                string supplemental = p["supplemental"];
                lines.Add("<p class=\"item\">");
                lines.Add("Supplemental Materials: <a href=\"" + supplemental + "\"><img src=\"Images/zip-icon.jpg\"/>ZIP</a>");
                lines.Add("</p>");
            }

            if (p.ContainsKey("database"))
            {
                string database = p["database"];
                lines.Add("<p class=\"item\">");
                lines.Add("Scene Database: <a href=\"" + database + "\"><img src=\"Images/zip-icon.jpg\"/>ZIP</a>");
                lines.Add("</p>");
            }

            if (p.ContainsKey("code"))
            {
                string code = p["code"];
                lines.Add("<p class=\"item\">");
                lines.Add("Database Example Code: <a href=\"" + code + "\"><img src=\"Images/code-icon.png\"/>CODE</a>");
                lines.Add("</p>");
            }

            if (p.ContainsKey("slides"))
            {
                string slides = p["slides"];
                lines.Add("<p class=\"item\">");
                lines.Add("SIGGRAPH presentation slides: <a href=\"" + slides + "\"><img src=\"Images/ppt-icon.png\"/>PPTX</a>");
                lines.Add("</p>");
            }

            if (p.ContainsKey("scholar"))
            {
                string scholar = p["scholar"];
                lines.Add("<p class=\"item\">");
                lines.Add("Google Scholar: <a href=\"" + scholar + "\"><img src=\"Images/google-icon.png\"/></a>");
                lines.Add("</p>");
            }

            lines.Add("<div class=\"item\">");
            lines.Add("BibTeX citation:");
            lines.Add("<div class=\"bibtexD\">");
            foreach(string s in bib)
            {
                lines.Add(s);
                lines.Add("<br/>");
            }
            lines.Add("</div>");
            lines.Add("</div>");

            lines.Add("</div>");
            lines.Add("<div id=\"clearfloatsD\"></div>");
            lines.Add("</div>");
            lines.Add("</div>");
        
            lines.Add(paperFooter);

            System.IO.File.WriteAllLines(outputDir + paper + ".html", lines);
        }
    }
}
