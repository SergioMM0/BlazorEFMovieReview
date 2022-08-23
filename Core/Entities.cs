using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Review
{
    public int Id { get; set; }
    public string Headline { get; set; }
    public int Rating { get; set; }
    public string ReviewerName { get; set; }
    public Movie Movie { get; set; }
    public int MovieId { get; set; }
}

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public int ReleaseYear { get; set; }
    public int BoxOfficeSumInMillions { get; set; }
    public ICollection<Review>? Reviews { get; set; }
}