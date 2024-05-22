using RotterdamDetectives_Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using RotterdamDetectives_Logic;

namespace RotterdamDetectives_Test.TestGame
{
    [TestClass]
    public class TestGameRest
    {
        [TestMethod]
        public void TestLeaveValid()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockGameMasterOf = "Alice";
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Leave("Bob");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestLeaveNotInGame()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Leave("Bob");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestLeaveGameMaster()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockGameMasterOf = "Alice";
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Leave("Alice");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestIsMrX()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockGameMasterOf = "Bob";
            mockGameDB.MockGetMrX = "Alice";
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.IsMrX("Alice");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCreate()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Create("Alice");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestCreateAlreadyGameMaster()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockGameMasterOf = "Alice";
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Create("Alice");

            // Assert
            Assert.IsFalse(result.IsOk);
        }
    }
}