using UnityEngine;
using Zenject;

namespace CT.Gameplay
{
    public class GamePresenter : MonoBehaviour
    {
        public class Factory : Factory<GamePresenter> { }
    }
}