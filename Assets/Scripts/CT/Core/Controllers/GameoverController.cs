using CT.Config;
using CT.Handlers;
using Log.GDebug;
using SM.Sound;
using Zenject;

namespace CT.Core.Controllers
{
    public class GameoverController : BaseGamplayController
    {
        [Inject] private ISoundManager _soundManager;
        [Inject] private EnemyHandler _enemyHandler;
        [Inject] private UserHandler _userHandler;

        public override void Dispose()
        {
            ResetGameItems();
            base.Dispose();
        }

        public override void Initialization()
        {
            base.Initialization();
            GDebug.Log("GAME OVER INITIALIZE", this, LogCategory.STATE_CONTROLLERS);

            _soundManager.PlaySFX(SoundDataConfig.GAMEOVER);
        }


        private void ResetGameItems()
        {
            _enemyHandler.DestroyAllEnemies();
            _userHandler.DestroyUser();
        }

        public class Factory : Factory<GameoverController> { }
    }
}
