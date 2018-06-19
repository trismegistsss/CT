using Zenject;

namespace CT.Objects.Enemy
{
    public class TankEllo : EnemyTank
    {
        public class Factory : Factory<TankEllo> { }
    }
}
