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

        public Dictionary<ChargePointInfo, int> SearchStationsInfo()
        {
            IList<EnergyStation> stationsByRadius = _map.Stations;
            var stationCoverageMap = new Dictionary<ChargePointInfo, int>();
            foreach (var inRadiusStation in stationsByRadius)
            {
                for (var chargePointX = inRadiusStation.Position.X - CollectionRange;
                     chargePointX < inRadiusStation.Position.X + CollectionRange; ++chargePointX)
                {
                    for (var chargePointY = inRadiusStation.Position.Y - CollectionRange;
                         chargePointY < inRadiusStation.Position.Y + CollectionRange; ++chargePointY)
                    {
                        Position chargePosition = new Position(chargePointX, chargePointY);
                        var chargePositionInfo = new ChargePointInfo(chargePosition, IsCellFree(chargePosition), 
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

        // public IList<EnergyStation> GetStationsByRadius(int radius)
        // {
        //     var centrePosition = _robots[_robotToMoveIndex].Position;
        //     var maxX = centrePosition.X + radius;
        //     var minX = centrePosition.X - radius;
        //     var maxY = centrePosition.Y + radius;
        //     var minY = centrePosition.Y - radius;
        //     var stationsByRadius = new List<EnergyStation>();
        //     foreach (var station in _map.Stations)
        //     {
        //         if ((station.Position.X >= minX && station.Position.X <= maxX) &&
        //             (station.Position.Y >= minY && station.Position.Y <= maxY))
        //         {
        //             stationsByRadius.Add(station);
        //         }
        //     }
        //     return stationsByRadius;
        // }
        public bool IsCellFree(Position cell)
        {
            foreach (var robot in _robots)
            {
                if (robot.Position == cell)
                    return false;
            }
            return true;
        }

        public Dictionary<Robot.Common.Robot, int> SearchDistanceToRobots()
        {
            var robotsDictionary = new Dictionary<Robot.Common.Robot, int>();
            foreach (var robot in _robots)
            {
                robotsDictionary.Add(robot,DistanceHelper.FindDistance(robot.Position,
                    _robots[_robotToMoveIndex].Position));
            }
            return robotsDictionary;
        }
    }
}
    