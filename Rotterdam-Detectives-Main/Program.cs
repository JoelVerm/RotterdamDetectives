using RotterdamDetectives_Presentation;
using RotterdamDetectives_Data;
using RotterdamDetectives_Logic;

var data = new DataSource("Server=(localdb)\\MSSQLLocalDB;Database=RotterdamDetectives;Integrated Security=True;");

var logic = new ProcessedDataSource(data);

var presentation = new Presentation(logic);
presentation.Start();
