using System;
using System.Collections.Generic;
using MartsinOleksandr.RobotsChallenge;
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
        var deltaX = end.X - current.X;
        var deltaY = end.Y - current.Y;
        if (Math.Abs(deltaX) + Math.Abs(deltaY) >= remainingEnergy)
        {
            return null;
        }
        double stepEnergy = deltaX * deltaX + deltaY * deltaY;
        var steps = (int)((stepEnergy / remainingEnergy ));
        var stepX = deltaX / steps;
        var stepY = deltaY / steps;
        if (stepX != current.X || stepY != current.Y || distance == 0)
            return new Position(current.X + stepX, current.Y + stepY);
        if (deltaX != 0)
        {
            stepX = Math.Sign(deltaX) * 1;
        }
        if (deltaY != 0)
        {
            stepY = Math.Sign(deltaY) * 1;
        }
        return new Position(current.X + stepX, current.Y + stepY);
    }
    
    public static Position OptimalStepsSplitter(Position start, Position end)
    {
        Position current = start;
        var deltaX = end.X - current.X;
        var deltaY = end.Y - current.Y;
        var maxSteps = 4;
        var maxXDistance = Math.Abs(deltaX) > maxSteps ? maxSteps * Math.Sign(deltaX) : deltaX;
        var maxYDistance = Math.Abs(deltaY) > maxSteps ? maxSteps * Math.Sign(deltaY) : deltaY;
        return new Position(current.X + maxXDistance, current.Y + maxYDistance);
    }
    
    public static int CalculateEnergyForOptimalStepsSplitter(Position start, Position end)
    {
        var deltaX = end.X - start.X;
        var deltaY = end.Y - start.Y;
        var maxSteps = 3;
        var totalEnergy = 0;

        while (deltaX != 0 || deltaY != 0)
        {
            var maxXDistance = Math.Abs(deltaX) > maxSteps ? maxSteps * Math.Sign(deltaX) : deltaX;
            var maxYDistance = Math.Abs(deltaY) > maxSteps ? maxSteps * Math.Sign(deltaY) : deltaY;
            totalEnergy += (int)(Math.Sqrt(maxXDistance * maxXDistance + maxYDistance * maxYDistance));

            deltaX -= maxXDistance;
            deltaY -= maxYDistance;
        }

        return totalEnergy;
    }
    
    public static int CalculateStepsForOptimalStepsSplitter(Position start, Position end)
    {
        var deltaX = end.X - start.X;
        var deltaY = end.Y - start.Y;
        var maxSteps = 3;
        var steps = 0;

        while (deltaX != 0 || deltaY != 0)
        {
            var maxXDistance = Math.Abs(deltaX) > maxSteps ? maxSteps * Math.Sign(deltaX) : deltaX;
            var maxYDistance = Math.Abs(deltaY) > maxSteps ? maxSteps * Math.Sign(deltaY) : deltaY;
            ++steps;

            deltaX -= maxXDistance;
            deltaY -= maxYDistance;
        }

        return steps;
    }
}