using UnityEngine;
using UnityEngine.UI;

public class HeroCollisionChecker : MonoBehaviour
{
    public GameObject enemy; // Referencja do obiektu wroga
    public Image blackScreen; // Referencja do czarnego ekranu
    public GameObject youLostText; // Referencja do tekstu "YouLost"

    void Start()
    {
        // Upewnij się, że czarny ekran i tekst "YouLost" są początkowo wyłączone
        blackScreen.gameObject.SetActive(false);
        youLostText.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Sprawdzanie, czy obiekt, z którym zderzył się bohater, to wróg
        if (collision.gameObject == enemy)
        {
            Debug.Log("Hero collided with the enemy!");
            ShowLossScreen();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Sprawdzanie, czy obiekt, z którym bohater wszedł w kolizję, to wróg (w przypadku triggerów)
        if (other.gameObject == enemy)
        {
            Debug.Log("Hero entered trigger with the enemy!");
            ShowLossScreen();
        }
    }

    void ShowLossScreen()
    {
        // Aktywacja czarnego ekranu i tekstu "YouLost"
        blackScreen.gameObject.SetActive(true);
        youLostText.SetActive(true);
    }
}