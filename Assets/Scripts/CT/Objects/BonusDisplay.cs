using TMPro;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace CT.Objects
{
    public class BonusDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _bonus;

        void Awake()
        {
            _bonus.gameObject.SetActive(false);
        }

        public void AddBonus(int bonus)
        {
            _bonus.gameObject.SetActive(true);
            _bonus.text = bonus.ToString();

            DOTween.To(() => _bonus.color, (val) => _bonus.color= val, new Color(1,1,1,0), 2f);
            transform.DOMoveY(transform.position.y + 3, 2f).SetEase(Ease.Unset).Play().OnKill(()=>
            {
                Destroy(gameObject);
            });
        }

        public class Factory : Factory<BonusDisplay> { }
    }
}
