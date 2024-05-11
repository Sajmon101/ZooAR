using UnityEngine;
using UnityEngine.UI;

public class TapToPlaySound : MonoBehaviour
{
    private Camera arCamera;
    public Text debugText;

    void Start()
    {
        // Pobierz kamer� AR
        arCamera = Camera.main;
    }

    void Update()
    {
        // Sprawd�, czy u�ytkownik dotkn�� ekranu
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //debugText.text = "dotkn�� ekran";
            Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                debugText.text = "trafi�o";
                // Sprawd�, czy trafiony obiekt ma komponent AudioSource i odtw�rz d�wi�k
                AudioSource audio = hit.transform.GetComponent<AudioSource>();
                if (audio != null && !audio.isPlaying)
                {
                    debugText.text = "gra";
                    audio.Play();
                }
            }
        }
    }
}
