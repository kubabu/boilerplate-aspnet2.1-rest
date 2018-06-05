using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IServeUsers _service;

        public UsersController(IServeUsers service)
        {
            _service = service;
        }

        // GET: api/users
        [HttpGet]
        public IEnumerable<User> GetUsers(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return _service.GetUsers();
            }
            return _service.GetUsers().Where(h => h.Name.ToLower().Contains(term.ToLower()));
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _service.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            bool updated = await _service.UpdateUserAsync(user);

            if (updated)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddUserAsync(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool updated = await _service.DeleteUserAsync(id);

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