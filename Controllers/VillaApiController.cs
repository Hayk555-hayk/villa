using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using villa.Data;
using villa.Models.Dto;

namespace villa.Controllers
{

    [Route("api/getVillas")]
    [ApiController]
    public class VillaController :ControllerBase 
    {

        private readonly ILogger<VillaController> _logger;

        public VillaController(ILogger<VillaController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public ActionResult<IEnumerable<VIllaDTO>> GetVillas()
        {
            _logger.LogInformation("Here are villas");
            return Ok(VillaStore.VillaList);
        }

        [HttpGet("{id:int}", Name="GetVilla")]
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VIllaDTO> CreateVilla([FromBody]VIllaDTO villaDTO)
        {
            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            
            if(villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if(VillaStore.VillaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Model Name should be unique");

                return BadRequest(ModelState);
            }

            villaDTO.Id = VillaStore.VillaList.OrderByDescending(u=>u.Id).FirstOrDefault().Id + 1;

            VillaStore.VillaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla", new {id = villaDTO.Id}, villaDTO);
        }        


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            
            if(villa == null)
            {
                return NotFound();
            }

            VillaStore.VillaList.Remove(villa);

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public IActionResult UpdateVilla(int id, [FromBody] VIllaDTO villaDTO)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;

            return NoContent();
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VIllaDTO> patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);

            if(villa == null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(villa, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
    //https://jsonpatch.com/
}