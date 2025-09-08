using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dumsound : MonoBehaviour
{
    private AudioSource audioSource;
    public TextMeshPro textOutput;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource.clip == null)
        {
            Debug.LogWarning("❌ Kein AudioClip in AudioSource zugewiesen!");
        }
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger getroffen von: " + other.name);

        if (other.CompareTag("Drumsticks"))
        {
            Drumsticks sv = other.GetComponent<Drumsticks>();
            float volume = 1f;

            if (sv != null)
            {
                float speed = sv.CurrentVelocity.magnitude;
                textOutput.text = $"Speed: {speed:F2} m/s";
                volume = Mathf.Clamp(speed * 5f, 0.1f, 10f); // nicht stumm
                Debug.Log("🟡 StickSpeed (manuell): " + speed + " → Volume: " + volume);
            }
            else
            {
                Debug.LogWarning("❌ Keine StickVelocity-Komponente gefunden");
            }

            audioSource.PlayOneShot(audioSource.clip, volume);
        }
    }
}