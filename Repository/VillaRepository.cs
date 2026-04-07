using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using villa.Data;
using villa.Models;
using villa.Repository.IRepository;

namespace villa.Repository
{
    public class VillaRepository : IVillaRepository 
    {
            private readonly ApplicationDBContext _db;
    
            public VillaRepository(ApplicationDBContext db)
            {
                _db = db;
            }
    
            public async Task Create(Villa entity)
            {
                await _db.Villas.AddAsync(entity);
                await Save();
            }
    
            public async Task<Villa> Get(Expression<Func<Villa, bool>>? filter = null, bool tracked = true)
            {
                IQueryable<Villa> query = tracked ? _db.Villas : _db.Villas.AsNoTracking();
    
                if (filter != null)
                {
                    query = query.Where(filter);
                }
    
                return await query.FirstOrDefaultAsync();
            }
    
            public async Task<List<Villa>> GetAll(Expression<Func<Villa, bool>>? filter = null)
            {
                IQueryable<Villa> query = _db.Villas;
    
                if (filter != null)
                {
                    query = query.Where(filter);
                }
    
                return await query.ToListAsync();
            }
    
            public async Task Remove(Villa entity)
            {
                _db.Villas.Remove(entity);
                await Save();
            }
    
            public async Task Save()
            {
                await _db.SaveChangesAsync();
            }
    }
}