using CT.Enums;
using UnityEngine;

namespace CT.Utils
{
    public class OutOfScreen
    {
        public static void CheckOutOfScreen(Transform transform)
        {
            var camera = Camera.main.ViewportToWorldPoint(Camera.main.transform.localScale);

            var tp = transform.position;

            if (tp.y < -camera.y)
            {
                transform.position = new Vector3(transform.position.x, camera.y, transform.position.z);
            }
            else if (tp.y > camera.y)
            {
                transform.position = new Vector3(transform.position.x, -camera.y, transform.position.z);
            }
            else if (tp.x > camera.x)
            {
                transform.position = new Vector3(-camera.x, transform.position.y, transform.position.z);
            }
            else if (tp.x < -camera.x)
            {
                transform.position = new Vector3(camera.x, transform.position.y, transform.position.z);
            }
        }
    }
}