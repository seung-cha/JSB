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
        // rb.velocity = transform.InverseTransformDirection(new Vector3(FTM(reader.dx), FTM(reader.dy), FTM(reader.dz)));
        transform.position = new Vector3(0.0f, FTM(reader.dx), 0.0f);
    }

    float FTM(float d)
    {
        return d / 3.281f;
    }
}
