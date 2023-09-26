using System.Collections.Generic;
using Robot.Common;

namespace MartsinOleksandr.RobotsChallenge
{
    public class MartsinAlgorithm: IRobotAlgorithm
    {
        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            robots[robotToMoveIndex].Energy = 10000;
            var decisionMaker = new DecisionMaker(map, robots, robotToMoveIndex);
            return decisionMaker.MakeDecision();
        }

        public string Author => "Martsin Oleksandr";
    }
}