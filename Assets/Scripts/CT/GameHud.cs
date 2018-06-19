using System.Collections.Generic;
using CT.Config;
using CT.Core.Enums;
using CT.Core.Signals;
using CT.Core.State;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace CT.HUD
{
    public class GameHud : MonoBehaviour
    {
        [Inject] private IStateManager _stateManager;

        [Inject] private OnAppPausedSignal _appPausedSignal;
        [Inject] private OnAppFocusedSignal _appFocusedSignal;
        [Inject] private OnAppQuitSignal _onAppQuitSignal;

        // ui top panels

        [SerializeField] private List<GameObject> _upPanels;
        [SerializeField] private GameObject _startPopUp;


        // ui popups

        private float _panelsTargetPosition;
        private float _panelsUpPosition;

        void Awake()
        {
            _panelsTargetPosition = _upPanels[0].transform.localPosition.y;
            _panelsUpPosition = _upPanels[0].transform.localPosition.y + GameConfig.Y_PANELS_OUT;

            Subscriptions();
        }

        #region Subscriptions

        private void Subscriptions()
        {
            _appPausedSignal.AsObservable.SubscribeOn(Scheduler.Immediate)
                .Subscribe(_ => {}).AddTo(this);

            _stateManager.CurrentState.Subscribe((pr)=>
            {
                if (pr == GameStateType.Default) return;

                ChangeUpPanelState(pr);
                ChangeStartPanelState(pr);

            }).AddTo(this);
        }

        #endregion

        #region State Panels Update

        private void ChangeUpPanelState(GameStateType gameType)
        {
            foreach (var upPanel in _upPanels)
            {
                var upPosition = new Vector3(
                    upPanel.transform.localPosition.x, 
                    _panelsUpPosition, 
                    upPanel.transform.localPosition.z);

                var targetPosition = new Vector3(
                    upPanel.transform.localPosition.x,
                    _panelsTargetPosition,
                    upPanel.transform.localPosition.z);

                switch (gameType)
                {
                    case GameStateType.Gamestart:
                        upPanel.transform.localPosition = upPosition;
                        break;
                    case GameStateType.Gameplay:
                        upPanel.SetActive(true);
                        upPanel.transform.localPosition = upPosition;
                        Animation(upPanel.transform, targetPosition);
                        break;
                    case GameStateType.Gameover:
                       // var uptween = Animation(upPanel.transform, upPosition);
                     //   uptween.OnKill(()=> {/*upPanel.SetActive(false);*/});
                        break;
                }
            }
        }

        private void ChangeStartPanelState(GameStateType gameType)
        {
            switch (gameType)
            {
                case GameStateType.Gamestart:
                    _startPopUp.SetActive(true);
                    break;
                case GameStateType.Gameplay:
                case GameStateType.Gameover:
                    _startPopUp.SetActive(false);
                    break;
            }
        }

        #endregion

       #region Animation

        private Tweener Animation(Transform objTransform, Vector3 target)
        {
            return objTransform.DOLocalMove(target, GameConfig.SPEED_MOVE_PANELS).SetEase(Ease.Unset).Play();
        }

        #endregion
    }
}
