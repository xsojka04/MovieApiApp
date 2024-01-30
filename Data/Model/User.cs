using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Model;

[Index(nameof(Name), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }
    [MinLength(5)]
    [MaxLength(50)]
    public string Name { get; set; }
    [MinLength(5)]
    public string Password { get; set; }
    public ICollection<LikedPerson> LikedPeople { get; set; }
    public ICollection<LikedMovie> LikedMovies { get; set; }


    public User()
    {
        Name = "";
        Password = "";
        LikedPeople = new List<LikedPerson>();
        LikedMovies = new List<LikedMovie>();
    }
}
