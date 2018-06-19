using CT.Utils;
using UnityEngine;

namespace CT.Controllers
{
    public class BaseMoveController : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _torque = -200f;
        [SerializeField] private float _driftSticky = 0.9f;
        [SerializeField] private float _driftSlippy = 1;
        [SerializeField] private float _maxVelocity = 2.5f;

        protected bool _accelerate = false;
        protected bool _brake = false;
        protected float _angular = 0f;

        private Vector2 ForwardVelocity
        {
            get { return transform.up *
                         Vector2.Dot(GetComponent<Rigidbody2D>().velocity, 
                             transform.up); }
        }

        private Vector2 RightVelocity
        {
            get { return transform.right * 
                         Vector2.Dot(GetComponent<Rigidbody2D>().velocity, 
                             transform.right); }
        }

        private void FixedUpdate()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            float driftFactor = _driftSticky;

            if (RightVelocity.magnitude > _maxVelocity)
                driftFactor = _driftSlippy;

            rb.velocity = ForwardVelocity + RightVelocity * driftFactor;

            if (_accelerate)
                rb.AddForce(transform.up * _speed);

            if (_brake)
                rb.AddForce(transform.up * -_speed);

            float tf = Mathf.Lerp(0, _torque, rb.velocity.magnitude / 2);

            rb.angularVelocity = _angular * tf;

            OutOfScreen.CheckOutOfScreen(transform);
        }
    }
}
