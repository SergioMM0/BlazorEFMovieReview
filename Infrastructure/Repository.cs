using System.ComponentModel.DataAnnotations;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.Results;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;


namespace Infrastructure;

public class Repository :  IRepository
{

    private Movie mockMovieObject;
    private Review mockReviewObject;
    private DbContextOptions<RepositoryDbContext> _opts;
    private MovieValidator _movieValidator;
    private ReviewValidator _reviewValidator;

    public Repository()
    {
        _reviewValidator = new ReviewValidator();
        _movieValidator = new MovieValidator();
        mockMovieObject =new Movie()
        {
            Id = 1, Summary = "Bob writes a program ...", Title = "Bob's Movie", ReleaseYear = 2022,
            BoxOfficeSumInMillions = 42
        };
        mockReviewObject = new Review()
        {
            Id = 1, Headline = "Super great movie", Rating = 5,
            ReviewerName = "Smølf", MovieId = 1, Movie = mockMovieObject
        };

        _opts = new DbContextOptionsBuilder<RepositoryDbContext>()
            .UseSqlite("Data source=..//GUI/db.db").Options;
    }

    public List<Review> GetReviews()
    {
        using (var context = new RepositoryDbContext(_opts, ServiceLifetime.Scoped))
        {
            return context.ReviewTable.Include(r =>r.Movie).ToList();
        }
    }

    public List<Movie> GetMovies()
    {
        using (var context = new RepositoryDbContext(_opts, ServiceLifetime.Scoped))
        {
            return context.MovieTable.ToList();
        }
    }

    public Movie DeleteMovie(int movieId)
    {
        using (var context = new RepositoryDbContext(_opts, ServiceLifetime.Scoped))
        {
            var obj = new Movie { Id = movieId };
            context.MovieTable.Remove(obj);
            context.SaveChanges();
            return obj;
        }
    }

    public Review DeleteReview(int reviewId)
    {
        using (var context = new RepositoryDbContext(_opts, ServiceLifetime.Scoped))
        {
            var obj = new Review() { Id = reviewId };
            context.ReviewTable.Remove(obj);
            context.SaveChanges();
            return obj;
        }
    }

    public Movie AddMovie(Movie movie)
    {
        using (var context = new RepositoryDbContext(_opts, ServiceLifetime.Scoped))
        {
            ValidationResult results = _movieValidator.Validate(movie);
            List<ValidationFailure> validationFailures = results.Errors;
            if (results.IsValid)
            {
                context.MovieTable.Add(movie);
                context.SaveChanges();
                return movie;    
            }
        }
        throw new ValidationException("Data not validated");
    }

    public Review AddReview(Review review)
    {
        using (var context = new RepositoryDbContext(_opts, ServiceLifetime.Scoped))
        {
            ValidationResult results = _reviewValidator.Validate(review);
            List<ValidationFailure> validationFailures = results.Errors;
            if (results.IsValid)
            {
                var movie = new Movie();
                movie = context.MovieTable.Find(review.MovieId);
                review.Movie = movie;
                context.ReviewTable.Add(review);
                context.SaveChanges();
                return review;    
            }
        }
        throw new ValidationException("Data not validated");
    }
}