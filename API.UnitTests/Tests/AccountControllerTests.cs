namespace API.UnitTests.Tests;

using System.Net;
using System.Text;
using API.DTOs;
using API.UnitTests.Helpers;
using Newtonsoft.Json.Linq;

public class AccountControllerTests
{
    private string apiRoute = "api/account";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string registerObjetct;
    private string loginObjetct;
    private HttpContent httpContent;
    
    public AccountControllerTests()
    {
        _client = TestHelper.Instance.Client;
        httpResponse = new HttpResponseMessage();
        requestUrl = string.Empty;
        registerObjetct = string.Empty;
        loginObjetct = string.Empty;
        httpContent = new StringContent(string.Empty);
    }

    [Theory]
    [InlineData("OK", "Angel", "Prueba12")]
    public async Task RegisterUserOK(string statusCode, string usernamex, string passwordx)
    {
        // Arrange
        requestUrl = $"{apiRoute}/register";
        var registerRequest = new RegisterRequest
        {
            Username = usernamex,
            Password = passwordx
        };

        registerObjetct = GetRegisterObject(registerRequest);
        httpContent = GetHttpContent(registerObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("BadRequest", "Bob", "Prueba12")]
    public async Task RegisterUserBR(string statusCode, string usernamex, string passwordx)
    {
        // Arrange
        requestUrl = $"{apiRoute}/register";
        var registerRequest = new RegisterRequest
        {
            Username = usernamex,
            Password = passwordx
        };

        registerObjetct = GetRegisterObject(registerRequest);
        httpContent = GetHttpContent(registerObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "Bob", "123456")]
    public async Task LoginrUserOK(string statusCode, string usernamex, string passwordx)
    {
        // Arrange
        requestUrl = $"{apiRoute}/login";
        var loginRequest = new LoginRequest
        {
            Username = usernamex,
            Password = passwordx
        };

        loginObjetct = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("Unauthorized", "Francisco", "HolaMundo")]
    public async Task LoginrUserUN1(string statusCode, string usernamex, string passwordx)
    {
        // Arrange
        requestUrl = $"{apiRoute}/login";
        var loginRequest = new LoginRequest
        {
            Username = usernamex,
            Password = passwordx
        };

        loginObjetct = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("Unauthorized", "Bob", "HolaMundoError")]
    public async Task LoginrUserUN2(string statusCode, string usernamex, string passwordx)
    {
        // Arrange
        requestUrl = $"{apiRoute}/login";
        var loginRequest = new LoginRequest
        {
            Username = usernamex,
            Password = passwordx
        };

        loginObjetct = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    #region Privated methods
    private static string GetRegisterObject(RegisterRequest registerDto)
    {
        var entityObject = new JObject()
            {
                { nameof(registerDto.Username), registerDto.Username },
                { nameof(registerDto.Password), registerDto.Password }
            };
        return entityObject.ToString();
    }

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