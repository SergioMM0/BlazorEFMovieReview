using FluentValidation;

namespace Entities;

public class ReviewValidator : AbstractValidator<Review>
{
    public ReviewValidator()
    {
        RuleFor(x => x.Headline).NotEmpty().NotNull();
        RuleFor(x => x.Movie).NotEmpty().NotNull();
        RuleFor(x => x.Rating).NotEmpty().GreaterThan(0).LessThan(6);
        RuleFor(x => x.ReviewerName).NotEmpty().NotNull();

    }
}