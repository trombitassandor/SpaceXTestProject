using System.Collections.Generic;

namespace SpaceXData
{
    [System.Serializable]
    public class Payload
    {
        public string name;
        public string type;
        public List<string> ships;
    }
}