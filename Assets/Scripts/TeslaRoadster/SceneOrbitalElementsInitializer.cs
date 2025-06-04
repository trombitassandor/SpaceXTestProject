using System;
using System.Collections.Generic;
using UnityEngine;

namespace TeslaRoadster
{
    public class SceneOrbitalElementsInitializer : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform orbitalElementParent;
        [SerializeField] private Transform orbitalElementPrefab;
        
        private List<Transform> orbitalElementInstances;

        public void Init(OrbitalElement[] orbitalElements, float worldScale)
        {
            orbitalElementInstances = new List<Transform>();
            
            foreach (var orbitalElement in orbitalElements)
            {
                var position = worldScale * orbitalElement.ComputePositionFromOrbitalElement();
                
                var orbitalElementInstance = Instantiate(
                    orbitalElementPrefab, 
                    position, 
                    orbitalElementPrefab.rotation, 
                    orbitalElementParent);
                
                orbitalElementInstance.localScale *= worldScale;
                
                orbitalElementInstances.Add(orbitalElementInstance);
            }
        }

        public void Dispose()
        {
            if (orbitalElementInstances == null)
            {
                return;
            }
            
            foreach (var orbitalElement in orbitalElementInstances)
            {
                Destroy(orbitalElement.gameObject);
            }
            
            orbitalElementInstances.Clear();
        }
    }
}