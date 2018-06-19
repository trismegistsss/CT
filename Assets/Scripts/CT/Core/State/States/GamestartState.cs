using System;
using CT.Config;
using CT.Core.Controllers;
using Log.GDebug;
using Zenject;

namespace CT.Core.State
{
    public class GamestartState : BaseState
    {
        [Inject]
        private readonly GamestartController.Factory _factory;
        private GamestartController _controller;
        
        public override void Initialize()
        {
            GDebug.Log("GamestartState initialize", this, LogCategory.STATE_MACHINE);
        }

        public override void Handle()
        {
            GDebug.Log("GamestartState Start", this, LogCategory.STATE_MACHINE);

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

        public class Factory : Factory<GamestartState> { }
    }
}
