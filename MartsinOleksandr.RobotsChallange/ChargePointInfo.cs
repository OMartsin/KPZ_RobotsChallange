using Robot.Common;

namespace MartsinOleksandr.RobotsChallange.Properties
{
    public class ChargePointInfo
    {
        public Position Position { get; }
        
        public EnergyStation Station { get;  }
        public int Distance { get; }

        public ChargePointInfo(Position position, EnergyStation station, int distance)
        {
            Position = position;
            Distance = distance;
            Station = station;
        }

        public override bool Equals(object obj)
        {
            var obJInfo = (ChargePointInfo)obj;
            return this.Equals(obJInfo);
        }

        protected bool Equals(ChargePointInfo other)
        {
            return Equals(Position, other.Position);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Position != null ? Position.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Distance;
                return hashCode;
            }
        }
    }
}