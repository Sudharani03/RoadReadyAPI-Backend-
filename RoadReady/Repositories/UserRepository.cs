using Microsoft.EntityFrameworkCore;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class UserRepository : IRepository<int, User>
    {
        private readonly CarRentalDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(CarRentalDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region --> AddUser
        public async Task<User> Add(User item)
        {
            // Check if the UserID already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == item.UserId);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException();
            }
            // Check if an admin with the same email already exists
            var existingUserByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == item.Email);
            if (existingUserByEmail != null)
            {
                throw new UserAlreadyExistsException();
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

        #region --> GetAllUsers
        public async Task<List<User>> GetAsync()
        {
            var users = await _context.Users.ToListAsync();
            //statement checks if the list is empty or not.
            if (!users.Any())
            {
                throw new UserListEmptyException();
            }
            return users;
        }
        #endregion

        #region --> GetUserById
        public async Task<User> GetAsyncById(int key)
        {
            var users = await GetAsync();
            var user = users.FirstOrDefault(u => u.UserId == key);
            if (user != null)
                return user;
            throw new NoSuchUserException();
        }
        #endregion

        #region --> UpdateUser
        public async Task<User> Update(User item)
        {
            var User = await GetAsyncById(item.UserId);
            if (User != null)
            {
                _context.Entry<User>(item).State = EntityState.Modified;
                //the above statement generates a update query when the save changes is called for,
                //for all attributes except the primary  key. 
                //Use the primary key in the where clause of the update query
                await _context.SaveChangesAsync();
                return item;
            }
            else
            {
                throw new NoSuchUserException();
            }
        }
        #endregion

        #region --> DeleteUser
        public async Task<User> Delete(int key)
        {
            var user = await GetAsyncById(key);
            if (user != null)
            {
                _context?.Users.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            }
            else
            {
                throw new NoSuchUserException();
            }
        }
        #endregion
    }
}
