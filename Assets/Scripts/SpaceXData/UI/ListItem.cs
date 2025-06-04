using UnityEngine;
using UnityEngine.UI;

namespace SpaceXData
{
    public class ListItem : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private LaunchInfo launchInfoPrefab;
        
        private Launch _launch;
        private ApiClient _apiClient;
        private Transform _launchInfoParent;

        public void Init(ApiClient apiClient, Launch launch, Transform launchInfoParent)
        {
            _apiClient = apiClient;
            _launch = launch;
            _launchInfoParent = launchInfoParent;
            button.onClick.AddListener(ShowLaunchInfo);
        }

        private void ShowLaunchInfo()
        {
            var launchInfoInstance = Instantiate(launchInfoPrefab, _launchInfoParent);
            launchInfoInstance.Init(_apiClient, _launch);
        }
    }
}