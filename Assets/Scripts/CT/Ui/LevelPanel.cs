using CT.Models;
using TMPro;
using UnityEngine;
using UniRx;
using Zenject;

namespace CT.HUD
{
    public class LevelPanel : MonoBehaviour
    {
        [Inject] private GameModel _gameModel;

        [SerializeField] private TextMeshProUGUI _levelCount;

        void Awake()
        {
            Subscriptions();
        }

        private void Subscriptions()
        {
            _gameModel.Level.Subscribe((pr) =>
            {
                if (null == pr) pr = 1;
                _levelCount.text = pr.ToString();
            }).AddTo(this);
        }
    }
}