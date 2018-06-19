using System.Runtime.InteropServices;
using CT.Config;
using CT.Core.Signals;
using CT.Models;
using CT.Objects;
using CT.Objects.Enemy;
using CT.Objects.Weapons;
using SM.Sound;
using Tools.Delay;
using UniRx;
using UnityEngine;
using Zenject;

namespace CT.Controllers
{
    public class DamageController : MonoBehaviour
    {
      //  [Inject] private 

        [Inject] private OnBulletCollisionSignal _bulletCollision;
        [Inject] private OnRespaunUserSignal _onRespaunUser;
        [Inject] private OnRespaunEnemySignal _onRespaunEnemy;

        [Inject] private GameManager _gameManager;
        [Inject] private IDelayService _delayService;

        [Inject] private Shield.Factory _shildFactory;
        [Inject] private Explosion.Factory _explosionFactory;
        [Inject] private HealthDisplay.Factory _damageDisplayFactory;
        [Inject] private BonusDisplay.Factory _bonusDisplay;

        [Header("Parameters:")]
        [SerializeField] private float _arm;
        [SerializeField] private float _shield;

        private HealthDisplay _healthDisplay;
        private float _health;
        private Weapon _weapon;
        private bool _isShieldBlock;

        void OnDestroy()
        {
            if(null!= _healthDisplay)
                Destroy(_healthDisplay.gameObject);
        }

        void Awake()
        {
            _health = 100;

            _healthDisplay = _damageDisplayFactory.Create();
            _healthDisplay.transform.SetParent(_gameManager.GamePresenter.transform);
   
            UpdateHealthDisplay();
            Subscriptions();
        }

        private void Subscriptions()
        {
            _bulletCollision.AsObservable.SubscribeOn(Scheduler.ThreadPool)
            .Subscribe((pr) =>
            {
                var obj = GetComponent<BaseTank>();
                if (pr.Item1 != obj.Id || null == pr.Item2) return;

                _weapon = pr.Item2;
                UpdateHealthLogic();

            }).AddTo(this);
        }

        void Update()
        {
            _healthDisplay.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, -2);

        }

        private void UpdateHealthLogic()
        {
            // Shield
            if (_shield > 0 && !_isShieldBlock)
            {
                _isShieldBlock = true;
               
                var sh =  _shildFactory.Create();
                sh.transform.SetParent(gameObject.transform);
                sh.transform.localPosition = new Vector3(0, 0, -2);
                
                _delayService.DelayedCall(1f, () =>
                {
                    _shield--;
                    _isShieldBlock = false;
                    UpdateHealthDisplay();
                });
            }

            //armor
            if (_shield <= 0)
            {
                var dmg = 1f;
                if (_arm < _weapon.Damage)
                    dmg = _weapon.Damage - _arm;

                _health -= dmg;

                UpdateHealthDisplay();

                if (_health <= 0)
                {
                   var expl = _explosionFactory.Create();
                    expl.transform.SetParent(_gameManager.GamePresenter.transform);
                    expl.transform.localPosition = new Vector3
                        (gameObject.transform.position.x, gameObject.transform.position.y, -2);

                    var btObj = GetComponent<BaseTank>();

                    if (btObj.Id > 0)
                    {
                        var etObj = GetComponent<EnemyTank>();

                        var bDisplay= _bonusDisplay.Create();
                        bDisplay.transform.SetParent(_gameManager.GamePresenter.transform);
                        bDisplay.transform.localPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -4);
                        bDisplay.AddBonus(etObj.GetBonus);

                        _onRespaunEnemy.Fire(etObj);
                    }
                    else
                        _onRespaunUser.Fire();
                }     
            }
        }

        private void UpdateHealthDisplay()
        {
            _healthDisplay.UpdateDamage(_health / 100);
            _healthDisplay.UpdateSheild(_shield);
        }
    }
}
