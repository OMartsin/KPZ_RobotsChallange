using Robot.Common;

namespace MartsinOleksandr.RobotsChallenge {
    public class ChargePointInfo {
        public Position Position { get; }

        public EnergyStation[] Stations { get; set; }
        public int Distance { get; }

        public ChargePointInfo(Position position, EnergyStation[] stations, int distance) {
            Position = position;
            Distance = distance;
            Stations = stations;
        }

        public override bool Equals(object obj) {
            var obJInfo = (ChargePointInfo)obj;
            return Equals(obJInfo);
        }

        private bool Equals(ChargePointInfo other) {
            return Equals(Position, other.Position);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (Position != null ? Position.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Distance;
                return hashCode;
            }
        }
    }
}