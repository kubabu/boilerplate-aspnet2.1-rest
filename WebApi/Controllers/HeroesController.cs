using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Heroes")]
    public class HeroesController : Controller
    {
        private readonly HeroService _service;

        public HeroesController(HeroService service)
        {
            _service = service;
        }

        // GET: api/Heroes
        [HttpGet]
        public IEnumerable<Hero> GetHeroes(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return _service.GetHeroes();
            }
            return _service.GetHeroes().Where(h => h.Name.ToLower().Contains(term));
        }

        // GET: api/Heroes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHero([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hero = await _service.GetHeroAsync(id);

            if (hero == null)
            {
                return NotFound();
            }

            return Ok(hero);
        }

        // PUT: api/Heroes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHero([FromRoute] int id, [FromBody] Hero hero)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hero.Id)
            {
                return BadRequest();
            }

            bool updated = await _service.UpdateHeroAsync(hero);

            if (updated)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/Heroes
        [HttpPost]
        public async Task<IActionResult> PostHero([FromBody] Hero hero)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddHeroAsync(hero);

            return CreatedAtAction("GetHero", new { id = hero.Id }, hero);
        }

        // DELETE: api/Heroes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHero([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool updated = await _service.DeleteHeroAsync(id);

            if (updated)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
        
    }
}