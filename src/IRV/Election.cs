using System.Collections.Generic;
using System.Linq;

namespace IRV
{
    public class Election
    {
        private readonly List<Vote> votes = new List<Vote>();

        public Election(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public int VoteCount => votes.Count;

        public string Winner()
        {
            if (!votes.Any()) return "Inconclusive";

            var excludedCandidates = new List<string>();
            var candidateCount = CountVotes(new List<string>()).Keys.Count;

            do
            {
                var counts = CountVotes(excludedCandidates);
                var winners = GetWinners(counts);
                if (winners.Count == 1)
                {
                    var winner = winners.First();
                    if ((double) winner.Value/VoteCount > 0.5)
                    {
                        return winner.Key;
                    }
                }
                excludedCandidates.AddRange(GetExcludedCandidates(counts));
                if (excludedCandidates.Count == candidateCount) return "Inconclusive";
            } while (true);
        }

        private static List<string> GetExcludedCandidates(Dictionary<string, int> counts)
        {
            var lowestTotal = counts.Values.Min();
            var losers = counts.Where(x => x.Value == lowestTotal).ToArray();
            return losers.Select(x => x.Key).ToList();
        }

        private static Dictionary<string, int> GetWinners(Dictionary<string, int> counts)
        {
            var highestTotal = counts.Values.Max();
            return counts.Where(x => x.Value == highestTotal).ToDictionary(x => x.Key, x => x.Value);
        }

        private Dictionary<string, int> CountVotes(List<string> excludedCandidates)
        {
            var counts = new Dictionary<string, int>();
            foreach (var vote in votes)
            {
                var topChoice = vote.TopChoice(excludedCandidates.ToArray());
                if (topChoice != "None")
                {
                    counts.IncrementCounter(topChoice);
                }
            }
            return counts;
        }

        public void RegisterVotes(params Vote[] newVotes)
        {
            votes.AddRange(newVotes);
        }
    }
}