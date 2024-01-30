using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model;
public class LikedPerson
{
  [Key]
  public int Id { get; set; }

  [ForeignKey("User")]
  public int UserRefId { get; set; }
  public User User { get; set; }

  public string PersonId { get; set; }
  
  public LikedPerson() 
  {
    User = new User();
    PersonId = "";
  }
}
