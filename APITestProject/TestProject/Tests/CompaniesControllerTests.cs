using Microsoft.AspNetCore.Mvc;
using Moq;
using ClaimsAPI.Controllers;
using ClaimsAPI.Interfaces;
using ClaimsAPI.Models;

namespace APITestProject.Tests
{
    [TestClass]
    public class CompaniesControllerTests
    {
        private Mock<ICompanyRepository>? _mockCompanyRepository;
        private CompaniesController? _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockCompanyRepository = new Mock<ICompanyRepository>();
            _controller = new CompaniesController(_mockCompanyRepository.Object);
        }

        [TestMethod]
        public async Task GetCompanyByIdAsync_Test()
        {
           
            int companyId = 1;
            var expectedCompany = new Company
            {
                Id = 1,
                Name = "Test Company 1",
                Address1 = "123 Main St",
                Address2 = "City",
                Address3 = "State",
                Postcode = "12345",
                Country = "USA",
                Active = true,
                InsuranceEndDate = new DateTime(2025, 1, 1)
            };
            _mockCompanyRepository.Setup(repo => repo.GetCompanyByIdAsync(companyId))
                .ReturnsAsync(expectedCompany);

           
            var result = await _controller.GetCompanyByIdAsync(companyId);

            Assert.IsInstanceOfType<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult, "OkObjectResult should not be null");

            var companyDetails = okObjectResult.Value as CompanyDetailsDto;
            Assert.IsNotNull(companyDetails, "companyDetails should not be null");

            Assert.AreEqual(expectedCompany.Id, companyDetails.Id);
            Assert.AreEqual(expectedCompany.Name, companyDetails.Name);
            Assert.AreEqual(expectedCompany.Address1, companyDetails.Address1);
            Assert.AreEqual(expectedCompany.Address2, companyDetails.Address2);
            Assert.AreEqual(expectedCompany.Address3, companyDetails.Address3);
            Assert.AreEqual(expectedCompany.Postcode, companyDetails.Postcode);
            Assert.AreEqual(expectedCompany.Country, companyDetails.Country);
            Assert.AreEqual(expectedCompany.Active, companyDetails.Active);
            Assert.AreEqual(expectedCompany.HasActiveInsurance, companyDetails.HasActiveInsurance);
        }

        [TestMethod]
        
        public async Task UpdateCompanyAsync_Test()
        {
            
            int companyId = 1;
            var existingCompany = new Company
            {
                Id = 1,
                Name = "Test Company 1",
                Address1 = "123 Main St",
                Address2 = "City",
                Address3 = "State",
                Postcode = "12345",
                Country = "USA",
                Active = true,
                InsuranceEndDate = new DateTime(2024, 1, 1)
            };
            var updatedCompanyDetails = new CompanyDetailsDto
            {
                Name = "Updated Test Company 1",
                Address1 = "456 Oak Rd",
                Address2 = "Town",
                Address3 = "State",
                Postcode = "67890",
                Country = "Canada",
                Active = false,
                HasActiveInsurance = false
            };
            _mockCompanyRepository.Setup(repo => repo.GetCompanyByIdAsync(companyId))
                .ReturnsAsync(existingCompany);

            
            var result = await _controller.UpdateCompanyAsync(companyId, updatedCompanyDetails);

            Assert.IsInstanceOfType<NoContentResult>(result);
            _mockCompanyRepository.Verify(repo => repo.UpdateCompanyAsync(It.Is<Company>(
                c => c.Id == existingCompany.Id &&
                     c.Name == updatedCompanyDetails.Name &&
                     c.Address1 == updatedCompanyDetails.Address1 &&
                     c.Address2 == updatedCompanyDetails.Address2 &&
                     c.Address3 == updatedCompanyDetails.Address3 &&
                     c.Postcode == updatedCompanyDetails.Postcode &&
                     c.Country == updatedCompanyDetails.Country &&
                     c.Active == updatedCompanyDetails.Active &&
                     c.InsuranceEndDate == (updatedCompanyDetails.HasActiveInsurance ? DateTime.Now.AddYears(1) : DateTime.MinValue))), Times.Once);
        }

        [TestMethod]

        public async Task GetCompanyByIdAsyncMismatch_Test()
        {
            
            int companyId = 1;
            var expectedCompany = new Company
            {
                Id = 1,
                Name = "Example Company",
                Address1 = "724 Kent Ave",
                Address2 = "City",
                Address3 = "State",
                Postcode = "12345",
                Country = "United Kingdom",
                Active = true,
                InsuranceEndDate = new DateTime(2024, 1, 1)
            };
            _mockCompanyRepository.Setup(repo => repo.GetCompanyByIdAsync(companyId))
                .ReturnsAsync(expectedCompany);

           
            var result = await _controller.GetCompanyByIdAsync(companyId);

          
            Assert.IsInstanceOfType<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult, "OkObjectResult should not be null");

            var companyDetails = okObjectResult.Value as CompanyDetailsDto;
            Assert.IsNotNull(companyDetails, "companyDetails should not be null");

            
            Assert.AreNotEqual("Company A", companyDetails.Name);
            Assert.AreNotEqual("33 Smith St", companyDetails.Address1);
            Assert.AreNotEqual("  ", companyDetails.Address2);
            Assert.AreNotEqual("456 Barron St", companyDetails.Address3);
            Assert.AreNotEqual("6789", companyDetails.Postcode);
            Assert.AreNotEqual("Germany", companyDetails.Country);
            Assert.AreNotEqual(false, companyDetails.Active);
            Assert.AreNotEqual(true, companyDetails.HasActiveInsurance);
        }
    }
}
