using System;
using NUnit.Framework;
using Robot.Common;

namespace MartsinOleksandr.RobotsChallenge.Test
{
    [TestFixture]
    public class TestRouteSplitter
    {
        [TestCase(1,2,1,14, 13)]
        public void TestSplitRoute(int aPositionX, int aPositionY, int bPositionX,int bPositionY,int remainingEnergy)
        {
            Console.WriteLine(RouteSplitter.SplitRoute(new Position(aPositionX,aPositionY),
                new Position(bPositionX,bPositionY),remainingEnergy));
            Assert.NotNull(RouteSplitter.SplitRoute(new Position(aPositionX,aPositionY),
                new Position(bPositionX,bPositionY),remainingEnergy));
        }
        
    }
}