using System.Net;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace CardTokenizer.API.Controllers;

[ApiController]
[Route("card-tokens")]
public class TokensController : ControllerBase
{
    private readonly ILogger<TokensController> _logger;

    public TokensController(ILogger<TokensController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "TokenizeCard")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TokenizeCardResponse), (int)HttpStatusCode.OK)]
    public IActionResult TokenizeCard(TokenizeCardRequest request)
    {
        return Ok(new TokenizeCardResponse
        {
            Id = Guid.NewGuid().ToString("N"),
            Token = GetHash(request.Number, request.CVV, request.HolderName),
            HolderName = request.HolderName,
            Month = request.Month,
            NumberLast4 = request.Number.Substring(request.Number.Length - 4),
            Year = request.Year
        });
    }

    public static string GetHash(params string[] args)
    {
        var input = Encoding.UTF8.GetBytes(string.Concat(args));
        using (HashAlgorithm algorithm = SHA256.Create())
        {
            return BitConverter.ToString(algorithm.ComputeHash(input))
                .Replace("-", String.Empty);
        }
    }
}

public class TokenizeCardRequest
{
    public string Number { get; set; } = null!;
    public int Month { get; set; }
    public int Year { get; set; }
    public string CVV { get; set; } = null!;
    public string HolderName { get; set; } = null!;
}

public class TokenizeCardResponse
{
    public string Id { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string NumberLast4 { get; set; } = null!;
    public string HolderName { get; set; } = null!;
    public int Month { get; set; }
    public int Year { get; set; }
}

