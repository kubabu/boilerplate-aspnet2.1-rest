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
        private OrderShippingContext _context;

        public HeroService(OrderShippingContext dbContextFunc)
        {
            _context = dbContextFunc;
        }

        public IEnumerable<Hero> GetHeroes()
        {
            return _context.Heroes.ToList();
        }

        public Hero GetHero(int id)
        {
            return _context.Heroes.Where(hero => id == hero.Id).Single();
        }
    }
}
