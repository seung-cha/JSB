using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAirplane : MonoBehaviour
{
    const float FTM = 1 / 3.281f;

    JSBSim.JSBWriter writer;
    JSBSim.JSBReader reader;

    Rigidbody rigidBody;

    [SerializeField]
    float throttle = 0,
        aileron = 0,
        rudder = 0,
        elevator = 0;

    // Start is called before the first frame update
    void Start()
    {
        writer = GetComponent<JSBSim.JSBWriter>();
        reader = GetComponent<JSBSim.JSBReader>();

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;   // Don't use gravity as JSBsim already simulates it.

        // Get the keys from JSBsim

        
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplyPhysics();
        ControlAircraft();
    }

    void ControlAircraft()
    {
        if (!writer.Ready)
        { 
            Debug.Log("Waiting for writer to be ready.");
            return;
        }

        // Temporary input bounds
        // Keys:
        //  W, S -> Throttle
        //  A, D -> Rudder
        //  E, Q -> Elevator
        //  C,Z -> Aileron

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Start aircraft");
            writer.Write(JSBSim.Keys.Propulsion.W_STARTER_CMD, 1);
            writer.Write(JSBSim.Keys.Propulsion.W_MAGNETO_CMD, 3);
        }

        // THROTTLE
        if (Input.GetKey(KeyCode.W))
        {
            throttle += Time.deltaTime;

            if (throttle > 1)
                throttle = 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            throttle -= Time.deltaTime;

            if (throttle < -1)
                throttle = -1;
        }

        // RUDDER
        if (Input.GetKey(KeyCode.D))
        {
            rudder -= Time.deltaTime;

            if (rudder < -1)
                rudder = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rudder += Time.deltaTime;

            if (rudder > 1)
                rudder = 1;
        }

        // ELEVATOR
        if (Input.GetKey(KeyCode.E))
        {
            elevator -= Time.deltaTime;

            if (elevator < -1)
                elevator = -1;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            elevator += Time.deltaTime;

            if (elevator > 1)
                elevator = 1;
        }


        // AILERON
        if (Input.GetKey(KeyCode.C))
        {
            aileron -= Time.deltaTime;

            if (aileron < -1)
                aileron = -1;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            aileron += Time.deltaTime;

            if (aileron > 1)
                aileron = 1;
        }



        writer.Write(JSBSim.Keys.FCS.W_THROTTLE_CMD_NORM, throttle);
        writer.Write(JSBSim.Keys.FCS.W_RUDDER_CMD_NORM, rudder);
        writer.Write(JSBSim.Keys.FCS.W_AILERON_CMD_NORM, aileron);
        writer.Write(JSBSim.Keys.FCS.W_ELEVATOR_CMD_NORM, elevator);


    }

    void ApplyPhysics()
    {
        // We don't use Rigidbody to apply velocities.
        // JSBSim itself calculates velocities.
        if (!reader.Ready)
        {
            Debug.Log("Waiting for JSBSim to be ready.");
            return;
        }


            float dx = reader.GetData(JSBSim.Keys.Velocities.V_FPS);
            float dy = -reader.GetData(JSBSim.Keys.Velocities.W_FPS);
            float dz = reader.GetData(JSBSim.Keys.Velocities.U_FPS);

            float angX = -reader.GetData(JSBSim.Keys.Velocities.Q_RAD_SEC) * Mathf.Rad2Deg;
            float angY = reader.GetData(JSBSim.Keys.Velocities.R_RAD_SEC) * Mathf.Rad2Deg;
            float angZ = -reader.GetData(JSBSim.Keys.Velocities.P_RAD_SEC) * Mathf.Rad2Deg;

        //Debug.Log("angY: " + angY);
        //Debug.Log("AngY Key: " + reader.GetKeyIndex(JSBSim.Keys.Velocities.R_RAD_SEC));

            transform.Translate(new Vector3(dx, dy, dz) * Time.deltaTime * FTM);  // To meter
            transform.Rotate(new Vector3(angX, angY, angZ) * Time.deltaTime);
    }

  
}
