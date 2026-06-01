using FluentValidation;
using PremierLeagueHub.DtoLayer.TeamDtos;

namespace PremierLeagueHub.BusinessLayer.ValidationRules.TeamValidators;

public class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
{
    public CreateTeamDtoValidator()
    {
        RuleFor(x => x.TeamName)
            .NotEmpty().WithMessage("Team name is required.")
            .MinimumLength(2).WithMessage("Team name must be at least 2 characters.")
            .MaximumLength(80).WithMessage("Team name must not exceed 80 characters.");

        RuleFor(x => x.ShortName)
            .NotEmpty().WithMessage("Short name is required.")
            .MinimumLength(2).WithMessage("Short name must be at least 2 characters.")
            .MaximumLength(10).WithMessage("Short name must not exceed 10 characters.");

        RuleFor(x => x.LogoUrl)
            .NotEmpty().WithMessage("Logo URL is required.")
            .Must(BeAValidUrl).WithMessage("Logo URL must be a valid URL.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(60).WithMessage("City must not exceed 60 characters.");

        RuleFor(x => x.StadiumName)
            .NotEmpty().WithMessage("Stadium name is required.")
            .MaximumLength(100).WithMessage("Stadium name must not exceed 100 characters.");

        RuleFor(x => x.FoundedYear)
            .InclusiveBetween(1800, DateTime.Now.Year)
            .WithMessage($"Founded year must be between 1800 and {DateTime.Now.Year}.");

        RuleFor(x => x.ManagerName)
            .NotEmpty().WithMessage("Manager name is required.")
            .MaximumLength(80).WithMessage("Manager name must not exceed 80 characters.");

        RuleFor(x => x.WebsiteUrl)
            .NotEmpty().WithMessage("Website URL is required.")
            .Must(BeAValidUrl).WithMessage("Website URL must be a valid URL.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.PrimaryColor)
            .NotEmpty().WithMessage("Primary color is required.")
            .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
            .WithMessage("Primary color must be a valid hex color. Example: #EF0107");

        RuleFor(x => x.SecondaryColor)
            .NotEmpty().WithMessage("Secondary color is required.")
            .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
            .WithMessage("Secondary color must be a valid hex color. Example: #FFFFFF");
    }

    private static bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var result)
               && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}