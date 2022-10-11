using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace maru {
    public static class Utility
    {
        /// <summary>
        /// カメラに合わせたベクトルに変更
        /// </summary>
        /// <param name="input">入力</param>
        /// <param name="camera">カメラ</param>
        /// <param name="selfObject">自分自身</param>
        /// <returns>カメラに合わせたベクトル</returns>
        public static Vector3 ConvartCameraVec(Vector3 input, Camera camera, GameObject selfObject)
        {
            var angle = Vector3.zero;

            if (input.x != 0 || input.z != 0)
            {
                //進行方向の向きを計算
                var front = camera.transform.forward;
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



