using RotterdamDetectives_Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using RotterdamDetectives_Logic;

namespace RotterdamDetectives_Test.TestGame
{
    [TestClass]
    public class TestGameJoin
    {
        [TestMethod]
        public void TestJoinValid()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockExists = true;
            mockGameDB.MockIsGameStarted = false;
            mockGameDB.MockIsPlayerInGame = false;
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Join("gameMaster", "player");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestJoinGameDoesNotExist()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockExists = false;
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Join("gameMaster", "player");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestJoinPlayerIsGameMaster()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockExists = true;
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Join("gameMaster", "gameMaster");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestJoinGameAlreadyStarted()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockExists = true;
            mockGameDB.MockIsGameStarted = true;
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Join("gameMaster", "player");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestJoinPlayerAlreadyInGame()
        {
            // Arrange
            var mockGameDB = new MockGameDB();
            mockGameDB.MockExists = true;
            mockGameDB.MockIsGameStarted = false;
            mockGameDB.MockIsPlayerInGame = true;
            var mockTicketDB = new MockTicketDB();
            var game = new Game(mockGameDB, new Ticket(mockTicketDB));

            // Act
            var result = game.Join("gameMaster", "player");

            // Assert
            Assert.IsFalse(result.IsOk);
        }
    }
}