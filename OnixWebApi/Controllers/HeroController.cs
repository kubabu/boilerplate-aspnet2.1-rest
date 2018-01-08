using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnixWebApi.Models;
using OnixWebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnixWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/hero")]
    public class HeroController : Controller
    {
        private HeroService _heroSvc;

        public HeroController(HeroService service)
        {
            _heroSvc = service;
        }


        // GET: api/hero
        [HttpGet]
        public IEnumerable<Hero> Get()
        {
            return _heroSvc.GetHeroes();
        }

        // GET: api/hero/5
        [HttpGet("{id}", Name = "Get")]
        public Hero Get(int id)
        {
            return _heroSvc.GetHero(id);
        }
        
        // POST: api/hero
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/hero/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
