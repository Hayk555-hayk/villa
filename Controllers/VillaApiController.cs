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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VIllaDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VIllaDTO> GetVilla(int id)
        {

            if(id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.VillaList.FirstOrDefault(u=>u.Id == id);

            if(villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }
    }
    
}