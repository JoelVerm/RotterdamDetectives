using RotterdamDetectives_Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using RotterdamDetectives_Logic;

namespace RotterdamDetectives_Test.TestAdmin
{
    [TestClass]
    public class TestAdmin
    {
        [TestMethod]
        public void TestConnectExisting()
        {
            // Arrange
            var mockStationDB = new MockStationDB();
            mockStationDB.MockExists = true;
            var admin = new Admin(new MockGameDB(), new MockPlayerDB(), mockStationDB, new MockTicketDB());

            // Act
            var result = admin.ConnectStations("Station1", "Station2", "Name", "ModeOfTransport");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestConnectMissing()
        {
            // Arrange
            var mockStationDB = new MockStationDB();
            mockStationDB.MockExists = false;
            var admin = new Admin(new MockGameDB(), new MockPlayerDB(), mockStationDB, new MockTicketDB());

            // Act
            var result = admin.ConnectStations("Station1", "Station2", "Name", "ModeOfTransport");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestDeleteExisting()
        {
            // Arrange
            var mockStationDB = new MockStationDB();
            mockStationDB.MockExists = true;
            var admin = new Admin(new MockGameDB(), new MockPlayerDB(), mockStationDB, new MockTicketDB());

            // Act
            var result = admin.DeleteStation("Station1");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestDeleteMissing()
        {
            // Arrange
            var mockStationDB = new MockStationDB();
            mockStationDB.MockExists = false;
            var admin = new Admin(new MockGameDB(), new MockPlayerDB(), mockStationDB, new MockTicketDB());

            // Act
            var result = admin.DeleteStation("Station1");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestDisconnectExisting()
        {
            // Arrange
            var mockStationDB = new MockStationDB();
            mockStationDB.MockExists = true;
            var admin = new Admin(new MockGameDB(), new MockPlayerDB(), mockStationDB, new MockTicketDB());

            // Act
            var result = admin.DisconnectStations("Station1", "Station2");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestDisconnectMissing()
        {
            // Arrange
            var mockStationDB = new MockStationDB();
            mockStationDB.MockExists = false;
            var admin = new Admin(new MockGameDB(), new MockPlayerDB(), mockStationDB, new MockTicketDB());

            // Act
            var result = admin.DisconnectStations("Station1", "Station2");

            // Assert
            Assert.IsFalse(result.IsOk);
        }
    }
}