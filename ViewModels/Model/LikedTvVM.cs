using Data.Model;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Model;

public class LikedTvVM
{
    public int Id { get; set; }

    [Required]
    public string TvId { get; set; }

    public LikedTvVM(LikedTv likedTv)
    {
        Id = likedTv.Id;
        TvId = likedTv.TvId;
    }

    public LikedTvVM() { }
}
