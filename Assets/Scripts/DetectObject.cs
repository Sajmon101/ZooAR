//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.XR.ARFoundation;
//public class DetectObject : MonoBehaviour
//{
//    public Text debugText;
//    Camera arCam;

//    void Start()
//    {
//        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
//    }

//    void Update()
//    {
//        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//        {
//            Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);
//            RaycastHit hit;
//            if (Physics.Raycast(ray, out hit))
//            {
//                AudioSource audio = hit.transform.GetComponent<AudioSource>();
//                if (audio != null && !audio.isPlaying)
//                {
//                    audio.Play();
//                }
//            }
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class DetectObject : MonoBehaviour
{
    public Text debugText;
    Camera arCam;
    public delegate void ARSurfaceHitHandler(Vector3 hitPoint);
    public static event ARSurfaceHitHandler OnARSurfaceHit;

    void Start()
    {
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Sprawdü, czy trafiony obiekt to powierzchnia AR
                if (hit.transform.GetComponent<ARPlane>() != null)
                {
                    OnARSurfaceHit?.Invoke(hit.point);
                }
                else
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
}
