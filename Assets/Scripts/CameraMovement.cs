using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSpeed = 0.1f;
    public Vector3 cameraVelocity;

    private void Update()
    {
        if (FindFirstObjectByType<PlayerController>().canMove)
        {
            transform.position += Vector3.forward * cameraSpeed;
        }
        
        cameraVelocity = Vector3.forward * cameraSpeed;
    }
}
