using Microsoft.AspNetCore.Mvc;
using ClaimsAPI.Interfaces;
using ClaimsAPI.Models;

namespace ClaimsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;

        public CompaniesController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyByIdAsync(int id)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(id);
            if (company == null)
                return NotFound();

            var companyDetails = new CompanyDetailsDto
            {
               Id = company.Id,
               Name = company.Name,
               Address1 = company.Address1,
               Address2 = company.Address2,
               Address3 = company.Address3,
               Postcode = company.Postcode,
               Country = company.Country,
               Active = company.Active,
               HasActiveInsurance = company.HasActiveInsurance
            };

            return Ok(companyDetails);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanyAsync(int id, [FromBody] CompanyDetailsDto companyDetails)
        {
            if (companyDetails == null)
                return BadRequest();

            var company = await _companyRepository.GetCompanyByIdAsync(id);
            if (company == null)
                return NotFound();

            company.Name = companyDetails.Name;
            company.Address1 = companyDetails.Address1;
            company.Address2 = companyDetails.Address2;
            company.Address3 = companyDetails.Address3;
            company.Postcode = companyDetails.Postcode;
            company.Country = companyDetails.Country;
            company.Active = companyDetails.Active;
            company.InsuranceEndDate = companyDetails.HasActiveInsurance ? DateTime.Now.AddYears(1) : DateTime.MinValue;

            await _companyRepository.UpdateCompanyAsync(company);

            return NoContent();
        }


    }
}
