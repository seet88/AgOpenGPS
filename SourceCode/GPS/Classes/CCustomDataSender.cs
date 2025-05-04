using MQTTnet.Client;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;

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
            return await SendDataViaMQTTWithTopic(topic, payload);
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

                default:
                    Debug.WriteLine("Unknown command type.");
                    break;
            }
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
        mainTablesVFT
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
}
