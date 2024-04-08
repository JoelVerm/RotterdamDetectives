namespace RotterdamDetectives_Presentation
{
    public static class Extensions
    {
        public static string OrDefault(this string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            return value;
        }
    }
}
