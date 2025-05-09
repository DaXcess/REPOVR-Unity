using UnityEngine;
using UnityEngine.InputSystem;

public class UITrackingDemo : MonoBehaviour
{
    private Camera targetCamera;
    private RectTransform canvasRect;
    
    [SerializeField] private Renderer targetRenderer;

    public InputActionReference buh;
    
    private void Awake()
    {
        var targetCanvas = GetComponent<Canvas>();
        
        targetCamera = targetCanvas.worldCamera;
        canvasRect = targetCanvas.GetComponent<RectTransform>();
    }

    private void Update()
    {
        Debug.Log(buh.action.ReadValueAsObject());
        
        var vrCamera = targetCamera;
        var rectOutput = canvasRect; 
       
     Bounds bounds = targetRenderer.bounds;

     rectOutput.position = bounds.center;
     rectOutput.sizeDelta = new Vector2(bounds.size.x + 0.2f, bounds.size.y + 0.2f);
     rectOutput.rotation = Quaternion.identity;
        
     return;

        // Get the 8 corners of the bounding box in world space (no camera rotation)
        Vector3[] corners = new Vector3[8];
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        int i = 0;
        for (int x = 0; x <= 1; x++)
            for (int y = 0; y <= 1; y++)
                for (int z = 0; z <= 1; z++)
                    corners[i++] = new Vector3(
                        x == 0 ? min.x : max.x,
                        y == 0 ? min.y : max.y,
                        z == 0 ? min.z : max.z
                    );

        // Project the bounds corners onto the world-space XY plane
        Vector2 minXY = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 maxXY = new Vector2(float.MinValue, float.MinValue);

        // Iterate through all corners to find the min/max x and y in the world-space XY plane
        foreach (var corner in corners)
        {
            // We're ignoring the camera's Z rotation here, just using the object's world position
            Vector3 local = corner;

            Vector2 xy = new Vector2(local.x, local.y); // Flatten to XY plane
            minXY = Vector2.Min(minXY, xy);
            maxXY = Vector2.Max(maxXY, xy);
        }

        Vector2 size = maxXY - minXY;  // Calculate the size of the rectangle in the XY plane

        // Apply the world position directly from the object’s world position (not from the camera)
        rectOutput.position = targetRenderer.transform.position;  // Use the object position

        // Get the camera's rotation without the Z-axis (roll)
        Vector3 cameraEuler = vrCamera.transform.eulerAngles;
        cameraEuler.z = 0f;  // Remove the roll (Z-axis rotation)
        Quaternion fixedRotation = Quaternion.Euler(cameraEuler);  // Rebuild the rotation without roll

        // Calculate the direction to face the camera
        Vector3 forward = vrCamera.transform.position - rectOutput.position;  // Camera-to-rect direction
        Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.up);  // Lock "up" to world up

        // Combine the fixed rotation with the target rotation (ensure it faces the camera but stays upright)
        rectOutput.rotation = fixedRotation * targetRotation;  // Apply the fixed rotation

        // Set the size of the rectangle in the local plane (no scaling needed if using RectTransform in world space)
        rectOutput.sizeDelta = size;
        rectOutput.localScale = Vector3.one; // Ensure clean scale (no unwanted distortion)
    }
}
