using System;
using System.Collections.Generic;
using MartsinOleksandr.RobotsChallange;
using Robot.Common;

public class RouteSplitter
{
    public static Position SplitRoute(Position start, Position end, int remainingEnergy)
    {
        Position current = start;        
        var distance = DistanceHelper.FindDistance(current, end);
        if (distance <= remainingEnergy)
        {
            return end;
        }
        else
        {
            int dx = end.X - current.X;
            int dy = end.Y - current.Y;
            int maxDistance = (int)Math.Sqrt(remainingEnergy);
            int nextX = current.X + (int)Math.Round(dx * (maxDistance / (double)distance));
            int nextY = current.Y + (int)Math.Round(dy * (maxDistance / (double)distance));
            Position nextPosition = new Position(nextX, nextY);
            return nextPosition;
        }
    }
}