using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;
using LitJson;
using UnityEngine;

public class ImageInfo
    {
        public string apiKey;
      string appId;
        public string Image;
        public string Meta;
        public string Name;
        public string Size;
        public string Timestamp;
        public string Type;
        public string Signature;
        public string apiSecret;


        private string url;

        public ImageInfo(string appKey, string appId, string secrect, string imagePath, string imageName, string imageSize, string url)
        {
            this.apiKey = appKey;
            this.Image = ImageToBase64(imagePath);
            this.Meta = ImageToBase64(imagePath);
            this.Name = imageName;
            this.Size = imageSize;
            this.Timestamp = ConvertDateTimeToInt(DateTime.Now).ToString();
            this.Type = "ImageTarget";
            this.apiSecret = secrect;
            this.url = url;
            this. appId = appId;
          string tempStr = "apiKey" + appKey + "appId"+ appId+"image" + Image + "meta" + Meta + "name" + Name + "size" + imageSize + "timestamp" + Timestamp + "type" + Type + secrect;
            this.Signature = GetSHA256hash(tempStr);
        }

        /// <summary>
        ///  Determine if similar images already exist on the cloud severs
        /// </summary>
        /// <returns></returns>
        public string DoesTheImageExist()
        {
            JsonData jsonData = new JsonData();
            jsonData["apiKey"] = apiKey;
             jsonData["appId"] = appId;

          jsonData["image"] = Image;
            jsonData["timestamp"] = Timestamp;
            string str = "apiKey" + jsonData["apiKey"] + "appId"+ jsonData["appId"]+"image" + jsonData["image"] + "timestamp" + jsonData["timestamp"] + apiSecret;
            jsonData["signature"] = GetSHA256hash(str);
         
        string result = UpLoadCloudManager._SInstance.DoPost(url + "/similar", jsonData.ToJson());

        Debug.Log(result);
        JsonData re = JsonMapper.ToObject<JsonData>(result);
        if (string.IsNullOrEmpty(re["result"]["results"].ToJson()))
            {
                return "";
            }
            else
            {
                return re["result"]["results"][0]["targetId"].ToJson();
            }
        }

        /// <summary>
        /// Convert the image to Base64
        /// </summary>
        /// <param name="imagePath">Picture path</param>
        /// <returns></returns>
        private string ImageToBase64(string imagePath)
        {
            try
            {
                FileStream fs = new System.IO.FileStream(imagePath, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                string base64String = Convert.ToBase64String(buffer);
                return base64String;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        /// <summary>
        /// Converting strings to SHA256 format
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetSHA256hash(string input)
        {
      

        byte[] clearBytes = Encoding.UTF8.GetBytes(input);
            SHA256 sha256 = new SHA256Managed();
            sha256.ComputeHash(clearBytes);
            byte[] hashedBytes = sha256.Hash;
            sha256.Clear();
            string output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return output;
        }

        /// <summary>
        /// Converting date format to timestamp
        /// </summary>
        /// <param name="time">system time</param>
        /// <returns></returns>
        private long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;
            return t;
        }

        /// <summary>
        /// Change picture information into JSON format
        /// </summary>
        /// <returns></returns>

        public string ConvertToJson()
        {
            JsonData jsonData = new JsonData();
            jsonData["apiKey"] = apiKey;
            jsonData["appId"] = appId;
            jsonData["image"] = Image;
            jsonData["meta"] = Meta;
            jsonData["name"] = Name;
            jsonData["size"] = Size;
            jsonData["timestamp"] = Timestamp;
            jsonData["type"] = Type;
            jsonData["signature"] = Signature;
            return jsonData.ToJson();
        }

    }

