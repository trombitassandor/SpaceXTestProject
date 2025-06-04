using System;
using System.Linq;
using UnityEngine;

namespace TeslaRoadster
{
    public class RoadsterManager : MonoBehaviour
    {
        [SerializeField] private TextAsset orbitalElementsTextAsset;
        [SerializeField] private string startDateString = "07.02.2018";
        [SerializeField] private string endDateString = "08.10.2019";
        [SerializeField] private float worldScale = 100f;
        [SerializeField] private SceneSunAndRoadsterInitializer sceneSunAndRoadsterInitializer;
        [SerializeField] private SceneOrbitalElementsInitializer sceneOrbitalElementsInitializer;
        [SerializeField] private SceneCameraInitializer sceneCameraInitializer;
        [SerializeField] private RoadsterSimulator simulator;
        [SerializeField] private RoadsterTail tail;
        [SerializeField] private OrbitalDataUI orbitalElementDataUi;
        [SerializeField] private bool showOrbitalElements;
        
        private void OnEnable()
        {
            var orbitalElements = orbitalElementsTextAsset.LoadOrbitalElements();
            
            var orbitalElementsBetweenDates = orbitalElements
                .Where(IsBetweenDates)
                .ToArray();

            var orderedOrbitalElements = orbitalElementsBetweenDates
                .OrderBy(orbitalElement => orbitalElement.observationDateUtc)
                .ToArray();
            
            var orderedOrbitalElementWorldPositions = orbitalElementsBetweenDates
                .Select(Helper.ComputePositionFromOrbitalElement)
                .ToArray();
            
            if (showOrbitalElements)
            {
                sceneOrbitalElementsInitializer.Init(orderedOrbitalElements, worldScale);
            }
            
            sceneSunAndRoadsterInitializer.Init(worldScale);
            sceneCameraInitializer.Init(orderedOrbitalElementWorldPositions, worldScale);
            
            tail.Init(worldScale);
            tail.AddPosition(orderedOrbitalElementWorldPositions[0]);

            simulator.OnReachedOrbitalElement += OnReachedOrbitalElement;
            simulator.OnReachedLastOrbitalElement += tail.Reset;
            simulator.Init(orderedOrbitalElements, orderedOrbitalElementWorldPositions, worldScale);
            simulator.enabled = true;
            
            orbitalElementDataUi.UpdateText(orderedOrbitalElements[0]);
        }

        private void OnDisable()
        {
            simulator.OnReachedOrbitalElement -= OnReachedOrbitalElement;
            simulator.OnReachedLastOrbitalElement -= tail.Reset;
            sceneSunAndRoadsterInitializer.Dispose();
            sceneOrbitalElementsInitializer.Dispose();
            tail.Dispose();
            orbitalElementDataUi.Dispose();
        }
        
        private void OnReachedOrbitalElement(OrbitalElement orbitalElement, Vector3 worldPosition)
        {
            tail.AddPosition(worldPosition);
            orbitalElementDataUi.UpdateText(orbitalElement);
        }

        private bool IsBetweenDates(OrbitalElement orbitalElement)
        {
            var startDate = DateTime.Parse(startDateString);
            var endDate = DateTime.Parse(endDateString);
            var isBetweenDates = orbitalElement.IsBetweenDates(startDate, endDate);
            return isBetweenDates;
        }
    }
}