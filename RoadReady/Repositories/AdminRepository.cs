using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class AdminRepository : IRepository<int, Admin>
    {
        private readonly CarRentalDbContext _context;
        private readonly ILogger<AdminRepository> _logger;
        public AdminRepository(CarRentalDbContext context, ILogger<AdminRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region --> AddAdmin
        public async Task<Admin> Add(Admin item)
        {
            // Check if the adminID already exists
            var existingAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.AdminId == item.AdminId);
            if (existingAdmin != null)
            {
                throw new AdminAlreadyExistsException();
            }
            // Check if an admin with the same email already exists
            var existingAdminByEmail = await _context.Admins.FirstOrDefaultAsync(a => a.Email == item.Email);
            if (existingAdminByEmail != null)
            {
                throw new AdminAlreadyExistsException();
            }
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException ex)
            {
                // Log the exception and return null
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception and return null
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region --> GetAdminList
        public async Task<List<Admin>> GetAsync()
        {
            var admins = await _context.Admins.ToListAsync();
            //statement checks if the list is empty or not.
            if (!admins.Any())
            {
                throw new AdminListEmptyException();
            }
            return admins;
        }
        #endregion

        #region --> GetAdminById
        public async Task<Admin> GetAsyncById(int key)
        {
            var admins = await GetAsync();
            var admin = admins.FirstOrDefault(a=>a.AdminId == key);
            if (admin != null)
                return admin;
            throw new NoSuchAdminException(); 
        }
        #endregion

        #region ---> UpdateAdmin
        public async Task<Admin> Update(Admin item)
        {
            var Admin = await GetAsyncById(item.AdminId);
            if (Admin != null)
            {
                _context.Entry<Admin>(item).State = EntityState.Modified;
                //the above statement generates a update query when the save changes is called for,
                //for all attributes except the primary  key. 
                //Use the primary key in the where clause of the update query
                await _context.SaveChangesAsync();
                return item;
            }
            else
            {
                throw new NoSuchAdminException();
            }
        }

        #endregion

        #region --> DeleteAdmin
        public async Task<Admin> Delete(int key)
        {
            var admin=await GetAsyncById(key);
            if (admin != null)
            {
                _context?.Admins.Remove(admin);
                await _context.SaveChangesAsync();
                return admin;
            }
            else
            { 
                throw new NoSuchAdminException();
            }
        }
        #endregion
    }
}
