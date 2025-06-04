using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SpaceXData
{
    public class ApiClient : MonoBehaviour
    {
        [SerializeField] private string launchesApiKey = "https://api.spacexdata.com/v4/launches";
        [SerializeField] private string payloadsApiKey = "https://api.spacexdata.com/v4/payloads/";
        [SerializeField] private string shipsApiKey = "https://api.spacexdata.com/v4/ships/";

        public IEnumerator FetchLaunchesRoutine(Action<Launch[]> onSuccess, Action<string> onError)
        {
            var request = UnityWebRequest.Get(launchesApiKey);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(request.error);
                yield break;
            }

            var jsonString = request.downloadHandler.text;
            var launches = JsonHelper.FromJson<Launch>(jsonString);

            onSuccess?.Invoke(launches);
        }

        public IEnumerator FetchShipsRoutine(Launch launch, Action<List<Ship>> callback)
        {
            var ships = new List<Ship>();

            foreach (var shipId in launch.ships)
            {
                var shipUrl = $"{shipsApiKey}{shipId}";
                var getShipRequest = UnityWebRequest.Get(shipUrl);
                yield return getShipRequest.SendWebRequest();
                
                if (getShipRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to fetch ship: " + getShipRequest.error);
                    continue;
                }

                var ship = JsonUtility.FromJson<Ship>(getShipRequest.downloadHandler.text);
                if (ship != null)
                {
                    ships.Add(ship);
                }
            }

            callback?.Invoke(ships);
        }
    }
}