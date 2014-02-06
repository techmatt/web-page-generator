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
            string conference = p["conference"];
            string teaser = p["teaser"];
            string abstractText = abstracts[paper];

            List<string> lines = new List<string>();
            lines.Add(paperHeader.Replace("<title></title>","<title>" + title + "</title>"));

            lines.Add("<div id=\"mainD\">");
            lines.Add("<div id=\"contentD\">");
            lines.Add("<div id=\"headerD\">");
            lines.Add("<h1>" + title + "</h1>");
            lines.Add("<div id=\"authorsD\">");

            foreach(string author in p["authors"].Split(','))
            {
                lines.Add("<span>");
                var a = authors[author];
                string website = a["website"];
                string name = a["name"];
                lines.Add("<a href=\"" + website + "\">" + name + "</a>");
                lines.Add("<sup>1</sup>");
                lines.Add("</span>");
            }

            lines.Add("</div>");

            lines.Add("<div id=\"affiliationsD\">");

            /*
             * <span>
                    <sup>1</sup>
                    Stanford University
                </span>
                <span>
                    <sup>2</sup>
                    Princeton University
                </span>
             * */
            lines.Add("</div>");

            lines.Add("<div id=\"centerpieceD\">");
            lines.Add("<img id=\"teaserD\" src=\"" + teaser + "\"/>");
            lines.Add("<span>" + conference + "</span>");
            lines.Add("</div>");

            lines.Add("<div id=\"gutsD\">");
            lines.Add("<div id=\"abstractD\">");
            lines.Add("<h2>Abstract</h2>");
            lines.Add("<p>");
            lines.Add(abstractText);
            lines.Add("</p>");
            lines.Add("</div>");

            lines.Add("<div id=\"extrasD\">");
            //lines.Add("<h2>Extras</h2>");
            lines.Add("<p class=\"item\">");
            lines.Add("Paper: <a href=\"scenesynth_paper.pdf\"><img src=\"img/pdf-icon.png\"/>PDF</a>");
            lines.Add("</p>");

            /*/*  <p class=\"item\">
                    Supplemental Materials: <a href=\"scenesynth_supp.zip\"><img src=\"img/zip-icon.jpg\"/>ZIP</a>
                </p>
                <p class=\"item\">
                    Scene Database: <a href=\"databaseFull.zip\"><img src=\"img/zip-icon.jpg\"/>ZIP</a>
                    </p>
                <p class=\"item\">
                    Database Example Code: <a href=\"http://code.google.com/p/stanford-scene-database/\"><img src=\"img/code-icon.png\"/>CODE</a>
                </p>
                <div class=\"item\">
                    BibTeX citation:
                    <div class=\"bibtex\">
                        @inproceedings{2012-scenesynth,
                        <br/>
                        author = {Matthew Fisher and Daniel Ritchie and Manolis Savva and Thomas Funkhouser and Pat Hanrahan},
                        <br/>
                        title = {Example-based Synthesis of 3D Object Arrangements},
                        <br/>
                        booktitle = {ACM SIGGRAPH Asia 2012 papers},
                        <br/>
                        series = {SIGGRAPH Asia '12},
                        <br/>
                        year = {2012}}
                    </div>
                </div>*/
            
            lines.Add("</div>");
            lines.Add("<div id=\"clearfloatsD\"></div>");
            lines.Add("</div>");
            lines.Add("</div>");
        
            lines.Add(paperFooter);

            System.IO.File.WriteAllLines(outputDir + paper + ".html", lines);
        }
    }
}
