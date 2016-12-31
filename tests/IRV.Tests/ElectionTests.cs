using System.Linq;
using NUnit.Framework;

namespace IRV.Tests
{
    [TestFixture]
    public class Election_construction_tests
    {
        [Test]
        public void An_election_can_be_created()
        {
            var election = new Election("Election Name");
            Assert.That(election.Name, Is.EqualTo("Election Name"));
        }
    }

    [TestFixture]
    public class Election_vote_registration_tests
    {
        private Election election;

        [SetUp]
        public void SetUp()
        {
            election = new Election("Election Name");
        }

        [Test]
        public void A_vote_can_be_registered()
        {
            election.RegisterVotes(new Vote("Voter 1"));
            Assert.That(election.VoteCount, Is.EqualTo(1));
        }

        [Test]
        public void A_votes_can_be_registered()
        {
            election.RegisterVotes(new Vote("Voter 1"), new Vote("Voter 2"), new Vote("Voter 3"), new Vote("Voter 4"));
            Assert.That(election.VoteCount, Is.EqualTo(4));
        }
    }

    [TestFixture]
    public class Election_with_two_candidates_tests
    {
        private Election election;

        [SetUp]
        public void SetUp()
        {
            election = new Election("Election Name");
        }

        [Test]
        public void No_votes_cast()
        {
            Assert.That(election.Winner, Is.EqualTo("Inconclusive"));
        }

        [TestCase("Candidate 1")]
        [TestCase("Candidate 2")]
        public void A_single_vote_is_cast(string candidate)
        {
            var vote = new Vote("Voter 1", candidate);
            election.RegisterVotes(vote);
            Assert.That(election.Winner, Is.EqualTo(candidate));
        }

        [Test]
        public void Two_votes_are_cast_for_different_candidates()
        {
            var vote1 = new Vote("Voter 1", "Candidate 1");
            var vote2 = new Vote("Voter 2", "Candidate 2");
            election.RegisterVotes(vote1, vote2);
            Assert.That(election.Winner, Is.EqualTo("Inconclusive"));
        }

        [Test]
        public void Three_votes_are_cast_with_a_conclusive_result()
        {
            var vote1 = new Vote("Voter 1", "Candidate 1");
            var vote2 = new Vote("Voter 2", "Candidate 2");
            var vote3 = new Vote("Voter 3", "Candidate 2");
            election.RegisterVotes(vote1, vote2, vote3);
            Assert.That(election.Winner, Is.EqualTo("Candidate 2"));
        }

        [TestCase(2, 1, "Candidate 1")]
        [TestCase(1, 2, "Candidate 2")]
        [TestCase(1, 1, "Inconclusive")]
        public void Multiple_votes_for_two_candidates(int candidate1, int candidate2, string winner)
        {
            election.RegisterVotes(Enumerable.Range(1, candidate1)
                .Select(x => new Vote($"Voter {x}", "Candidate 1")).ToArray());
            election.RegisterVotes(Enumerable.Range(1, candidate2)
                .Select(x => new Vote($"Voter {x}", "Candidate 2")).ToArray());
            
            Assert.That(election.Winner, Is.EqualTo(winner));
            Assert.That(election.VoteCount, Is.EqualTo(candidate1 + candidate2));
        }
    }
}