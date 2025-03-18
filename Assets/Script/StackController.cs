using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask hexagonLayerMask;
    [SerializeField] private LayerMask gridHexagonLayerMask;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Data")]
    [SerializeField] private GridCell targetCell;

    [Header("Actions")]
    public static Action<GridCell> onStackPlaced;

    private HexagonStack currentHexStack;
    private Vector3 currentStackInitialPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ManageControl();
    }

    private void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
            ManageMouseDown();
        else if (Input.GetMouseButton(0) && currentHexStack!=null)
            ManageMouseDrag();
        else if (Input.GetMouseButtonUp(0) && currentHexStack != null)
            ManageMouseUp();
    }
    private void ManageMouseDown()
    {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500, hexagonLayerMask))
        {
            Debug.Log("We have not detected any hexagon");
            return;
        }
        currentHexStack = hit.collider.GetComponent<Hexagon>().HexagonStack;
        currentStackInitialPos = currentHexStack.transform.position;
    }
    private void ManageMouseUp()
    {
        if (targetCell == null)
        {
            currentHexStack.transform.position = currentStackInitialPos;
            currentHexStack = null;
            return;
        }

        currentHexStack.transform.position = targetCell.transform.position.With(y: .2f);
        currentHexStack.transform.SetParent(targetCell.transform);
        currentHexStack.Place();

        targetCell.AssignStack(currentHexStack);

        onStackPlaced?.Invoke(targetCell);

        targetCell = null;
        currentHexStack = null;
    }

    private void ManageMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, gridHexagonLayerMask);

        if (hit.collider == null)
            DraggingAboveGround();
        else
            DraggingAboveGridCell(hit);
    }

    private void DraggingAboveGround()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, groundLayerMask);

        if (hit.collider == null)
        {
            Debug.LogError("No ground detected, this is unusual...");
            return;
        }

        Vector3 currentStackTargetPos = hit.point.With(y: 2);

        currentHexStack.transform.position=Vector3.MoveTowards(
            currentHexStack.transform.position,
            currentStackTargetPos,
            Time.deltaTime*30);

        targetCell = null;
    }


    private void DraggingAboveGridCell(RaycastHit hit)
    {
        GridCell gridCell = hit.collider.GetComponent<GridCell>();

        if (gridCell.IsOccupied)
            DraggingAboveGround();
        else
            DraggingAboveNonOccupiedGridCell(gridCell);
    }

    private void DraggingAboveNonOccupiedGridCell(GridCell gridCell)
    {
        Vector3 currentStackTargetPos = gridCell.transform.position.With(y: 2);

        currentHexStack.transform.position = Vector3.MoveTowards(
            currentHexStack.transform.position,
            currentStackTargetPos,
            Time.deltaTime * 30);

        targetCell = gridCell;
    }




    private Ray GetClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);


}
