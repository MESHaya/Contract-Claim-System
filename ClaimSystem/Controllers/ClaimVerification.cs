using ClaimSystem.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

public class ClaimVerificationService
{
    private readonly ClaimDbContext _claimRepository;
    private readonly IValidator<Claims> _validator;

    // Single constructor with injected dependencies
    public ClaimVerificationService(ClaimDbContext claimRepository)
    {
        _claimRepository = claimRepository;

        // Inline validation rules using FluentValidation
        _validator = new InlineValidator<Claims>
        {
            v => v.RuleFor(c => c.HourlyRate)
                .InclusiveBetween(50.00m, 200.00m)
                .WithMessage("Hourly rate must be between 50 and 200."),
            v => v.RuleFor(c => c.HoursWorked)
                .LessThanOrEqualTo(40)
                .WithMessage("Hours worked must not exceed 40.")
            // Additional rules can go here
        };
    }

    public ValidationResult VerifyClaim(Claims model)
    {
        // Use FluentValidation rules to validate the claim
        var validationResult = _validator.Validate(model);

        // Additional custom checks if needed
        if (validationResult.IsValid)
        {
            // Add any extra checks here, e.g., database checks
            // Example: Validate claim data against other claims in the database
        }

        return validationResult;
    }
}
