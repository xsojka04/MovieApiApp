using Data;
using Data.Model;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ViewModel.Model;
using webapi.Controllers;

namespace webapi.Service;
public class LikedTvService
{
    private readonly WapContext _dbContext;

    public LikedTvService(WapContext context)
    {
        _dbContext = context;
    }

    public IEnumerable<LikedTvVM> All(int userId)
    {
        var likedTvs = _dbContext.LikedTvs.Where(x => x.UserRefId == userId).ToList();
        var viewModels = new List<LikedTvVM>();
        foreach (var likedTv in likedTvs)
        {
            viewModels.Add(new LikedTvVM(likedTv));
        }
        return viewModels;
    }

    public void Add(int userId, LikedTvVM likedTvVM)
    {
        var likedTv = new LikedTv { TvId = likedTvVM.TvId, User = _dbContext.Users.First(x => x.Id == userId) };
        _dbContext.LikedTvs.Add(likedTv);
        _dbContext.SaveChanges();
    }

    public void Delete(int userId, int likedTvId)
    {
        var likedTvs = _dbContext.LikedTvs.Where(x => x.UserRefId == userId && x.TvId == likedTvId.ToString());
        foreach (var likedTv in likedTvs)
        {
            _dbContext.LikedTvs.Remove(likedTv);
        }
        _dbContext.SaveChanges();
    }
}
