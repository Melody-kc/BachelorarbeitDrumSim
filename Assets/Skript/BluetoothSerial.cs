using UnityEngine;
using System.IO.Ports;
using System;
using System.Threading;

public class BluetoothSerial : MonoBehaviour
{
    [Header("Serial Port Settings")]
    public string portName = "COM4";      // COM-Port des Bluetooth-Moduls (z. B. HC-05)
    public int baudRate = 115200;           // Muss zum Bluetooth-Modul passen
    public bool deactivate = false;

    private SerialPort serialPort;
    private readonly object portLock = new object();
    private Thread readThread;
    private bool isRunning;

    void Start()
    {
        if (deactivate) return;

        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.ReadTimeout = 100;
            serialPort.Open();
            isRunning = true;

            // Starte Lese-Thread
            readThread = new Thread(ReadFromPort);
            readThread.Start();

            Debug.Log("Serial port opened on " + portName);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not open serial port: " + e.Message);
        }
    }

    // Daten an Arduino/Bluetooth senden
    public void SendToArduino(string message)
    {
        if (deactivate || serialPort == null || !serialPort.IsOpen)
        {
            Debug.LogWarning("Port not open or deactivated");
            return;
        }

        ThreadPool.QueueUserWorkItem(_ =>
        {
            try
            {
                lock (portLock)
                {
                    serialPort.WriteLine(message.Replace(',', '.'));
                    Debug.Log("Sent: " + message);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Send failed: " + e.Message);
            }
        });
    }

    // Asynchron Daten lesen (optional)
    private void ReadFromPort()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string incomingData = serialPort.ReadLine();
                Debug.Log("Received: " + incomingData);
                // Hier könntest du Daten verarbeiten oder Events triggern
            }
            catch (TimeoutException)
            {
                // Ignorieren — bedeutet einfach: Kein neues Datenpaket
            }
            catch (Exception e)
            {
                Debug.LogWarning("Read failed: " + e.Message);
            }
        }
    }

    // Wichtig: sauberes Schließen beim Beenden
    void OnApplicationQuit()
    {
        isRunning = false;

        try
        {
            if (readThread != null && readThread.IsAlive)
                readThread.Join();  // Warten, bis Lese-Thread sicher beendet ist
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error stopping thread: " + e.Message);
        }

        try
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                Debug.Log("Serial port closed.");
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error closing port: " + e.Message);
        }
    }
}
