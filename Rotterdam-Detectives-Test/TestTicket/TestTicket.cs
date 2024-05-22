using RotterdamDetectives_Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using RotterdamDetectives_Logic;

namespace RotterdamDetectives_Test.TestTicket
{
    [TestClass]
    public class TestTicket
    {
        [TestMethod]
        public void TestUseValid()
        {
            // Arrange
            var mockTicketDB = new MockTicketDB();
            mockTicketDB.MockGetMaxTickets["tram"] = 3;
            mockTicketDB.MockGetHistoryCount = 1;
            var ticket = new Ticket(mockTicketDB);

            // Act
            var result = ticket.Use("player1", "tram");

            // Assert
            Assert.IsTrue(result.IsOk);
        }

        [TestMethod]
        public void TestUseNoTicketsLeft()
        {
            // Arrange
            var mockTicketDB = new MockTicketDB();
            mockTicketDB.MockGetMaxTickets["tram"] = 3;
            mockTicketDB.MockGetHistoryCount = 3;
            var ticket = new Ticket(mockTicketDB);

            // Act
            var result = ticket.Use("player1", "tram");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestUseWrongMode()
        {
            // Arrange
            var mockTicketDB = new MockTicketDB();
            mockTicketDB.MockGetMaxTickets["tram"] = 3;
            mockTicketDB.MockGetHistoryCount = 1;
            var ticket = new Ticket(mockTicketDB);

            // Act
            var result = ticket.Use("player1", "bus");

            // Assert
            Assert.IsFalse(result.IsOk);
        }

        [TestMethod]
        public void TestGetSpare()
        {
            // Arrange
            var mockTicketDB = new MockTicketDB();
            mockTicketDB.MockGetMaxTickets["tram"] = 3;
            mockTicketDB.MockGetHistoryCount = 1;
            var ticket = new Ticket(mockTicketDB);

            // Act
            var result = ticket.GetSpare("player1")["tram"];

            // Assert
            Assert.AreEqual(2, result);
        }
    }
}