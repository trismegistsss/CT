using CT.Config;
using CT.Enums;
using CT.Models;
using CT.Objects.Weapons;
using Log.GDebug;
using UnityEngine;
using Zenject;

namespace CT.Factories
{
    public class WeaponsFactory
    {
        [Inject] private Berta.Factory _bertaFactory;
        [Inject] private Sparka.Factory _sparkaFactory;
        [Inject] private Isus.Factory _IsusFactory;

        private Weapon _currentWeapon;

        public Weapon GetCurrentWeapon
        {
            get {return _currentWeapon;}
        }

        public Weapon InjectGun(WeaponType type)
        {
            if (type == WeaponType.None)
                return null;

            _currentWeapon = null;

            Weapon nWeapon = null;

            switch (type)
            {
                case WeaponType.Berta:
                    nWeapon = _bertaFactory.Create();
                    break;
                case WeaponType.Sparka:
                    nWeapon = _sparkaFactory.Create();
                    break;
                case WeaponType.Isus:
                    nWeapon = _IsusFactory.Create();
                    break;
                default:
                    GDebug.LogError("Current Weapon type not found :" + type, this, LogCategory.WEAPON_FABRIC);
                    break;
            }

            if (null != nWeapon)
            {
                _currentWeapon = nWeapon;
                _currentWeapon.NameWeapon = type.ToString();
            }
            
            return nWeapon;
        }


    }
}
