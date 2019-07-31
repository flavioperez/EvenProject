using System.Net;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Tests
{
    public class Tests
    {
        private readonly string UrlBase;
        private readonly string AppKey;
        private readonly string SumFunctionName;

        public Tests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("jsconfig.json")
                .Build();

            UrlBase = config["urlBase"];
            AppKey = config["functionAppKey"];
            SumFunctionName = config["testNumberFunctionName"];
        }

        [Test]
        public void Get_WithNaturalParameterUndefined_ErrorRespose()
        {
            var client = MakeClient();
            var request = MakeSumRequest();
            var response = client.Get(request);

            Assert.AreEqual("\"Please pass a natural number (positve integer) natural on the query string or in the request body\"", response.Content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void Post_WithNaturalDefinedButNoNaturalValue_ErrorRespose()
        {
            var client = MakeClient();
            var request = MakeSumRequest();
            request.AddQueryParameter("natural", "badvalue");
            var response = client.Post(request);

            Assert.AreEqual("\"Please pass a natural number (positve integer) natural on the query string or in the request body\"", response.Content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void Post_WithNaturalWellDefined_ReturnTrue()
        {
            var client = MakeClient();
            var request = MakeSumRequest();
            request.AddQueryParameter("natural", "2");
            var response = client.Post(request);

            Assert.AreEqual("\"Is Even: True\"", response.Content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        public void Post_WithNaturalWellDefined_ReturnFalse()
        {
            var client = MakeClient();
            var request = MakeSumRequest();
            request.AddQueryParameter("natural", "1");
            var response = client.Post(request);

            Assert.AreEqual("\"Is Even: False\"", response.Content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        private IRestClient MakeClient()
        {
            RestClient client = new RestClient(UrlBase);
            return client;
        }

        private IRestRequest MakeSumRequest()
        {
            RestRequest request = new RestRequest(SumFunctionName, Method.GET);
            request.AddQueryParameter("code", AppKey);
            return request;
        }
    }
}