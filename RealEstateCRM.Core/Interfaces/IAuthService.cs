using RealEstateCRM.Core.Entities;

namespace RealEstateCRM.Core.Interfaces;

public interface IAuthService
{
    Task<string> GenerateJwtTokenAsync(ApplicationUser user);
}