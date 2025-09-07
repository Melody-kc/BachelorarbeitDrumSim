using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dumsound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Nur reagieren, wenn ein Drumstick das Fell ber�hrt
        if (other.CompareTag("Drumstick"))
        {
            // Optional: Lautst�rke abh�ngig von Geschwindigkeit des Sticks
            Rigidbody rb = other.attachedRigidbody;
            float volume = 1f;

            if (rb != null)
                volume = Mathf.Clamp01(rb.velocity.magnitude / 5f);

            audioSource.PlayOneShot(audioSource.clip, volume);
        }
    }
}
