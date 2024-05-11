using UnityEngine;
using UnityEngine.UI;

public class TapToPlaySound : MonoBehaviour
{
    private Camera arCamera;
    public Text debugText;

    void Start()
    {
        // Pobierz kamerê AR
        arCamera = Camera.main;
    }

    void Update()
    {
        // SprawdŸ, czy u¿ytkownik dotkn¹³ ekranu
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //debugText.text = "dotkn¹³ ekran";
            Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                debugText.text = "trafi³o";
                // SprawdŸ, czy trafiony obiekt ma komponent AudioSource i odtwórz dŸwiêk
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
