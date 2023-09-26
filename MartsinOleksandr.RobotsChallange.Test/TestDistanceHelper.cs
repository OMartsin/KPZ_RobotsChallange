using NUnit.Framework;
using Robot.Common;
using Assert = NUnit.Framework.Assert;

namespace MartsinOleksandr.RobotsChallenge.Test
{
    [TestFixture]
    public class DistanceHelperTest
    {
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(5, 5, 1, 1, 32)]
        [TestCase(0, 0, 3, 4, 25)]
        [TestCase(0, 0, 3, 3, 18)]
        public void TestFindDistance(int x1, int y1, int x2, int y2, int expectedDistance)
        {
            // Arrange
            Position positionA = new Position { X = x1, Y = y1 };
            Position positionB = new Position { X = x2, Y = y2 };

            // Act
            int result = DistanceHelper.FindDistance(positionA, positionB);

            // Assert
            Assert.AreEqual(expectedDistance, result);
        }
    }
}