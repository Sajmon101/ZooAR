using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
public class DetectObject : MonoBehaviour
{
    public Text debugText;
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject spawnableObject;
    Camera arCam;
    GameObject spawnedObject;
    // Start is called before the first frame update
    void Start()
    {
        spawnedObject = null;
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);
        Debug.LogError(Input.touchCount);
        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject ==
            null)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    AudioSource audio = hit.transform.GetComponent<AudioSource>();
                    if (audio != null && !audio.isPlaying)
                    {
                        audio.Play();
                    }
                }
            }
        }
    }
    private void SpawnPrefab(Vector3 spawnPos)
    {
        Instantiate(spawnableObject, spawnPos, Quaternion.identity);
    }
}