using CT.Config;
using CT.Core.Enums;
using Log.GDebug;
using UniRx;
using Zenject;

namespace CT.Core.State
{
    public class GameStateManager :IStateManager, ITickable
    {
        [Inject]
        private IFactory<GameStateType, BaseState> _gameStateFactory;

        private GameStateType _previousState;
        private GameStateType _currentState;
        private BaseState _currentStateObject = null;

        public ReactiveProperty<GameStateType> CurrentState { get; private set; }

        [Inject]
        public GameStateManager()
        {
            CurrentState = new ReactiveProperty<GameStateType>();
        }

        public void Tick()
        {
            if (null != _currentStateObject)
                _currentStateObject.Tick();
        }

        public void ChangeState(GameStateType gameState)
        {
            if (null!=_currentStateObject)
            {
                _currentStateObject.Dispose();
                _currentStateObject = null;
            }

            _previousState = _currentState;
            _currentState = gameState;
            CurrentState.Value = _currentState;

            _currentStateObject = _gameStateFactory.Create(gameState);
            _currentStateObject.Handle();

            GDebug.Log("Change state to " + gameState, this, LogCategory.STATE_MACHINE);
        }


       
    }
}
