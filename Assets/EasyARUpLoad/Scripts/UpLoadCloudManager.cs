using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;


    public class UpLoadCloudManager : MonoBehaviour
    {
        public static UpLoadCloudManager _SInstance;
        public string apiKey;
       public string appId;
        public string apiSecret;
        public string cloudSeverUrl;

        [HideInInspector]
        public string TargetId = "";


        private UpLoadCloudManager() { }
        private ImageInfo imageInfo = null;

        private void Awake()
        {
            _SInstance = this;
        }
        private void Start()
        {
            
        }
        /// <summary>
        /// Upload pictures to cloud servers
        /// </summary>
        /// <param name="imagePath"> The path of uploading pictures </param>
        public void UpLoadToCloud(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                imageInfo = new ImageInfo(apiKey,appId, apiSecret, imagePath, DateTime.Now.ToString(), 20.ToString(), cloudSeverUrl);
                Thread thread = new Thread(UpLoadCloud);
                thread.Start();
            }
            else
            {
                Debug.LogError("File not exist");
            }
        }

        /// <summary>
        /// Send images to the cloud
        /// </summary>
        /// <param name="json"> required parameter </param>
        /// <returns></returns>
        public string DoPost(string url, string jsonStr)
        {
            string _url = url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            byte[] byteData = Encoding.UTF8.GetBytes(jsonStr);
            int length = byteData.Length;
            request.ContentLength = length;
            Stream writer = request.GetRequestStream();
            writer.Write(byteData, 0, length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
        }

        private void UpLoadCloud()
        {
               string targetId = imageInfo.DoesTheImageExist();
   
            if (targetId == "")
            {

                string result = DoPost(cloudSeverUrl + "/targets", imageInfo.ConvertToJson());

              Debug.Log(result);
                JsonData re = JsonMapper.ToObject<JsonData>(result);
                this.TargetId = (string)re["result"]["targetId"];
            }
            else
            {
              
                this.TargetId = targetId;
            }
        }

    }
