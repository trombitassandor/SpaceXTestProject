using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpaceXData
{
    internal class LaunchInfo : MonoBehaviour
    {
        [SerializeField] private Transform headerParent;
        [SerializeField] private Transform listItemParent;
        [SerializeField] private Transform shipRowPrefab;
        [SerializeField] private TMP_Text textPrefab;
        
        public void Init(ApiClient apiClient, Launch launch)
        {
            Instantiate(textPrefab, headerParent).text = nameof(Ship.name);
            Instantiate(textPrefab, headerParent).text = nameof(Ship.type);
            Instantiate(textPrefab, headerParent).text = nameof(Ship.home_port);
            Instantiate(textPrefab, headerParent).text = nameof(Ship.missions);
            StartCoroutine(apiClient.FetchShipsRoutine(launch, OnShipsFetched));
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
        
        private void OnShipsFetched(List<Ship> ships)
        {
            foreach (var ship in ships)
            {
                var listItemInstance = Instantiate(shipRowPrefab, listItemParent);
                Instantiate(textPrefab, listItemInstance).text = ship.name;
                Instantiate(textPrefab, listItemInstance).text = ship.type;
                Instantiate(textPrefab, listItemInstance).text = ship.home_port;
                Instantiate(textPrefab, listItemInstance).text = ship.missions.Count.ToString();
            }
        }
    }
}