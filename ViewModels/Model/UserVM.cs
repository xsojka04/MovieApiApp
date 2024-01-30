using Data.Model;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Model;

public class UserVM
{
    public int Id { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MinLength(5)]
    public string? Password { get; set; }

    public UserVM(User user)
    {
        Id = user.Id;
        Name = user.Name;
    }

    public UserVM(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public UserVM() { }
}
