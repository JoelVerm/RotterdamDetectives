using RotterdamDetectives_Presentation;
using RotterdamDetectives_Data;
using RotterdamDetectives_Logic;
using RotterdamDetectives_Main;

var connectionString = File.ReadAllText("ConnectionString.txt");

var gameDB = new GameDB(connectionString);
var playerDB = new PlayerDB(connectionString);
var stationDB = new StationDB(connectionString);
var ticketDB = new TicketDB(connectionString);

var passwordHasher = new PasswordHasher();

var ticket = new Ticket(ticketDB);
var station = new Station(stationDB);
var game = new Game(gameDB, ticket);
var player = new Player(playerDB, station, ticket, passwordHasher);
var admin = new Admin(gameDB, playerDB, stationDB, ticketDB);

var presentation = new Presentation(game, player, station, ticket, admin);
presentation.Start();
