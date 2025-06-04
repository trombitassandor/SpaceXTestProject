using System.Collections.Generic;

namespace SpaceXData
{
    [System.Serializable]
    public class Ship
    {
        public string name;
        public string type;
        public string home_port;
        public List<string> missions;
    }
}