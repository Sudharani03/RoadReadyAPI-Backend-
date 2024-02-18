using Microsoft.EntityFrameworkCore;
using RoadReady.Contexts;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class ValidationRepository : IRepository<string, Validation>
    {
        private readonly CarRentalDbContext _context;

        public ValidationRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        #region -->Add Validation
        public async Task<Validation> Add(Validation item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }
        #endregion

        #region -->Delete Validation
        public async Task<Validation> Delete(string key)
        {
            var validation = await GetAsyncById(key);
            if (validation != null)
            {
                _context.Remove(validation);
                _context.SaveChanges();
                return validation;
            }
            return null;
        }
        #endregion

        #region --> GetValidationById
        public async Task<Validation> GetAsyncById(string key)
        {
            var validation = _context.Validations.SingleOrDefault(u => u.Username == key);
            return validation;
        }
        #endregion

        #region --> GetAllValidation
        public async Task<List<Validation>> GetAsync()
        {
            var validations = await _context.Validations.ToListAsync();
            return validations;
        }
        #endregion

        #region --> UpdateValidation
        public async Task<Validation> Update(Validation item)
        {
            var validation = await GetAsyncById(item.Username);
            if (validation != null)
            {
                _context.Entry<Validation>(item).State = EntityState.Modified;
                _context.SaveChanges();
                return item;
            }
            return null;
        }
        #endregion
    }
}
