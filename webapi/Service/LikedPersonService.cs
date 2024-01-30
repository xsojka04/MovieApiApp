using Data;
using Data.Model;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ViewModel.Model;

namespace webapi.Service;
public class LikedPersonService
{
    private readonly WapContext _dbContext;

    public LikedPersonService(WapContext context)
    {
        _dbContext = context;
    }

    public IEnumerable<LikedPersonVM> All(int userId)
    {
        var likedPeople = _dbContext.LikedPeople.Where(x => x.UserRefId == userId).ToList();
        var viewModels = new List<LikedPersonVM>();
        foreach (var likedPerson in likedPeople)
        {
            viewModels.Add(new LikedPersonVM(likedPerson));
        }
        return viewModels;
    }

    public void Add(int userId, LikedPersonVM likedPersonVM)
    {
        var likedPerson = new LikedPerson { PersonId = likedPersonVM.PersonId, User = _dbContext.Users.First(x => x.Id == userId) };
        _dbContext.LikedPeople.Add(likedPerson);
        _dbContext.SaveChanges();
    }

    public void Delete(int userId, int likedPersonId)
    {
        var likedPersons = _dbContext.LikedPeople.Where(x => x.UserRefId == userId && x.PersonId == likedPersonId.ToString());
        foreach (var likedPerson in likedPersons)
        {
            _dbContext.LikedPeople.Remove(likedPerson);
        }
        _dbContext.SaveChanges();

    }
}
