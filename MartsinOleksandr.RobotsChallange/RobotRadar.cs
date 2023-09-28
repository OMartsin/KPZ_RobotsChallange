using System.Collections.Generic;
using System.Linq;
using Robot.Common;

namespace MartsinOleksandr.RobotsChallenge {
    public class RobotRadar {
        private const int CollectionRange = 2;

        private readonly Map _map;
        private readonly IList<Robot.Common.Robot> _robots;
        private readonly int _robotToMoveIndex;

        public RobotRadar(Map map, IList<Robot.Common.Robot> robots, int robotToMoveIndex) {
            _map = map;
            _robots = robots;
            _robotToMoveIndex = robotToMoveIndex;
        }

        public List<ChargePointInfo> SearchStationsInfo() {
            IList<EnergyStation> stations = _map.Stations;
            var stationCoverageMap = new List<ChargePointInfo>();
            foreach (var station in stations) {
                if (station.Energy == 0) continue;
                if(IsMyRobotOnStation(station)) continue;
                for (var chargePointX = station.Position.X - CollectionRange;
                     chargePointX < station.Position.X + CollectionRange;
                     ++chargePointX) {
                    for (var chargePointY = station.Position.Y - CollectionRange;
                         chargePointY < station.Position.Y + CollectionRange;
                         ++chargePointY) {
                        var targetPosition = new Position(chargePointX, chargePointY);
                        ChargePointInfo existingElement =
                            stationCoverageMap.FirstOrDefault(info => info.Position == targetPosition);
                        if (existingElement != null) {
                            existingElement.Stations = existingElement.Stations.Concat(new[] { station }).ToArray();
                        }
                        else {
                            var newElement = new ChargePointInfo(targetPosition, new[] { station },
                                RouteSplitter.CalculateEnergyForOptimalStepsSplitter
                                    (_robots[_robotToMoveIndex].Position, station.Position));
                            stationCoverageMap.Add(newElement);
                        }
                    }
                }
            }
            return stationCoverageMap;
        }

        private bool IsMyRobotOnStation(EnergyStation station) {
            foreach (var robot in _robots) {
                if (robot.Position.X < station.Position.X + CollectionRange &&
                    robot.Position.X > station.Position.X - CollectionRange &&
                    robot.Position.Y < station.Position.Y + CollectionRange &&
                    robot.Position.Y > station.Position.Y - CollectionRange &&
                    string.Equals(robot.OwnerName, _robots[_robotToMoveIndex].OwnerName)) {
                    return true;
                }
            }
            return false;
        }

        public ChargePointInfo SearchStationsInCollectRadius() {
            ChargePointInfo posInfo = null;
            var currRobotPosition = _robots[_robotToMoveIndex].Position;
            foreach (var station in _map.Stations) {
                if (station.Energy != 0 &&
                    station.Position.X >= currRobotPosition.X - CollectionRange &&
                    station.Position.X <= currRobotPosition.X + CollectionRange &&
                    station.Position.Y >= currRobotPosition.Y - CollectionRange &&
                    station.Position.Y <= currRobotPosition.Y + CollectionRange) {
                    if (posInfo == null)
                        posInfo
                            = new ChargePointInfo(currRobotPosition, new[] { station }, 0);
                    else
                        posInfo.Stations
                            = posInfo.Stations.Concat(new[] { station }).ToArray();
                }
            }

            return posInfo;
        }

        public Dictionary<Robot.Common.Robot, int> SearchDistanceToRobots() {
            var robotsDictionary = new Dictionary<Robot.Common.Robot, int>();
            foreach (var robot in _robots) {
                if (!string.Equals(robot.OwnerName, _robots[_robotToMoveIndex].OwnerName)) {
                    robotsDictionary.Add(robot, DistanceHelper.FindDistance(robot.Position,
                        _robots[_robotToMoveIndex].Position));
                }
            }

            return robotsDictionary;
        }

        public int CountMyRobots() {
            int count = 1;
            foreach (var robot in _robots) {
                if (string.Equals(robot.OwnerName, _robots[_robotToMoveIndex].OwnerName)) {
                    ++count;
                }
            }
            return count;
        }
    }
}