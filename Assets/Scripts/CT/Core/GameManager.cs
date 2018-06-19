using System;
using CT.Config;
using CT.Core.Enums;
using CT.Core.Signals;
using CT.Core.State;
using CT.Gameplay;
using CT.Models;
using Log.GDebug;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace CT
{
    public class GameManager : IInitializable, IDisposable
    {
        [Inject] private IStateManager _stateManager;
        [Inject] private GameModel _gameModel;

        [Inject] private OnAppPausedSignal _appPausedSignal;
        [Inject] private OnAppFocusedSignal _appFocusedSignal;
        [Inject] private OnAppQuitSignal _onAppQuitSignal;

        [Inject] private OnGamestartSignal _onGamestartSignal;
        [Inject] private OnGameplaySignal _onGameplaySignal;
        [Inject] private OnGameoverSignal _onGameoverSignal;

        [Inject] private GamePresenter.Factory _gamePresenterFactory;

        private CompositeDisposable _disposables;

        private GamePresenter _gamePresenter;

        public GamePresenter GamePresenter
        {
            get { return _gamePresenter; }
            set { _gamePresenter = value; }
        }

        public void Initialize()
        {
            GDebug.Log("Application start", this, LogCategory.GAME_MANAGER);

            _gamePresenter = _gamePresenterFactory.Create();
            _gamePresenter.transform.position = Vector3.zero;
            _gamePresenter.transform.localScale = Vector3.one;

            _disposables = new CompositeDisposable();

            Subscriptions();

            _onGamestartSignal.Fire();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        #region Subscriptions

        private void Subscriptions()
        {
            // system signals

            Observable.EveryApplicationPause()
                .Subscribe(OnApplicationPause).AddTo(_disposables);

            Observable.EveryApplicationFocus()
                .Subscribe(OnApplicationFocus).AddTo(_disposables);

            Observable.OnceApplicationQuit()
                .Subscribe(OnApplicationQuit).AddTo(_disposables);

            // state signals

            _onGamestartSignal.AsObservable
                .SubscribeOn(Scheduler.ThreadPool)
                .Subscribe(_=> { OnChangeState(GameStateType.Gamestart); }).AddTo(_disposables);

            _onGameplaySignal.AsObservable
                .SubscribeOn(Scheduler.ThreadPool)
                .Subscribe(_ => { OnChangeState(GameStateType.Gameplay); }).AddTo(_disposables);

            _onGameoverSignal.AsObservable
                .SubscribeOn(Scheduler.ThreadPool)
                .Subscribe(_ => { OnChangeState(GameStateType.Gameover); }).AddTo(_disposables);
        }

        #endregion

        #region Handler Base App Signals

        private void OnApplicationPause(bool isActive)
        {
            _appPausedSignal.Fire(isActive);
        }

        private void OnApplicationFocus(bool isActive)
        {
            _appFocusedSignal.Fire(isActive);
        }

        private void OnApplicationQuit(Unit unit)
        {
            GDebug.Log("Application quit",this, LogCategory.GAME_MANAGER);

            _onAppQuitSignal.Fire();
        }

        #endregion

        #region Handler State App Signals

        private void OnChangeState(GameStateType currentState)
        {
            _stateManager.ChangeState(currentState);
        }

        #endregion

    }
}

