using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Drumsticks : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 CurrentVelocity { get; private set; }

    private Vector3 lastPosition;


    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }
    void OnCollisionStay(Collision col)
    {
        if (col.collider.CompareTag("Drum"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity *= 0.1f; // langsam abbremsen
        }
    }
}
