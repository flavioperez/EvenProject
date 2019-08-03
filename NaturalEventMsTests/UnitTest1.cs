using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaturalEven;

namespace NaturalEventMsTests
{
    [TestClass]
    public class UnitTest1: FunctionTestHelper.FunctionTest
    {
        [TestMethod]
        public async Task Request_WithNaturalParameterUndefined_ErrorRespose()
        {
            var query = new Dictionary<String, StringValues>();
            var body = "";
            var logger = Mock.Of<ILogger>(); //using Moq for example
            var result = await Function1.Run(req: HttpRequestSetup(query, body), log: logger);
            var resultObject = (BadRequestObjectResult)result;
            Assert.AreEqual("Please pass a natural number (positve integer) natural on the query string or in the request body", resultObject.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, resultObject.StatusCode.Value);
        }

        [TestMethod]
        public async Task Request_WithNaturalDefinedButNoNaturalValue_ErrorRespose()
        {
            var query = new Dictionary<String, StringValues>();
            query.TryAdd("natural", "ushio");
            var body = "";
            var logger = Mock.Of<ILogger>(); 
            var result = await Function1.Run(req: HttpRequestSetup(query, body), log: logger);
            var response = (BadRequestObjectResult)result;
            Assert.AreEqual("Please pass a natural number (positve integer) natural on the query string or in the request body", response.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Request_WithNaturalWellDefinedOdd_ReturnFalse()
        {
            var query = new Dictionary<String, StringValues>();
            query.TryAdd("natural", "1");
            var body = "";
            var logger = Mock.Of<ILogger>();
            var result = await Function1.Run(req: HttpRequestSetup(query, body), log: logger);
            var response = (OkObjectResult)result;
            Assert.AreEqual("Is Even: False", response.Value);
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Request_WithNaturalWellDefined_ReturnTrue()
        {
            var query = new Dictionary<String, StringValues>();
            query.TryAdd("natural", "2");
            var body = "";
            var logger = Mock.Of<ILogger>();
            var result = await Function1.Run(req: HttpRequestSetup(query, body), log: logger);
            var response = (OkObjectResult)result;
            Assert.AreEqual("Is Even: True", response.Value);
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
        }
    }
}

