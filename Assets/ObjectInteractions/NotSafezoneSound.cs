using UnityEngine;

public class PlayerSoundControl : MonoBehaviour
{
    public AudioSource playerAudioSource; // Referencja do AudioSource

    void Start()
    {
        if (playerAudioSource == null)
        {
            playerAudioSource = GetComponent<AudioSource>();
        }
        
        // Ustawienie odtwarzania dźwięku w pętli
        playerAudioSource.loop = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Safezone"))
        {
            if (playerAudioSource.isPlaying)
            {
                playerAudioSource.Stop();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Safezone"))
        {
            if (!playerAudioSource.isPlaying)
            {
                playerAudioSource.Play();
            }
        }
    }
}