using System.IO;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace TestProject1
{

    [TestFixture]
    public class MyFirstTests
    {
        public readonly string validToken = "fhcd8kkQROq8MABvetE0ow==";
        public readonly string invalidToken = "fhcd8kkQROq8MABvetE0ow==1234";
        public readonly string validHash = "04D6F121E6F511525276F56BE5CD66A122D500D78DFBA6809D07C198A073525C";

        [Test]
        //Steps:
        //1. Get token and hash from UI
        //2. Send request with correct token and hash
        //3. Check results

        public void ValidateHash_ValidTokeb()
        {
            //Step 1
            var validApiURL = $"https://opentip.kaspersky.com/api/v1/search/hash?request= {validHash}";

            //Step 2
            var request = WebRequest.Create(validApiURL);
            request.Headers.Add("x-api-key", validToken);

            //Srep 3
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var json = sr.ReadToEnd();
            sr.Close();
            var o = JObject.Parse(json);
            var zone = o.SelectToken("Zone").ToString();
            var fileStatus = o.SelectToken("FileGeneralInfo.FileStatus").ToString();
            Assert.AreEqual("Green", zone);
            Assert.AreEqual("NoThreats", fileStatus);
            Assert.Pass();
        }

        [Test]
        //Steps:
        //1. Get token and hash from UI
        //2. Send request with incorrect token
        //3. Check result

        public void ValidateHash_InvalidToken()
        {
            //Step 1
            var validApiURL = $"https://opentip.kaspersky.com/api/v1/search/hash?request= {validHash}";

            //Step 2
            var client = new HttpClient();
            var requestInvalidTest = new HttpRequestMessage(HttpMethod.Get, validApiURL);
            requestInvalidTest.Headers.Add("x-api-key", invalidToken);

            //Step 3
            HttpResponseMessage invalidResponse = client.SendAsync(requestInvalidTest, HttpCompletionOption.ResponseHeadersRead).Result;
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, invalidResponse.StatusCode, "Request with invalid token is not failed");
            Assert.Pass();

        }
    }
}