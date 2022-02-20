using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    private new Camera camera;  // overloading obsolete property

    void Awake()
    {
        camera = GetComponent<Camera>();

        // fancy way to dynamically allow us to walk behind objects
        camera.transparencySortMode = TransparencySortMode.CustomAxis;
        camera.transparencySortAxis = Vector3.up;
    }

    private void FixedUpdate()
    {
        // if target moved, we need to move the camera
        if (transform.position != target.position) {
            // clamp to min/max and compensate for camera Z offset
            Vector3 targetPosition = new Vector3(
                Mathf.Clamp(target.position.x, minPosition.x, maxPosition.x),
                Mathf.Clamp(target.position.y, minPosition.y, maxPosition.y),
                transform.position.z
            );

            // transform to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }
}
