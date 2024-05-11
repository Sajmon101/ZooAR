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

    private List<GameObject> allInstances = new List<GameObject>(); // Lista do przechowywania wszystkich instancji
    [SerializeField] private Button removeButton; // Przycisk do usuwania modeli

    [SerializeField] GameObject prefabElephant;
    [SerializeField] GameObject prefabPanda;

    void Awake()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
        removeButton.onClick.AddListener(RemoveAllInstances); // Dodaj s³uchacza do przycisku

    }

    void Start()
    {
        prefabs.Add("Elephant", prefabElephant);
        prefabs.Add("Panda", prefabPanda);
    }


    void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            if (prefabs.TryGetValue(trackedImage.referenceImage.name, out GameObject prefab))
            {
                var instance = Instantiate(prefab, trackedImage.transform.position, Quaternion.identity);
                allInstances.Add(instance); // Dodaj now¹ instancjê do listy
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
        removeButton.onClick.RemoveListener(RemoveAllInstances); // Usuñ s³uchacza
    }
}
