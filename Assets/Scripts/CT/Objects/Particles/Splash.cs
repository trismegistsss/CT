using UnityEngine;
using Zenject;

namespace CT.Objects
{
    public class Splash : MonoBehaviour
    {
        public class Factory : Factory<Splash>{}
    }
}
