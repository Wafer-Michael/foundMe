using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_GraphNode
{
    public void SetIndex(int index);
    public int GetIndex();
    public bool IsActive();
}
