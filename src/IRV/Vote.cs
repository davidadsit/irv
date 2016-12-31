using System.Linq;

namespace IRV
{
    public class Vote
    {
        public string Name { get; }
        public string[] Selections { get; }

        public Vote(string name, params string[] selections)
        {
            Name = name;
            Selections = selections;
        }

        public string TopChoice(params string[] excludedCandidates)
        {
            return Selections.FirstOrDefault(x => excludedCandidates.All(y => x != y)) ?? "None";
        }
    }
}