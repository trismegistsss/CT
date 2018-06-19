using CT.Config;
using CT.Core.Enums;
using CT.Core.Signals;
using CT.Core.State;
using DG.Tweening;
using SM.Sound;
using Tools.Delay;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CT.HUD
{
    public class GameStartPopUp : MonoBehaviour
    {
        [Inject] private IStateManager _stateManager;
        [Inject] private ISoundManager _soundManager;
        [Inject] private IDelayService _delayService;

        [Inject] private OnGameplaySignal _onGameplaySignal;

        [SerializeField] private GameObject _nameGame;
        [SerializeField] private Button _startButton;

        private Vector3 _nameTargetPosition;
        private Vector3 _buttonTargetPosition;
        private Vector3 _nameUpPosition;
        private Vector3 _buttonUpPosition;

        void Awake()
        {
            var nameTransform = _nameGame.transform;
            var startButttonTransform = _startButton.transform;

            _nameTargetPosition = nameTransform.localPosition;
            _buttonTargetPosition = startButttonTransform.localPosition;

            _nameUpPosition = new Vector3(nameTransform.localPosition.x,
                nameTransform.localPosition.y + GameConfig.Y_GAME_NAME_OUT,
                nameTransform.localPosition.z);

            _buttonUpPosition = new Vector3(startButttonTransform.localPosition.x,
                startButttonTransform.localPosition.y - GameConfig.Y_GAME_NAME_OUT,
                startButttonTransform.localPosition.z);

            Subscriptions();
        }

        void OnEnable()
        {
            _nameGame.transform.localPosition = _nameUpPosition;
            _startButton.transform.localPosition = _buttonUpPosition;

            _startButton.enabled = false;
        }

        private void Subscriptions()
        {
            _stateManager.CurrentState.Subscribe((pr) =>
              {
                  if (null == pr || pr != GameStateType.Gamestart) return;
                  StartAnimation();
              }).AddTo(this);

            _startButton.OnClickAsObservable().Subscribe(_ => { OnClickStartButton(); }).AddTo(this);
        }

        private void StartAnimation()
        {
            _delayService.DelayedCall(GameConfig.DELAY_TIME_OPEN_LOGO, () =>
            {
                Animation(_nameGame.GetComponent<Transform>(), _nameTargetPosition);

                _delayService.DelayedCall(0.1f, () =>
                {
                    var tw = Animation(_startButton.GetComponent<Transform>(), _buttonTargetPosition);
                    tw.OnKill(() => { _startButton.enabled = true; });
                });
            });     
        }

        private void OnClickStartButton()
        {
            _startButton.enabled = false;

            _soundManager.PlaySFX(SoundDataConfig.HIT_BUTTON1);

            Animation(_nameGame.GetComponent<Transform>(), _nameUpPosition);
            var tw = Animation(_startButton.GetComponent<Transform>(), _buttonUpPosition);
            tw.OnKill(() =>
            {
                gameObject.SetActive(false);
                _onGameplaySignal.Fire();
            });

            _soundManager.FadeMusic(0.0f, 1f);
        }

        private Tweener Animation(Transform objTransform, Vector3 target)
        {
            return objTransform.DOLocalMove(target, GameConfig.SPEED_MOVE_LOGO).SetEase(Ease.OutBounce).Play();
        }
    }
}
