using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Assets.Scripts.ScriptHelper
{
    public class RayCastThrower: MonoBehaviour
    {
        public  static float offset = 0.3f;
        public static Color color = Color.red;
        public static  IEnumerable<GameObject> ThrowRayCast(float xSpawnPoint,float ySpawnPoint, float rayDistance,float rayAngle,float rayCount ,Vector2 xRotation)
        {
            Vector2 origin = new Vector2(xSpawnPoint + offset* xRotation.x, ySpawnPoint);

            for (int i = 0; i < rayCount; i++)
            {
                float currentAngle = rayAngle * (-0.5f + (float)i / (rayCount - 1));
                Vector2 currentDirection = Quaternion.Euler(0, 0, currentAngle) * xRotation ;

                RaycastHit2D hit = Physics2D.Raycast(origin, currentDirection, rayDistance);

                Debug.DrawRay(origin, currentDirection * rayDistance, color, 10f);

                if (hit.collider == null) continue;
   
                 yield return hit.collider.gameObject;
            }
        }
    }
}
