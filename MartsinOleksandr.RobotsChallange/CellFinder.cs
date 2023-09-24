using System;
using System.Collections.Generic;
using System.Linq;
using MartsinOleksandr.RobotsChallange.Properties;
using Robot.Common;

namespace MartsinOleksandr.RobotsChallange
{
    public class CellFinder
    {
        public static ChargePointInfo FindNearestCell(Dictionary<ChargePointInfo, int> stationCoverageMap)
        {
            if (stationCoverageMap == null || stationCoverageMap.Count == 0)
            {
                return null;
            }
            ChargePointInfo nearestCell = stationCoverageMap.Keys
                .OrderBy(cp => cp.Distance)
                .First();
            return nearestCell;
        }

        public static ChargePointInfo FindMostProfitableCellByStationsInCellCollect
        (Dictionary<ChargePointInfo, int> stationCoverageMap, int currChargeModifier)
        {
            if (stationCoverageMap == null || stationCoverageMap.Count == 0)
            {
                return null;
            }
            ChargePointInfo mostProfitableCell = null;
            var tCurrChargeModifier = currChargeModifier;
            foreach (var cell in stationCoverageMap)
            {
                if (tCurrChargeModifier < cell.Value)
                {
                    tCurrChargeModifier = cell.Value;
                    mostProfitableCell = cell.Key;
                }
                if (mostProfitableCell != null && cell.Key.Distance > 0 &&  
                    mostProfitableCell.Distance > cell.Key.Distance && tCurrChargeModifier == cell.Value)
                {
                    mostProfitableCell = cell.Key;
                    tCurrChargeModifier = cell.Value;
                }
            }
            return mostProfitableCell;
        }

        public static ChargePointInfo FindMostProfitableCellByEnergyCollect(IList<EnergyStation> stations, 
            Robot.Common.Robot robot)
        {
            ChargePointInfo mostProfitableCell = null;
            int minDistance = int.MaxValue;
            int maxEnergy = int.MinValue;
            foreach (var station in stations)
            {
                int distance = DistanceHelper.FindDistance(station.Position, robot.Position);
                if (station.Energy > maxEnergy || (station.Energy >= maxEnergy - 20 && distance < minDistance))
                {
                    maxEnergy = station.Energy;
                    minDistance = distance;
                    mostProfitableCell = new ChargePointInfo(station.Position, station, distance);
                }
            }
            return mostProfitableCell;
        }
    }
}