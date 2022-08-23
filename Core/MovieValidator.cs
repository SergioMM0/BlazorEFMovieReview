namespace Entities;
using FluentValidation;

public class MovieValidator : AbstractValidator<Movie>
{
    public MovieValidator()
    {
        RuleFor(x=>x.Summary).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ReleaseYear).NotEqual(DateTime.Today.Year);
    }
}