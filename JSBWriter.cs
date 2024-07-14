using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;

using System.Text;

namespace JSBSim
{


    public class JSBWriter : MonoBehaviour
    {
        TcpClient client = new TcpClient();
        NetworkStream writer;

        public bool Ready { get { return writer != null; } }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Waiting for JSBWriter to connect.");
            client.Connect(IPAddress.Parse("127.0.0.1"), Keys.Simulation.WRITE_PORT);

            Debug.Log($"JSBWriter connected to port {Keys.Simulation.WRITE_PORT}");

            writer = client.GetStream();

        }

        public void Write(string key, float value)
        {
            if (writer == null)
                return;

            byte[] b = Encoding.ASCII.GetBytes($"set {key} {value}\n");
            writer.Write(b, 0, b.Length);
        }

    }

}