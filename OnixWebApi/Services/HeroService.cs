using Microsoft.EntityFrameworkCore;
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
            return _context.Heroes;
        }

        public Task<Hero> GetHeroAsync(int id)
        {
            return _context.Heroes.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> UpdateHeroAsync(Hero hero)
        {
            _context.Entry(hero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeroExists(hero.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public async Task<Hero> AddHeroAsync(Hero hero)
        {
            _context.Heroes.Add(hero);
            await _context.SaveChangesAsync();
            return hero;
        }

        public async Task<bool> DeleteHeroAsync(int id)
        {
            var hero = await _context.Heroes.SingleOrDefaultAsync(m => m.Id == id);
            if (hero == null)
            {
                return false;
            }

            _context.Heroes.Remove(hero);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool HeroExists(int id)
        {
            return _context.Heroes.Any(e => e.Id == id);
        }
    }
}
 