using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlacementScript : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private GridInputScript inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectSelectionUI selectionUI;

    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    private void Update()
    {
        if (inputManager.TryGetSelectedMapPosition(out Vector3 mousePosition))
        {
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            Vector3 cellWorldPosition = grid.CellToWorld(gridPosition);
            Vector3 offset = new Vector3(grid.cellSize.x / 2f, 0f, grid.cellSize.z / 2f);
            cellIndicator.transform.position = cellWorldPosition + offset;

            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    TryPlaceObject(gridPosition);
                }
            }
        }
    }

    private void TryPlaceObject(Vector3Int gridPosition)
    {
        if (occupiedCells.Contains(gridPosition))
        {
            Debug.Log("Cell is already occupied! Can't place object here.");
            return;
        }

        ObjectData selectedObject = selectionUI.SelectedObject;

        Vector3 cellWorldPosition = grid.CellToWorld(gridPosition);
        Vector3 offset = new Vector3(grid.cellSize.x / 2f, 0f, grid.cellSize.z / 2f);

        Instantiate(selectedObject.Prefab, cellWorldPosition + offset, Quaternion.identity);

        occupiedCells.Add(gridPosition);
    }
}
