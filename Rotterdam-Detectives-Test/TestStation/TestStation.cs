using RotterdamDetectives_Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using RotterdamDetectives_Logic;

namespace RotterdamDetectives_Test.TestStation
{
    [TestClass]
    public class TestStation
    {
        [TestMethod]
        public void TestAddConnectionValid()
        {
            // Arrange
            var mockStationDB = new MockStationDB();
            var station = new Station(mockStationDB);

            // Act
            var result = station.AddConnection("Rotterdam", "Amsterdam", "Intercity", "Train");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestAddConnectionInvalid()
        {
            // Arrange
            var mockStationDB = new MockStationDB();
            mockStationDB.MockConnectionsFrom.Add(new MockConnection { Destination = "Amsterdam", Name = "Intercity", ModeOfTransport = "Train" });
            var station = new Station(mockStationDB);

            // Act
            var result = station.AddConnection("Rotterdam", "Amsterdam", "Intercity", "Train");

            // Assert
            Assert.IsFalse(result.IsOk);
        }
    }
}