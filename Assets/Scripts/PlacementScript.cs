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
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
