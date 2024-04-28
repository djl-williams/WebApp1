using Microsoft.AspNetCore.Mvc;
using ClaimsAPI.Interfaces;
using ClaimsAPI.Models;

namespace ClaimsAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]

    public class ClaimsController : ControllerBase
    {
        private readonly IClaimRepository _claimRepository;

        public ClaimsController(IClaimRepository claimRepository)
        {
            _claimRepository = claimRepository;
        }

        [HttpGet("companies/{companyId}")]
        public async Task<IActionResult> GetClaimsByCompanyIdAsync(int companyId)
        {
            var claims = await _claimRepository.GetClaimsByCompanyIdAsync(companyId);
            return Ok(claims);
        }

        [HttpGet("{ucr}")]
        public async Task<IActionResult> GetClaimByUcrAsync(string ucr)
        {
            var claim = await _claimRepository.GetClaimByUcrAsync(ucr);
            if (claim == null)
                return NotFound();

            if (claim.ClaimDate > DateTime.Now)
            {
                return BadRequest("Claim date cannot be in the future.");
            }

            if (claim.LossDate > DateTime.Now)
            {
                return BadRequest("Loss date cannot be in the future.");
            }

            var claimAge = DateTime.Now.Subtract(claim.ClaimDate).Days;
            var claimDetails = new ClaimDetailsDto
            {
                UCR = claim.UCR,
                CompanyId = claim.CompanyId,
                ClaimDate = claim.ClaimDate,
                LossDate = claim.LossDate,
                AssuredName = claim.AssuredName,
                IncurredLoss = claim.IncurredLoss,
                Closed = claim.Closed,
                ClaimAgeInDays = claimAge
            };

            return Ok(claimDetails);
        }

        [HttpPut("{ucr}")]
        public async Task<IActionResult> UpdateClaimAsync(string ucr, [FromBody] Claim claim)
        {
            if (claim == null)
                return BadRequest();

            var existingClaim = await _claimRepository.GetClaimByUcrAsync(ucr);
            if (existingClaim == null)
                return NotFound();

            claim.UCR = ucr;
            await _claimRepository.UpdateClaimAsync(claim);

            return NoContent();
        }
    }
}
