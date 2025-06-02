using UnityEngine;

public class WireLine : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 4;
    }

    void LateUpdate()
    {
        if (startPoint == null || endPoint == null) return;

        Vector3 p0 = startPoint.position;
        Vector3 p3 = endPoint.position;

        Vector3 dir = (p3 - p0).normalized; // Unused now?
        float midX = (p3.x - p0.x) / 2f;
        float midZ = (p3.z - p0.z) / 2f;
        Vector3 offsetX = new Vector3(midX, 0, midZ); // Move to midpoint (along x and z-axes) between two blocks
        Vector3 offsetY = new Vector3(0, 1.0f, 0); // Move to y-axis of child block

        Vector3 p1 = p0 + offsetX;
        Vector3 p2 = p1 + offsetY;

        lineRenderer.SetPosition(0, p0);
        lineRenderer.SetPosition(1, p1);
        lineRenderer.SetPosition(2, p2);
        lineRenderer.SetPosition(3, p3);
    }
}
