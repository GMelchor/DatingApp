namespace API.UnitTests.Tests;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
public class BuggyControllerTests
{
    private string apiRoute = "api/buggy";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string loginObjetct;
    private HttpContent httpContent;
    
    public BuggyControllerTests()
    {
        _client = TestHelper.Instance.Client;
        httpResponse = new HttpResponseMessage();
        requestUrl = string.Empty;
        loginObjetct = string.Empty;
        httpContent = new StringContent(string.Empty);
    }
    [Theory]
    [InlineData("OK", "bob", "123456")]
    public async Task GetSecretShouldOK(string statusCode, string username, string password)
    {
        // Arrange
        requestUrl = "api/account/login";
        var loginRequest = new LoginRequest
        {
            Username = username,
            Password = password
        };
        loginObjetct = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse!.Token);
        requestUrl = $"{apiRoute}/auth";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }
    [Theory]
    [InlineData("NotFound")]
    public async Task GetNotFoundShouldNotFound(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/not-found";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }
    [Theory]
    [InlineData("InternalServerError")]
    public async Task GetServerErrorShouldNotInternalServerError(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/server-error";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }
    [Theory]
    [InlineData("BadRequest")]
    public async Task GetBadRequestShouldBadRequest(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/bad-request";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }
    #region Privated methods
    private static string GetLoginObject(LoginRequest loginDto)
    {
        var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };
        return entityObject.ToString();
    }
    private static StringContent GetHttpContent(string objectToCode)
    {
        return new StringContent(objectToCode, Encoding.UTF8, "application/json");
    }
    #endregion
}