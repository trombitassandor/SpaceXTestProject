using System.Collections.Generic;
using UnityEngine;

namespace TeslaRoadster
{
    public class RoadsterTail : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private int maxPositions = 20;
        [SerializeField] private float startWidth = 0.01f;
        [SerializeField] private float endWidth = 0.01f;
        
        private Queue<Vector3> positionHistory = new();
        private float worldScale;

        private void Awake()
        {
            positionHistory = new Queue<Vector3>(maxPositions);
        }

        public void Init(float worldScale)
        {
            this.worldScale = worldScale;
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 0;
            lineRenderer.useWorldSpace = true;
            lineRenderer.startWidth = startWidth * worldScale;
            lineRenderer.endWidth = endWidth * worldScale;
        }

        public void AddPosition(Vector3 position)
        {
            positionHistory.Enqueue(position * worldScale);

            if (positionHistory.Count > maxPositions)
            {
                positionHistory.Dequeue();
            }

            lineRenderer.positionCount = positionHistory.Count;
            lineRenderer.SetPositions(positionHistory.ToArray());
        }

        public void Reset()
        {
            positionHistory.Clear();
            lineRenderer.positionCount = 0;
        }

        public void Dispose()
        {
            lineRenderer.enabled = false;
            positionHistory.Clear();
            lineRenderer.positionCount = 0;
        }
    }
}