using System;
using System.Linq;
using NUnit.Framework;

namespace IRV.Tests
{
    [TestFixture]
    public class Election_construction
    {
        [Test]
        public void An_election_can_be_created()
        {
            var election = new Election("Election Name");
            Assert.That(election.Name, Is.EqualTo("Election Name"));
        }
    }

    [TestFixture]
    public class Election_vote_registration
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
    public class Election_with_two_candidates
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

        [TestCase("A")]
        [TestCase("B")]
        public void A_single_vote_is_cast(string candidate)
        {
            var vote = new Vote("Voter 1", candidate);
            election.RegisterVotes(vote);
            Assert.That(election.Winner, Is.EqualTo(candidate));
        }

        [Test]
        public void Two_votes_are_cast_for_different_candidates()
        {
            var vote1 = new Vote("Voter 1", "A");
            var vote2 = new Vote("Voter 2", "B");
            election.RegisterVotes(vote1, vote2);
            Assert.That(election.Winner, Is.EqualTo("Inconclusive"));
        }

        [Test]
        public void Three_votes_are_cast_with_a_conclusive_result()
        {
            var vote1 = new Vote("Voter 1", "A");
            var vote2 = new Vote("Voter 2", "B");
            var vote3 = new Vote("Voter 3", "B");
            election.RegisterVotes(vote1, vote2, vote3);
            Assert.That(election.Winner, Is.EqualTo("B"));
        }

        [TestCase(2, 1, "A")]
        [TestCase(1, 2, "B")]
        [TestCase(1, 1, "Inconclusive")]
        public void Multiple_votes_for_two_candidates(int candidate1, int candidate2, string winner)
        {
            election.RegisterVotes(Enumerable.Range(1, candidate1)
                .Select(x => new Vote($"Voter {x}", "A"))
                .ToArray());
            election.RegisterVotes(Enumerable.Range(1, candidate2)
                .Select(x => new Vote($"Voter {candidate1 + x}", "B"))
                .ToArray());

            Assert.That(election.Winner, Is.EqualTo(winner));
            Assert.That(election.VoteCount, Is.EqualTo(candidate1 + candidate2));
        }
    }

    [TestFixture]
    public class Election_with_three_candidates
    {
        private Election election;

        [SetUp]
        public void SetUp()
        {
            election = new Election("Election Name");
        }

        [TestCase(3, 1, 1, "A")]
        [TestCase(1, 3, 1, "B")]
        [TestCase(1, 1, 3, "C")]
        [TestCase(1, 1, 1, "Inconclusive")]
        public void Multiple_votes_for_two_candidates(int candidate1, int candidate2, int candidate3, string winner)
        {
            election.RegisterVotes(Enumerable.Range(1, candidate1)
                .Select(x => new Vote($"Voter {x}", "A"))
                .ToArray());
            election.RegisterVotes(Enumerable.Range(1, candidate2)
                .Select(x => new Vote($"Voter {candidate1 + x}", "B"))
                .ToArray());
            election.RegisterVotes(Enumerable.Range(1, candidate3)
                .Select(x => new Vote($"Voter {candidate1 + candidate2 + x}", "C"))
                .ToArray());

            Assert.That(election.Winner, Is.EqualTo(winner));
            Assert.That(election.VoteCount, Is.EqualTo(candidate1 + candidate2 + candidate3));
        }
    }

    [TestFixture]
    public class Election_with_three_candidates_requiring_runnoff
    {
        private Election election;

        [SetUp]
        public void SetUp()
        {
            election = new Election("Election Name");
        }

        [Test]
        public void With_a_third_party_spoiler()
        {
            election.RegisterVotes(new Vote("Voter 1", "A"));
            election.RegisterVotes(new Vote("Voter 2", "A"));

            election.RegisterVotes(new Vote("Voter 3", "B"));
            election.RegisterVotes(new Vote("Voter 4", "B"));

            election.RegisterVotes(new Vote("Voter 5", "C"));

            Assert.That(election.Winner, Is.EqualTo("Inconclusive"));
        }

        [Test]
        public void When_the_third_party_vote_selects_a_second_choice()
        {
            election.RegisterVotes(new Vote("Voter 1", "A"));
            election.RegisterVotes(new Vote("Voter 2", "A"));
            election.RegisterVotes(new Vote("Voter 3", "A"));

            election.RegisterVotes(new Vote("Voter 4", "B"));
            election.RegisterVotes(new Vote("Voter 5", "B"));
            election.RegisterVotes(new Vote("Voter 6", "B"));

            election.RegisterVotes(new Vote("Voter 7", "C", "A"));

            Assert.That(election.Winner, Is.EqualTo("A"));
        }

        [Test]
        public void When_a_candidate_has_the_most_but_not_a_majority()
        {
            election.RegisterVotes(new Vote("Voter 1", "A"));
            election.RegisterVotes(new Vote("Voter 2", "A"));
            election.RegisterVotes(new Vote("Voter 3", "A"));
            election.RegisterVotes(new Vote("Voter 4", "A"));

            election.RegisterVotes(new Vote("Voter 5", "B"));
            election.RegisterVotes(new Vote("Voter 6", "B"));
            election.RegisterVotes(new Vote("Voter 7", "B"));

            election.RegisterVotes(new Vote("Voter 8", "C", "B"));
            election.RegisterVotes(new Vote("Voter 9", "C", "B"));

            Assert.That(election.Winner, Is.EqualTo("B"));
        }
    }
    [TestFixture]
    public class Election_with_five_candidates_requiring_runnoff
    {
        private Election election;

        [SetUp]
        public void SetUp()
        {
            election = new Election("Election Name");
        }

        [Test]
        public void When_multiple_iterations_are_required_to_reach_a_winner()
        {
            election.RegisterVotes(new Vote("Voter", "A", "C"));
            election.RegisterVotes(new Vote("Voter", "A", "C"));
            election.RegisterVotes(new Vote("Voter", "A", "D"));
            election.RegisterVotes(new Vote("Voter", "A", "E"));

            election.RegisterVotes(new Vote("Voter", "B", "C"));
            election.RegisterVotes(new Vote("Voter", "B", "E", "C"));
            election.RegisterVotes(new Vote("Voter", "B", "A"));

            election.RegisterVotes(new Vote("Voter", "C", "B"));
            election.RegisterVotes(new Vote("Voter", "C", "E"));

            election.RegisterVotes(new Vote("Voter", "D", "C", "A"));
            election.RegisterVotes(new Vote("Voter", "D", "E", "C"));

            election.RegisterVotes(new Vote("Voter", "E", "C"));

            Assert.That(election.Winner(), Is.EqualTo("C"));
        }
    }
}