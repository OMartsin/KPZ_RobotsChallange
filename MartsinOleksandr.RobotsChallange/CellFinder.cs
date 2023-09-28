using System;
using System.Collections.Generic;
using System.Linq;
using Robot.Common;

namespace MartsinOleksandr.RobotsChallenge {
    public class CellFinder {
        public static ChargePointInfo FindNearestCell(List<ChargePointInfo> stationCoverageMap) {
            if (stationCoverageMap == null || stationCoverageMap.Count == 0) {
                return null;
            }
            ChargePointInfo nearestCell = stationCoverageMap
                .OrderBy(cp => cp.Distance)
                .First();
            return nearestCell;
        }

        public static (Position, int)? FindMostProfitableCell
            (List<ChargePointInfo> stationCoverageMap, ChargePointInfo currPosition, int robotEnergy) {
            if (stationCoverageMap == null || stationCoverageMap.Count == 0) {
                return null;
            }
            Position mostProfitableCell = null;
            var profit = Int32.MinValue;
            foreach (var cell in stationCoverageMap) {
                var distance = cell.Distance;
                var steps = RouteSplitter.CalculateStepsForOptimalStepsSplitter
                    (currPosition.Position, cell.Position);
                if (distance > robotEnergy) continue;
                if (distance == 0) continue;
                var tempProfit = (cell.Stations.Sum(station => station.Energy) -
                                  distance) - (steps * 60
                                               + currPosition.Stations.Sum(station => station.Energy));
                if (tempProfit <= profit) continue;
                profit = tempProfit;
                mostProfitableCell = cell.Position;
            }
            return (mostProfitableCell, profit);
        }
    }
}