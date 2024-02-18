using RoadReady.Models;

namespace RoadReady.Interface
{
    public interface IAdminService
    {
        public Task<Admin> AddAdmin(Admin admin);
        public Task<List<Admin>> GetAllAdmins();
        public Task<Admin> UpdateAdminUserName(int adminId, string username);
        public Task<Admin> UpdateAdminPassword(int adminId, byte[] password);
        public Task<Admin> UpdateAdminPhoneNumber(int adminId, string phoneNumber);
        public Task<Admin> UpdateAdminEmail(int adminId, string email);
        public Task<Admin> DeleteAdmin(int adminId);
        public Task<Admin> GetAdminById(int adminId);

    }
}
