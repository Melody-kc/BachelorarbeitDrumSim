using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEditor.PackageManager;
using UnityEngine;
public class Drumtrigger : MonoBehaviour
{


    public float triggerValue = 1.0f; // Schlagstärke (z.?B. Velocity, oder fest definiert)
    public BluetoothSerial bluetooth; // Dein Bluetooth-Script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drumstick")) // Du kannst den Schläger per Tag markieren
        {
            if (bluetooth != null)
            {
                bluetooth.SendToArduino($"0;{triggerValue};0;0;0\n");
                Debug.Log("Drum hit! Value sent: " + triggerValue);
            }
        }
    }

// Start is called before the first frame update
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
