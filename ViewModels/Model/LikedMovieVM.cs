using Data.Model;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Model;

public class LikedMovieVM
{
    public int Id { get; set; }

    [Required]
    public string MovieId { get; set; }

    public LikedMovieVM(LikedMovie likedMovie)
    {
        Id = likedMovie.Id;
        MovieId = likedMovie.MovieId;
    }

    public LikedMovieVM() { }
}
