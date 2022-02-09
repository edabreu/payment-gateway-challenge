using System;

namespace Domain.Models;

public class Payment
{
	public Payment(string processingId, bool approved, string status, string reference, int amount, string currecy)
		: this (Guid.NewGuid().ToString(), processingId, approved, status, reference, amount, currecy)
	{
    }

	public Payment(string id, string processingId, bool approved, string status, string reference, int amount, string currecy)
	{
		Id = id;
        ProcessingId = processingId;
        Approved = approved;
        Status = status;
        Reference = reference;
		Amount = amount;
		Currency = currecy;
	}

	public string Id { get; }
	public string ProcessingId { get; }
    public bool Approved { get; }
    public string Status { get; }
    public string Reference { get; }
	public int Amount { get; }
    public string Currency { get; }
	public CardToken? CardToken { get; private set; }

	public void SetCardToken(CardToken cardToken)
	{
		CardToken = cardToken;
	}
}

