using CT.Config;
using CT.Handlers;
using CT.Models;
using Log.GDebug;
using SM.Sound;
using UnityEngine;
using Zenject;

namespace CT.Core.Controllers
{
    public class GamestartController : BaseGamplayController
    {
        [Inject] private ISoundManager _soundManager;

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Initialization()
        {
            base.Initialization();
            GDebug.Log("GAME START INITIALIZE", this, LogCategory.STATE_CONTROLLERS);

             _soundManager.PlayMusic(SoundDataConfig.INTRO_MUSIC);
        }

        public class Factory : Factory<GamestartController> { }
    }
}