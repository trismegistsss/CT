
using CT.Enums;
using CT.Models;
using CT.Objects.Enemy;
using CT.Objects.Weapons;
using Zenject;

namespace CT.Core.Signals
{
    // Base Signals
    public class OnAppStartedSignal : Signal<OnAppStartedSignal> { }
    public class OnAppPausedSignal : Signal<OnAppPausedSignal, bool> { }
    public class OnAppFocusedSignal : Signal<OnAppFocusedSignal, bool> { }
    public class OnAppQuitSignal : Signal<OnAppQuitSignal> { }

    // State signals
    public class OnGamestartSignal : Signal<OnGamestartSignal> { }
    public class OnGameplaySignal : Signal<OnGameplaySignal> { }
    public class OnGameoverSignal : Signal<OnGameoverSignal> { }

    // UI Signals 
    public class OnScoreUpdateSignal : Signal<OnScoreUpdateSignal, int> { }
    public class OnLiveUpdateSignal : Signal<OnLiveUpdateSignal, int> { }
    public class OnWeaponChangeSignal : Signal<OnWeaponChangeSignal, WeaponType> { }

    // Model Signals
    public class OnModelUpdateSignal : Signal<OnModelUpdateSignal, GameInfo> { }

    // GAMEPLAY
    public class OnBulletCollisionSignal : Signal<OnBulletCollisionSignal, int, Weapon> { }
    public class OnRespaunUserSignal : Signal<OnRespaunUserSignal> { }
    public class OnRespaunEnemySignal : Signal<OnRespaunEnemySignal, EnemyTank> { }
}
