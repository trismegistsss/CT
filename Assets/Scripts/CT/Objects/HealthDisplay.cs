using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CT.Objects
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Scrollbar _damageBar;
        [SerializeField] private TextMeshProUGUI _shield;

        void Awake()
        {
            _shield.gameObject.SetActive(false);
        }

        public void UpdateDamage(float damage)
        {
            _damageBar.size = damage;
        }

        public void UpdateSheild(float shield)
        {
            if (shield > 0)
            {
                _shield.gameObject.SetActive(true);
                _shield.text = shield.ToString();
            }
            else
             _shield.gameObject.SetActive(false);
        }

        public class Factory : Factory<HealthDisplay> { }
    }
}
