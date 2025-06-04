using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace TeslaRoadster
{
    public class OrbitalDataUI : MonoBehaviour, IDisposable
    {
        public TMP_Text uiText;

        public void UpdateText(OrbitalElement orbitalElement)
        {
            var localDateTime = orbitalElement.observationDateUtc.ToLocalTime();

            var info = 
                "<b>Current Orbital Data</b>\n" +
                $"Date: {localDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}\n" +
                $"Epoch JD: {orbitalElement.epochJulianDate}\n" +
                $"Semi-major Axis (au): {orbitalElement.semiMajorAxisAu:F6}\n" +
                $"Eccentricity: {orbitalElement.orbitEccentricity:F6}\n" +
                $"Inclination (°): {orbitalElement.orbitInclinationDeg:F3}\n" +
                $"Longitude of Asc. Node (°): {orbitalElement.ascendingNodeLongitudeDeg:F3}\n" +
                $"Argument of Periapsis (°): {orbitalElement.periapsisArgumentDeg:F3}\n" +
                $"Mean Anomaly (°): {orbitalElement.meanAnomalyDeg:F3}\n" +
                $"True Anomaly (°): {orbitalElement.trueAnomalyDeg:F3}";

            uiText.text = info;
        }

        public void Dispose()
        {
            uiText.text = string.Empty;
        }
    }
}