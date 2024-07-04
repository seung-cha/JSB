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
    public string IPAddr = null;

    // Start is called before the first frame update

    Dictionary<string, float> data;

    TcpClient client;
    TcpListener listener;

    StreamReader reader;

    bool first = true;
    string buffer = null;
    async Task Start()
    {
        //Application.targetFrameRate = 90;
        // May make this customisable.
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1138);

        listener = new TcpListener(ep);
        listener.Start();

        Debug.Log("JSB Listener started. Start JSB Sim to receive data.");
        client = await listener.AcceptTcpClientAsync();
        reader = new StreamReader(client.GetStream());
        // StartCoroutine(ReceiveBytes());


    }


    // Update is called once per frame
    void Update()
    {
        ProcessJSBData();

        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    string[] arr = buffer.Split(',');

        //    Debug.Log(buffer);
        //    foreach(string s in arr)
        //    {
        //        Debug.Log(s);
        //    }
        //    buffer = null;
        //}
    }

    public float dx;
    public float dy;
    public float dz;

    public float angX;
    public float angY;
    public float angZ;



    void ProcessJSBData()
    {
        // Wait until JSB connects and data is flowing
        if (client == null)
            return;

        // If something is in the buffer, wait until it's consumed.
        if(buffer == null)
        {
            buffer = reader.ReadLine();
        }

        if (buffer == null)
            return;

        Debug.Log(buffer);
        if(first)
        {
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



        }


        buffer = null;
    }


    //bool first = true;
    //IEnumerator ReceiveBytes()
    //{
    //    while(true)
    //    {
    //        yield return null;
    //        byte[] d = new byte[1 << 12];
    //        var len = client.Client.Receive(d);
    //        client.
    //        // Debug.Log(System.Text.Encoding.UTF8.GetString(d));
    //        ProcessData(System.Text.Encoding.UTF8.GetString(d));
    //    }

    //}


    //void ProcessData(string dataStr)
    //{
    //    if(first)
    //    {
    //        var strs = dataStr.Split('\n');
    //        foreach(var str in strs)
    //        {
    //            Debug.Log(str);
    //        }

    //        first = false;
    //    }
    //}
}
