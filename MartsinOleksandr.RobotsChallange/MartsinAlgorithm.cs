using System.Collections.Generic;
using Robot.Common;

namespace MartsinOleksandr.RobotsChallange
{
    public class MartsinAlgorithm: IRobotAlgorithm
    {
        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            var decisionMaker = new DecisionMaker(map, robots, robotToMoveIndex);
            return decisionMaker.MakeDecision();
        }

        public string Author => "Martsin Oleksandr";
    }
}