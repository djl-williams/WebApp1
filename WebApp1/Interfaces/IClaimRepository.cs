using ClaimsAPI.Models;

namespace ClaimsAPI.Interfaces
{
    public interface IClaimRepository
    {
        Task<IReadOnlyList<Claim>> GetClaimsByCompanyIdAsync(int companyId);
        Task<Claim> GetClaimByUcrAsync(string ucr);
        Task UpdateClaimAsync(Claim claim);
    }
}
