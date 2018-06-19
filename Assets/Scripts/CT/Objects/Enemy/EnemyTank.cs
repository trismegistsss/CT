using UnityEngine;
using Zenject;

namespace CT.Objects.Enemy
{
    public class EnemyTank : BaseTank
    {
        [Header("Tank :")]
        [SerializeField] private SpriteRenderer _bodyColorBack;
        [SerializeField] private Color _clorBack;

        [Header("Destroy Bonus :")]
        [SerializeField] private int _destroyBonus;

        [Header("Level :")]
        [SerializeField] private int _level;

        public int GetBonus
        {
            get { return _destroyBonus; }
        }

        protected override void Initialize()
        {
            base.Initialize();

            _bodyColorBack.color = _clorBack;  
        }
    }
}