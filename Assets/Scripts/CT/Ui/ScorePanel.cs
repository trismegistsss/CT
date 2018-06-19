using CT.Models;
using TMPro;
using UnityEngine;
using UniRx;
using Zenject;

namespace CT.HUD
{
    public class ScorePanel : MonoBehaviour
    {
        [Inject] private GameModel _gameModel;

        [SerializeField] private TextMeshProUGUI _scoreCount;

        void Awake()
        {
            Subscriptions();
        }

        private void Subscriptions()
        {
            _gameModel.Score.Subscribe((pr) =>
            {
                if (null == pr) pr = 0;
                _scoreCount.text = pr.ToString();
            }).AddTo(this);
        }
    }
}
