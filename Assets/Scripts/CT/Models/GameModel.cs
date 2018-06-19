using System.Collections.Generic;
using CT.Config;
using CT.Core.Signals;
using CT.Enums;
using UniRx;
using Zenject;

namespace CT.Models
{
    public class GameModel : IReactiveWrapper<GameInfo>
    {   
        // Dynamic UI

        public ReactiveProperty<int> Score { get; private set; }
        public ReactiveProperty<int> Live { get; private set; }
        public ReactiveProperty<int> Level { get; private set; }
        public ReactiveProperty<WeaponType> Weapons { get; private set; }

        [Inject]
        public GameModel(OnModelUpdateSignal modelUpdate)
        {
            Score = new ReactiveProperty<int>();
            Live = new ReactiveProperty<int>(GameConfig.START_LIVE);
            Level = new ReactiveProperty<int>(GameConfig.START_LEVEL);
            Weapons = new ReactiveProperty<WeaponType>();

            modelUpdate.AsObservable
                .SubscribeOn(Scheduler.ThreadPool)
                .Subscribe(Wrap);
        }

        public void Wrap(GameInfo model)
        {
            
        }

        public GameInfo Unwrap()
        {

            return null;
        }
    }
}
