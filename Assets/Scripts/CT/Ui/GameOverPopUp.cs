using CT.Config;
using CT.Core.Enums;
using CT.Core.Signals;
using CT.Core.State;
using DG.Tweening;
using SM.Sound;
using Tools.Delay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using UniRx;

namespace CT.Models
{
    public class GameOverPopUp : MonoBehaviour
    {
        [Inject] private IStateManager _stateManager;
        [Inject] private ISoundManager _soundManager;
        [Inject] private IDelayService _delayService;

        [Inject] private OnGamestartSignal _onGamestartSignal;

        [SerializeField] private GameObject _gameOver;
        [SerializeField] private Button _reStartButton;

        private CanvasGroup _cGroup;
        private Tweener _tw;

        void Awake()
        {
            _cGroup = GetComponent<CanvasGroup>();
            _cGroup.alpha = 0;
            Subscriptions();
            gameObject.SetActive(false);
        }

        private void Subscriptions()
        {
            _stateManager.CurrentState.Subscribe((pr) =>
            {
                if (null == pr || pr != GameStateType.Gameover) return;
                Animation(true);
            }).AddTo(this);

            _reStartButton.OnClickAsObservable().Subscribe(_ => { OnClickStartButton(); }).AddTo(this);
        }

        private void OnClickStartButton()
        {
            _soundManager.PlaySFX(SoundDataConfig.HIT_BUTTON1);

            Animation(false).OnComplete(() =>
            {
                gameObject.SetActive(false);
                _onGamestartSignal.Fire();
            });
        }

        private Tweener Animation(bool visible)
        {
            gameObject.SetActive(true);
            return  DOTween.To(() => _cGroup.alpha, (val) => _cGroup.alpha = val, visible?1:0, 1f);
        }
    }
}
