using UnityEngine;

public class PlacementScript : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private GridInputScript inputManager;
    [SerializeField] private Grid grid;

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        Vector3 cellWorldPosition = grid.CellToWorld(gridPosition);

        Vector3 offset = new Vector3(grid.cellSize.x / 2f, 0f, grid.cellSize.z / 2f);
        cellIndicator.transform.position = cellWorldPosition + offset;
    }


}
