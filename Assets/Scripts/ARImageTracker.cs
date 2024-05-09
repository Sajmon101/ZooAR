using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARImageTracker : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();


    private GameObject currentInstance;

    [SerializeField] GameObject prefabElephant;
    [SerializeField] GameObject prefabPanda;
    void Awake()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
       
    }

    void Start()
    {
        prefabs.Add("Elephant", prefabElephant);
        prefabs.Add("Panda", prefabPanda);
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Usuniêcie poprzedniego obiektu, jeœli istnieje
        if (currentInstance != null)
        {
            Destroy(currentInstance);
        }

        foreach (var trackedImage in eventArgs.added)
        {
            if (prefabs.TryGetValue(trackedImage.referenceImage.name, out GameObject prefab))
            {
                // Instancjonowanie nowego obiektu i zapisanie referencji
                currentInstance = Instantiate(prefab, trackedImage.transform.position, Quaternion.identity);
            }
        }

        // Opcjonalnie: mo¿esz tak¿e aktualizowaæ po³o¿enie istniej¹cego obiektu z eventArgs.updated
        foreach (var trackedImage in eventArgs.updated)
        {
            if (currentInstance != null && prefabs.TryGetValue(trackedImage.referenceImage.name, out GameObject prefab))
            {
                currentInstance.transform.position = trackedImage.transform.position;
                currentInstance.transform.rotation = Quaternion.identity;
            }
        }
    }


    void OnDestroy()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }
}
