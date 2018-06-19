using System.Collections.Generic;
using CT.Config;
using CT.Controllers;
using CT.Objects;
using CT.Enums;
using CT.Factories;
using CT.Models;
using CT.Objects.Weapons;
using UniRx;
using UnityEngine;
using Zenject;

namespace CT.Objects
{
    public class UserTank : BaseTank
    {
        [Inject] private GameModel _gameModel;
        [Inject] private GameManager _gameManager;
        [Inject] private WeaponsFactory _weaponsFactory;

        [SerializeField] private Transform _bodyTank;
        [SerializeField] private List<WeaponType> _avalableWeaponsPool;

        private Weapon _currentWeapon;

        public Weapon GetCurrentWeapon
        {
            get { return _currentWeapon; }
        }

        public List<WeaponType> GetAvailableWeapons
        {
            get { return _avalableWeaponsPool; }
        }

        void OnDestroy()
        {
            DestroyWeapon();
        }

        void DestroyWeapon()
        {
            if(null!= _currentWeapon)
                Destroy(_currentWeapon.gameObject);
        }

        protected override void Initialize()
        {
            base.Initialize();
            Subscriptions();
        }

        protected override void Subscriptions()
        {
            _gameModel.Weapons.Subscribe((pr) =>
            {
                if (null == pr || pr == WeaponType.None) return;
                ChangeWeapon(pr);
            }).AddTo(this);
        }

        private void ChangeWeapon(WeaponType wtype)
        {
            DestroyWeapon();

            _currentWeapon =  _weaponsFactory.InjectGun( wtype);

            if (null == _currentWeapon) return;

            _currentWeapon.transform.SetParent(_bodyTank);
            _currentWeapon.transform.localPosition = GameConfig.DEFAULT_GAMEOBJECTS_POSITION;
            _currentWeapon.transform.localScale = Vector3.one;
            _currentWeapon.transform.rotation = new Quaternion(0f,0f,0f,0f);
        }



        public class Factory : Factory<UserTank>{}
    }
}