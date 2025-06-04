using System.Collections.Generic;

namespace SpaceXData
{
    [System.Serializable]
    public class Launch
    {
        public string name;
        public string date_utc;
        public bool upcoming;
        public string rocket;
        public List<string> payloads;
        public List<string> ships;
    }
}