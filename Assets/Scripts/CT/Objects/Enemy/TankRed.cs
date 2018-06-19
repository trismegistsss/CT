using Zenject;
namespace CT.Objects.Enemy
{
    public class TankRed : EnemyTank
    {
        public class Factory : Factory<TankRed> { }
    }
}