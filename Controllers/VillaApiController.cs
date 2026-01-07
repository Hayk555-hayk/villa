using Microsoft.AspNetCore.Mvc;
using villa.Data;
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
            return VillaStore.VillaList;
        }
    }
    
}