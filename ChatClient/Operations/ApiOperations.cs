using ChatClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatClient.Operations
{
    class ApiOperations
    {
        public ApiOperations()
        {
            baseUrl = "http://localhost:5000/api";
        }

        private string baseUrl;

        public User AuthenticateUser(string username, string password)
        {
            string endpoint = string.Format(baseUrl, "/users/login");
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                username = username,
                password = password
            });

            WebClient wc = new();
            wc.Headers["Content-Type"] = "application/json";

            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<User>(response);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public User GetUserDetails(User user)
        {
            string endpoint = string.Format(baseUrl, "/users/", user.Id);
            string accessToken = user.AccessToken;

            WebClient wc = new();
            wc.Headers["Content-Type"] = "application/json";
            wc.Headers["Authorization"] = accessToken;

            try
            {
                string response = wc.DownloadString(endpoint);

                user = JsonConvert.DeserializeObject<User>(response);
                user.AccessToken = accessToken;

                return user;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public User RegisterUser(string username, string password, string firstname,
    string lastname, string middlename, int age)
        {
            string endpoint = baseUrl + "/users";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                username = username,
                password = password,
                firstname = firstname,
                lastname = lastname,
                middlename = middlename,
                age = age
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<User>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
