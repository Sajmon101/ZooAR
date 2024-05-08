using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARImageTracker : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    [SerializeField] GameObject prefabElephant;
    void Awake()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
        Debug.Log("FFF");
    }

    void Start()
    {
        prefabs.Add("Elephant", prefabElephant);
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            if (prefabs.TryGetValue(trackedImage.referenceImage.name, out GameObject prefab))
            {
                Instantiate(prefab, trackedImage.transform.position, Quaternion.identity);
            }
        }
    }

    void OnDestroy()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    void Update()
    {
        Debug.Log("GGG");
    }
}
