using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageTracker : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> trackedInstances = new Dictionary<string, GameObject>();

    private List<GameObject> allInstances = new List<GameObject>();
    private AudioSource confirmSound;
    [SerializeField] private Button removeButton;
    [SerializeField] GameObject prefabElephant;
    [SerializeField] GameObject prefabPanda;
    [SerializeField] GameObject prefabZebra;
    [SerializeField] Text debugText;

    void Awake()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
        removeButton.onClick.AddListener(RemoveAllInstances);
        confirmSound = GetComponent<AudioSource>();

    }

    void Start()
    {
        prefabs.Add("Elephant", prefabElephant);
        prefabs.Add("Panda", prefabPanda);
        prefabs.Add("Zebra", prefabZebra);
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            if (!trackedInstances.ContainsKey(trackedImage.referenceImage.name))
            {
                InstantiatePrefab(trackedImage);
            }
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking &&
                !trackedInstances.ContainsKey(trackedImage.referenceImage.name))
            {
                InstantiatePrefab(trackedImage);
            }
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            if (trackedInstances.TryGetValue(trackedImage.referenceImage.name, out GameObject instance))
            {
                Destroy(instance);
                trackedInstances.Remove(trackedImage.referenceImage.name);
            }
        }
    }

    private void InstantiatePrefab(ARTrackedImage trackedImage)
    {
        if (prefabs.TryGetValue(trackedImage.referenceImage.name, out GameObject prefab))
        {
            var instance = Instantiate(prefab, trackedImage.transform.position, Quaternion.identity);
            trackedInstances[trackedImage.referenceImage.name] = instance;
            debugText.text = prefab.name;

            if (!confirmSound.isPlaying)
            {
                confirmSound.Play();
            }
        }
    }

    public void RemoveAllInstances()
    {
        foreach (var instance in trackedInstances.Values)
        {
            if (instance != null)
            {
                Destroy(instance);
            }
        }
        trackedInstances.Clear();
    }

    void OnDestroy()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
        removeButton.onClick.RemoveListener(RemoveAllInstances);
    }
}
