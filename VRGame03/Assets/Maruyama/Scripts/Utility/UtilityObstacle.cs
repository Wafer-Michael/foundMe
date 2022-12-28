using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace maru
{
    public static class UtilityObstacle
    {
        [SerializeField]
        public static string[] DEFAULT_RAY_OBSTACLE_LAYER_STRINGS = new string[] { "L_Obstacle" };

        public static bool IsRayObstacle(Vector3 startPosition, Vector3 targetPosition, float sphereRange = 0.1f)
        {
            return IsRayObstacle(startPosition, targetPosition, sphereRange, DEFAULT_RAY_OBSTACLE_LAYER_STRINGS);
        }

        public static bool IsRayObstacle(Vector3 startPosition, Vector3 targetPosition, float sphereRange = 0.1f, params string[] obstacleLayers)
        {
            int obstacleLayer = LayerMask.GetMask(obstacleLayers);

            var colliders = Physics.OverlapSphere(startPosition, sphereRange, obstacleLayer);  //オブジェクトに接触時に透けるバグ解消用

            return (Physics.Linecast(startPosition, targetPosition, obstacleLayer) || colliders.Length != 0);
        }
    }
}


