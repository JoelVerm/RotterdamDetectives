using RotterdamDetectives_Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using RotterdamDetectives_Logic;
using RotterdamDetectives_DataInterface;

namespace RotterdamDetectives_Test.TestPlayer
{
    [TestClass]
    public class TestPlayer
    {
        [TestMethod]
        public void TestMoveToStationSuccess()
        {
            // Arrange
            var mockPlayerDB = new MockPlayerDB();
            mockPlayerDB.MockCurrentStation = "station1";
            var mockStationDB = new MockStationDB();
            mockStationDB.MockConnectionsFrom = [new MockConnection { Destination = "station", ModeOfTransport = "modeOfTransport" }];
            var mockTicketDB = new MockTicketDB();
            mockTicketDB.MockGetMaxTickets["modeOfTransport"] = 5;
            var mockPasswordHasher = new MockPasswordHasher();
            var mockStation = new Station(mockStationDB);
            var mockTicket = new Ticket(mockTicketDB);
            var player = new Player(mockPlayerDB, mockStation, mockTicket, mockPasswordHasher);

            // Act
            var result = player.MoveToStation("player", "station", "modeOfTransport");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestMoveToStationPlayerDoesNotExist()
        {
            // Arrange
            var mockPlayerDB = new MockPlayerDB();
            var mockStationDB = new MockStationDB();
            var mockTicketDB = new MockTicketDB();
            var mockPasswordHasher = new MockPasswordHasher();
            var mockStation = new Station(mockStationDB);
            var mockTicket = new Ticket(mockTicketDB);
            var player = new Player(mockPlayerDB, mockStation, mockTicket, mockPasswordHasher);

            // Act
            var result = player.MoveToStation("player", "station", "modeOfTransport");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestMoveToStationStationNotConnected()
        {
            // Arrange
            var mockPlayerDB = new MockPlayerDB();
            mockPlayerDB.MockCurrentStation = "station1";
            var mockStationDB = new MockStationDB();
            var mockTicketDB = new MockTicketDB();
            var mockPasswordHasher = new MockPasswordHasher();
            var mockStation = new Station(mockStationDB);
            var mockTicket = new Ticket(mockTicketDB);
            var player = new Player(mockPlayerDB, mockStation, mockTicket, mockPasswordHasher);

            // Act
            var result = player.MoveToStation("player", "station", "modeOfTransport");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestMoveToStationWrongModeOfTransport()
        {
            // Arrange
            var mockPlayerDB = new MockPlayerDB();
            mockPlayerDB.MockCurrentStation = "station1";
            var mockStationDB = new MockStationDB();
            mockStationDB.MockConnectionsFrom = [new MockConnection { Destination = "station", ModeOfTransport = "modeOfTransport" }];
            var mockTicketDB = new MockTicketDB();
            var mockPasswordHasher = new MockPasswordHasher();
            var mockStation = new Station(mockStationDB);
            var mockTicket = new Ticket(mockTicketDB);
            var player = new Player(mockPlayerDB, mockStation, mockTicket, mockPasswordHasher);

            // Act
            var result = player.MoveToStation("player", "station", "wrongModeOfTransport");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestMoveToStationNotEnoughTickets()
        {
            // Arrange
            var mockPlayerDB = new MockPlayerDB();
            mockPlayerDB.MockCurrentStation = "station1";
            var mockStationDB = new MockStationDB();
            mockStationDB.MockConnectionsFrom = [new MockConnection { Destination = "station", ModeOfTransport = "modeOfTransport" }];
            var mockTicketDB = new MockTicketDB();
            mockTicketDB.MockGetMaxTickets["modeOfTransport"] = 0;
            var mockPasswordHasher = new MockPasswordHasher();
            var mockStation = new Station(mockStationDB);
            var mockTicket = new Ticket(mockTicketDB);
            var player = new Player(mockPlayerDB, mockStation, mockTicket, mockPasswordHasher);

            // Act
            var result = player.MoveToStation("player", "station", "modeOfTransport");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestRegisterSuccess()
        {
            // Arrange
            var mockPlayerDB = new MockPlayerDB();
            var mockStationDB = new MockStationDB();
            mockStationDB.MockStations = ["station1", "station2"];
            var mockTicketDB = new MockTicketDB();
            var mockPasswordHasher = new MockPasswordHasher();
            var mockStation = new Station(mockStationDB);
            var mockTicket = new Ticket(mockTicketDB);
            var player = new Player(mockPlayerDB, mockStation, mockTicket, mockPasswordHasher);

            // Act
            var result = player.Register("player", "password");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestRegisterUsernameAlreadyExists()
        {
            // Arrange
            var mockPlayerDB = new MockPlayerDB();
            mockPlayerDB.MockPasswordHash = "hash";
            var mockStationDB = new MockStationDB();
            var mockTicketDB = new MockTicketDB();
            var mockPasswordHasher = new MockPasswordHasher();
            var mockStation = new Station(mockStationDB);
            var mockTicket = new Ticket(mockTicketDB);
            var player = new Player(mockPlayerDB, mockStation, mockTicket, mockPasswordHasher);

            // Act
            var result = player.Register("player", "password");

            // Assert
            Assert.IsFalse(result.IsOk);
        }
    }
}