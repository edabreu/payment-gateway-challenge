using System;
using Domain.Models;

namespace Domain.Abstractions.Repositories;

public interface IPaymentsRepository
{
    Task<Payment> InsertAsync(Payment payment);

    Task<Payment> GetAsync(string id);
}

