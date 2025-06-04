using System;

namespace TeslaRoadster
{
    public class OrbitalElement
    {
        public double epochJulianDate;             // Time of observation in Julian Date format
        public DateTime observationDateUtc;        // Time of observation in human-readable UTC
        public double semiMajorAxisAu;             // Average orbital distance (AU)
        public double orbitEccentricity;           // How stretched the orbit is (0 = circle)
        public double orbitInclinationDeg;         // Tilt of the orbit relative to the reference plane
        public double ascendingNodeLongitudeDeg;   // Angle from reference direction to the ascending node
        public double periapsisArgumentDeg;        // Angle from ascending node to closest approach point
        public double meanAnomalyDeg;              // Fraction of orbital period passed since periapsis (in degrees)
        public double trueAnomalyDeg;              // Actual position of the object in orbit (in degrees)
    }
}