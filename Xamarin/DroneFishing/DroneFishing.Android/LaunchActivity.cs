using System;
using System.Collections.Generic;
using System.Net;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Newtonsoft.Json.Linq;
using PerpetualEngine.Storage;

namespace DroneFishing.Android
{
    [Activity(Label = "DIY Drone Fishing", MainLauncher = true, Icon = "@drawable/icon")]
    public class LaunchActivity : Activity
    {
        private int servoPosition1 = 0;
        private int servoPosition2 = 180;
        private bool servoState = false; //False means hasnt moved
        private bool connected = false; // Not connected to device
        private string deviceId;
        private string accessToken;

        /// <summary>
        /// Called when the Activity is created, only once
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SimpleStorage.SetContext(ApplicationContext);

            if (bundle != null)
            {
                //Load Bundle Values
                deviceId = bundle.GetString("device_id");
                accessToken = bundle.GetString("access_token");
            }
            else
            {
                var storage = SimpleStorage.EditGroup("Devices");
                deviceId = storage.Get("device_id");
                accessToken = storage.Get("access_token");
            }

            Log.Debug(GetType().FullName, "LaunchActivity - OC - Recovered instance state");

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            FindViewById<EditText>(Resource.Id.textDeviceId).Text = deviceId;
            FindViewById<EditText>(Resource.Id.textAccessToken).Text = accessToken;

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.dropBaitButton);
            button.Click += Button_Click1;

            Button button2 = FindViewById<Button>(Resource.Id.connectDeviceButton);
            button2.Click += Button2_Click;
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //Try and connect
            EditText deviceIdText = FindViewById<EditText>(Resource.Id.textDeviceId);
            EditText accessTokenText = FindViewById<EditText>(Resource.Id.textAccessToken);

            if (Guard(accessTokenText.Text, deviceIdText.Text)) return;

            string controlUrl = "https://api.spark.io/v1/devices/{0}?access_token={1}";
            controlUrl = String.Format(controlUrl, deviceIdText.Text, accessTokenText.Text);

            using (WebClient client = new WebClient())
            {
                try
                {
                    string response = client.UploadString(controlUrl, "GET");
                    Log.Debug(GetType().FullName, String.Format("Response received was :{0}", response));

                    connected = bool.Parse(JObject.Parse(response)["connected"].Value<string>());
                    if (connected)
                    {
                        Button button = FindViewById<Button>(Resource.Id.dropBaitButton);
                        button.Enabled = true;
                    }
                    else
                    {
                        Button button = FindViewById<Button>(Resource.Id.dropBaitButton);
                        button.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    //Exception@!
                    Log.Error(GetType().FullName, String.Format("ERROR was :{0}", ex.Message));
                }
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            EditText deviceIdText = FindViewById<EditText>(Resource.Id.textDeviceId);
            EditText accessTokenText = FindViewById<EditText>(Resource.Id.textAccessToken);

            if (Guard(accessTokenText.Text, deviceIdText.Text)) return;

            outState.PutString("device_id", deviceIdText.Text);
            outState.PutString("access_token", accessTokenText.Text);

            var storage = SimpleStorage.EditGroup("Devices");
            storage.Put("device_id", deviceIdText.Text);
            storage.Put("access_token", accessTokenText.Text);

            Log.Debug(GetType().FullName, "LaunchActivity - Saving instance state");

            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            //Load Bundle Values
            deviceId = savedInstanceState.GetString("device_id");
            accessToken = savedInstanceState.GetString("access_token");

            FindViewById<EditText>(Resource.Id.textDeviceId).Text = deviceId; 
            FindViewById<EditText>(Resource.Id.textAccessToken).Text = accessToken;

            Log.Debug(GetType().FullName, "LaunchActivity - Recovered instance state");
            base.OnRestoreInstanceState(savedInstanceState);

        }

        private void Button_Click1(object sender, EventArgs e)
        {
            EditText deviceIdText = FindViewById<EditText>(Resource.Id.textDeviceId);
            EditText accessTokenText = FindViewById<EditText>(Resource.Id.textAccessToken);
            int servoPosition;

            if (Guard(accessTokenText.Text, deviceIdText.Text)) return;

            if (!servoState)
            {
                servoPosition = servoPosition2;
                servoState = true;
                FindViewById<Button>(Resource.Id.dropBaitButton).Text = "Return to closed position.";
            }
            else
            {
                servoPosition = servoPosition1;
                servoState = false;
                FindViewById<Button>(Resource.Id.dropBaitButton).Text = "Drop the bait!.";
            }

            string controlUrl = "https://api.spark.io/v1/devices/{0}/servo";
            controlUrl = String.Format(controlUrl, deviceIdText.Text);

            using (WebClient client = new WebClient())
            {
                var reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("access_token", accessTokenText.Text);
                reqparm.Add("args", servoPosition.ToString());
                try
                {
                    byte[] responsebytes = client.UploadValues(controlUrl, "POST", reqparm);
                    Log.Debug(GetType().FullName, String.Format("Response received was :{0}", System.Text.Encoding.ASCII.GetString(responsebytes)));
                }
                catch (Exception ex)
                {
                    //Exception@!
                    Log.Error(GetType().FullName, String.Format("ERROR was :{0}", ex.Message));
                }
            }

        }

        private static bool Guard(String accessTokenText, String deviceIdText)
        {
            //Control the servo
            if (String.IsNullOrEmpty(accessTokenText))
            {
                //problem
                return true;
            }

            if (String.IsNullOrEmpty(deviceIdText))
            {
                //Problem
                return true;
            }
            return false;
        }
    }
}