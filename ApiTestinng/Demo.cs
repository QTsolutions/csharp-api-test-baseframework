using ApiTestinng;
using ApiTestinng.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace ApiTesting
{
    public class Demo
    {
        public static APIHelper user;
        public static RestClient url;
        public static RestResponse response;
        public void getResponse(RestClient url, RestRequest request)
        {
            response = user.GetResponse(url, request);
            
        }

        public void setUp(string endpoint)
        {
             user = new APIHelper();
             url = user.SetUrl(endpoint);
        }
        public void GetStatusCode(int code)
        {
            HttpStatusCode statusCode = response.StatusCode;
            int numericCode = (int)statusCode;
            Assert.IsTrue(numericCode.Equals(code));
        }

        public ListOfUsersDTO GetUsers(string endpoint)
        {
            Demo d = new Demo();
            d.setUp(endpoint);
            RestRequest request = user.CreateGetRequest();
            d.getResponse(url, request);
            d.GetStatusCode(200);
            ListOfUsersDTO content = user.GetContent<ListOfUsersDTO>(response);
            return content;
        }

        public CreateUserDTO CreateUser(string endpoint,dynamic payload)
        {
            Demo d = new Demo();
            d.setUp(endpoint);
            var jsonReq = user.Serialize(payload);
            var request = user.CreatePostRequest(jsonReq);
            d.getResponse(url, request);
            d.GetStatusCode(201);
            CreateUserDTO content = user.GetContent<CreateUserDTO>(response);
            return content;
        }

        public UpdateUserDTO UpdateUser(string endpoint,dynamic payload)
        {
            Demo d = new Demo();
            d.setUp(endpoint);
            var request = user.CreatePutRequest(payload);
            d.getResponse(url, request);
            d.GetStatusCode(200);
            UpdateUserDTO content = user.GetContent<UpdateUserDTO>(response);
            return content;
        }
        
        public ListOfUsersDTO DeleteUser(string endpoint)
        {
            Demo d = new Demo();
            d.setUp(endpoint);
            var request = user.CreateDeleteRequest();
            d.getResponse(url, request);
            ListOfUsersDTO content = user.GetContent<ListOfUsersDTO>(response);
            return content;
        }
    }

}
