using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model;
public class LikedTv
{
  [Key]
  public int Id { get; set; }
  [ForeignKey("User")]
  public int UserRefId { get; set; }
  public User User { get; set; }
  public string TvId { get; set; }

  public LikedTv(User user, string tvId)
  {
    User = user;
    UserRefId = user.Id;
    TvId = tvId;
  }

  public LikedTv()
  {
    User = new User();
    TvId = "";
  }
}
