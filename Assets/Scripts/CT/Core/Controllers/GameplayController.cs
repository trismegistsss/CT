using System;
using System.Collections.Generic;
using System.Linq;
using CT.Config;
using CT.Core.Signals;
using CT.Enums;
using CT.Handlers;
using CT.Models;
using CT.Utils;
using Log.GDebug;
using SM.Sound;
using Tools.Delay;
using UniRx;
using UnityEngine;
using Zenject;

namespace CT.Core.Controllers
{
    public class GameplayController : BaseGamplayController
    {
        [Inject] private ISoundManager _soundManager;
        [Inject] private IDelayService _delayService;
        [Inject] private GameModel _gameModel;
        [Inject] private BulletHandler _bulletHandler;
        [Inject] private EnemyHandler _enemyHandler;
        [Inject] private UserHandler _userHandler;
        [Inject] private GameItemsInfo _gameItemsInfo;

        [Inject] private OnRespaunUserSignal _onRespaunUser;
        [Inject] private OnRespaunEnemySignal _onRespaunEnemy;
        [Inject] private OnGameoverSignal _onGameoverSignal;

        private int _weaponIndex;
        private bool _isBlockClick;
        private int _levelScorePoint;

        public override void Dispose()
        {
            _weaponIndex = 0;

            base.Dispose();
        }

        public override void Initialization()
        {
            base.Initialization();
            GDebug.Log("GAME PLAY INITIALIZE", this, LogCategory.STATE_CONTROLLERS);

            ResetGameplayState();
            Subscriptions();

            SpawnUser();
            SpawnEnemies();

            _soundManager.PlayMusic(SoundDataConfig.GAME_MUSIC);
        }

        private void ResetGameplayState()
        {
            _levelScorePoint = GameConfig.SCORE_TO_NEXT_LEVEL;
            _gameModel.Score.Value = 0;
            _gameModel.Level.Value = GameConfig.START_LEVEL;
            _gameModel.Live.Value = GameConfig.START_LIVE;

            _enemyHandler.DestroyAllEnemies();
            _userHandler.DestroyUser();
        }

        private void Subscriptions()
        {
            _onRespaunUser.AsObservable.SubscribeOn(Scheduler.ThreadPool)
                .Subscribe((pr) =>
                {
                    if (null == pr) return;

                    _gameModel.Live.Value -= 1;

                    _userHandler.DestroyUser();

                    if (_gameModel.Live.Value <= 0)
                    {
                        _soundManager.FadeMusic(0.0f, 1f);
                        _delayService.DelayedCall(1, () => { _onGameoverSignal.Fire(); });
                    }
                    else
                    {
                        _soundManager.PlaySFX(SoundDataConfig.DESTROY);
                        _delayService.DelayedCall(1, () => { SpawnUser(); });
                    }

                }).AddTo(Disposables);

            _onRespaunEnemy.AsObservable.SubscribeOn(Scheduler.ThreadPool)
                .Subscribe((pr) =>
                {
                    if (null == pr) return;

                    _gameModel.Score.Value += pr.GetBonus;
                    _enemyHandler.DestroyEnemy(pr);

                    if (_levelScorePoint < _gameModel.Score.Value)
                    {
                        _levelScorePoint += GameConfig.SCORE_TO_NEXT_LEVEL;
                        _gameModel.Level.Value += 1;
                    }

                    _soundManager.PlaySFX(SoundDataConfig.DESTROY);

                    SpawnEnemies();

                }).AddTo(Disposables);
        }

        public override void Tick()
        {
            WeaponControll();
            FireUserControll();
        }

        #region Spawn Tanks

        private void SpawnUser()
        {
           _userHandler.InitUser();
            _weaponIndex = 0;
            ChangeWeapon(1);
        }

        private void SpawnEnemies()
        {
            _enemyHandler.InitEnemies();
        }

        #endregion

        #region User Fire Tank

        public void FireUserControll()
        {
            if (null != _gameItemsInfo.UserTank && !_isBlockClick)
            {
                if (Input.GetButton("Fire"))
                {
                    _bulletHandler.InitFire(_gameItemsInfo.UserTank, _gameItemsInfo.UserTank.GetCurrentWeapon, LayerTags.User);
                    _isBlockClick = true;
                    _delayService.DelayedCall(0.5f, () => { _isBlockClick = false; });

                }
            }
        }

        #endregion

        #region User Weapon Change

        public void WeaponControll()
        {
            if (null != _gameItemsInfo.UserTank && !_isBlockClick)
            {
                if (Input.GetAxis("Weapon") > 0)
                    ChangeWeapon(1);
                else if(Input.GetAxis("Weapon") < 0)
                    ChangeWeapon(-1);
            }
        }

        private void ChangeWeapon(int direction)
        { 
            var wlist = EnumUtils.EnumToList<WeaponType>();
            var cIndex = _weaponIndex + direction;

            if (cIndex >= wlist.Count)
            {
                cIndex = wlist.Count - 1;
                return;
            }
            else if (cIndex < 1)
            {
                cIndex = 1;
                return;
            }

            _isBlockClick = true;

            if (CheckApruveWeapon(wlist[cIndex]))
            {
                _weaponIndex = cIndex;
                _gameModel.Weapons.Value = wlist[cIndex];

                _delayService.DelayedCall(0.5f, () => { _isBlockClick = false; });

                _soundManager.PlaySFX(SoundDataConfig.CHANGE_WEAPON);

                return;
            }

            _isBlockClick = false;
        }

        private bool CheckApruveWeapon(WeaponType check)
        {
            if (null == _gameItemsInfo.UserTank) return false;

            var aw = _gameItemsInfo.UserTank.GetAvailableWeapons;
            return aw.Any(pr => pr == check);
        }

        #endregion

        public class Factory : Factory<GameplayController> { }
    }
}
