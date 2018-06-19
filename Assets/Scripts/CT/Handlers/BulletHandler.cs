using CT.Config;
using CT.Enums;
using CT.Objects;
using CT.Objects.Weapons;
using Log.GDebug;
using Tools.Delay;
using Zenject;

namespace CT.Handlers
{
    public class BulletHandler 
    {
        [Inject] private Bullet.Factory _bulletFactory;
        [Inject] private GameManager _gameManager;
        [Inject] private IDelayService _delayService;

        public void InitFire(BaseTank tank, Weapon weapon, LayerTags frendlyLayer)
        {
            if (null == weapon)
            {
                GDebug.LogWarning("Weapon parameters is null", this, LogCategory.FIRE_CONTROLLER);
                return;
            }

            var dl1 = 0f;
            var dl2 = 0f;

            for (int i = 0; i < weapon.MaxBulletInShot; i++)
            {
                _delayService.DelayedCall(dl1, () =>
                {
                    foreach (var point in weapon.GamePointsPool)
                    {
                        if (weapon.InMomentShot)
                            AddBullet(point, weapon, frendlyLayer);
                        else
                        { 
                            _delayService.DelayedCall(dl2, () => { AddBullet(point, weapon, frendlyLayer); });
                            dl2 += 0.1f;
                        }        
                    }
                });

                dl1 += 0.1f;
            }
        }

        private void AddBullet(GunPoints point, Weapon weapon, LayerTags frendlyLayer)
        {
            if (null == point.StartbulletPoint || null == point.FinishBulletPoint) return;

            var bl = _bulletFactory.Create();
            bl.initialize(point, weapon, frendlyLayer);
            bl.transform.SetParent(_gameManager.GamePresenter.transform);         
        }
    }
}