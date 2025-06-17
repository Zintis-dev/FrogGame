using UnityEngine;

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