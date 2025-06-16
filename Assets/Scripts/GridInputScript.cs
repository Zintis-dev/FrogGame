using UnityEngine;

public class GridInputScript : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayermask;

    public bool TryGetSelectedMapPosition(out Vector3 position)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, placementLayermask) && hit.collider.CompareTag("Grid"))
        {
            position = hit.point;
            return true;
        }

        position = Vector3.zero;
        return false;
    }
}
