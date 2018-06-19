using Zenject;

namespace CT.Objects.Enemy
{
    public class TankPurple : EnemyTank
    {
        public class Factory : Factory<TankPurple> { }
    }
}
