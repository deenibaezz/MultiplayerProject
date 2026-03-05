using UnityEngine;

public class ClampToCamera : MonoBehaviour
{
    private Camera cam;
    private float verticalBound;
    private float horizontalBound;

    void Start()
    {
        cam = Camera.main;
        verticalBound = cam.orthographicSize;
        horizontalBound = verticalBound * cam.aspect;
    }

    void LateUpdate()
    {
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, -horizontalBound, horizontalBound);
        p.y = Mathf.Clamp(p.y, -verticalBound, verticalBound);
        transform.position = p;
    }
}