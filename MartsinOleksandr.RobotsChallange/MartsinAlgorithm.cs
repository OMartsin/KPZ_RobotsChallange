using System.Collections.Generic;
using Robot.Common;

namespace MartsinOleksandr.RobotsChallenge {
    public class MartsinAlgorithm : IRobotAlgorithm {
        private int _currentRound;
        public MartsinAlgorithm() {
            Logger.OnLogRound += (sender, args) => ++_currentRound;
        }
        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map) {
            var decisionMaker = new DecisionMaker(map, robots, robotToMoveIndex, _currentRound);
            return decisionMaker.MakeDecision();
        }
        public string Author => "Martsin Oleksandr";
    }
}