using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace TeslaRoadster
{
    public static class Helper
    {
        public static bool IsBetweenDates(this OrbitalElement orbitalElement, 
            DateTime startDate, DateTime endDate)
        {
            var isBetweenDates = orbitalElement.observationDateUtc >= startDate && 
                                 orbitalElement.observationDateUtc <= endDate;
            return isBetweenDates;
        }
        
        public static List<OrbitalElement> LoadOrbitalElements(this TextAsset textAsset)
        {
            var orbitalElements = new List<OrbitalElement>();
            var lines = textAsset.text.Split('\n');
            const int firstLine = 1;

            for (var i = firstLine; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                var columns = lines[i].Trim().Split(',');

                var element = new OrbitalElement
                {
                    epochJulianDate = double.Parse(columns[0], CultureInfo.InvariantCulture),
                    observationDateUtc = DateTime.Parse(columns[1], CultureInfo.InvariantCulture),
                    semiMajorAxisAu = double.Parse(columns[2], CultureInfo.InvariantCulture),
                    orbitEccentricity = double.Parse(columns[3], CultureInfo.InvariantCulture),
                    orbitInclinationDeg = double.Parse(columns[4], CultureInfo.InvariantCulture),
                    ascendingNodeLongitudeDeg = double.Parse(columns[5], CultureInfo.InvariantCulture),
                    periapsisArgumentDeg = double.Parse(columns[6], CultureInfo.InvariantCulture),
                    meanAnomalyDeg = double.Parse(columns[7], CultureInfo.InvariantCulture),
                    trueAnomalyDeg = double.Parse(columns[8], CultureInfo.InvariantCulture),
                };

                orbitalElements.Add(element);
            }

            return orbitalElements;
        }
    
        /// <summary>
        /// Simplified 2D Orbit Position (in orbital plane)
        /// </summary>
        /// <param name="orbitalElement"></param>
        /// <returns></returns>
        public static Vector3 ComputePositionFromOrbitalElement(this OrbitalElement orbitalElement)
        {
            // 1. Convert degrees to radians
            var trueAnomalyRad = Deg2Rad(orbitalElement.trueAnomalyDeg);
            var inclinationRad = Deg2Rad(orbitalElement.orbitInclinationDeg);
            var longitudeOfAscendingNodeRad = Deg2Rad(orbitalElement.ascendingNodeLongitudeDeg);
            var argumentOfPeriapsisRad = Deg2Rad(orbitalElement.periapsisArgumentDeg);

            // 2. Compute distance from focus (Sun) to the object at the given true anomaly
            var orbitalRadius = 
                orbitalElement.semiMajorAxisAu * (1 - Math.Pow(orbitalElement.orbitEccentricity, 2)) / 
                (1 + orbitalElement.orbitEccentricity * Math.Cos(trueAnomalyRad));

            // 3. Compute position in orbital plane (2D)
            var xOrbital = orbitalRadius * Math.Cos(trueAnomalyRad);
            var yOrbital = orbitalRadius * Math.Sin(trueAnomalyRad);
            var zOrbital = 0;

            // 4. Rotate to 3D space using orbital inclination and angles
            // Rotation matrix: orbital to ecliptic coordinates
            var cosAscNode = Math.Cos(longitudeOfAscendingNodeRad);
            var sinAscNode = Math.Sin(longitudeOfAscendingNodeRad);
            var cosInclination = Math.Cos(inclinationRad);
            var sinInclination = Math.Sin(inclinationRad);
            var cosPeriapsis = Math.Cos(argumentOfPeriapsisRad);
            var sinPeriapsis = Math.Sin(argumentOfPeriapsisRad);

            var x = (cosAscNode * cosPeriapsis - sinAscNode * sinPeriapsis * cosInclination) * 
                xOrbital + (-cosAscNode * sinPeriapsis - sinAscNode * cosPeriapsis * cosInclination) * yOrbital;
        
            var y = (sinAscNode * cosPeriapsis + cosAscNode * sinPeriapsis * cosInclination) * 
                xOrbital + (-sinAscNode * sinPeriapsis + cosAscNode * cosPeriapsis * cosInclination) * yOrbital;
        
            var z = (sinPeriapsis * sinInclination) * xOrbital + (cosPeriapsis * sinInclination) * yOrbital;

            var vector = new Vector3((float)x, (float)y, (float)z);

            return vector;
        }
        
        public static Bounds GetBounds(Vector3[] positions, float worldScale)
        {
            var bounds = new Bounds(positions[0] * worldScale, Vector3.zero);
            foreach (var position in positions)
            {
                bounds.Encapsulate(position * worldScale);
            }
            return bounds;
        }

        private static double Deg2Rad(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
