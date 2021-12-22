using System.Collections.Generic;

namespace Model
{
    public class Episode
    {
        public int ChapterId { get; set; }
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
                ChapterId = int.Parse(e[0]),
                ChapterName = e[1],
                ChapterEpisodeId = int.Parse(e[2]),
                Name = e[3],
                Id = int.Parse(e[4]),
                Completed = bool.Parse(e[5]),
                HighScore = e[6] == string.Empty ? 0 : int.Parse(e[6]),
            }));

            return episodes;
        }
    }
}
