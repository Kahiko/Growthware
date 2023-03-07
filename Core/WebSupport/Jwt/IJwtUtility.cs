using GrowthWare.Framework.Models;

namespace GrowthWare.WebSupport.Jwt;
public interface IJwtUtils
{
    public string GenerateJwtToken(MAccountProfile account);
    public string ValidateJwtToken(string token);
    public MRefreshToken GenerateRefreshToken(string ipAddress);
}