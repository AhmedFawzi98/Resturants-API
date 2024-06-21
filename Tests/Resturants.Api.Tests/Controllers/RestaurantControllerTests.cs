using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Resturants.Api.Tests.Controllers;

[TestFixture]
public class RestaurantControllerTests 
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task GetAllResturants_IfValidRequest_ShouldReturn200Ok()
    {
        var response = await _client.GetAsync("api/restaurants?pagesize=5&&pagenumber=1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetAllResturants_IfInValidRequest_ShouldReturn400badRequest()
    {
        var response = await _client.GetAsync("api/restaurants");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}