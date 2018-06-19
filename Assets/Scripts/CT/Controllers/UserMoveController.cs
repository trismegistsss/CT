using UnityEngine;

namespace CT.Controllers
{
    public class UserMoveController : BaseMoveController
    {
        private bool _isControl = true;

        void Update()
        {
            if (_isControl)
            {
                if (Input.GetButton("Accelerate"))
                    _accelerate = true;
                else
                    _accelerate = false;

                if (Input.GetButton("Brake"))
                    _brake = true;
                else
                    _brake = false;

                _angular = Input.GetAxis("Horizontal");
            }
        }
    }
}
