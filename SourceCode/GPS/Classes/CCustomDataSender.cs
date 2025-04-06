using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgOpenGPS.Classes
{
    public class CCustomDataSender
    {

        //pointers to mainform controls
        private readonly FormGPS mf;
        private string mqttUrl = "";
        private string mqttPort = "";
        private string mqttUser = "";
        private string mqttPass = "";
        private string mqttTopic = "";

        private string traccarUrl = "";
        private string traccarClientId = "";

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
        }


    }
}
