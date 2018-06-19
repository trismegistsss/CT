using CT.Enums;
using CT.Factories;
using CT.Models;
using DG.Tweening;
using TMPro;
using Tools.Delay;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CT.HUD
{
    public class WeaponPanel : MonoBehaviour
    {
        [Inject] private GameModel _gameModel;
        [Inject] private IDelayService _delayService;
        [Inject] private WeaponsFactory _weaponsFactory;

        [SerializeField] private TextMeshProUGUI _nameWeapon;
        [SerializeField] private Image _iconWeapon;

        private Tweener _currentTween;

        void OnDestroy()
        {
            DestroyTween();
        }

        void DestroyTween()
        {
            if (null != _currentTween)
                _currentTween.Kill();
        }

        void Awake()
        {
            Subscriptions();
        }

        private void Subscriptions()
        {
            _gameModel.Weapons.Subscribe((pr) =>
            {
                if (null == pr || pr == WeaponType.None) return;

                _delayService.DelayedCall(0.1f, () =>
                {
                    ChangeCurrentWeapon(pr);
                });
                
            }).AddTo(this);
        }

        private void ChangeCurrentWeapon(WeaponType wtype)
        {
            var weapon = _weaponsFactory.GetCurrentWeapon;
            if (null != weapon)
            {
                _iconWeapon.sprite = weapon.Icon;
                _iconWeapon.SetNativeSize();
                _nameWeapon.text = weapon.NameWeapon;

                Animation();
            }     
        }

        private void Animation()
        {
            DestroyTween();

            var iconObject = _iconWeapon.gameObject;
            iconObject.transform.localScale = Vector3.zero;

            _currentTween = iconObject.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce).Play();
        }
    }
}