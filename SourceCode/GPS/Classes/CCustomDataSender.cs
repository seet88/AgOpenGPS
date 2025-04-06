using MQTTnet.Client;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
            await mqttClient.SubscribeAsync(topic);
            Console.WriteLine("Subscribed to topic 'test/topic'. Waiting for messages...");
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                var z = e.ApplicationMessage.PayloadSegment.Array;
                Debug.WriteLine($"Received message: {Encoding.UTF8.GetString(z)}");
                return Task.CompletedTask;
            };

            // Publish a message to the topic
            var message = new MQTTnet.MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload("Hello MQTT with credentials!")
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




    }
}
