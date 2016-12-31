using NUnit.Framework;

namespace IRV.Tests
{
    [TestFixture]
    public class VoteTests
    {
        [Test]
        public void A_vote_can_be_created_with_no_candidate_selections()
        {
            var vote = new Vote("Voter");
            Assert.That(vote.Name, Is.EqualTo("Voter"));
        }

        [Test]
        public void A_vote_can_be_created_with_one_candidate_selection()
        {
            var vote = new Vote("Voter", "Candidate 1");
            Assert.That(vote.TopChoice(), Is.EqualTo("Candidate 1"));
        }

        [Test]
        public void A_vote_can_be_created_with_two_candidate_selections()
        {
            var vote = new Vote("Voter", "Candidate 1", "Candidate 2");
            Assert.That(vote.TopChoice(), Is.EqualTo("Candidate 1"));
        }

        [Test]
        public void A_vote_can_be_created_with_three_candidate_selections()
        {
            var vote = new Vote("Voter", "Candidate 3", "Candidate 1", "Candidate 2");
            Assert.That(vote.TopChoice(), Is.EqualTo("Candidate 3"));
        }

        [Test]
        public void TopChoice_with_one_candidate_excluded()
        {
            var vote = new Vote("Voter", "Candidate 3", "Candidate 1", "Candidate 2");
            Assert.That(vote.TopChoice("Candidate 3"), Is.EqualTo("Candidate 1"));
        }

        [Test]
        public void TopChoice_with_two_candidate_excluded()
        {
            var vote = new Vote("Voter", "Candidate 3", "Candidate 1", "Candidate 2");
            Assert.That(vote.TopChoice("Candidate 3", "Candidate 1"), Is.EqualTo("Candidate 2"));
        }

        [Test]
        public void TopChoice_with_all_candidate_excluded()
        {
            var vote = new Vote("Voter", "Candidate 3", "Candidate 1", "Candidate 2");
            Assert.That(vote.TopChoice("Candidate 3", "Candidate 1", "Candidate 2"), Is.EqualTo("None"));
        }
    }
}