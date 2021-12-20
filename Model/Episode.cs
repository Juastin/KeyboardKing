using System.Collections.Generic;

namespace Model
{
    public class Episode
    {
        public string ChapterName { get; set; }
        public int ChapterEpisodeId { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public bool Completed { get; set; }
        public int HighScore { get; set; }

        public Queue<EpisodeStep> EpisodeSteps { get; set; }
        public int PassThreshold { get; set; }

        public Episode()
        {
            EpisodeSteps = new Queue<EpisodeStep>();
        }

        public static List<Episode> ParseEpisodes(List<List<string>> input)
        {
            List<Episode> episodes = new List<Episode>();
            input.ForEach(e => episodes.Add(new Episode()
            {
                ChapterName = e[0],
                ChapterEpisodeId = int.Parse(e[1]),
                Name = e[2],
                Id = int.Parse(e[3]),
                Completed = bool.Parse(e[4]),
                HighScore = e[5] == string.Empty ? 0 : int.Parse(e[5]),
            }));

            return episodes;
        }
    }
}
