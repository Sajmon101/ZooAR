using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARImageTracker : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    private List<GameObject> allInstances = new List<GameObject>();
    [SerializeField] private Button removeButton;
    [SerializeField] GameObject prefabElephant;
    [SerializeField] GameObject prefabPanda;
    [SerializeField] GameObject prefabZebra;

    void Awake()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
        removeButton.onClick.AddListener(RemoveAllInstances);

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
            if (prefabs.TryGetValue(trackedImage.referenceImage.name, out GameObject prefab))
            {
                var instance = Instantiate(prefab, trackedImage.transform.position, Quaternion.identity);
                allInstances.Add(instance);
            }
        }
    }

    public void RemoveAllInstances()
    {
        foreach (var instance in allInstances)
        {
            if (instance != null)
                Destroy(instance);
        }
        allInstances.Clear(); // Wyczyœæ listê po usuniêciu wszystkich instancji
    }

    void OnDestroy()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
        removeButton.onClick.RemoveListener(RemoveAllInstances);
    }
}
