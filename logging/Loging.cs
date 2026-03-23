namespace villa.logging
{
    public class Loging : ILoging
    {
        public void Log(string message, string type)
        {
            if (type == "Error")
            {
                Console.WriteLine($"ERROR - {message}");
            } else
            {
                Console.WriteLine($"INFO - {type} - {message}");
            }
        }
    } 
}