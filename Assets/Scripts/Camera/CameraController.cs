using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cardCameraTransform;
    public Transform gridCameraTransform;

    public float moveSpeed = 5f;

    private bool isDragging;

    // Update is called once per frame
    void Update()
    {
        Transform target = isDragging
            ? gridCameraTransform
            : cardCameraTransform;

        transform.SetPositionAndRotation(
            Vector3.Lerp(
                transform.position,
                target.position,
                Time.deltaTime * moveSpeed
            ),
            Quaternion.Lerp(
                transform.rotation,
                target.rotation,
                Time.deltaTime * moveSpeed
            )
        );
    }

    public void SetDragMode(bool active)
    {
        isDragging = active;
    }
}
