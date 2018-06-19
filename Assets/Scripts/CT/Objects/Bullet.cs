using System;
using System.Collections.Generic;
using CT.Config;
using CT.Core.Signals;
using CT.Enums;
using CT.Objects.Weapons;
using CT.Utils;
using DG.Tweening;
using SM.Sound;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;
using Random = UnityEngine.Random;

namespace CT.Objects
{
    public class Bullet : MonoBehaviour
    {
        private readonly List<string> _bulletsColorPool = new List<string>
        {
            "BD3B00FF",
            "BD7900FF",
            "BDB800FF",
            "74BD00FF",
            "00BDA6FF",
            "0087BDFF",
            "0048BDFF",
            "AB00BDFF",
            "BD006AFF",
        };

        [Inject] private Splash.Factory _splashParticle;
        [Inject] private GameManager _gameManager;
        [Inject] private ISoundManager _soundManager;
        [Inject] private OnBulletCollisionSignal _onBulletCollision;

        [SerializeField] private SpriteRenderer _colorBack;

        public int Id { get; set; }

        private Weapon _weapon;
        private float _speed;
        private Rigidbody2D _rBody;
        private Vector3 _target;
        private Tweener _tw;
        private LayerTags _frendlyLayerTags;
       
        void OnDestroy()
        {
            if (null != _rBody)
                _rBody = null;

            if (null != _tw)
                _tw.Kill();
        }

        void Awake()
        {
            _frendlyLayerTags = LayerTags.Bullet;
            _rBody = GetComponent<Rigidbody2D>();
            Subscriptions();
        }

        private void Subscriptions()
        {
            _rBody.OnCollisionEnter2DAsObservable().Subscribe((pr) =>
            {
                if (pr.gameObject.CompareTag(LayerTags.Bullet.ToString()) ||
                    pr.gameObject.CompareTag(_frendlyLayerTags.ToString()))
                {
                    return;
                }
                    
                var obj =  pr.gameObject.GetComponent<BaseTank>();

                if (null != obj)
                    _onBulletCollision.Fire(obj.Id, _weapon);

                if (null!=_tw)
                    _tw.Kill();

            }).AddTo(this);
        }

        public void initialize(GunPoints point, Weapon weapon, LayerTags frendlyLayer)
        {
            _frendlyLayerTags = frendlyLayer;

            var r = Random.Range(0, 10);

            if (r > 4)
                _soundManager.PlaySFX(SoundDataConfig.FIRE);

            _speed = weapon.Speed;
            _weapon = weapon;

            transform.position = point.StartbulletPoint.transform.position;
            _target = point.FinishBulletPoint.transform.position;
            _colorBack.color = HexColorUtils.HexToColor(_bulletsColorPool[Random.Range(0, _bulletsColorPool.Count-1)]);
            transform.localScale = new Vector3(weapon.Scale, weapon.Scale, weapon.Scale);

            _tw = transform.DOMove(_target, Time.deltaTime * _speed).SetEase(Ease.Linear).Play().OnComplete(() =>
            {
                if (r > 4)
                {
                    _soundManager.PlaySFX(SoundDataConfig.SPLASH);

                    var sPart = _splashParticle.Create();
                    sPart.transform.SetParent(_gameManager.GamePresenter.transform);
                    sPart.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
                }
                
            }).OnKill(() => {Destroy(gameObject); });
        }

        public class Factory : Factory<Bullet> {}
    }
}