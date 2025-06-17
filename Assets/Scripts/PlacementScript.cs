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
    private Dictionary<GameObject, Vector3Int> placedObjects = new Dictionary<GameObject, Vector3Int>();

    private void Start()
    {
        Vector3 centerWorld = grid.transform.position;
        Vector3Int centerCell = grid.WorldToCell(centerWorld);

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                Vector3Int offset = new Vector3Int(x, 0, z);
                Vector3Int occupiedCell = centerCell + offset;
                occupiedCells.Add(occupiedCell);
            }
        }
    }

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

        GameObject placedObject = Instantiate(selectedObject.Prefab, cellWorldPosition + offset, Quaternion.identity);

        occupiedCells.Add(gridPosition);
        placedObjects.Add(placedObject, gridPosition);

        ObjectDestroyNotifier notifier = placedObject.AddComponent<ObjectDestroyNotifier>();
        notifier.Setup(this);
    }

    public void NotifyObjectDestroyed(GameObject obj)
    {
        if (placedObjects.TryGetValue(obj, out Vector3Int gridPos))
        {
            occupiedCells.Remove(gridPos);
            placedObjects.Remove(obj);
        }
    }
}
public class ObjectDestroyNotifier : MonoBehaviour
{
    private PlacementScript placementScript;

    public void Setup(PlacementScript script)
    {
        placementScript = script;
    }

    private void OnDestroy()
    {
        if (placementScript != null)
        {
            placementScript.NotifyObjectDestroyed(gameObject);
        }
    }
}