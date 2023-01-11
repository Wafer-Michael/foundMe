using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarEdge : GraphEdge
{
    public AstarEdge(GraphNode fromNode, GraphNode toNode) :
        base(fromNode, toNode)
    { }
}
