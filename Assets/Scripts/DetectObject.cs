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
                // SprawdŸ, czy trafiony obiekt to powierzchnia AR
                if (hit.transform.GetComponent<ARPlane>() != null)
                {
                    debugText.text = "Trafiono w powierzchniê AR";
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
