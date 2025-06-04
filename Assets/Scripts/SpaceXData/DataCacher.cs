using System.IO;
using UnityEngine;

namespace SpaceXData
{
    public class DataCacher : MonoBehaviour
    {
        [SerializeField] private string fileName = "launches.json";
        
        public void SaveLaunches(Launch[] launches)
        {
            var json = JsonHelper.ToJson(launches);
            var path = GetFilePath();
            File.WriteAllText(path, json);
            Debug.Log("Launches cached to: " + path);
        }
    
        public Launch[] LoadLaunches()
        {
            var path = GetFilePath();
            
            if (!File.Exists(path))
            {
                Debug.LogWarning("No cached file found.");
                return null;
            }

            var json = File.ReadAllText(path);
            return JsonHelper.FromJson<Launch>(json);
        }

        private string GetFilePath()
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}