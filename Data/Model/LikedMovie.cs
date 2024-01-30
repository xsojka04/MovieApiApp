using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model;
public class LikedMovie
{
  [Key]
  public int Id { get; set; }
  [ForeignKey("User")]
  public int UserRefId { get; set; }
  public User User { get; set; }
  public string MovieId { get; set; }

  public LikedMovie(User user, string movieId)
  {
    User = user;
    UserRefId = user.Id;
    MovieId = movieId;
  }

  public LikedMovie()
  {
    User = new User();
    MovieId = "";
  }
}
