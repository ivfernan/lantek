using System.Net;
using System.Text.Json;
using AutoFixture;
using lantek.client;
using lantek.model;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;

namespace UnitTest;

[TestClass]
public class CustomMachineServiceTest
{
    private CuttingMachineClient clientImpl;   
    private Fixture fixture;

    private Mock<HttpMessageHandler> mockHttpMessageHandler;

    public CustomMachineServiceTest(){
        fixture = new Fixture();

        mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var magicHttpClient = new HttpClient(mockHttpMessageHandler.Object);

        var dict = new Dictionary<string, string>
        {
            {"url", "https://app-academy-neu-codechallenge.azurewebsites.net/api/2d/cut"},
            {"user", fixture.Create<string>()},
            {"password", fixture.Create<string>()}
        };
        var testConfigurationService = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();

        clientImpl = new CuttingMachineClient(magicHttpClient, testConfigurationService);
    }

    [TestMethod]
    public void TestGetCuttingMachinesEmpty()
    {
        var cmlist = new List<CuttingMachine>();
        var json = JsonSerializer.Serialize(cmlist);


        mockHttpMessageHandler
        .Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        )
        .ReturnsAsync(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json)
        })
        .Verifiable();

            var machines = clientImpl.getCuttingMachines();

            Assert.AreEqual(0,machines.Result.Count);

    }

    [TestMethod]
    public void TestGetCuttingMachinesReturn1CuttingMachine()
    {
        var cm =  fixture.Create<CuttingMachine>();
        var cmlist = new List<CuttingMachine>();
        cmlist.Add(cm);

        var json = JsonSerializer.Serialize(cmlist);

        mockHttpMessageHandler
        .Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        )
        .ReturnsAsync(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json)
        })
        .Verifiable();

        var machines = clientImpl.getCuttingMachines();
        Assert.AreEqual(1,machines.Result.Count);
        Assert.AreEqual(cm.name, machines.Result.ElementAt(0).name);
    }

    [TestMethod]
    public void TestGetCuttingMachinesReturnBadRequest()
    {
        var cm =  fixture.Create<CuttingMachine>();
        var cmlist = new List<CuttingMachine>();
        cmlist.Add(cm);

        var json = JsonSerializer.Serialize(cmlist);

        mockHttpMessageHandler
        .Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        )
        .ReturnsAsync(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent(json)
        })
        .Verifiable();

        var machines = clientImpl.getCuttingMachines();
        Assert.AreEqual(0,machines.Result.Count);
    }
}