using System.Collections.Generic;
using CT.Config;
using CT.Models;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CT.HUD
{
    public class TankPanel : MonoBehaviour
    {
        [Inject] private GameModel _gameModel;

        [SerializeField] private Sprite _liveIcon;
        [SerializeField] private Transform _container;

        private List<GameObject> livePool;

        void OnDestroy()
        {
            if (null != livePool)
            {
                livePool.ForEach(pr=>Destroy(pr));
                livePool.Clear();
            }

            livePool = null;
        }

        void Awake()
        {
            livePool = new List<GameObject>();
            Subscriptions();
        }

        private void Subscriptions()
        {
            _gameModel.Live.Subscribe((pr) =>
            {
                if (null == pr) pr= GameConfig.START_LIVE;
                ChangePanel(pr);
            }).AddTo(this);
        }

        private void ChangePanel(int count)
        {
            if (count > livePool.Count)
            {
                for (var i = count - livePool.Count; i > 0; i--)
                    AddLiveIcon();
            }
            else
            {
                if (livePool.Count > 0)
                {
                    Animation(livePool[0], false).OnKill(() =>
                    {
                        Destroy(livePool[0]);
                        livePool.RemoveAt(0);
                    });
                }
            }
        }

        private void AddLiveIcon()
        {
            var obj = Instantiate(new GameObject());
            var im = obj.AddComponent<Image>();
            im.sprite = Instantiate(_liveIcon);
            im.SetNativeSize();
            obj.transform.SetParent(_container.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.zero;
            obj.transform.rotation = new Quaternion(0f,0f,1f,2f);
            livePool.Add(obj);
            Animation(obj, true);
        }

        private Tweener Animation(GameObject obj, bool inject)
        {
            if(inject)
              return  obj.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce).Play();
            else
              return  obj.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.Unset).Play();
        }
    }
}
