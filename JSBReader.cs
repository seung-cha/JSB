using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading.Tasks;

using System.IO;

/// <summary>
/// TCP Listener for JSB sim. Only one instance should exist.
/// </summary>
/// <remarks>
/// 
/// </remarks>
public class JSBReader : MonoBehaviour
{
    [SerializeField]
    [Tooltip("IP address for different back-end. You should leave this empty in most cases.")]
    private string IPAddr = "";

    // Start is called before the first frame update

    readonly Dictionary<string, float> ReadData;

    TcpClient readClient;
    TcpClient writeClient;

    TcpListener listener;


    StreamReader reader;


    NetworkStream writer;

    bool first = true;
    string buffer = null;

    public float throttle;
    public float elevator;
    public float rudder;
    public float aileron;



    async Task Start()
    {
        //Application.targetFrameRate = 90;
        // May make this customisable.

        IPEndPoint ep;

        if (IPAddr.Length == 0)
        {
            ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1138);
        }
        else
        {
            ep = new IPEndPoint(IPAddress.Parse(IPAddr), 1138);
        }

        writeClient = new TcpClient();
       

        listener = new TcpListener(ep);
        listener.Start();

        Debug.Log("JSB Listener started. Start JSB Sim to receive data.");
        readClient = await listener.AcceptTcpClientAsync();
        reader = new StreamReader(readClient.GetStream());

        

        writeClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1100));
        writer = writeClient.GetStream();

    }


    // Update is called once per frame
    void Update()
    {
        ProcessJSBData();

        if (writeClient.Connected)
        {
            Debug.Log("Connected");
        }
        else
        {
            Debug.Log("Not Connected");

        }

        InputJSB();
     

    }





    void InputJSB()
    {

        // THROTTLE
        if(Input.GetKey(KeyCode.W))
        {
            throttle += Time.deltaTime;

            if (throttle > 1)
                throttle = 1;
        }
        
        if(Input.GetKey(KeyCode.S))
        {
            throttle -= Time.deltaTime;

            if (throttle < 0)
                throttle = 0;
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


        if (Input.GetKeyDown(KeyCode.Space))
        {
            byte[] a = Encoding.ASCII.GetBytes("set propulsion/magneto_cmd 3\nset propulsion/starter_cmd 1\n");
            writer.Write(a, 0, a.Length);
        }

        byte[] b = Encoding.ASCII.GetBytes($"set fcs/throttle-cmd-norm {throttle}\n");
        writer.Write(b, 0, b.Length);

        byte[] c = Encoding.ASCII.GetBytes($"set fcs/rudder-cmd-norm {rudder}\n");
        writer.Write(c, 0, c.Length);

        byte[] d = Encoding.ASCII.GetBytes($"set fcs/elevator-cmd-norm {elevator}\n");
        writer.Write(d, 0, d.Length);

        byte[] E = Encoding.ASCII.GetBytes($"set fcs/aileron-cmd-norm {aileron}\n");
        writer.Write(E, 0, E.Length);



    }






    public float dx;
    public float dy;
    public float dz;

    public float angX;
    public float angY;
    public float angZ;


    public float throt;
    public float start;
    public float mag;
    public float rud;


    public float rudderCmdNorm;
    public float rudderPosDeg;
    public float rudderPosNorm;
    public float rudderPosRad;

    public float eleCmdNorm;
    public float elePosDeg;
    public float elePosNorm;
    public float elePosRad;

    public float aileronBoth;

    public float aileronLDeg;
    public float aileronLNorm;
    public float aileronLRad;

    public float aileronRDeg;
    public float aileronRNorm;
    public float aileronRRad;


    void ProcessJSBData()
    {
        // Wait until JSB connects and data is flowing
        if (readClient == null)
            return;

        // If something is in the buffer, wait until it's consumed.
        if(buffer == null)
        {
            buffer = reader.ReadLine();
        }

        // Do nothing if buffer is empty.
        if (buffer == null)
            return;

         Debug.Log(buffer);

        if(first)
        {
            // First input is CSV-like titles.
            first = false;
        }
        else
        {
            // temp
            // time, qbar, vtotal, ubody, vbody, wbody, uaero, vaero, waero, vn, ve, vd
            string[] arr = buffer.Split(',');

            dz = float.Parse(arr[1]);
            dx = float.Parse(arr[2]);
            dy = -float.Parse(arr[3]);

            angZ = -float.Parse(arr[4]);    // Phi is roll, inverted (positive -> clockwise)
            angX = -float.Parse(arr[5]);    // Inverted
            angY = float.Parse(arr[6]);    // Psi is yaw, not inverted

            throt = float.Parse(arr[7]);
            start = float.Parse(arr[8]);
            mag = float.Parse(arr[9]);
            rud = float.Parse(arr[10]);

            rudderCmdNorm = float.Parse(arr[11]);
            rudderPosDeg = float.Parse(arr[12]);
            rudderPosNorm = float.Parse(arr[13]);
            rudderPosRad = float.Parse(arr[14]);

            eleCmdNorm = float.Parse(arr[15]);
            elePosDeg = float.Parse(arr[16]);
            elePosNorm = float.Parse(arr[17]);
            elePosRad = float.Parse(arr[18]);


            aileronBoth = float.Parse(arr[19]);

            aileronLDeg = float.Parse(arr[20]);
            aileronLNorm = float.Parse(arr[21]);
            aileronLRad = float.Parse(arr[22]);

            aileronRDeg = float.Parse(arr[23]);
            aileronRNorm = float.Parse(arr[24]);
            aileronRRad = float.Parse(arr[25]);






        }


        buffer = null;
    }


}
