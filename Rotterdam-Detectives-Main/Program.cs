using RotterdamDetectives_Presentation;
using RotterdamDetectives_Data;
using RotterdamDetectives_Logic;
using RotterdamDetectives_Main;

var data = new DataSource("Server=(localdb)\\MSSQLLocalDB;Database=RotterdamDetectives;Integrated Security=True;");

var passwordHasher = new PasswordHasher();

var logic = new Logic(data, passwordHasher);

var presentation = new Presentation(logic);
presentation.Start();
