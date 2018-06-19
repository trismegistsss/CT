using Zenject;

namespace CT.Objects.Enemy
{
    public class TankGreen : EnemyTank
    {
        public class Factory : Factory<TankGreen> { }
    }
}
