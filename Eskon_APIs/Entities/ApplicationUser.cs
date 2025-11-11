using Microsoft.AspNetCore.Identity;

namespace Eskon_APIs.Entities;

public sealed class ApplicationUser:IdentityUser
{
    public string FirstName { get; set; } =string.Empty;
    public string LastName { get; set; }=string.Empty;

    public bool IsDisabled { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; } = [];

    #region Added this
    public ICollection<House> OwnedHouses { get; set; } = new List<House>(); 
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<SavedList> SavedLists { get; set; } = new List<SavedList>();
    #endregion
}
