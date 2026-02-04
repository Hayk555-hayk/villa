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
        public ActionResult<IEnumerable<VIllaDTO>> GetVillas()
        {
            return Ok(VillaStore.VillaList);
        }

        [HttpGet("{id:int}")]
        public ActionResult<VIllaDTO> GetVilla(int id)
        {
            return Ok(VillaStore.VillaList.FirstOrDefault(u=>u.Id == id));
        }
    }
    
}