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

        public static ChargePointInfo FindMostProfitableCell(Dictionary<ChargePointInfo, int> stationCoverageMap, 
            int currChargeModifier)
        {
            if (stationCoverageMap == null || stationCoverageMap.Count == 0)
            {
                return null;
            }
            var freeCells = stationCoverageMap.Where(kv => 
                kv.Key.IsFree).ToList();
            if (freeCells.Count == 0)
            {
                return null;
            }
            ChargePointInfo mostProfitableCell = null;
            foreach (var cell in freeCells)
            {
                if (currChargeModifier < cell.Value)
                {
                    mostProfitableCell = cell.Key;
                }
                // int maxDistance = 30 * (cell.Value - currChargeModifier);
                // if (cell.Key.Distance <= maxDistance)
                // {
                    
                //}
            }
            return mostProfitableCell;
        }
    }
}