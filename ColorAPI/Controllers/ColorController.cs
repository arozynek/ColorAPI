using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace ColorAPI.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly DataContext _context;

        public ColorController(DataContext context)
        {
            _context = context;
        }
        private bool IsHexValue(string hexValue)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(hexValue, @"\A\#[0-9a-fA-F]{6}\b\Z");
        }

        [HttpGet]
        public async Task<ActionResult<List<Color>>> Get()
        {
           
            return Ok(await _context.Colors.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> Get(int id)
        {
            var dbColor = await _context.Colors.FindAsync(id);
            if (dbColor == null)
            {
                return BadRequest("Color not found");
            }

            return Ok(dbColor);
        }

        [HttpGet("palette/{maxId}")]
        public async Task<ActionResult<List<Color>>> GetPalette(int id)
        {
            var dbPalette = _context.Colors.FindAsync(id);
            if (dbPalette == null)
            {
                return BadRequest("Palette not found");
            }

            return Ok(dbPalette);
        }

        [HttpPost]
        public async Task<ActionResult<List<Color>>> AddColor(Color newColor)
        {
            if (!IsHexValue(newColor.ColorHEX)) 
                return BadRequest("ColorHEX value is in wrong format");
            _context.Colors.Add(newColor);
            await _context.SaveChangesAsync();
            return Ok(await _context.Colors.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Color>>> UpdateColor(Color request, int key)
        {
            if (key != 1234) return BadRequest("The key is wrong, you have no permission to change the data");

            var dbColor = await _context.Colors.FindAsync(request.Id);

            if (dbColor == null) return BadRequest("This color doesn't exist, create a new one!");

            if (!IsHexValue(request.ColorHEX))
                return BadRequest("ColorHEX value is in wrong format");
            dbColor.ColorHEX = request.ColorHEX;
            dbColor.ColorSpectrum = request.ColorSpectrum;

            await _context.SaveChangesAsync();

            return Ok(await _context.Colors.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Color>> DeleteColor(int id, int key)
        {
            if (key != 1234) return BadRequest("The key is wrong, you have no permission to delete the data");
            var dbColor = await _context.Colors.FindAsync(id);
            if (dbColor == null)
            {
                return BadRequest("Color not found");
            }
            _context.Colors.Remove(dbColor);
            await _context.SaveChangesAsync();
           
            return Ok(await _context.Colors.ToListAsync());
        }

    }
}
