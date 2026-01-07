using Microsoft.AspNetCore.Mvc;
using villa.Models.Dto;

namespace villa.Controllers
{

    [Route("api/getVillas")]
    [ApiController]
    public class VillaController :ControllerBase 
    {
        [HttpGet]
        public IEnumerable<VIllaDTO> GetVillas()
        {
            return new List<VIllaDTO>
            {
                new VIllaDTO {Id = 1, Name = "v1"},
                new VIllaDTO {Id = 2, Name = "v2"}
            };
        }
    }
    
}