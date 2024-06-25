using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading.Tasks;

public class JSBReader : MonoBehaviour
{
    // Start is called before the first frame update

    Dictionary<string, float> data;

    UdpClient client;
    IPEndPoint ep;
    void Start()
    {
        client = new UdpClient();
        ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1138);

        client.Client.Bind(ep);
        StartCoroutine(ReceiveBytes());
    }


    // Update is called once per frame
    void Update()
    {

    }


    bool first = true;
    IEnumerator ReceiveBytes()
    {
        while(true)
        {
            yield return null;
            byte[] d = new byte[1 << 12];
            var len = client.Client.Receive(d);
            // Debug.Log(System.Text.Encoding.UTF8.GetString(d));
            ProcessData(System.Text.Encoding.UTF8.GetString(d));
        }

    }


    void ProcessData(string dataStr)
    {
        if(first)
        {
            var strs = dataStr.Split('\n');
            foreach(var str in strs)
            {
                Debug.Log(str);
            }

            first = false;
        }
    }
}
