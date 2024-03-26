using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.ImplementingClasses
{
    public class GenericRepo<T>:IGenericRepo<T> where T : class
    {
        private readonly AppDbContext _context;
        protected  DbSet<T> _dbSet;
        private readonly IMapper _mapper;
        public GenericRepo(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _mapper = mapper;
        }
       
        public virtual async Task<bool> AddAsync(T entity)
        {
            if (entity != null)
            {
                await _dbSet.AddAsync(entity) ;
                return true ;
            }

            return false;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
      
        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
   
        public virtual bool Update(T entity)
        {
            if (entity == null)
                return false;

            _dbSet.Update(entity);
            return true;
            
        }
       
        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return true;
            }
            return false;
        }

    }
}
