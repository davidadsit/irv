using System.Collections.Generic;
using System.Globalization;
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

        public string Winner
        {
            get
            {
                if (!votes.Any()) return "Inconclusive";

                var counts = new Dictionary<string, int>();
                foreach (var vote in votes)
                {
                    counts.IncrementCounter(vote.TopChoice());
                }
                var highestTotal = counts.Values.Max();
                var winners = counts.Where(x => x.Value == highestTotal).ToArray();
                if (winners.Count() == 1)
                {
                    return winners.First().Key;
                }
                return "Inconclusive";
            }
        }

        public void RegisterVotes(params Vote[] newVotes)
        {
            votes.AddRange(newVotes);
        }
    }
}