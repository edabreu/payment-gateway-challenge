using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentGateway.Integration.Tests.Extensions;

public static class HttpClientExtensions
{
	private static readonly JsonSerializerOptions s_serializerOptions = new JsonSerializerOptions
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};

	public static async Task<T> SendAsync<T>(this HttpClient httpClient, HttpMethod httpMethod, string url, Action<HttpRequestMessage> configHttpRequestMessage = null)
	{
		using var httpRequest = new HttpRequestMessage(httpMethod, url);
		if (configHttpRequestMessage != null)
        {
			configHttpRequestMessage(httpRequest);
		}

        var httpResponse = await httpClient.SendAsync(httpRequest);

        return await DeserializeAsync<T>(httpResponse);
	}

	public static async Task<T> SendAsync<C, T>(this HttpClient httpClient, HttpMethod httpMethod, string url, C content, Action<HttpRequestMessage> configHttpRequestMessage = null)
	{
		using var httpRequest = new HttpRequestMessage(httpMethod, url);
		httpRequest.Content = new StringContent(
			JsonSerializer.Serialize(content, s_serializerOptions),
			System.Text.Encoding.UTF8,
			MediaTypeNames.Application.Json);
		if (configHttpRequestMessage != null)
		{
			configHttpRequestMessage(httpRequest);
		}

		var httpResponse = await httpClient.SendAsync(httpRequest);

		return await DeserializeAsync<T>(httpResponse);
	}

	private static async Task<T> DeserializeAsync<T>(this HttpResponseMessage httpResponse)
	{
		if (!httpResponse.IsSuccessStatusCode)
        {
			throw new Exceptions.ApiException()
			{
				Status = (int)httpResponse.StatusCode
			}; ;
        }

		var stream = await httpResponse.Content.ReadAsStreamAsync();
		return await JsonSerializer.DeserializeAsync<T>(stream, s_serializerOptions);
	}
}