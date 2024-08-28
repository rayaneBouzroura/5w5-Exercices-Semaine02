using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using WebApi.DTO;
using WebApplication1.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestDataController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        //controller to inject dbcontext
        public TestDataController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateTestData(CreateTestDataDTO createTestDataDTO)
        {
            TestData td = new TestData() { Name = createTestDataDTO.Name };
            _dbContext.TestData.Add(td);
            _dbContext.SaveChanges();
            //to notify notre user that the test data has been created
            return Ok(new object[] { "test data "+createTestDataDTO.Name +" created" });
        }
    }
}
