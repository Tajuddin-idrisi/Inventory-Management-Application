namespace InventoryManagement.API.Services
{
    public interface ITokenService
    {
        string GenerateToken(string username, string? role = null);
    }
}