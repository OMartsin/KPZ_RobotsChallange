using System;
using System.Collections.Generic;
using Robot.Common;

namespace MartsinOleksandr.RobotsChallange
{
    public class DecisionMaker
    {
        private readonly Map _map;
        private readonly IList<Robot.Common.Robot> _robots;
        private readonly int _robotToMoveIndex;
        private const int Radius = 25;
        private const int LowEnergy = 30;

        public DecisionMaker(Map map, IList<Robot.Common.Robot> robots, int robotToMoveIndex)
        {
            _map = map;
            _robots = robots;
            _robotToMoveIndex = robotToMoveIndex;
        }

        public RobotCommand MakeDecision()
        {
            var currRobot = _robots[_robotToMoveIndex];
            var robotRadar = new RobotRadar(_map, _robots, _robotToMoveIndex);
            if (currRobot.Energy > 350)
            {
                return new CreateNewRobotCommand
                {
                    NewRobotEnergy = 100
                };
            }
            var chargeCellsInfo = robotRadar.SearchStationsInfo();
            foreach (var kvp in chargeCellsInfo)
            {
                Console.WriteLine($"Key: {kvp.Key.Position.X +" "+ kvp.Key.Position.Y}, Value: {kvp.Value}");
            }
            var robotsInfo = robotRadar.SearchDistanceToRobots();
            if (currRobot.Energy < LowEnergy)
            {
                var nearestCell = CellFinder.FindNearestCell(chargeCellsInfo);
                if (nearestCell.Distance < currRobot.Energy)
                {
                    return new MoveCommand() { NewPosition = nearestCell.Position };
                }
                return new MoveCommand()
                {
                    NewPosition = RouteSplitter.SplitRoute
                        (currRobot.Position, nearestCell.Position, currRobot.Energy)
                };
            }
            var numOfStationsInRadius = RobotChargePossibilityChecker.CheckRobotEnergyStations(chargeCellsInfo,
                _robots[_robotToMoveIndex]);
            var mostProfitableCell = CellFinder.FindMostProfitableCell(chargeCellsInfo,
                numOfStationsInRadius);
            if (mostProfitableCell == null && numOfStationsInRadius != 0)
            {
                return new CollectEnergyCommand();
            }

            if (mostProfitableCell == null)
            {
                return null;
            }
            return new MoveCommand() { NewPosition = mostProfitableCell.Position };
        }
        
    }
}