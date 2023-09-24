using System.Collections.Generic;
using NUnit.Framework;
using Robot.Common;
using Assert = NUnit.Framework.Assert;

namespace MartsinOleksandr.RobotsChallange.Test
{
    [TestFixture]
    public class TestAlgorithm
    {
        [Test]
        public void TestNearbyStationCommand()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var stationPosition = new Position(4, 4);
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = stationPosition, RecoveryRate = 2 });
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 200, Position = new Position(2, 3) }
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsTrue(command is CollectEnergyCommand);
        }
        
        [Test]
        public void TestNearbyOccupiedByYouStationCommand()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var stationPosition = new Position(4, 4);
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = stationPosition, RecoveryRate = 2 });
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 200, Position = new Position(3, 2) },
                new Robot.Common.Robot() { Energy = 200, Position = new Position(4, 4) }
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsTrue(command is CollectEnergyCommand);
        }
        
        
        [Test]
        public void TestNearbyOccupiedByAnotherStationCommand()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var stationPosition = new Position(4, 4);
            map.Stations.Add(new EnergyStation() { Energy = 0, Position = stationPosition, RecoveryRate = 2 });
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 200, Position = new Position(3, 2) },
                new Robot.Common.Robot() { Energy = 200, Position = new Position(4, 4) }
            };
            var command = algorithm.DoStep(robots, 1, map);
            Assert.IsTrue(command is CollectEnergyCommand);
        }

        [Test]
        public void TestMoveCommand_NoStation()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            // No energy stations
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 200, Position = new Position(2, 3) }
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsNull(command); // Expect no command (stay idle) with no stations.
        }
        
        [Test]
        public void TestFarAwayStationCommand()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var stationPosition = new Position(52, 3);
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = stationPosition, RecoveryRate = 2 });
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 75, Position = new Position(2, 3) }
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsTrue(command is MoveCommand);
        }

        [Test]
        public void TestMoveCommand_MultipleStations()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var station1Position = new Position(1, 2);
            var station2Position = new Position(100, 4);
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = station1Position});
            map.Stations.Add(new EnergyStation() { Energy = 500, Position = station2Position });
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 200, Position = new Position(2, 3) }
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsTrue(command is CollectEnergyCommand); // Expect MoveCommand with multiple stations.
        }

        [Test]
        public void TestAuthor()
        {
            Assert.AreEqual("Martsin Oleksandr", new MartsinAlgorithm().Author);
        }
        
        [Test]
        public void TestMoveCommand_MultipleStationsWithOccupiedPlace()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var station1Position = new Position(10, 10);
            var station2Position = new Position(10, 4);
            map.Stations.Add(new EnergyStation() { Energy = 3000, Position = station1Position});
            map.Stations.Add(new EnergyStation() { Energy = 30, Position = station2Position });
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 200, Position = new Position(10, 4), OwnerName = "Alex"},
                new Robot.Common.Robot() { Energy = 200, Position = new Position(10, 5), OwnerName = "Alex2"}
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsTrue(command is MoveCommand); 
        }

        [Test]
        public void TestMoveCommand_MultipleStationsWithMoreProfitableCell()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var station1Position = new Position(1, 2);
            var station2Position = new Position(5, 4);
            var station3Position = new Position(5, 6);
            map.Stations.Add(new EnergyStation() { Energy = 30, Position = station1Position});
            map.Stations.Add(new EnergyStation() { Energy = 39, Position = station2Position });
            map.Stations.Add( new EnergyStation(){ Energy = 46, Position = station3Position});
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 200, Position = new Position(2, 3) }
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsTrue(command is MoveCommand); // Expect MoveCommand with multiple stations.
        }
        [Test]
        public void TestMoveCommand_LowEnergy()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var stationPosition = new Position(10, 2);
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = stationPosition, RecoveryRate = 2 });
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 20, Position = new Position(2, 3) } // Low energy robot
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsTrue(command is MoveCommand);
        }
        
        [Test]
        public void TestMoveCommand_LowEnergyNearStation()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var stationPosition = new Position(6, 2);
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = stationPosition, RecoveryRate = 2 });
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 20, Position = new Position(2, 3) } // Low energy robot
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsTrue(command is MoveCommand);
        }

        [Test]
        public void TestCreateNewRobotCommand()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var stationPosition = new Position(100, 2);
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = stationPosition, RecoveryRate = 2 });
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 500, Position = new Position(2, 3) } // Low energy robot
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsTrue(command is CreateNewRobotCommand);
        }
        [Test]
        public void TestAttackAnotherOwnerRobotCommand()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 200, Position = new Position(2, 3) , OwnerName = "Alex"} ,
                new Robot.Common.Robot() { Energy = 1000, Position = new Position(5, 3) , OwnerName = "Another owner"} 
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsTrue(command is MoveCommand);
        }
        
        [Test]
        public void TestAttackMyRobotCommand()
        {
            var algorithm = new MartsinAlgorithm();
            var map = new Map();
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() { Energy = 200, Position = new Position(2, 3) , OwnerName = "Alex"} ,
                new Robot.Common.Robot() { Energy = 200, Position = new Position(5, 3) , OwnerName = "Alex"} 
            };
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsNull(command);
        }
        
    }
}