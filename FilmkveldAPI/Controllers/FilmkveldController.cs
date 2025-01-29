// Filmkveld Controller
using Microsoft.AspNetCore.Mvc;
using FilmkveldAPI.Data;
using FilmkveldAPI.Models;

namespace FilmkveldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmkveldController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilmkveldController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.MovieNights.ToList());
        }

        [HttpPost]
        public IActionResult Create(Filmkveld movieNight)
        {
            _context.MovieNights.Add(movieNight);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAll), new { id = movieNight.FilmkveldId }, movieNight);
        }
    }
}