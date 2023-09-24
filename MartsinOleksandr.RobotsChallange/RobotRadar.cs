using System;
using System.Collections.Generic;
using System.Linq;
using MartsinOleksandr.RobotsChallange.Properties;
using Robot.Common;

namespace MartsinOleksandr.RobotsChallange
{
    public class RobotRadar
    {
        private const int CollectionRange = 2;

        private readonly Map _map;
        private readonly IList<Robot.Common.Robot> _robots;
        private readonly int _robotToMoveIndex;

        public RobotRadar(Map map, IList<Robot.Common.Robot> robots, int robotToMoveIndex)
        {
            _map = map;
            _robots = robots;
            _robotToMoveIndex = robotToMoveIndex;
        }

        public Dictionary<ChargePointInfo, int> SearchFreeStationsInfo()
        {
            IList<EnergyStation> stations = _map.Stations;
            var stationCoverageMap = new Dictionary<ChargePointInfo, int>();
            foreach (var station in stations)
            {
                if (!IsStationFree(station))
                {
                    continue;
                }
                for (var chargePointX = station.Position.X - CollectionRange;
                     chargePointX < station.Position.X + CollectionRange; ++chargePointX)
                {
                    for (var chargePointY = station.Position.Y - CollectionRange;
                         chargePointY < station.Position.Y + CollectionRange; ++chargePointY)
                    {
                        Position chargePosition = new Position(chargePointX, chargePointY);
                        var chargePositionInfo = new ChargePointInfo(chargePosition, station,
                                DistanceHelper.FindDistance(chargePosition,_robots[_robotToMoveIndex].Position));
                        if (stationCoverageMap.ContainsKey(chargePositionInfo))
                        {
                            stationCoverageMap[chargePositionInfo]++;
                        }
                        else
                        {
                            stationCoverageMap[chargePositionInfo] = 1;
                        }
                    }
                }
            }
            return stationCoverageMap.OrderByDescending(kv => kv.Value)
                .ToDictionary(kv => kv.Key, kv => kv.Value);;
        }
        
        private bool IsStationFree(EnergyStation station)
        {
            foreach (var robot in _robots)
            {
                if(_robots.IndexOf(robot) == _robotToMoveIndex) continue;
                if (robot.Position.X >= station.Position.X - CollectionRange &&
                    robot.Position.X <= station.Position.X + CollectionRange &&
                    robot.Position.Y <= station.Position.Y + CollectionRange &&
                    robot.Position.Y >= station.Position.Y - CollectionRange)
                    return false;
            }
            return true;
        }
        
        public int SearchStationsCountInCollectRadius()
        {
            var count = 0;
            var currRobotPosition = _robots[_robotToMoveIndex].Position;
            foreach (var station in _map.Stations)
            {
                if(IsStationFree(station) &&
                   station.Position.X >= currRobotPosition.X - CollectionRange &&
                   station.Position.X <= currRobotPosition.X + CollectionRange &&
                   station.Position.Y >= currRobotPosition.Y - CollectionRange &&
                   station.Position.Y <= currRobotPosition.Y + CollectionRange)
                {
                    ++count;
                }
            }
            return count;
        }

        public Dictionary<Robot.Common.Robot, int> SearchDistanceToRobots()
        {
            var robotsDictionary = new Dictionary<Robot.Common.Robot, int>();
            foreach (var robot in _robots)
            {
                if (!string.Equals(robot.OwnerName, _robots[_robotToMoveIndex].OwnerName))
                {
                    robotsDictionary.Add(robot,DistanceHelper.FindDistance(robot.Position,
                        _robots[_robotToMoveIndex].Position));
                }
            }
            return robotsDictionary;
        }

        public int CountMyRobots()
        {
            int count = 0;
            foreach (var robot in _robots)
            {
                if (string.Equals(robot.OwnerName, _robots[_robotToMoveIndex].OwnerName))
                {
                    ++count;
                }
            }

            return count;
        }
    }
}
    