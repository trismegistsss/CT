using System.Collections;
using UnityEngine;
using System;

namespace CT.Utils
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleAutoDestroy : MonoBehaviour
    {
        public bool OnlyDeactivate = false;

        void OnEnable()
        {
            StartCoroutine("CheckIfAlive");
        }

        public event Action<GameObject> OnComplete;
        public virtual void Complete()
        {
            if (OnComplete != null)
            {
                var complete = OnComplete;
                OnComplete = null;
                complete(gameObject);
            }
        }

        IEnumerator CheckIfAlive()
        {
            var ps = GetComponent<ParticleSystem>();
            while (ps != null)
            {
                yield return new WaitForSeconds(ps.duration + ps.startLifetime);
                if (!ps.IsAlive(false))
                {
                    if (OnlyDeactivate)
                    {
                        gameObject.SetActive(false);
                        Complete();
                    }
                    else
                    {
                        Complete();
                        Destroy(gameObject);
                    }
                    break;
                }
            }
        }

    }
}
