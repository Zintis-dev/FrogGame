using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 15f;
    [SerializeField] private float rotateSpeed = 100f;

    [SerializeField] private Vector2 zLimit = new Vector2(-6, 6);
    [SerializeField] private Vector2 xLimit = new Vector2(-10, 10);
    [SerializeField] private Vector2 yLimit = new Vector2(4, 20);

    [SerializeField] private Vector3 defaultPosition = new Vector3 (0, 8, -4);
    [SerializeField] private Vector3 defaultRotation = new Vector3(70, 0, 0);

    void Update()
    {

        // Camera movement
        Vector3 inputDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) inputDirection.z += 2f;
        if (Input.GetKey(KeyCode.S)) inputDirection.z -= 2f;
        if (Input.GetKey(KeyCode.A)) inputDirection.x -= 1f;
        if (Input.GetKey(KeyCode.D)) inputDirection.x += 1f;
        if (Input.GetKey(KeyCode.LeftShift)) inputDirection.y += 1f;
        if (Input.GetKey(KeyCode.LeftControl)) inputDirection.y -= 1f;

        if (inputDirection != Vector3.zero)
        {
            inputDirection.Normalize();
        }

        Vector3 moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x + Vector3.up * inputDirection.y;
        transform.position += moveDirection * cameraSpeed * Time.deltaTime;

        // Camera Rotation
        float rotateDirection = 0f;

        if (Input.GetKey(KeyCode.E)) rotateDirection += 1f;
        if (Input.GetKey(KeyCode.Q)) rotateDirection -= 1f;

        transform.eulerAngles += new Vector3(0, rotateDirection * rotateSpeed * Time.deltaTime, 0);

        // Clamp Position
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp (clampedPosition.x, xLimit.x, xLimit.y);
        clampedPosition.z = Mathf.Clamp (clampedPosition.z, zLimit.x, zLimit.y);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, yLimit.x, yLimit.y);
        transform.position = clampedPosition;

        // Reset to default location
        if (Input.GetKey(KeyCode.R))
        {
            transform.position = defaultPosition;
            transform.eulerAngles = defaultRotation;
        }
    }
}
