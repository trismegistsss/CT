using System;
using CT.Enums;
using CT.Handlers;
using CT.Models;
using CT.Objects;
using CT.Objects.Enemy;
using CT.Objects.Weapons;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace CT.Controllers
{
    public class EnemyMoveController : BaseMoveController
    {
        [Inject] private GameItemsInfo _gameItemsInfo;
        [Inject] private BulletHandler _bulletHandler;

        private float _shootTime = 3f;
        private float _shootTimer = 0;
        private float _moveTime = 0;
        private float _moveTimer = 0;

        private EnemyStates _currentState;
        private Rigidbody2D _rBody;

        void Awake()
        {
            _rBody = GetComponent<Rigidbody2D>();
            _angular = 0;

            Subscriptions();
        }

        private void Subscriptions()
        {
            _rBody.OnCollisionEnter2DAsObservable().Subscribe((pr) =>
            {
                if (pr.gameObject.CompareTag(LayerTags.User.ToString()) ||
                    pr.gameObject.CompareTag(LayerTags.Enemies.ToString()))
                {
                    _brake = true;
                }

            }).AddTo(this);
        }

        void Update()
        {
            if (null == _gameItemsInfo.UserTank)
            {
                _accelerate = false;
                _brake = false;
                return;
            }

            var ePos = gameObject.transform.position;
            var pPos = _gameItemsInfo.UserTank.transform.position;
            var distanceToPlayer = (pPos - ePos).magnitude;

            // move
            _accelerate = !_brake;
            _moveTimer += Time.fixedDeltaTime;

            if (_moveTimer > _moveTime)
            {
                _moveTimer = 0;

                _angular =UnityEngine.Random.Range(-1,2);
                _moveTime = UnityEngine.Random.Range(_angular==0?1:0.3f, _angular == 0 ? 3: 1f);
                
                _brake = false;
            }

            // shoot
            _shootTimer += Time.fixedDeltaTime;

            if (_shootTimer > _shootTime)
            {
                _shootTimer = 0;
                _shootTime = UnityEngine.Random.Range(0.5f, 2);

                var et = GetComponent<EnemyTank>();
                var ew = GetComponent<Weapon>();

                _bulletHandler.InitFire(et, ew, LayerTags.Enemies);
            }
        }


        [SerializeField] private EnemyFeaturesType _featuresType;
    }
}
