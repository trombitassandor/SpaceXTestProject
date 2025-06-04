using UnityEngine;

namespace TeslaRoadster
{
    public class SceneCameraInitializer : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private float viewPadding = 1.2f;
        [SerializeField] private Vector3 cameraDirection = new(1, 1, -1);
        
        public void Init(Vector3[] positions, float worldScale)
        {
            var bounds = Helper.GetBounds(positions, worldScale);
            var center = bounds.center;
            var maxExtent = bounds.extents.magnitude;

            // Adjust the camera distance based on FOV
            var fovRad = targetCamera.fieldOfView * Mathf.Deg2Rad;
            var distance = (maxExtent * viewPadding) / Mathf.Tan(fovRad * 0.5f);

            // Normalize camera direction
            var offsetDirection = cameraDirection.normalized;
            var cameraPosition = center + offsetDirection * distance;

            targetCamera.transform.position = cameraPosition;
            targetCamera.transform.LookAt(center);
        }
    }
}