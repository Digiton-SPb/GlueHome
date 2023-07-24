using System;
using TT.Deliveries.Common.Contracts;

namespace TT.Deliveries.Domain.Models;

/// <summary>
/// Recipient entity
/// </summary>
public class Recipient : IEntity
{
    public Recipient() {}
    public Recipient(string name, string address, string email, string phoneNumber)
    {
        this.Name = name;
        this.Address = address;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
    }

    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
