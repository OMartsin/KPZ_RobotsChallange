using System.Collections.Generic;
using MartsinOleksandr.RobotsChallange.Properties;

namespace MartsinOleksandr.RobotsChallange
{
    public class RobotChargePossibilityChecker
    {
        public static int CheckRobotEnergyStations(Dictionary<ChargePointInfo, int> stationCoverageMap, 
            Robot.Common.Robot robot)
        {
            foreach (var kvp in stationCoverageMap)
            {
                var chargePointInfo = kvp.Key;
                if (chargePointInfo.Position == robot.Position)
                {
                    return kvp.Value;
                }
            }
            return 0;
        }
    }
}