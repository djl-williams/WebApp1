using ClaimsAPI.Interfaces;
using ClaimsAPI.Models;

namespace ClaimsAPI.Repos
{
    public class ClaimRepository : IClaimRepository
    {
        private static readonly List<Claim> _claims = new List<Claim>
        {
        new Claim { UCR = "UCR001", CompanyId = 1, ClaimDate = new DateTime(2023, 4, 1), LossDate = new DateTime(2023, 3, 15), AssuredName = "Test Company 1", IncurredLoss = 10000, Closed = false },
        new Claim { UCR = "UCR002", CompanyId = 1, ClaimDate = new DateTime(2023, 3, 1), LossDate = new DateTime(2023, 2, 28), AssuredName = "Test Company 1", IncurredLoss = 5000, Closed = true },
        new Claim { UCR = "UCR003", CompanyId = 2, ClaimDate = new DateTime(2023, 4, 15), LossDate = new DateTime(2023, 4, 10), AssuredName = "Test Company 2", IncurredLoss = 8000, Closed = false }
        };

        public Task<IReadOnlyList<Claim>> GetClaimsByCompanyIdAsync(int companyId)
        {
            return Task.FromResult<IReadOnlyList<Claim>>(_claims.Where(c => c.CompanyId == companyId).ToList().AsReadOnly());
        }

        public Task<Claim> GetClaimByUcrAsync(string ucr)
        {
            return Task.FromResult(_claims.FirstOrDefault(c => c.UCR == ucr) ?? new Claim());
        }

        public Task UpdateClaimAsync(Claim claim)
        {
            var existingClaim = _claims.FirstOrDefault(c => c.UCR == claim.UCR);
            if (existingClaim != null)
            {
                existingClaim.ClaimDate = claim.ClaimDate;
                existingClaim.LossDate = claim.LossDate;
                existingClaim.AssuredName = claim.AssuredName;
                existingClaim.IncurredLoss = claim.IncurredLoss;
                existingClaim.Closed = claim.Closed;
            }

            return Task.CompletedTask;
        }

    }
}
