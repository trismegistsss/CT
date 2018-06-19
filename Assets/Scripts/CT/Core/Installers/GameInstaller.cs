using System;
using CT.Controllers;
using CT.Core.Controllers;
using CT.Core.Enums;
using CT.Core.Signals;
using CT.Core.State;
using CT.Factories;
using CT.Objects;
using CT.Gameplay;
using CT.Handlers;
using CT.Models;
using CT.Objects.Enemy;
using CT.Objects.Weapons;
using SM.Sound;
using Tools.Delay;
using UnityEngine;
using Zenject;

namespace CT.Core.Installer
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [Inject] private SystemModules _systemModules;
        [Inject] private GamePlayItems _gamePlayItems;
        [Inject] private ParticlesItems _particlesItems;

        public override void InstallBindings()
        {
            InstallCore();
            InstallSignals();
            InstallsSystems();
            InstallObjects();
        }

        #region Installs

        private void InstallCore()
        {   
            Container.BindInterfacesAndSelfTo<GameModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameInfo>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameItemsInfo>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<DelayService>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamestartState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameoverState>().AsSingle();

            Container.BindInterfacesAndSelfTo<BulletHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<UserHandler>().AsSingle();

            Container.Bind<WeaponsFactory>().AsSingle();
            Container.Bind<EnemiesFactory>().AsSingle();

            Container.Bind<IFactory<GameStateType, BaseState>>().To<GameStateFactory>().AsSingle();
            Container.BindFactory<GamestartState, GamestartState.Factory>().WhenInjectedInto<GameStateFactory>();
            Container.BindFactory<GameplayState, GameplayState.Factory>().WhenInjectedInto<GameStateFactory>();
            Container.BindFactory<GameoverState, GameoverState.Factory>().WhenInjectedInto<GameStateFactory>();

            Container.BindFactory<GamestartController, GamestartController.Factory>().WhenInjectedInto<GamestartState>();
            Container.BindFactory<GameplayController, GameplayController.Factory>().WhenInjectedInto<GameplayState>();
            Container.BindFactory<GameoverController, GameoverController.Factory>().WhenInjectedInto<GameoverState>();
        }

        private void InstallsSystems()
        {
            Container.BindInterfacesAndSelfTo<SoundManager>().FromComponentInNewPrefab(_systemModules.SoundManagerPrefab).AsSingle();
        }

        private void InstallObjects()
        {
            BindPrefabFactory<GamePresenter, GamePresenter.Factory>(_gamePlayItems.GamePalyPresenter);

            BindPrefabFactory<Bullet, Bullet.Factory>(_gamePlayItems.BulletPrefab);
            BindPrefabFactory<UserTank, UserTank.Factory>(_gamePlayItems.UserTankPrefab);

            BindPrefabFactory<TankGreen, TankGreen.Factory>(_gamePlayItems.Enemy1Green);
            BindPrefabFactory<TankEllo, TankEllo.Factory>(_gamePlayItems.Enemy2Ello);
            BindPrefabFactory<TankBlue, TankBlue.Factory>(_gamePlayItems.Enemy3Blue);
            BindPrefabFactory<TankPurple, TankPurple.Factory>(_gamePlayItems.Enemy4Purple);
            BindPrefabFactory<TankRed, TankRed.Factory>(_gamePlayItems.Enemy5Red);

            BindPrefabFactory<Berta, Berta.Factory>(_gamePlayItems.BertaGun);
            BindPrefabFactory<Sparka, Sparka.Factory>(_gamePlayItems.SparkaGun);
            BindPrefabFactory<Isus, Isus.Factory>(_gamePlayItems.IsusGun);

            BindPrefabFactory<HealthDisplay, HealthDisplay.Factory>(_gamePlayItems.DamageDisplay);
            BindPrefabFactory<BonusDisplay, BonusDisplay.Factory>(_gamePlayItems.BonusDisplay);

            BindPrefabFactory<Splash, Splash.Factory>(_particlesItems.Splash);
            BindPrefabFactory<Explosion, Explosion.Factory>(_particlesItems.Explosion);
            BindPrefabFactory<Track, Track.Factory>(_particlesItems.Track);
            BindPrefabFactory<Shield, Shield.Factory>(_particlesItems.Shield);
        }

        private void BindPrefabFactory<TPrefab, TFactory>(GameObject prefab) where TFactory : Factory<TPrefab>
        {
            if (prefab == null)
                throw new ArgumentException();

            Container.BindFactory<TPrefab, TFactory>()
                .FromComponentInNewPrefab(prefab)
                .WithGameObjectName(prefab.name);
        }

        private void InstallSignals()
        {
            // Base Signals
            Container.DeclareSignal<OnAppStartedSignal>();
            Container.DeclareSignal<OnAppPausedSignal>();
            Container.DeclareSignal<OnAppFocusedSignal>();
            Container.DeclareSignal<OnAppQuitSignal>();

            // state signal
            Container.DeclareSignal<OnGamestartSignal>();
            Container.DeclareSignal<OnGameplaySignal>();
            Container.DeclareSignal<OnGameoverSignal>();

            // model Signals
            Container.DeclareSignal<OnModelUpdateSignal>();

            // UI Signals 
            Container.DeclareSignal<OnScoreUpdateSignal>();
            Container.DeclareSignal<OnLiveUpdateSignal>();
            Container.DeclareSignal<OnWeaponChangeSignal>();

            // GAMEPLAY Signals
            Container.DeclareSignal<OnBulletCollisionSignal>();
            Container.DeclareSignal<OnRespaunUserSignal>();
            Container.DeclareSignal<OnRespaunEnemySignal>();
        }

        #endregion

        #region Prefabs

        [Serializable]
        public class SystemModules
        {
            public GameObject SoundManagerPrefab;
        }

        [Serializable]
        public class GamePlayItems
        {
            public GameObject GamePalyPresenter;
            public GameObject UserTankPrefab;
            public GameObject Enemy1Green;
            public GameObject Enemy2Ello;
            public GameObject Enemy3Blue;
            public GameObject Enemy4Purple;
            public GameObject Enemy5Red;
            public GameObject BulletPrefab;
            public GameObject BertaGun;
            public GameObject SparkaGun;
            public GameObject IsusGun;
            public GameObject DamageDisplay;
            public GameObject BonusDisplay;
        }

        [Serializable]
        public class ParticlesItems
        {
            public GameObject Explosion;
            public GameObject Splash;
            public GameObject Track;
            public GameObject Shield;
        }

#endregion
    }
}
