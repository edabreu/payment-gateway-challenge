using System;

namespace Data.Gateways.Options;

public class GatewaySettings
{
	public Gateway? CardTokenizer { get; set; }
	public Gateway? CkoBank { get; set; }
}

public class Gateway
{
	public string Endpoint { get; set; } = string.Empty;
}

