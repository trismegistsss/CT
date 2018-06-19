using UnityEngine;
using Zenject;

namespace CT.Objects
{
    public class Shield : MonoBehaviour
    {
        public class Factory : Factory<Shield> { }
    }
}
