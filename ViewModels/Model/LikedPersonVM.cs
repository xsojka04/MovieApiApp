using Data.Model;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Model;

public class LikedPersonVM
{
    public int Id { get; set; }

    [Required]
    public string PersonId { get; set; }

    public LikedPersonVM(LikedPerson likedPerson)
    {
        Id = likedPerson.Id;
        PersonId = likedPerson.PersonId;
    }

    public LikedPersonVM() { }
}
