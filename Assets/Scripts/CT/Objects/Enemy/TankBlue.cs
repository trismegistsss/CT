using UnityEngine;
using Zenject;

namespace CT.Objects.Enemy
{
    public class TankBlue : EnemyTank
    {
        public class Factory : Factory<TankBlue> { }
    }
}

