using CT.Config;
using CT.Enums;
using CT.Objects.Enemy;
using Log.GDebug;
using Zenject;

namespace CT.Factories
{
    public class EnemiesFactory
    {
        [Inject] private TankGreen.Factory _tankGreenFactory;
        [Inject] private TankEllo.Factory _tankElloFactory;
        [Inject] private TankBlue.Factory _tankBlueFactory;
        [Inject] private TankPurple.Factory _tankPurpleFactory;
        [Inject] private TankRed.Factory _tankRedFactory;

        public EnemyTank InjectEnemy(EnemyType type)
        {
            EnemyTank nEnemy = null;

            switch (type)
            {
                case EnemyType.TankGreen:
                    nEnemy = _tankGreenFactory.Create();
                    break;
                case EnemyType.TankEllo:
                    nEnemy = _tankElloFactory.Create();
                    break;
                case EnemyType.TankBlue:
                    nEnemy = _tankBlueFactory.Create();
                    break;
                case EnemyType.TankPurple:
                    nEnemy = _tankPurpleFactory.Create();
                    break;
                case EnemyType.TankRed:
                    nEnemy = _tankRedFactory.Create();
                    break;       
                default:
                    GDebug.LogError("Current Enemy type not found :" + type, this, LogCategory.ENEMY_FABRIC);
                    break;
            }

            return nEnemy;
        }
    }
}