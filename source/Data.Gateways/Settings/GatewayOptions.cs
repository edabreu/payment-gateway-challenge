using System;

namespace Data.Gateways.Options;

public class GatewayOptions
{
	public Gateway CardTokenizer { get; set; } = default!;
	public Gateway CkoBank { get; set; } = default!;
}

public class Gateway
{
	public string Endpoint { get; set; } = null!;
}

