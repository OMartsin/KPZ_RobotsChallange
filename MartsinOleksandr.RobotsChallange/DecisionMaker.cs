using System;
using System.Collections.Generic;
using MartsinOleksandr.RobotsChallange.Properties;
using Robot.Common;

namespace MartsinOleksandr.RobotsChallange
{
    public class DecisionMaker
    {
        private readonly Map _map;
        private readonly IList<Robot.Common.Robot> _robots;
        private readonly int _robotToMoveIndex;
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
            if (currRobot.Energy > 450 && robotRadar.CountMyRobots() <= 15)
            {
                return new CreateNewRobotCommand();
            }
            var chargeCellsInfo = robotRadar.SearchFreeStationsInfo();
            if (currRobot.Energy < LowEnergy)
            {
                return LowEnergyAlgorithm(chargeCellsInfo);
            }
            var attackInfo = GetRobotsAttackProfit(robotRadar);
            if (attackInfo.HasValue)
            {
                var attackedRobot = attackInfo.Value.Key;
                if (attackInfo.Value.Value > 0 &&
                    DistanceHelper.FindDistance(attackedRobot.Position, currRobot.Position) < currRobot.Energy)
                {
                    return new MoveCommand() { NewPosition = attackedRobot.Position };
                }
            }
            if (_map.Stations == null || _map.Stations.Count == 0)
            {
                return null;
            }
            var bigCommand = BigStationCollectEnergyAlgorithm();
            if (bigCommand != null)
            {
                return bigCommand;
            }
            var numOfStationsInRadius = robotRadar.SearchStationsCountInCollectRadius();
            var mpcByCollectNum = 
                CellFinder.FindMostProfitableCellByStationsInCellCollect(chargeCellsInfo, numOfStationsInRadius);
            if (mpcByCollectNum != null && 
                RouteSplitter.CalculateEnergyForOptimalStepsSplitter
                    (mpcByCollectNum.Position, currRobot.Position) <= 32)
            {
                return new MoveCommand()
                {
                    NewPosition = RouteSplitter.OptimalStepsSplitter(currRobot.Position,mpcByCollectNum.Position)
                };
            }
            if ((mpcByCollectNum == null || RouteSplitter.CalculateEnergyForOptimalStepsSplitter
                    (mpcByCollectNum.Position, currRobot.Position) > 100))
            {
                return new CollectEnergyCommand();
            }
            var freeNearestCell = CellFinder.FindNearestCell(chargeCellsInfo);
            return new MoveCommand()
            {
                NewPosition = RouteSplitter.OptimalStepsSplitter(currRobot.Position,freeNearestCell.Position)
            };
        }
        
        private RobotCommand LowEnergyAlgorithm(Dictionary<ChargePointInfo,int> chargeCellsInfo)
        {
            var currRobot = _robots[_robotToMoveIndex];
            var nearestCell = CellFinder.FindNearestCell(chargeCellsInfo);
            if (nearestCell.Distance < currRobot.Energy)
            {
                return new MoveCommand() { NewPosition = nearestCell.Position };
            }

            var pos = RouteSplitter.SplitRoute (currRobot.Position, nearestCell.Position,
                currRobot.Energy - 4);
            if (pos == null)
            {
                return null;
            }
            return new MoveCommand()
            {
                NewPosition = pos
            };
        }
        
        private KeyValuePair<Robot.Common.Robot, int>? GetRobotsAttackProfit(RobotRadar robotRadar)
        {
            var robotsInfo = robotRadar.SearchDistanceToRobots();
            int profit = 0;
            Robot.Common.Robot robot = null;
            foreach (var robotInfo in robotsInfo)
            {
                if (robotInfo.Value + 30 < (robotInfo.Key.Energy * 0.1) && robotInfo.Value < 50)
                {
                    profit = (int)Math.Round(robotInfo.Key.Energy * 0.1)  - (robotInfo.Value + 30);
                    robot = robotInfo.Key;
                }
            }

            if (robot == null)
            {
                return null;
            }
            return new KeyValuePair<Robot.Common.Robot,int>(robot, profit);
        }

        private RobotCommand BigStationCollectEnergyAlgorithm()
        {
            var currRobot = _robots[_robotToMoveIndex];
            var mostProfitableCellByCollectEnergy =
                CellFinder.FindMostProfitableCellByEnergyCollect(_map.Stations, currRobot);
            if (mostProfitableCellByCollectEnergy.Distance < 9)
            {
                return new CollectEnergyCommand();
            }
            if (mostProfitableCellByCollectEnergy.Station.Energy > 120 && 
                RouteSplitter.CalculateEnergyForOptimalStepsSplitter
                    (currRobot.Position,mostProfitableCellByCollectEnergy.Position) < 
                mostProfitableCellByCollectEnergy.Station.Energy)
            {
                return new MoveCommand()
                { NewPosition = RouteSplitter.OptimalStepsSplitter
                    (mostProfitableCellByCollectEnergy.Position,currRobot.Position) };
            }
            return null;
        }

    }
}