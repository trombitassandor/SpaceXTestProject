using System;
using UnityEngine;

namespace TeslaRoadster
{
    public class SceneSunAndRoadsterInitializer : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform sun;
        [SerializeField] private Light sunLight;
        [SerializeField] private Transform roadster;

        private Vector3 sunInitialLocalScale;
        private float sunInitialIntensity;
        private float sunInitialIndirectMultiplier;
        private float sunInitialRange;
        private Vector3 roadsterInitialLocalScale;

        public void Init(float worldScale)
        {
            sunInitialLocalScale = sun.localScale;
            sunInitialIntensity = sunLight.intensity;
            sunInitialIndirectMultiplier = sunLight.bounceIntensity;
            sunInitialRange = sunLight.range;
            roadsterInitialLocalScale = roadster.localScale;
            
            sun.localScale *= worldScale;
            sunLight.intensity *= worldScale;
            sunLight.bounceIntensity *= worldScale;
            sunLight.range *= worldScale;
            roadster.localScale *= worldScale;
        }


        public void Dispose()
        {
            sun.localScale = sunInitialLocalScale;
            sunLight.intensity = sunInitialIntensity;
            sunLight.bounceIntensity = sunInitialIndirectMultiplier;
            sunLight.range = sunInitialRange;
            roadster.localScale = roadsterInitialLocalScale;
        }
    }
}