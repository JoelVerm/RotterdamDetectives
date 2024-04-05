using RotterdamDetectives_Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace RotterdamDetectives_Test
{
    [TestClass]
    public class TestHomeController
    {
        [TestMethod]
        public void TestIndex()
        {
            // Arrange
            var processedDataSource = new MockProcessedDataSource();
            processedDataSource.ShouldReturnLoggedinUser = true;

            var controller = new HomeController(processedDataSource);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}