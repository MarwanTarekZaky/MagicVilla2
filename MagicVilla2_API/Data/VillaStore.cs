using MagicVilla2_API.Models.Dto;

namespace MagicVilla2_API.Data;

public static class VillaStore
{
    public static List<VillaDTO> VillaList = new List<VillaDTO>
    {
        new VillaDTO() { Id = 1, Name = "Pool view", Occupancy = 4, Sqft = 100 },
        new VillaDTO() { Id = 2, Name = "Beach view", Occupancy = 3, Sqft = 300}
    };
}