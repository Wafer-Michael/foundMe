using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{
    public class CalculateCamera
    {
        /// <summary>
        /// カメラの中にいるかどうか
        /// </summary>
        /// <param name="position">ポジション</param>
        /// <param name="camera">カメラ</param>
        /// <returns></returns>
        public static bool IsInCamera(Vector3 position, Camera camera)
        {
            var viewportPosition = camera.WorldToViewportPoint(position);

            if (0.0f <= viewportPosition.x && viewportPosition.x <= 1.0f
                && 0.0f <= viewportPosition.y && viewportPosition.y <= 1.0f
                )
            {
                return true;
            }

            return false;
        }

        public static Vector3 ConvertToCameraVec(Vector3 input, Camera camera, GameObject selfObject)
        {
            var angle = Vector3.zero;

            if (input.x != 0 || input.z != 0)
            {
                //進行方向の向きを計算
                var front = selfObject.transform.position - camera.transform.position;
                front.y = 0;

                float frontAngle = Mathf.Atan2(front.z, front.x);
                float cntlAngle = Mathf.Atan2(-input.x, input.z);
                float totalAngle = frontAngle + cntlAngle;

                angle = new Vector3(Mathf.Cos(totalAngle), 0, Mathf.Sin(totalAngle));

                angle.y = 0;
            }

            return angle;
        }
    }
}


