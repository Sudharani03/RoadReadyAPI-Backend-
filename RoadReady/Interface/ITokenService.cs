using RoadReady.Models.DTOs;

namespace RoadReady.Interface
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(LoginValidationDto validation);
    }
}
