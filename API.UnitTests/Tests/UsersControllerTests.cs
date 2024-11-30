namespace API.UnitTests.Tests;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.UnitTests.Helpers;
using Newtonsoft.Json.Linq;

public class UsersControllerTests
{
    private string apiRoute = "/api/users";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string loginObjetct;
    private HttpContent httpContent;

    public UsersControllerTests()
    {
        _client = TestHelper.Instance.Client;
        httpResponse = new HttpResponseMessage();
        requestUrl = string.Empty;
        loginObjetct = string.Empty;
        httpContent = new StringContent(string.Empty);
    }

    [Theory]
    [InlineData("OK")]
    public async Task GetUsersOK(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}";
        httpResponse = await _client.GetAsync(requestUrl);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "bob")]
    public async Task GetUserOK(string statusCode, string Usernamex)
    {
        //login
        // Arrange
        requestUrl = "api/account/login";
        var loginRequest = new LoginRequest
        {
            Username = "Bob",
            Password = "123456"
        };
        loginObjetct = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse!.Token);
        requestUrl = $"{apiRoute}/{Usernamex}";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("NotFound", "Francisco")]
    public async Task GetUserNF(string statusCode, string Usernamex)
    {
        //login
        // Arrange
        requestUrl = "api/account/login";
        var loginRequest = new LoginRequest
        {
            Username = "Bob",
            Password = "123456"
        };
        loginObjetct = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse!.Token);
        requestUrl = $"{apiRoute}/{Usernamex}";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("Unauthorized", "Bob")]
    public async Task GetUserUN(string statusCode, string Usernamex)
    {
        // Arrange
        requestUrl = $"{apiRoute}/{Usernamex}";

        httpResponse = await _client.GetAsync(requestUrl);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("NoContent", "Updated intro", "Updated lookingFor", "Updated interests", "Updated city", "Updated country")]
    public async Task UpdateUserOK(string statusCode, string introduction, string lookingFor, string interests, string city, string country)
    {
        //login
        // Arrange
        requestUrl = "api/account/login";
        var loginRequest = new LoginRequest
        {
            Username = "Bob",
            Password = "123456"
        };
        loginObjetct = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse!.Token);
        requestUrl = $"{apiRoute}";

        var updateUserRequest = new MemberUpdateRequest
        {
            Introduction = introduction,
            LookingFor = lookingFor,
            Interests = interests,
            City = city,
            Country = country
        };

        var updateUserObject = JObject.FromObject(updateUserRequest);
        httpContent = new StringContent(updateUserObject.ToString(), Encoding.UTF8, "application/json");
        // Act
        httpResponse = await _client.PutAsync(requestUrl, httpContent);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("Unauthorized", "Updated intro", "Updated lookingFor", "Updated interests", "Updated city", "Updated country")]
    public async Task UpdateUserUN(string statusCode, string introduction, string lookingFor, string interests, string city, string country)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearerx", "tokenx");
        requestUrl = $"{apiRoute}";

        var updateUserRequest = new MemberUpdateRequest
        {
            Introduction = introduction,
            LookingFor = lookingFor,
            Interests = interests,
            City = city,
            Country = country
        };

        var updateUserObject = JObject.FromObject(updateUserRequest);
        httpContent = new StringContent(updateUserObject.ToString(), Encoding.UTF8, "application/json");
        // Act
        httpResponse = await _client.PutAsync(requestUrl, httpContent);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
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

    private static string GetUpdateMemberObject(MemberUpdateRequest memberUpdateDto)
    {
        var entityObject = new JObject()
            {
                { nameof(memberUpdateDto.Introduction), memberUpdateDto.Introduction },
                { nameof(memberUpdateDto.LookingFor), memberUpdateDto.LookingFor },
                { nameof(memberUpdateDto.Interests), memberUpdateDto.Interests },
                { nameof(memberUpdateDto.City), memberUpdateDto.City },
                { nameof(memberUpdateDto.Country), memberUpdateDto.Country }
            };
        return entityObject.ToString();
    }

    private static StringContent GetHttpContent(string objectToCode)
    {
        return new StringContent(objectToCode, Encoding.UTF8, "application/json");
    }
    #endregion
}