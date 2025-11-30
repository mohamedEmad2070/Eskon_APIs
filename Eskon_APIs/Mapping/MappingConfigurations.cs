using Eskon_APIs.Contracts.House;

namespace Eskon_APIs.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);

        config.NewConfig<House, HouseSummaryResponse>()
            .Map(dest => dest.CoverImageUrl,
                src => src.MediaItems
                    .Where(m => m.IsCover)
                    .Select(m => m.URL)
                    .FirstOrDefault()
                  ?? src.MediaItems
                       .OrderBy(m => m.SortOrder)
                       .Select(m => m.URL)
                       .FirstOrDefault()
                  ?? string.Empty)
            .Map(dest => dest.FormattedLocation,
                src => string.Join(", ", new[] { src.Location.City, src.Location.Street }.Where(x => !string.IsNullOrWhiteSpace(x))))
            .Map(dest => dest.OwnerId, src => src.OwnerId);


        config.NewConfig<House, HouseDetailResponse>()
            .Map(dest => dest.Amenities, src => src.HouseAmenities.Select(ha => ha.Amenity))
            .Map(dest => dest.Owner.FullName, src => $"{src.Owner.FirstName} {src.Owner.LastName}")
            .Map(dest => dest.Owner.UserId, src => src.OwnerId)
            .Map(dest => dest.Owner.Email, src => src.Owner.Email);
    }
}