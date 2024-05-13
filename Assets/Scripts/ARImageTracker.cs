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
    private Dictionary<string, List<GameObject>> trackedInstances = new Dictionary<string, List<GameObject>>();
    private AudioSource confirmSound;
    GameObject currentPrefab;
    ARTrackedImage currentTrackImage;
    [SerializeField] private Button removeButton;
    [SerializeField] GameObject prefabElephant;
    [SerializeField] GameObject prefabPanda;
    [SerializeField] GameObject prefabZebra;
    [SerializeField] Text debugText;
    [SerializeField] Text animalInfo;

    void Awake()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
        DetectObject.OnARSurfaceHit += HandleSurfaceHit;
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
                SetPrefab(trackedImage);
            }
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking &&
                !trackedInstances.ContainsKey(trackedImage.referenceImage.name))
            {
                SetPrefab(trackedImage);
            }
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            if (trackedInstances.TryGetValue(trackedImage.referenceImage.name, out List<GameObject> instances))
            {
                foreach (var instance in instances)
                {
                    Destroy(instance);
                }
                trackedInstances.Remove(trackedImage.referenceImage.name);
            }
        }
    }

    private void SetPrefab(ARTrackedImage trackedImage)
    {
        if (prefabs.TryGetValue(trackedImage.referenceImage.name, out currentPrefab))
        {
            currentTrackImage = trackedImage;
            animalInfo.text = trackedImage.referenceImage.name;
            if (!confirmSound.isPlaying)
            {
                confirmSound.Play();
            }
        }
    }

    private void DrawPrefab(Vector3 positionToDraw)
    {
        var instance = Instantiate(currentPrefab, positionToDraw, Quaternion.identity);

        if (!trackedInstances.ContainsKey(currentTrackImage.referenceImage.name))
        {
            trackedInstances[currentTrackImage.referenceImage.name] = new List<GameObject>();
        }

        trackedInstances[currentTrackImage.referenceImage.name].Add(instance);
        debugText.text = currentPrefab.name;
    }

    void HandleSurfaceHit(Vector3 hitPoint)
    {
        // Obs³u¿ trafienie w powierzchniê AR
        debugText.text = "O tak";
        // Dodaj tutaj kod, który ma siê wykonaæ po trafieniu w powierzchniê AR
        DrawPrefab(hitPoint);
    }

    public void RemoveAllInstances()
    {
        Debug.Log("RemoveAllInstances called");
        foreach (var instanceList in trackedInstances.Values)
        {
            foreach (var instance in instanceList)
            {
                if (instance != null)
                {
                    Destroy(instance);
                }
            }
        }
        trackedInstances.Clear();
    }

    void OnDestroy()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
        DetectObject.OnARSurfaceHit -= HandleSurfaceHit;
        removeButton.onClick.RemoveListener(RemoveAllInstances);
    }
}
