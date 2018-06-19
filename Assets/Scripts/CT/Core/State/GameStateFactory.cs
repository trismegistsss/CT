using CT.Core.Enums;
using ModestTree;
using Zenject;

namespace CT.Core.State
{
    public class GameStateFactory : IFactory<GameStateType, BaseState>
    {
        private readonly GamestartState.Factory _gamestartFactory;
        private readonly GameplayState.Factory _gameplayFactory;
        private readonly GameoverState.Factory _gameoverFactory;

        public GameStateFactory(GamestartState.Factory gamestartFactory,
                                GameplayState.Factory gameplayFactory,
                                GameoverState.Factory gameoverFactory)
        {
            _gamestartFactory = gamestartFactory;
            _gameplayFactory = gameplayFactory;
            _gameoverFactory = gameoverFactory;
        }

        public BaseState Create(GameStateType gameState)
        {
            switch (gameState)
            {
                case GameStateType.Gamestart:
                    return _gamestartFactory.Create();

                case GameStateType.Gameplay:
                    return _gameplayFactory.Create();

                case GameStateType.Gameover:
                    return _gameoverFactory.Create();

                default:
                    throw Assert.CreateException("Unknown state: {}".Fmt(gameState));            
            }
        }
    }
}