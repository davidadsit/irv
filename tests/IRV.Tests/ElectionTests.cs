using NUnit.Framework;
using IRV;

namespace IRV.Tests
{
    [TestFixture]
    public class ElectionTests
    {
        [Test]
        public void An_election_can_be_created()
        {
            var election = new Election();
        }
    }
}
