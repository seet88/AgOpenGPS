using MQTTnet.Client;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using AgOpenGPS.Forms.Field;

namespace AgOpenGPS.Classes
{
    public class CCustomDataSender
    {

        //pointers to mainform controls
        private readonly FormGPS mf;
        private string mqttUrl = "";
        private int mqttPort = 0;
        private string mqttUser = "";
        private string mqttPass = "";
        private string mqttTopic = "";

        private string traccarUrl = "";
        private string traccarClientId = "";

        private string topic = "test1";
        private string subTopic = "customCommands";


        IMqttClient mqttClient;

        public CCustomDataSender(FormGPS _f)
        {
            //constructor
            mf = _f;

            mqttUrl = Properties.Settings.Default.setMQTT_URL;
            mqttPort = Properties.Settings.Default.setMQTT_port;
            mqttUser = Properties.Settings.Default.setMQTT_user_name;
            mqttPass = Properties.Settings.Default.setMQTT_user_password;

            traccarUrl = Properties.Settings.Default.setTraccar_url;
            traccarClientId = Properties.Settings.Default.setTraccar_client_id;
            InitMQTTClient();
        }

        public async void InitMQTTClient()
        {
            // Define the MQTT client
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();

            // Define MQTT client options with username and password
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttUrl, mqttPort) // Replace with your MQTT broker address
                .WithCredentials(mqttUser, mqttPass) // Set your username and password here
                .WithTls() // Use TLS if required
                .Build();

            //// Set up handlers for connection, disconnection, and message reception
            //mqttClient.UseConnectedHandler(e =>
            //{
            //    Console.WriteLine("Connected to MQTT broker.");
            //});

            //mqttClient.UseDisconnectedHandler(e =>
            //{
            //    Console.WriteLine("Disconnected from MQTT broker.");
            //});

            //mqttClient.UseApplicationMessageReceivedHandler(e =>
            //{
            //    Console.WriteLine("Message received:");
            //    Console.WriteLine($"Topic: {e.ApplicationMessage.Topic}");
            //    Console.WriteLine($"Payload: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            //});

            // Connect to the broker asynchronously
            try
            {
                var res = await mqttClient.ConnectAsync(options);
                Console.WriteLine("Connected successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to broker: " + ex.Message);
                return;
            }

            // Subscribe to a topic
            await mqttClient.SubscribeAsync(subTopic);
            Console.WriteLine("Subscribed to topic 'test/topic'. Waiting for messages...");
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                var payloadBytes = e.ApplicationMessage.PayloadSegment.Array;
                if (payloadBytes == null) return Task.CompletedTask;
                var payloadString = Encoding.UTF8.GetString(payloadBytes);
                Debug.WriteLine($"Received message: {payloadString}");
                //MqttMessage obj = JsonSerializer.Deserialize<MqttMessage>(payload);
                ParseIncomingCommnads(payloadString);
                return Task.CompletedTask;
            };


            string messagePayload = JsonSerializer.Serialize(new MqttMessage() { msgType = MQTTMessageType.hello, value = "Hello from AgOpenGPS", timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });


            // Publish a message to the topic
            var message = new MQTTnet.MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(messagePayload)
                .Build();
            await mqttClient.PublishAsync(message);
            Console.WriteLine("Message sent to topic 'test/topic'.");

            // Keep the application running to receive messages
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();

