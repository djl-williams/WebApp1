using ClaimsAPI.Models;

namespace ClaimsAPI.Interfaces
{
    public interface ICompanyRepository
    {   
        Task<Company> GetCompanyByIdAsync(int id);
        Task<IReadOnlyList<Company>> GetCompaniesAsync();
        Task UpdateCompanyAsync(Company company);
    }
}
