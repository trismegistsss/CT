using System;
using CT.Config;
using CT.Models;
using CT.Objects;
using SM.Sound;
using UniRx;
using UnityEngine;
using Zenject;

namespace CT.Handlers
{
    public class UserHandler : IInitializable, IDisposable
    {
        [Inject] private UserTank.Factory _userTankFactory;
        [Inject] private GameManager _gameManager;
        [Inject] private GameItemsInfo _gameItemsInfo;
        [Inject] private ISoundManager _soundManager;

        CompositeDisposable _disposables = new CompositeDisposable();

        public void Dispose()
        {
            if (_disposables != null)
                _disposables.Dispose();
        }

        public void DestroyUser()
        {
            _gameItemsInfo.DestroyUserTank();
        }

        public void Initialize()
        {
            
        }

        public void InitUser()
        {
            UserTank UserTank = _userTankFactory.Create();
            UserTank.Id = 0;
            UserTank.transform.SetParent(_gameManager.GamePresenter.transform);
            UserTank.transform.localPosition = GameConfig.DEFAULT_GAMEOBJECTS_POSITION;
            UserTank.transform.localScale = Vector3.one;

            _gameItemsInfo.AddUserTank(UserTank);
        }
    }
}