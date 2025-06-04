using System;
using UnityEngine;

namespace SpaceXData
{
    public class LaunchManager : MonoBehaviour
    {
        [SerializeField] private ApiClient apiClient;
        [SerializeField] private DataCacher dataCacher;
        [SerializeField] private UI ui;

        private void OnEnable()
        {
            StartCoroutine(apiClient.FetchLaunchesRoutine(onSuccess: launches =>
            {
                dataCacher.SaveLaunches(launches);
                ui.Init(apiClient, launches);
            }, onError: error =>
            {
                Debug.LogError("API Error: " + error);
                var launches = dataCacher.LoadLaunches();
                ui.Init(apiClient, launches);
            }));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            ui.Dispose();
        }
    }
}