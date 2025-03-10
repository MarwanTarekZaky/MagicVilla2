using MagicVilla2_API.Data;
using MagicVilla2_API.Models;
using MagicVilla2_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla2_API.Controllers;
[Route("api/VillaAPI")]
[ApiController]
public class VillaAPIController : ControllerBase
{
    // private readonly ILogger<VillaAPIController> _logger;
    
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>>  GetVillas()
    {
        // _logger.LogInformation("Getting All Villas");
        return Ok(VillaStore.VillaList);
    }

    
    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType( StatusCodes.Status404NotFound)]
    
    public ActionResult<VillaDTO> GetVilla(int id)
    {

        if (id == 0)
        {
            // _logger.LogError("Getting Villa with id 0");
            return BadRequest();
        }

        var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
        if (villa == null)
        {
            return NotFound();
        }
        return Ok(villa);
    }

    [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villa)
    {
        if (VillaStore.VillaList.FirstOrDefault(v => v.Name.ToLower() == villa.Name.ToLower()) != null)
        {
            ModelState.AddModelError("Name", "Villa already exists");
            return BadRequest();
        }
        if (villa == null)
        {
            return BadRequest();
        }

        if (villa.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        villa.Id= VillaStore.VillaList.Count + (1);
        VillaStore.VillaList.Add(villa);
        return CreatedAtRoute("GetVilla" ,new { id = villa.Id }, villa);
    }

    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    public IActionResult DeleteVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }
        var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
        if (villa == null)
        {
            return NotFound();
        }
        VillaStore.VillaList.Remove(villa);
        return NoContent();
    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
    {
        if (villaDTO == null || villaDTO.Id != id)
        {
            return BadRequest();
        }
        var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }
        var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
        if (villa == null)
        {
            return NotFound();
        }
        patchDTO.ApplyTo(villa, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return NoContent();
    }
}
