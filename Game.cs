using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    internal class Game
    {
        private readonly List<Dice> dices;
        private readonly FairRandomGenerator rng;
        private int selectedByComputer;
        private int selectedByPlayer;
        private byte[] computerSelectionKey;
        private int diceOne;
        private int diceTwo;
        private int firstSelectionPlayer;
        private int firstSelectionComputer;
        private int secondSelectionPlayer;
        private int secondSelectionComputer;
        private int faceComputer;
        private int facePlayer;
        private TableGenerator gen;

        public Game(List<Dice> dices)
        {
            this.dices = dices;
            this.rng = new FairRandomGenerator();
            gen = new TableGenerator(dices, ProbabilityCalculator.CalculateProbabilities(dices));
        }

        public void Start()
        {
            Console.WriteLine("Let's determine who makes the first move.");

            byte[] key = rng.GenerateKey();
            int computerSelection = rng.GenerateSecureRoll(2);
            string hmac = rng.GenerateHMAC(key, computerSelection);
            Console.WriteLine($"I selected a random value in the range 0..1 (HMAC={hmac}).");

            Console.WriteLine("Try to guess my selection.\n0 - 0\n1 - 1\nX - exit\n? - help");
            string userInput = Console.ReadLine();
            bool notChoose = true;
            while (notChoose)
            {
                switch (userInput)
                {
                    case "X":
                        {
                            notChoose = false;
                            Environment.Exit(0);
                            break;
                        }
                    case "0":
                        {
                            Console.WriteLine("Your selection: " + userInput);
                            notChoose = false;
                            break;
                        }
                    case "1":
                        {
                            Console.WriteLine("Your selection: " + userInput);
                            notChoose = false;
                            break;
                        }
                    case "?":
                        {
                            gen.PrintTable();
                            Console.WriteLine("Try to guess my selection.\n0 - 0\n1 - 1\nX - exit\n? - help");
                            userInput = Console.ReadLine();
                            break;
                        }
                    default:
                        {
                            notChoose = false;
                            Console.WriteLine("Undefined option, press any key to close program");
                            Console.ReadKey();
                            return;
                        }
                }
            }

            int userSelection = int.Parse(userInput);
            Console.WriteLine($"My selection: {computerSelection} (KEY={BitConverter.ToString(key).Replace("-", "")}).");
            notChoose = true;

            bool computerFirst = computerSelection != userSelection;
            if (computerFirst)
            {
                selectedByComputer = new Random().Next(dices.Count);
                Console.WriteLine($"I make the first move and choose the [{dices[selectedByComputer].ToString()}] dice");
                Console.WriteLine("Choose your dice:");
                for (int i = 0; i < selectedByComputer; i++)
                {
                    Console.WriteLine($"{i} - {dices[i].ToString()}");
                }
                for (int i = selectedByComputer + 1; i < dices.Count; i++)
                {
                    Console.WriteLine($"{i-1} - {dices[i].ToString()}");
                }
                Console.WriteLine("X - exit\n? - help");
                string userInput1 = Console.ReadLine();
                switch (userInput1)
                {
                    case "X":
                        {
                            notChoose = false;
                            Environment.Exit(0);
                            break;
                        }
                    case "?":
                        {
                            gen.PrintTable();
                            Console.WriteLine("Choose your dice:");
                            for (int i = 0; i < selectedByComputer; i++)
                            {
                                Console.WriteLine($"{i} - {dices[i].ToString()}");
                            }
                            for (int i = selectedByComputer + 1; i < dices.Count; i++)
                            {
                                Console.WriteLine($"{i - 1} - {dices[i].ToString()}");
                            }
                            Console.WriteLine("X - exit\n? - help");
                            userInput1 = Console.ReadLine();
                            break;
                        }
                    default:
                        {
                            notChoose = false;
                            int inp;
                            bool result = int.TryParse(userInput1, out inp);
                            if (result && inp >= 0 && inp < dices.Count - 1)
                            {
                                selectedByPlayer = inp;
                                if (selectedByPlayer >= selectedByComputer)
                                {
                                    selectedByPlayer++;
                                }
                                Console.WriteLine($"Your choose the [{dices[selectedByPlayer].ToString()}] dice.");
                            }
                            else
                            {
                                throw new Exception("Undefined option, press any key to close program.");
                            }
                            break;
                        }
                }
            }
            else
            {
                Console.WriteLine("Choose your dice:");
                for (int i = 0; i < dices.Count; i++)
                {
                    Console.WriteLine($"{i} - {dices[i].ToString()}");
                }
                Console.WriteLine("X - exit\n? - help");
                string userInput1 = Console.ReadLine();
                switch (userInput1)
                {
                    case "X":
                        {
                            notChoose = false;
                            Environment.Exit(0);
                            break;
                        }
                    case "?":
                        {
                            gen.PrintTable();
                            Console.WriteLine("Choose your dice:");
                            for (int i = 0; i < selectedByComputer; i++)
                            {
                                Console.WriteLine($"{i} - {dices[i].ToString()}");
                            }
                            for (int i = selectedByComputer + 1; i < dices.Count; i++)
                            {
                                Console.WriteLine($"{i - 1} - {dices[i].ToString()}");
                            }
                            Console.WriteLine("X - exit\n? - help");
                            userInput1 = Console.ReadLine();
                            break;
                        }
                    default:
                        {
                            notChoose = false;
                            int inp;
                            bool result = int.TryParse(userInput1, out inp);
                            if (result && inp >= 0 && inp < dices.Count)
                            {
                                selectedByPlayer = inp;
                                Console.WriteLine($"Your make the first move and choose the [{dices[selectedByPlayer].ToString()}] dice.");
                            }
                            else
                            {
                                throw new Exception("Undefined option, press any key to close program.");
                            }
                            break;
                        }
                }
                selectedByComputer = new Random().Next(dices.Count - 1);
                if (selectedByComputer >= selectedByPlayer)
                {
                    selectedByComputer++;
                }
                Console.WriteLine($"I choose the [{dices[selectedByComputer].ToString()}] dice.");
            }

            PlayRound(computerFirst);
        }

        private void PlayRound(bool computerFirst)
        {
            if (computerFirst)
            {
                Console.WriteLine("It's time for my throw.");
                ComputerTurn();
                UserTurn(true, "My", out diceOne);
                Console.WriteLine("It's time for your throw.");
                ComputerTurn();
                UserTurn(false, "Your", out diceTwo);
            }
            else
            {
                Console.WriteLine("It's time for your throw.");
                ComputerTurn();
                UserTurn(false, "Your", out diceTwo);
                Console.WriteLine("It's time for my throw.");
                ComputerTurn();
                UserTurn(true, "My", out diceOne);
            }

            if (diceTwo > diceOne)
            {
                Console.WriteLine($"You win ({diceTwo} > {diceOne})!");
                return;
            }
            if (diceOne > diceTwo)
            {
                Console.WriteLine($"You lost ({diceTwo} < {diceOne})!");
                return;
            }
            Console.WriteLine($"Draw ({diceTwo} = {diceOne})!");
            return;
        }

        private void ComputerTurn()
        {
            computerSelectionKey = rng.GenerateKey();
            firstSelectionComputer = rng.GenerateSecureRoll(6);
            string hmac = rng.GenerateHMAC(computerSelectionKey, firstSelectionComputer);
            Console.WriteLine($"I selected a random value in the range 0..5 (HMAC={hmac}).\nAdd your number modulo 6.");
        }

        private void UserTurn(bool computerDice, string name, out int face)
        {
            bool notChoose = true;
            for (int i = 0; i < 6; i++)
                Console.WriteLine($"{i} - {i}");
            Console.WriteLine("X - exit\n? - help");
            string userInput1 = Console.ReadLine();
            switch (userInput1)
            {
                case "X":
                    {
                        notChoose = false;
                        Environment.Exit(0);
                        break;
                    }
                case "?":
                    {
                        gen.PrintTable();
                        Console.WriteLine("Add your number modulo 6.");
                        for (int i = 0; i < 6; i++)
                            Console.WriteLine($"{i} - {i}");
                        Console.WriteLine("X - exit\n? - help");
                        userInput1 = Console.ReadLine();
                        break;
                    }
                default:
                    {
                        notChoose = false;
                        int inp;
                        bool result = int.TryParse(userInput1, out inp);
                        if (result && inp >= 0 && inp < 6)
                        {
                            firstSelectionPlayer = inp;
                            Console.WriteLine($"Your selection: {firstSelectionPlayer}");
                        }
                        else
                        {
                            throw new Exception("Undefined option, press any key to close program.");
                        }
                        break;
                    }
            }
            Console.WriteLine($"My number is {firstSelectionComputer} (KEY={BitConverter.ToString(computerSelectionKey).Replace("-", "")}).");
            int mod6 = (firstSelectionComputer + firstSelectionPlayer) % 6;
            Console.WriteLine($"The result is {firstSelectionComputer} + {firstSelectionPlayer} = {mod6} (mod 6).");
            face = dices[computerDice?selectedByComputer:selectedByPlayer][mod6];
            Console.WriteLine($"{name} throw is {face}.");
        }
    }
}
