using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Make sure this script is only compiled in the editor
#if UNITY_EDITOR
[ExecuteInEditMode]
public class CameraFrustumGizmo : MonoBehaviour
{
    public Color color = Color.blue;
    
    private void OnDrawGizmos()
    {
        Camera cam = GetComponent<Camera>();
        if (cam == null) return;

        float height = 0f; // The height at which you want to draw the frustum
        float cameraHeight = cam.transform.position.y;

        // Only proceed if the camera is above the specified height
        if (cameraHeight > height)
        {
            float frustumHeightAtDistance = (cameraHeight - height) * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidthAtDistance = frustumHeightAtDistance * cam.aspect;

            // Calculate the corners of the frustum
            Vector3 center = new Vector3(0, height, 0);
            Vector3 forward = cam.transform.forward;
            forward.y = 0;
            forward.Normalize();
            Vector3 right = cam.transform.right;
            right.y = 0;
            right.Normalize();

            Vector3 topLeft = center - right * frustumWidthAtDistance + forward * frustumHeightAtDistance;
            Vector3 topRight = center + right * frustumWidthAtDistance + forward * frustumHeightAtDistance;
            Vector3 bottomLeft = center - right * frustumWidthAtDistance - forward * frustumHeightAtDistance;
            Vector3 bottomRight = center + right * frustumWidthAtDistance - forward * frustumHeightAtDistance;

            // Draw lines between the corners
            Gizmos.color = color;
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}
#endif
