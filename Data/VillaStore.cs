using villa.Models.Dto;

namespace villa.Data
{
    public static class VillaStore {
        public static List<VIllaDTO> VillaList = new List<VIllaDTO>
            {
                new VIllaDTO {Id = 1, Name = "v1"},
                new VIllaDTO {Id = 2, Name = "v2"}
            };
    }
}