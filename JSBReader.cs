using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;

using System.Threading.Tasks;

using System.IO;


namespace JSBSim
{
    /// <summary>
    /// TCP Listener for JSB sim. Only one instance should exist.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class JSBReader : MonoBehaviour
    {

        // JSB returns data in the order defined in <output>.
        // This makes it possible to insert data in O(n) as oppposed to using string to query: O(n^2logn)
        // We still maintain header-to-index dictionary.

        [SerializeField]
        List<float> jsbData = new List<float>();
        Dictionary<string, int> headerIndex = new Dictionary<string, int>();

        TcpClient readClient;
        TcpListener listener;
        StreamReader reader;


        bool first = true;
        string buffer = null;

        public bool Ready { get { return jsbData.Count != 0; } }


        /// <summary>
        /// Get data via string query
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException">Thrown if the provided key does not exist.</exception>
        public float GetData(string key)
        {
            if (!headerIndex.ContainsKey(key))
            {
                throw new InvalidDataException($"Provided query does not exist in JSB reader: {key}");
            }

            return jsbData[headerIndex[key]];
        }

        /// <summary>
        /// Get data via direct access. This is done in O(1).
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetData(int index)
        {
            return jsbData[index];
        }

        /// <summary>
        /// Retrieve the index of the provided key.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetKeyIndex(string key)
        {
            if (!headerIndex.ContainsKey(key))
            {
                throw new InvalidDataException($"Provided query does not exist in JSB reader: {key}");
            }

            return headerIndex[key];
        }


        async Task Start()
        {

            IPEndPoint ep;
            ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Keys.Simulation.READ_PORT);

            listener = new TcpListener(ep);
            listener.Start();

            Debug.Log($"JSB Listener started at port {Keys.Simulation.READ_PORT}. Start JSB Sim to receive data.", this);
            readClient = await listener.AcceptTcpClientAsync();
            reader = new StreamReader(readClient.GetStream());

        }


        // Update is called once per frame
        void Update()
        {
            ProcessJSBData();
        }

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
                
             //Debug.Log(buffer);

            if(first)
            {
                // First input is CSV-like titles.
                DefineHeader(buffer);
                first = false;
            }
            else
            {
                //Populate the dictionary.
                // Should not cause any error as long as the dictionary is well-formed.
                string[] data = buffer.Split(',');
                
                if(data.Length != headerIndex.Count)
                {
                    Debug.LogWarning($"No. data received is different to the no. headers defined! : {data.Length} vs {headerIndex.Count}", this);
                }

                for(int i = 0; i < jsbData.Count; i++)
                {
                    jsbData[i] = float.Parse(data[i]);
                }

            }

            buffer = null;
        }

        /// <summary>
        /// Define the dictionary using the CSV style header produced on the first output of JSB sim.
        /// </summary>
        /// <param name="csv"></param>
        void DefineHeader(string csv)
        {
            Debug.Log(csv);

            string[] columns = buffer.Split(',');

            // First column is always < LABELS >, which is useless.
            for(int i = 1; i < columns.Length; i++)
            {
                // Validate header. Should not be able to parse.
                string str = columns[i];

                float f;
                if(float.TryParse(str, out f))
                {
                    Debug.LogError("JSBReader detected a value instead of a header while initialising dictionary." +
                        $"Make sure to run JSBSim AFTER the game is run. Read: {str}", this);

                    return;
                }

                if(!headerIndex.TryAdd(str, i - 1))
                {
                    Debug.LogError($"Header {str} is already defined.", this);
                }

                //Debug.Log("Adding: " + str + "]");


            }

            jsbData = new List<float>(new float[columns.Length - 1]);

        }
    
    }

}