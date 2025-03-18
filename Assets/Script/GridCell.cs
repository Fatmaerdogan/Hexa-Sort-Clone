using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public HexagonStack Stack { get; private set; }

    public bool IsOccupied
    {
        get => Stack != null;
        private set { }
    }

    public void AssignStack(HexagonStack stack)
    {
        Stack = stack;
    }
}

