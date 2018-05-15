using BE;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Converter
{
    class ServiceGateway
    {
        string connection = "http://localhost:64943/api/customer/";

        public void postMany(List<Customer> lst)
        {
            RestClient client = new RestClient();
            // You may need to change the port number in the BaseUrl below
            // before you can run the request.
            client.BaseUrl = new Uri(connection);
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(lst), ParameterType.RequestBody);
            var response = client.Execute<Customer>(request);
        }
    }
}
