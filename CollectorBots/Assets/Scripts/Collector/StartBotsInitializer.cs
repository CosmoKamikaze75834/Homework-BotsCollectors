using System.Collections.Generic;
using UnityEngine;

public class StartBotsInitializer : MonoBehaviour
{
    [SerializeField] private Base _startBase;
    [SerializeField] private List<Collector> _startCollectors;

    private void Start()
    {
        for (int i = 0; i < _startCollectors.Count; i++)
            _startBase.DistributorResources.AddBot(_startCollectors[i]);
    }
}