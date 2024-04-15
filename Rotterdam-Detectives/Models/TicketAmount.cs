using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Models
{
    public class TicketAmount(ModeOfTransport modeOfTransport, int amount)
    {
        public ModeOfTransport ModeOfTransport { get; } = modeOfTransport;
        public int Amount { get; } = amount;
    }
}
