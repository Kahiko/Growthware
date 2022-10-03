using GrowthWare.Framework.Models;

namespace GrowthWare.WebSupport.Utilities.Jwt;
public interface IJwtUtils
{
    public string GenerateJwtToken(MAccountProfile account);
    public string ValidateJwtToken(string token);
    public RefreshToken GenerateRefreshToken(string ipAddress);
}