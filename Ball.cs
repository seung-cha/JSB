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
        //rb.velocity =  new Vector3(FTM(reader.dx), FTM(reader.dy), FTM(reader.dz));
        rb.velocity = transform.InverseTransformDirection(new Vector3(FTM(reader.dx), FTM(reader.dy), FTM(reader.dz)));
        rb.angularVelocity = new Vector3(reader.angX, reader.angY, reader.angZ);
    }

    float FTM(float d)
    {
        return d / 3.281f;
    }
}
