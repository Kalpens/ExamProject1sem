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
        string connection = "https://customerexamproject.appspot.com/api/customer";

        public void postMany(List<Customer> lst)
        {
            RestClient client = new RestClient();
            // You may need to change the port number in the BaseUrl below
            // before you can run the request.
            client.BaseUrl = new Uri(connection);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(lst);
            client.Execute(request);
        }
    }
}
