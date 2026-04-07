using AutoMapper;

namespace villa
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Models.Villa, Models.Dto.VillaDto>();
            CreateMap<Models.Dto.VillaDto, Models.Villa>();

            CreateMap<Models.Villa, Models.Dto.VillaCreateDto>();
            CreateMap<Models.Dto.VillaCreateDto, Models.Villa>();

            CreateMap<Models.Villa, Models.Dto.VillaUpdateDto>();
            CreateMap<Models.Dto.VillaUpdateDto, Models.Villa>();
        }
    }
}