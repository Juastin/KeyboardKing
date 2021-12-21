using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Episode> Episodes { get; set; }

        public string Badge { 
            get
            {
                if (Episodes.Where(e => !e.Completed).Any())
                    return $"{Id}_grey";

                return $"{Id}";
            } 
        }


        public static List<Chapter> ParseAllChapters(List<List<string>> input)
        {
            //[0] chapterid, [1] chaptername, [2] episodechapterid, [3] episodename, [4] episodeid, [5] completed, [6] highscore
            List<Episode> episodes = Episode.ParseEpisodes(input);

            List<Chapter> chapters = episodes
                .GroupBy(e => e.ChapterId)
                .ToList()
                .Select(e => new Chapter() { Id = e.Key, Name = e.ToList()[0].ChapterName, Episodes = e.ToList() })
                .ToList();

            return chapters;
        }
    }
}
