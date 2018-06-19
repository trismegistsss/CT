using CT.Core.Enums;
using UniRx;

namespace CT.Core.State
{
    public interface IStateManager
    {
        ReactiveProperty<GameStateType> CurrentState { get; }
        void ChangeState(GameStateType stateType);
    }
}
