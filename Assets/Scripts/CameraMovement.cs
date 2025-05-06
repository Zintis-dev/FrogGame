using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSpeed = 15f;
    public float rotateSpeed = 100f;

    private Vector2 zLimit = new Vector2(-20, 20);
    private Vector2 xLimit = new Vector2(-20, 20);

    void Update()
    {

        // Camera movement
        Vector3 inputDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) inputDirection.z += 1f;
        if (Input.GetKey(KeyCode.S)) inputDirection.z -= 1f;
        if (Input.GetKey(KeyCode.A)) inputDirection.x -= 1f;
        if (Input.GetKey(KeyCode.D)) inputDirection.x += 1f;

        if (inputDirection != Vector3.zero)
        {
            inputDirection.Normalize();
        }

        Vector3 moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;
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
        transform.position = clampedPosition;
    }
}
