using System.Collections.Generic;

namespace TransportLink.Domain.Entities;

public sealed class Company
{
    private Company()
    {
    }

    public Company(Guid id, string name, string address, string contactEmail)
    {
        Id = id;
        Name = name;
        Address = address;
        ContactEmail = contactEmail;
    }

    public Guid Id { get; init; }

    public string Name { get; private set; } = string.Empty;

    public string Address { get; private set; } = string.Empty;

    public string ContactEmail { get; private set; } = string.Empty;

    public ICollection<Driver> Drivers { get; private set; } = new List<Driver>();

    public CompanyPayrollConfig? PayrollConfig { get; private set; }

    public ICollection<Payment> Payments { get; private set; } = new List<Payment>();
}
