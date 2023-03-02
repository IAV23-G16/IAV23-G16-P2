using System.Collections;
using System.Collections.Generic;
using UCM.IAV.Navegacion;
using UnityEngine;

public class SceneCenterCamTarget : MonoBehaviour
{
    public void UpdatePos(GraphGrid graphGrid)
    {
        Debug.Log(graphGrid.GetNumCols() + ", " + graphGrid.GetNumRows());

        transform.position = new Vector3(graphGrid.GetNumCols() / 2f * graphGrid.cellSize, 0, graphGrid.GetNumRows() / 2f * graphGrid.cellSize);
    }
}
