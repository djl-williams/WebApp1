using ClaimsAPI.Controllers;
using ClaimsAPI.Interfaces;
using ClaimsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace APITestProject.Tests
{
    [TestClass]

    public class ClaimsControllerTests
    {
        private Mock<IClaimRepository> _mockClaimRepository;
        private ClaimsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockClaimRepository = new Mock<IClaimRepository>();
            _controller = new ClaimsController(_mockClaimRepository.Object);
        }

        [TestMethod]
        public async Task GetClaimsByCompanyIdAsync_Test()
        {
           
            int companyId = 1;
            var expectedClaims = new List<Claim>
        {
            new Claim { UCR = "UCR001", CompanyId = 1, ClaimDate = new DateTime(2023, 4, 1), LossDate = new DateTime(2023, 3, 15), AssuredName = "Test Company 1", IncurredLoss = 10000, Closed = false },
            new Claim { UCR = "UCR002", CompanyId = 1, ClaimDate = new DateTime(2023, 3, 1), LossDate = new DateTime(2024, 2, 28), AssuredName = "Test Company 1", IncurredLoss = 5000, Closed = true }
        };
            _mockClaimRepository.Setup(repo => repo.GetClaimsByCompanyIdAsync(companyId))
                .ReturnsAsync(expectedClaims.AsReadOnly());

           
            var result = await _controller.GetClaimsByCompanyIdAsync(companyId);

            Assert.IsInstanceOfType<OkObjectResult>(result);
            var claims = (result as OkObjectResult).Value as IReadOnlyList<Claim>;
            Assert.AreEqual(expectedClaims.Count, claims.Count);
            CollectionAssert.AreEquivalent(expectedClaims, (System.Collections.ICollection?)claims);
        }

        [TestMethod]
        public async Task GetClaimByUcrAsync_Test()
        {

            string ucr = "UCR001";
            var expectedClaim = new Claim
            {
                UCR = "UCR001",
                CompanyId = 1,
                ClaimDate = new DateTime(2023, 4, 1),
                LossDate = new DateTime(2023, 3, 15),
                AssuredName = "Test Company 1",
                IncurredLoss = 10000,
                Closed = false
            };
            _mockClaimRepository.Setup(repo => repo.GetClaimByUcrAsync(ucr))
                .ReturnsAsync(expectedClaim);

            
            var result = await _controller.GetClaimByUcrAsync(ucr);

            Assert.IsInstanceOfType<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult, "OkObjectResult should not be null");

            var claimDetails = okObjectResult.Value as ClaimDetailsDto;
            Assert.IsNotNull(claimDetails, "claimDetails should not be null");

            Assert.AreEqual(expectedClaim.UCR, claimDetails.UCR);
            Assert.AreEqual(expectedClaim.CompanyId, claimDetails.CompanyId);
            Assert.AreEqual(expectedClaim.ClaimDate, claimDetails.ClaimDate);
            Assert.AreEqual(expectedClaim.LossDate, claimDetails.LossDate);
            Assert.AreEqual(expectedClaim.AssuredName, claimDetails.AssuredName);
            Assert.AreEqual(expectedClaim.IncurredLoss, claimDetails.IncurredLoss);
            Assert.AreEqual(expectedClaim.Closed, claimDetails.Closed);
        }

        [TestMethod]
        public async Task UpdateClaimAsync_Test()
        {

            string ucr = "UCR001";
            var existingClaim = new Claim { UCR = "UCR001", CompanyId = 1, ClaimDate = new DateTime(2023, 4, 1), LossDate = new DateTime(2023, 3, 15), AssuredName = "Test Company 1", IncurredLoss = 10000, Closed = false };
            var updatedClaim = new Claim { UCR = "UCR001", CompanyId = 1, ClaimDate = new DateTime(2023, 4, 10), LossDate = new DateTime(2023, 4, 5), AssuredName = "Test Company 1", IncurredLoss = 12000, Closed = true };
            _mockClaimRepository.Setup(repo => repo.GetClaimByUcrAsync(ucr))
                .ReturnsAsync(existingClaim);

            
            var result = await _controller.UpdateClaimAsync(ucr, updatedClaim);

            
            Assert.IsInstanceOfType<NoContentResult>(result);
            _mockClaimRepository.Verify(repo => repo.UpdateClaimAsync(It.Is<Claim>(
                c => c.UCR == updatedClaim.UCR &&
                     c.CompanyId == updatedClaim.CompanyId &&
                     c.ClaimDate == updatedClaim.ClaimDate &&
                     c.LossDate == updatedClaim.LossDate &&
                     c.AssuredName == updatedClaim.AssuredName &&
                     c.IncurredLoss == updatedClaim.IncurredLoss &&
                     c.Closed == updatedClaim.Closed)), Times.Once);
        }

        [TestMethod]
        public async Task GetClaimByUcrAsyncLossDateInFuture_Test()
        {
            
            string ucr = "UCR005";
            var expectedClaim = new Claim
            {
                UCR = "UCR005",
                CompanyId = 1,
                ClaimDate = new DateTime(2024, 4, 1),
                LossDate = DateTime.Now.AddDays(7), 
                AssuredName = "Company A",
                IncurredLoss = 10000,
                Closed = false
            };
            _mockClaimRepository.Setup(repo => repo.GetClaimByUcrAsync(ucr))
                .ReturnsAsync(expectedClaim);

            
            var result = await _controller.GetClaimByUcrAsync(ucr);

            
            Assert.IsInstanceOfType<BadRequestObjectResult>(result);
        }
    }

}

