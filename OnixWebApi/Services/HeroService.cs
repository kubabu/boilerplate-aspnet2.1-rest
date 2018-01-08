using OnixWebApi.Models;
using OnixWebApi.Models.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnixWebApi.Services
{
    public class HeroService
    {
        IEnumerable<Hero> Heroes = new List<Hero>()
        {
            new Hero() { Id = 11, Name = "Mr. Nice" },
            new Hero() { Id = 12, Name = "Narco" },
            new Hero() { Id = 13, Name = "Bombasto" },
            new Hero() { Id = 14, Name = "Celeritas" },
            new Hero() { Id = 15, Name = "Magneta" },
            new Hero() { Id = 16, Name = "RubberMan" },
            new Hero() { Id = 17, Name = "Dynama" },
            new Hero() { Id = 18, Name = "Dr IQ" },
            new Hero() { Id = 19, Name = "Magma" },
            new Hero() { Id = 20, Name = "Tornado" }
        };

        //private OrderShippingContext _context;

        public HeroService() //OrderShippingContext dbContextFunc)
        {
            //_context = dbContextFunc;
        }

        public IEnumerable<Hero> GetHeroes()
        {
            //using (var context = _dbContextFunc())
            //{ _context.
            var heroes = Heroes.ToList();
            //}

            return Heroes;
        }

        public Hero GetHero(int id)
        {
            return Heroes.Where(hero => id == hero.Id).Single();
        }
    }
}
