using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace maru {
    public static class Utility
    {
        /// <summary>
        /// �J�����ɍ��킹���x�N�g���ɕύX
        /// </summary>
        /// <param name="input">����</param>
        /// <param name="camera">�J����</param>
        /// <param name="selfObject">�������g</param>
        /// <returns>�J�����ɍ��킹���x�N�g��</returns>
        public static Vector3 ConvartCameraVec(Vector3 input, Camera camera, GameObject selfObject)
        {
            var angle = Vector3.zero;

            if (input.x != 0 || input.z != 0)
            {
                //�i�s�����̌������v�Z
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



