
using UnityEngine;
using Zenject;

namespace CT.Objects
{
    public class Track : MonoBehaviour
    {
        public class Factory : Factory<Track> { }
    }
}