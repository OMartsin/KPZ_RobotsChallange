using Robot.Common;

namespace MartsinOleksandr.RobotsChallange.Properties
{
    public class ChargePointInfo
    {
        public Position Position { get; }

        public bool IsFree { get; }

        public int Distance { get; }

        public ChargePointInfo(Position position, bool isFree, int distance)
        {
            Position = position;
            IsFree = isFree;
            Distance = distance;
        }

        public override bool Equals(object obj)
        {
            var obJInfo = (ChargePointInfo)obj;
            return this.Equals(obJInfo);
        }

        protected bool Equals(ChargePointInfo other)
        {
            return Equals(Position, other.Position) && IsFree == other.IsFree && Distance == other.Distance;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Position != null ? Position.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsFree.GetHashCode();
                hashCode = (hashCode * 397) ^ Distance;
                return hashCode;
            }
        }
    }
}