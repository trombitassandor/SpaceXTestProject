using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SpaceXData
{
    public class UI : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform headerParent;
        [SerializeField] private Transform listItemParent;
        [SerializeField] private ListItem listItemPrefab;
        [SerializeField] private TMP_Text textPrefab;
        [SerializeField] private Transform launchInfoParent;
        [SerializeField] private string dateFormat = "yyyy-MM-dd HH:mm";
        [SerializeField] private string launchUpcomingText = "yes";
        [SerializeField] private string launchNoUpcomingText = "no";
        [SerializeField] private float headerTextWidth;
        [SerializeField] private float[] listItemTextWidths;

        private List<ListItem> _listItems;

        private void Awake()
        {
            _listItems = new List<ListItem>();
        }

        private void Start()
        {
            DisplayHeader();
        }
        
        public void Init(ApiClient apiClient, Launch[] launches)
        {
            DisplayLaunches(apiClient, launches);
        }

        public void Dispose()
        {
            foreach (var item in _listItems)
            {
                Destroy(item.gameObject);
            }

            _listItems.Clear();
        }

        private void DisplayHeader()
        {
            InstantiateText(headerParent, nameof(Launch.name), headerTextWidth);
            InstantiateText(headerParent, nameof(Launch.date_utc), headerTextWidth);
            InstantiateText(headerParent, nameof(Launch.upcoming), headerTextWidth);
            InstantiateText(headerParent, nameof(Launch.rocket), headerTextWidth);
            InstantiateText(headerParent, nameof(Launch.payloads), headerTextWidth);
        }

        private void DisplayLaunches(ApiClient apiClient, Launch[] launches)
        {
            foreach (var launch in launches)
            {
                InstantiateLaunchListItem(apiClient, launch);
            }
        }

        private void InstantiateLaunchListItem(ApiClient apiClient, Launch launch)
        {
            var widthIndex = 0;
            var listItemInstance = Instantiate(listItemPrefab, listItemParent);
            var listItemTransform = listItemInstance.transform;
            InstantiateText(listItemTransform, launch.name, listItemTextWidths[widthIndex++]);
            InstantiateText(listItemTransform, GetFormattedDateText(launch), listItemTextWidths[widthIndex++]);
            InstantiateText(listItemTransform, GetLaunchUpcomingText(launch), listItemTextWidths[widthIndex++]);
            InstantiateText(listItemTransform, launch.rocket, listItemTextWidths[widthIndex++]);
            InstantiateText(listItemTransform, GetPayloadsText(launch), listItemTextWidths[widthIndex++]);
            listItemInstance.Init(apiClient, launch, launchInfoParent);
            _listItems.Add(listItemInstance);
        }

        private void InstantiateText(Transform parent, string text, float width)
        {
            var textInstance = Instantiate(textPrefab, parent);
            textInstance.text = text;
            var sizeDelta = textInstance.rectTransform.sizeDelta;
            sizeDelta.x = width;
            textInstance.rectTransform.sizeDelta = sizeDelta;
        }

        private string GetFormattedDateText(Launch launch)
        {
            return !DateTime.TryParse(launch.date_utc, out var dateTime) 
                ? launch.date_utc 
                : dateTime.ToString(dateFormat);
        }

        private string GetLaunchUpcomingText(Launch launch)
        {
            return launch.upcoming 
                ? launchUpcomingText
                : launchNoUpcomingText;
        }

        private string GetPayloadsText(Launch launch)
        {
            return launch.payloads.Count.ToString();
        }
    }
}