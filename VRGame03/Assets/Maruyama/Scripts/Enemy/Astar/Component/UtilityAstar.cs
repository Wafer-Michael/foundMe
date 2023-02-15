using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using GraphType = SparseGraph<AstarNode, AstarEdge>;

namespace maru
{
    static public class UtilityAstar
    {
        /// <summary>
        /// 一番近いノードを検索する。
        /// </summary>
        /// <param name="selfTrans"></param>
        /// <returns></returns>
        static public AstarNode FindNearAstarNode(GraphType graph, Transform selfTrans)
        {
            return FindNearAstarNode(graph, selfTrans.position);
        }

        /// <summary>
        /// 一番近いノードを検索する。
        /// </summary>
        /// <param name="selfTrans"></param>
        /// <returns></returns>
        static public AstarNode FindNearAstarNode(GraphType graph, Vector3 selfPosition)
        {
            var nodes = graph.GetNodes();
            if (nodes.Count == 0)
            {
                return null;
            }

            var sotrNodes = nodes.OrderBy(value => { return (value.GetPosition() - selfPosition).magnitude; });

            return sotrNodes.ToList()[0];
        }
    }
}



