using System;
using System.Collections.Generic;
using System.Linq;
using MartsinOleksandr.RobotsChallange;
using MartsinOleksandr.RobotsChallenge.Properties;
using Robot.Common;

namespace MartsinOleksandr.RobotsChallenge
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
            var chargeCellsInfo = robotRadar.SearchStationsInfo();
            if (currRobot.Energy < LowEnergy)
            {
                return LowEnergyAlgorithm(chargeCellsInfo);
            }

            RobotCommand mostEffectiveCommand = null;
            int energyProfit = Int32.MinValue;
            var attackInfo = GetMostProfitableRobotToAttack(robotRadar);
            if (attackInfo.HasValue)
            {
                var attackedRobot = attackInfo.Value.Key;
                mostEffectiveCommand =  new MoveCommand() { NewPosition = attackedRobot.Position };
                energyProfit = attackInfo.Value.Value;
            }
            if (_map.Stations == null || _map.Stations.Count == 0)
            {
                return mostEffectiveCommand;
            }
            var currCellInfo = robotRadar.SearchStationsInCollectRadius();
            var energySum = Int32.MinValue;
            var mpc = CellFinder.FindMostProfitableCell(chargeCellsInfo, 
                new ChargePointInfo(currRobot.Position, new EnergyStation[]{ }, 0), 
                currRobot.Energy);
            if (currCellInfo != null)
            {
                energySum = currCellInfo.Stations.Sum(station => station.Energy);
                mpc = CellFinder.FindMostProfitableCell(chargeCellsInfo, currCellInfo, currRobot.Energy);
            }
            if (energyProfit < energySum)
            {
                energyProfit = energySum;
                mostEffectiveCommand = new CollectEnergyCommand();
            }
            if (mpc.HasValue)
            {
                if (energyProfit < mpc.Value.Item2)
                {
                    mostEffectiveCommand = new MoveCommand() { NewPosition = RouteSplitter.OptimalStepsSplitter(
                        currRobot.Position,mpc.Value.Item1) };
                }
            }
            if (mostEffectiveCommand == null)
            {
                LowEnergyAlgorithm(chargeCellsInfo);
            }
            return mostEffectiveCommand;
        }
        
        private RobotCommand LowEnergyAlgorithm(List<ChargePointInfo> chargeCellsInfo)
        {
            var currRobot = _robots[_robotToMoveIndex];
            var nearestCell = CellFinder.FindNearestCell(chargeCellsInfo);
            if (nearestCell == null)
            {
                return null;
            }
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
        
        private KeyValuePair<Robot.Common.Robot, int>? GetMostProfitableRobotToAttack
            (RobotRadar robotRadar)
        {
            var robotsInfo = robotRadar.SearchDistanceToRobots();
            int profit = 0;
            Robot.Common.Robot robot = null;
            foreach (var robotInfo in robotsInfo)
            {
                var tempProfit = (int)Math.Round(robotInfo.Key.Energy * 0.1) - (robotInfo.Value + 30);
                if (tempProfit > 0 && tempProfit > profit && 
                    robotInfo.Value < _robots[_robotToMoveIndex].Energy)
                {
                    profit = tempProfit;
                    robot = robotInfo.Key;
                }
            }
            if (robot == null)
            {
                return null;
            }
            return new KeyValuePair<Robot.Common.Robot,int>(robot, profit);
        }
    }
}