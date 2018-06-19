using UnityEngine;
using Zenject;

namespace CT.Objects
{
    public class Explosion : MonoBehaviour
    {
        public class Factory : Factory<Explosion> {}
    }
}