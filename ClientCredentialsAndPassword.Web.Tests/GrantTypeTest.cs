using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ClientCredentialsAndPassword.Web.Tests
{
    public class GrantTypeTest
    {
        [Fact]
        public async Task ClientCredentials_Test()
        {
            // request token
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            Assert.False(tokenResponse.IsError);
            Console.WriteLine(tokenResponse.Json);

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5010/values");
            Assert.True(response.IsSuccessStatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }

        [Fact]
        public async Task ResourceOwnerPassword_Test()
        {
            // request token
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1");

            Assert.False(tokenResponse.IsError);
            Console.WriteLine(tokenResponse.Json);

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5010/identity");
            Assert.True(response.IsSuccessStatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(JArray.Parse(content));
        }
    }
}
