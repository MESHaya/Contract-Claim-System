using ClaimSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class ClaimVerificationService
{
    private readonly ClaimDbContext _claimRepository;

    public ClaimVerificationService(ClaimDbContext claimRepository)
    {
       
        _claimRepository = claimRepository;
    }

    public bool VerifyClaim(Claims model)
    {
        // Define rules (for example)
        const decimal MinHourlyRate = 50.00m;
        const decimal MaxHourlyRate = 200.00m;
        const int MaxHoursWorked = 40;

        if (model.HoursWorked > MaxHoursWorked)
        {
            // Hours worked exceeds limit
            return false;
        }

        if (model.HourlyRate < MinHourlyRate || model.HourlyRate > MaxHourlyRate)
        {
            // Hourly rate is not within the valid range
            return false;
        }

        // Additional checks like validating claim data against the database or other conditions can be added here
        return true;
    }
}
