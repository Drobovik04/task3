namespace task3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<Dice> dice = DiceParser.Parse(args);
                Game game = new Game(dice);
                game.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
