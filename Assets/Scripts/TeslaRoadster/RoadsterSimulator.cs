using System;
using UnityEngine;

namespace TeslaRoadster
{
    public class RoadsterSimulator : MonoBehaviour
    {
        [SerializeField] private float simulationSpeedHoursPerSecond = 24f;

        public Action<OrbitalElement, Vector3> OnReachedOrbitalElement;
        public Action OnReachedLastOrbitalElement;
        
        private OrbitalElement[] orbitalElements;
        private Vector3[] worldPositions;
        private float worldScale;
        private int lastValidIndex;
        private DateTime firstDate;

        private double simulationTimeHoursElapsed;
        private int currentIndex;

        private double currentTimeHours;
        private double nextTimeHours;
        
        private Vector3 currentPosition;
        private Vector3 nextPosition;

        public void Init(OrbitalElement[] orbitalElements, Vector3[] worldPositions, float worldScale)
        {
            this.orbitalElements = orbitalElements;
            this.worldPositions = worldPositions;
            this.worldScale = worldScale;
            lastValidIndex = orbitalElements.Length - 2;
            firstDate = orbitalElements[0].observationDateUtc;
            Init();
        }

        private void Update()
        {
            simulationTimeHoursElapsed += simulationSpeedHoursPerSecond * Time.deltaTime;

            var hasReachedNewPosition = false;

            while (currentIndex < lastValidIndex && 
                   nextTimeHours <= simulationTimeHoursElapsed)
            {
                SetCurrentIndex(currentIndex + 1);
                hasReachedNewPosition = true;
            }

            if (hasReachedNewPosition)
            {
                InvokeReachedOrbitalElement();
            }

            if (currentIndex >= lastValidIndex)
            {
                OnReachedLastOrbitalElement?.Invoke();
                Init();
                return;
            }
            
            var t = Mathf.InverseLerp(
                (float)currentTimeHours, 
                (float)nextTimeHours, 
                (float)simulationTimeHoursElapsed);
            
            transform.position = worldScale * Vector3.Lerp(currentPosition, nextPosition, (float)t);
        }
        
        private void InvokeReachedOrbitalElement()
        {
            OnReachedOrbitalElement?.Invoke(orbitalElements[currentIndex], currentPosition);
        }

        private void Init()
        {
            simulationTimeHoursElapsed = 0f;
            SetCurrentIndex(0);
        }

        private void SetCurrentIndex(int index)
        {
            currentIndex = index;
            
            currentTimeHours = GetTimeHours(orbitalElements[index]);
            nextTimeHours = GetTimeHours(orbitalElements[index + 1]);
            
            currentPosition = worldPositions[index];
            nextPosition = worldPositions[index + 1];
        }
        
        private double GetTimeHours(OrbitalElement orbitalElement)
        {
            return (orbitalElement.observationDateUtc - firstDate).TotalHours;
        }
    }
}