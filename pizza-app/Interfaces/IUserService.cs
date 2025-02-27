using pizza_app.DTO.UserDto;
using pizza_app.Entities.UserModel;

namespace pizza_app.Interfaces
{
    public interface IUserService
    {
        Task<(bool Success, string Message, string UserId, string Role)> RegisterAsync(RegisterModel model);
        Task<(bool Success, string Message)> DeleteUserAsync(string userId);
        Task<(bool Success, string Message, List<UserDTO> Users)> GetAllUsersAsync();
        Task<(bool Success, string Message, List<string> Roles)> GetAllRolesAsync();

    }
}
