using UnityEngine;
using Zenject;

namespace CT.Objects
{
    public class BaseTank : MonoBehaviour
    {
        public int Id { get; set; }

        public void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {  
            Subscriptions();
        }

        protected virtual void Subscriptions()
        {

        }
    }
}
