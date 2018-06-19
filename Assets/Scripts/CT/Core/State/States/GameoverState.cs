using System;
using CT.Config;
using CT.Core.Controllers;
using Log.GDebug;
using Zenject;

namespace CT.Core.State
{
    public class GameoverState : BaseState
    {
        [Inject]
        private readonly GameoverController.Factory _factory;
        private GameoverController _controller;

        public override void Initialize()
        {
            GDebug.Log("Big SphericalTribune initialize", this, LogCategory.STATE_MACHINE);
        }

        public override void Handle()
        {
            GDebug.Log("Big SphericalTribune Start", this, LogCategory.STATE_MACHINE);

            _controller = _factory.Create();
            _controller.Initialization();
        }

        public override void Tick()
        {
            if (null == _controller) return;
            _controller.Tick();
        }

        public override void Dispose()
        {
            if (null == _controller) return;
            _controller.Dispose();
            _controller = null;
        }


        [Serializable]
        public class Settings
        {

        }

        public class Factory : Factory<GameoverState>
        {
            
        }
    }
}
