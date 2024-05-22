using RotterdamDetectives_Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using RotterdamDetectives_Logic;

namespace RotterdamDetectives_Test.TestGame
{
    [TestClass]
    public class TestGameStartEnd
    {
        [TestMethod]
        public void TestStartValid()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockGetPlayers.Add("Alice");
            mockGameDB.MockGetPlayers.Add("Bob");
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Start("Charlie");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestStartInvalid()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Start("Alice");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestEndValid()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockIsGameStarted = true;
            mockGameDB.MockGameMasterOf = "Alice";
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.End("Alice");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestEndNotStarted()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockIsGameStarted = false;
            mockGameDB.MockGameMasterOf = "Alice";
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.End("Alice");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestEndNotGameMaster()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockIsGameStarted = true;
            mockGameDB.MockGameMasterOf = "Bob";
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.End("Alice");

            // Assert
            Assert.IsFalse(result.IsOk);
        }
    }
}