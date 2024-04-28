using ClaimsAPI.Interfaces;
using ClaimsAPI.Models;

namespace ClaimsAPI.Repos
{
    public class CompanyRepository : ICompanyRepository
    {
        private static readonly List<Company> _companies = new List<Company>
        {
        new Company { Id = 1, Name = "Test Company 1", Address1 = "123 Main St", Address2 = "City", Address3 = "State", Postcode = "12345", Country = "USA", Active = true, InsuranceEndDate = new DateTime(2025, 1, 1) },
        new Company { Id = 2, Name = "Test Company 2", Address1 = "456 Oak Rd", Address2 = "Town", Address3 = "State", Postcode = "67890", Country = "United Kingdom", Active = false, InsuranceEndDate = new DateTime(2023, 6, 1) },
        new Company { Id = 3, Name = "Test Company 3", Address1 = "789 Bell Ave", Address2 = "Town", Address3 = "State", Postcode = "12415", Country = "Australia", Active = true, InsuranceEndDate = new DateTime(2026, 3, 1) }
        };

        public Task<Company> GetCompanyByIdAsync(int id)
        {
            return Task.FromResult(_companies.FirstOrDefault(c => c.Id == id) ?? new Company());
        }

        public Task<IReadOnlyList<Company>> GetCompaniesAsync()
        {
            return Task.FromResult<IReadOnlyList<Company>>(_companies.AsReadOnly());
        }

        public Task UpdateCompanyAsync(Company company)
        {
            var existingCompany = _companies.FirstOrDefault(c => c.Id == company.Id);
            if (existingCompany != null)
            {
                existingCompany.Name = company.Name;
                existingCompany.Address1 = company.Address1;
                existingCompany.Address2 = company.Address2;
                existingCompany.Address3 = company.Address3;
                existingCompany.Postcode = company.Postcode;
                existingCompany.Country = company.Country;
                existingCompany.Active = company.Active;
                existingCompany.InsuranceEndDate = company.InsuranceEndDate;
            }

            return Task.CompletedTask;
        }
    }
}