            // Disconnect from the broker
            //await mqttClient.DisconnectAsync();
        }

        public async Task<bool> SendDataViaMQTT(string payload)
        {
            var res = await SendDataViaMQTTWithTopic(topic, payload);
            return res;
        }

        public async Task<bool> SendDataViaMQTTWithTopic(string topic, string payload)
        {
            if (mqttClient == null || !mqttClient.IsConnected)
            {
                InitMQTTClient();
                Console.WriteLine("MQTT client is not connected.");
                return false;
            }
            var message = new MQTTnet.MqttApplicationMessageBuilder()
               .WithTopic(topic)
               .WithPayload(payload)
               .Build();
            var res = await mqttClient.PublishAsync(message);
            return res.ReasonCode == MqttClientPublishReasonCode.Success;

        }

        public void ParseIncomingCommnads(string payload)
        {
            // Parse the incoming commands from the payload
            // You can implement your logic here to handle different commands
            Debug.WriteLine($"Received command: {payload}");
            MqttReceiveMessage obj = JsonSerializer.Deserialize<MqttReceiveMessage>(payload);
            if (obj == null) return;
            switch (obj.msgType)
            {
                case MQTTCommandType.sendTaskData:
                    // Handle sendTaskData command
                    Debug.WriteLine("Received sendTaskData command.");
                    mf.customMqttMessages.SendTaskMetadata();
                    break;
                case MQTTCommandType.sendMainTablesVFT:
                    // Handle sendMainTablesVFT command
                    Debug.WriteLine("Received sendMainTablesVFT command.");
                    mf.customMqttMessages.SendMainTablesVFT();
                    break;
                case MQTTCommandType.sendLocalCordsToGPSStatics:
                    // Handle sendlocalCordsToGPSStatics command
                    Debug.WriteLine("Received sendlocalCordsToGPSStatics command.");
                    break;
                case MQTTCommandType.sendBoundary:
                    // Handle sendBoundary command
                    Debug.WriteLine("Received sendBoundary command.");
                    break;
                case MQTTCommandType.sendCurrentNavigationLine:
                    // Handle sendCurrentNavigationLine command
                    Debug.WriteLine("Received sendCurrentNavigationLine command.");
                    break;
                case MQTTCommandType.sendSectionsInfo:
                    // Handle sendSectionsInfo command
                    Debug.WriteLine("Received sendSectionsInfo command.");
                    break;
                case MQTTCommandType.sendInputIoTProps:
                    // Handle sendInputIoTProps command
                    Debug.WriteLine("Received sendInputIoTProps command.");
                    MqttInputIoTProps config = JsonSerializer.Deserialize<MqttInputIoTProps>(obj.value.ToString());
                    HandleInputIotConfig(config);
                    break;

                default:
                    Debug.WriteLine("Unknown command type.");
                    break;
            }
        }


        public void HandleInputIotConfig(MqttInputIoTProps config)
        {
            // Handle the Input IoT configuration
            // You can implement your logic here to process the configuration
            Debug.WriteLine("Received Input IoT configuration.");
            // Example: Update settings based on the received configuration
            // mf.UpdateIoTSettings(config.inputIoTProps);
            var inputIotProps = config.InputIoTProps;
            switch (config.ProtocolType)
            {
                case ProtocolType.UDP:
                    Debug.WriteLine("Protocol Type: UDP");
                    HandleInputIotDataViaUDP(config);
                    break;
                case ProtocolType.HttpGet:
                    Debug.WriteLine("Protocol Type: HTTP-GET");
                    break;
                case ProtocolType.HttpPost:
                    Debug.WriteLine("Protocol Type: HTTP-POST");
                    break;
                default:
                    Debug.WriteLine("Unknown Protocol Type");
                    break;
            }
            //split inputIotProps to Dictionary<string, string>
            //var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(inputIotProps.ToString());
            //if (dict == null) return;
            //foreach (var kvp in dict)
            //{
            //    Debug.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
            //    // Update settings based on the received key-value pairs
            //    // Example: mf.UpdateIoTSetting(kvp.Key, kvp.Value);
            //}
            ////string url = "http://192.168.55.70/number/test_number/set?value=60";
            ////var task = SendGetRequestAsync(url, null);
            //string msg = "{ \"test_number\":15,\"received_temperature2\":18,\"name\":\"frug234o\"}";
            //var res = SendUdpRequestAsync("192.168.55.255", 8400, msg);
            //var z = res.Result;
        }

        public void HandleInputIotDataViaUDP(MqttInputIoTProps config)
        {
            string ip = "192.168.55.255";
            Dictionary<string, object> convertedData = config.InputIoTProps;

            ToolProtocolConfigBase inputConfig = mf?.toolCustom?.config?.Input;
            if (inputConfig != null)
            {
                convertedData = KeyMapper.ConvertKeys(inputConfig, config.InputIoTProps, false);
            }
            var res = SendUdpRequestAsync(ip, config.Port ?? 8401, JsonSerializer.Serialize(convertedData));
            var z = res.Result;
        }


        /// <summary>
        /// Sends a message via UDP and waits for a response (with timeout).
        /// </summary>
        public static async Task<string> SendUdpRequestAsync(string ip, int port, string message, int timeoutMs = 2000)
        {
            using (UdpClient udpClient = new UdpClient())
            {
                Console.WriteLine($"Sending UDP to {ip}:{port}...");

                // 1. Send
                udpClient.Connect(ip, port);
                byte[] sendBytes = Encoding.UTF8.GetBytes(message);
                await udpClient.SendAsync(sendBytes, sendBytes.Length);

                // 2. Receive with Timeout
                // UDP receive blocks indefinitely, so we use Task.WhenAny to implement a timeout.
                Console.WriteLine("Waiting for response...");

                var receiveTask = udpClient.ReceiveAsync();
                var delayTask = Task.Delay(timeoutMs);

                var completedTask = await Task.WhenAny(receiveTask, delayTask);

                if (completedTask == receiveTask)
                {
                    // Success: Data received before timeout
                    var result = await receiveTask;
                    return Encoding.UTF8.GetString(result.Buffer);
                }
                else
                {
                    // Timeout
                    return "Timeout: No response received from server.";
                }
            }
        }


        /// <summary>
        /// Sends a GET request using HttpWebRequest (Native .NET 4.8)
        /// </summary>
        public static async Task<string> SendGetRequestAsync(string url, Dictionary<string, string> parameters)
        {
            // 1. Build Query String
            string queryString = BuildQueryString(parameters);
            string finalUrl = url + queryString;

            Console.WriteLine($"Requesting: {finalUrl}");

            // 2. Create Request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);
            request.Method = "GET";

            // 3. Get Response
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Sends a POST request using HttpWebRequest (Native .NET 4.8)
        /// </summary>
        public static async Task<string> SendPostRequestAsync(string url, Dictionary<string, string> parameters)
        {
            Console.WriteLine($"Posting to: {url}");

            // 1. Create Request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // 2. Prepare Data
            // In .NET 4.8 POST, we must write the parameters as a string to the request stream manually
            string postData = "";
            if (parameters != null && parameters.Count > 0)
            {
                // We strip the leading '?' from the query string helper for the body
                postData = BuildQueryString(parameters).TrimStart('?');
            }

            byte[] data = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;

            // 3. Write Data to Request Stream
            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                await requestStream.WriteAsync(data, 0, data.Length);
            }

            // 4. Get Response
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Helper to build "key=value&key2=value2" string
        /// </summary>
        private static string BuildQueryString(Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return "";

            var queryParams = parameters.Select(p =>
                $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"
            );

            return "?" + string.Join("&", queryParams);
        }




    }

    public enum MQTTCommandType
    {
        sendTaskData,
        sendLocalCordsToGPSStatics,
        sendBoundary,
        sendCurrentNavigationLine,
        sendSectionsInfo,
        sendMainTablesVFT,
        sendInputIoTProps,

    }


    public enum MQTTMessageType
    {
        currentABLine,
        sectionsInfo,
        currentCurveLine,
        localCordsToGPSStatics,
        boundary,
        hello,
        taskMetadata,
        mainTablesVFT,
        customIoTData,
    }

    public class MqttReceiveMessage
    {
        public MQTTCommandType msgType { get; set; }
        public object value { get; set; }
        public string timestamp { get; set; }
        public string clientId { get; set; }
    }

    public class MqttMessage
    {
        public MQTTMessageType msgType { get; set; }
        public object value { get; set; }
        public string timestamp { get; set; }
        public string clientId { get; set; }
    }

    //public class InputIoTConfig
    //{        public object inputIoTProps { get; set; }
    //}

    /// <summary>
    /// Defines the allowed protocol types.
    /// The JsonConverter attribute ensures these serialize as strings ("HTTP-GET") 
    /// rather than integers (0, 1, 2).
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProtocolType
    {
        UDP,

        [JsonPropertyName("HTTP-GET")]
        HttpGet,

        [JsonPropertyName("HTTP-POST")]
        HttpPost
    }

    public class MqttInputIoTProps
    {
        /// <summary>
        /// Maps to: { [key: string]: string | number | boolean | object | undefined }
        /// We use Dictionary<string, object> to handle the dynamic value types.
        /// </summary>
        [JsonPropertyName("inputIoTProps")]
        public Dictionary<string, object> InputIoTProps { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Maps to: 'UDP' | 'HTTP-GET' | 'HTTP-POST'
        /// </summary>
        [JsonPropertyName("protocolType")]
        public ProtocolType ProtocolType { get; set; }

        /// <summary>
        /// Maps to: lastIPNumber?: number
        /// Nullable int (int?) handles the optional nature.
        /// </summary>
        [JsonPropertyName("lastIPNumber")]
        public int? LastIPNumber { get; set; }

        /// <summary>
        /// Maps to: port?: number
        /// </summary>
        [JsonPropertyName("port")]
        public int? Port { get; set; }
    }


    public static class KeyMapper
    {
        /// <summary>
        /// Creates a dictionary mapping IotKeys to SmsKeys from a configuration's configMap.
        /// </summary>
        /// <param name="config">The configuration object (UDP or HTTP).</param>
        /// <returns>A dictionary where Key=IotKey and Value=SmsKey.</returns>
        private static Dictionary<string, string> GetKeyMapping(ToolProtocolConfigBase config, bool isOutput = true)
        {
            IEnumerable<ToolConfigMapValueBase> configMap = null;

            if (config is ToolConfigHTTP httpConfig)
            {
                configMap = httpConfig.ConfigMap;
            }
            else if (config is ToolConfigUDP udpConfig)
            {
                configMap = udpConfig.ConfigMap;
            }
            else
            {
                // Return empty dictionary if config is null or an unexpected type
                return new Dictionary<string, string>();
            }

            // Create the lookup dictionary (IotKey -> SmsKey)
            // Using ToDictionary with StringComparer for case-insensitive lookup
            try
            {
                if (isOutput)
                {

                    return configMap
                        .Where(item => !string.IsNullOrEmpty(item.IotKey) && !string.IsNullOrEmpty(item.SmsKey))
                        .ToDictionary(
                            item => item.IotKey,
                            item => item.SmsKey,
                            StringComparer.OrdinalIgnoreCase
                        );
                }
                else
                {
                    return configMap
                    .Where(item => !string.IsNullOrEmpty(item.SmsKey) && !string.IsNullOrEmpty(item.IotKey))
                    .ToDictionary(
                        item => item.SmsKey,
                        item => item.IotKey,
                        StringComparer.OrdinalIgnoreCase
                    );
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error creating key map: Duplicate 'iotKey' found in configMap. Details: {ex.Message}");
                // Handle the error gracefully by grouping and taking the first mapping for duplicates
                if (isOutput)
                {
                    return configMap
                        .Where(item => !string.IsNullOrEmpty(item.IotKey) && !string.IsNullOrEmpty(item.SmsKey))
                        .GroupBy(item => item.IotKey, StringComparer.OrdinalIgnoreCase)
                        .ToDictionary(
                            g => g.Key,
                            g => g.First().SmsKey,
                            StringComparer.OrdinalIgnoreCase
                        );
                }
                else
                {
                    return configMap
                        .Where(item => !string.IsNullOrEmpty(item.SmsKey) && !string.IsNullOrEmpty(item.IotKey))
                        .GroupBy(item => item.SmsKey, StringComparer.OrdinalIgnoreCase)
                        .ToDictionary(
                            g => g.Key,
                            g => g.First().IotKey,
                            StringComparer.OrdinalIgnoreCase
                        );

                }
            }
        }


        /// <summary>
        /// Converts the keys in a dynamic data object from iotKey to the mapped smsKey 
        /// based on the provided configuration, handling key collisions.
        /// </summary>
        /// <param name="config">The protocol configuration (Input or Output) containing the configMap.</param>
        /// <param name="dynamicData">A dictionary representing the dynamic incoming data.</param>
        /// <returns>A new dictionary with keys converted to smsKeys where applicable.</returns>
        public static Dictionary<string, object> ConvertKeys(
            ToolProtocolConfigBase config,
            Dictionary<string, object> dynamicData,
            bool isOutput = true
        )
        {
            if (config == null || dynamicData == null)
            {
                return dynamicData ?? new Dictionary<string, object>();
            }

            // 1. Get the mapping table (IotKey -> SmsKey)
            var keyMap = GetKeyMapping(config, isOutput);

            // 2. Create the new dictionary to hold the transformed data
            var transformedData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // 3. Iterate through the keys in the incoming dynamic data
            foreach (var kvp in dynamicData)
            {
                string originalKey = kvp.Key;
                object value = kvp.Value;

                // Try to find a mapped smsKey
                if (keyMap.TryGetValue(originalKey, out string smsKey))
                {
                    // Case 1: Key is mapped (iotKey -> smsKey)
                    // Use the mapped smsKey as the new key.
                    // Using indexer assignment ([key] = value) which overwrites if it somehow exists, 
                    // avoiding the ArgumentException that Add() would throw.
                    transformedData[smsKey] = value;
                    Console.WriteLine($"Key '{originalKey}' successfully mapped to '{smsKey}'.");
                }
                else
                {
                    // Case 2: Key is NOT mapped (Unmapped key)
                    // Check if this unmapped key conflicts with an already mapped smsKey.
                    if (!transformedData.ContainsKey(originalKey))
                    {
                        // No conflict, keep the original key
                        transformedData.Add(originalKey, value);
                    }
                    else
                    {
                        // Collision detected! The unmapped key (e.g., 'T1') is identical 
                        // to a key that was just added (e.g., 'current_temp' mapped to 'T1').
                        // We prioritize the mapped key and ignore the unmapped key to prevent the ArgumentException.
                        Console.WriteLine($"WARNING: Unmapped key '{originalKey}' ignored due to collision with a mapped key.");
                    }
                }
            }

            return transformedData;
        }
    }
}
