using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    JSBReader reader;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(FTM(reader.dx), FTM(reader.dy), FTM(reader.dz)) * Time.deltaTime);
        rb.angularVelocity = new Vector3(reader.angX, reader.angY, reader.angZ);

        float rollAngle = reader.angZ * Mathf.Rad2Deg * Time.deltaTime;  // Convert rad/sec to deg/frame
        float pitchAngle = reader.angX * Mathf.Rad2Deg * Time.deltaTime; // Convert rad/sec to deg/frame
        float yawAngle = reader.angY * Mathf.Rad2Deg * Time.deltaTime;   // Convert rad/sec to deg/frame

        // Apply rotations to the aircraft
        transform.Rotate(pitchAngle, yawAngle, rollAngle);
        //rb.angularVelocity = new Vector3(reader.angX, reader.angY, reader.angZ);
        //rb.velocity =  new Vector3(FTM(reader.dx), FTM(reader.dy), FTM(reader.dz));
        //rb.velocity = new Vector3(FTM(reader.dx), FTM(reader.dy), FTM(reader.dz)) ;
    }

    float FTM(float d)
    {
        return d / 3.281f;
    }
}
