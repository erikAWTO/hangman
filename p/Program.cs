using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Hangman
{
    public class Program
    {
        private static List<PlayerInfo> highScore = new List<PlayerInfo>();

        private static double[] currentTime = { 0, 0 };

        private static int[] currentScore = { 0, 0 };

        private static bool newGame = true;

        // Anropas när Timer för spelare 1 överflödas
        private static void OnTimedEvent1(object source, ElapsedEventArgs e)
        {
            currentTime[0] += 0.01;
        }

        // Anropas när Timer för spelare 2 överflödas
        private static void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            currentTime[1] += 0.01;
        }

        // Visar highscorelistan
        private static void ShowHighScore()
        {
            Console.WriteLine("Topp 5, sorterat med lägst poäng först");
            Console.WriteLine();

            foreach (var player in highScore)
            {
                Console.WriteLine("Spelare: {0} Poäng: {1} Tid: {2}", player.name, player.score, Math.Round(player.time, 3));
            }
            Console.WriteLine();
            Console.WriteLine("Tryck på valfri tangent för ny runda");
            Console.WriteLine("Tryck på esc för att avsluta spelet");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Escape:
                    newGame = false;
                    break;

                default:
                    Console.Clear();
                    break;
            }
        }

        // Sorterar highscore listan baserat på poäng i stigande ordning
        private static void SortHighScore()
        {
            highScore = highScore.OrderBy(PlayerInfo => PlayerInfo.score).ToList();
        }

        // Lägger till en spelare i highscorelistan
        private static void AddToHighScore(int currentPlayer, string name)
        {
            PlayerInfo player = new PlayerInfo(name, currentScore[currentPlayer], Math.Round(currentTime[currentPlayer], 3));

            if (highScore.Count() < 5)
            {
                highScore.Add(player);
            }
            else
            {
                if (highScore[4].score > player.score)
                {
                    highScore.RemoveAt(4);
                    highScore.Add(player);
                }
                return;
            }
        }

        // Nollställer poäng för den aktuella rundan
        private static void ResetScore()
        {
            currentScore[0] = 0;
            currentScore[1] = 0;
        }

        // Nollställer tid för den aktuella rundan
        private static void ResetTime()
        {
            currentTime[0] = 0;
            currentTime[1] = 0;
        }

        // Skapar en string med blanksteg med samma längd som det hemliga ordet
        private static string CreateHiddenSecretWord(string word)
        {
            string hiddenWord = "";

            for (int i = 0; i < word.Length; i++)
            {
                hiddenWord += "_";
            }

            return hiddenWord;
        }

        // Slumpar fram ett ord från ordlistan
        private static string PickSecretWord()
        {
            Random rand = new Random(Environment.TickCount);

            return wordList[rand.Next(wordList.Length)];
        }

        // Byter ut en char i en string
        private static string ReplaceCharInString(string text, int position, char letter)
        {
            text = text.Remove(position, 1);
            text = text.Insert(position, letter.ToString());

            return text;
        }

        // Visar resultat för den aktuella rundan
        private static void ShowResults(string name1, string name2)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("{0} Poäng: {1} Tid: {2}s", name1, currentScore[0], Math.Round(currentTime[0], 3));
            Console.WriteLine();
            Console.WriteLine("{0} Poäng: {1} Tid: {2}s", name2, currentScore[1], Math.Round(currentTime[1], 3));
            Console.WriteLine();

            if (currentScore[0] > currentScore[1])
            {
                Console.WriteLine("Vinnare: " + name2);
            }
            else if (currentScore[0] < currentScore[1])
            {
                Console.WriteLine("Vinnare: " + name1);
            }
            else // Om poängen är lika
            {
                // Jämför vem som har bäst tid
                if (currentTime[0] < currentTime[1])
                {
                    Console.WriteLine("Vinnare: " + name1);
                }
                else
                {
                    Console.WriteLine("Vinnare: " + name2);
                }
            }
        }

        // Ordlista
        private static string[] wordList =
        {
            "ratt",
            "apa",
            "uppgift",
            "beer",
            "blomma",
            "leon",
            "lampa",
            "smörgås",
            "kruka",
            "ko"
        };

        public static void Main(string[] args)
        {
            while (newGame == true)
            {
                Console.WriteLine("Skriv in namn på spelare 1: ");
                string name1 = Console.ReadLine();

                Console.WriteLine("Skriv in namn på spelare 2: ");
                string name2 = Console.ReadLine();

                for (int currentPlayer = 0; currentPlayer < 2; currentPlayer++)
                {
                    Timer timer1 = new Timer();
                    timer1.Interval = 10;

                    if (currentPlayer == 0)
                    {
                        timer1.Elapsed += new ElapsedEventHandler(OnTimedEvent1);
                    }
                    else
                    {
                        timer1.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
                    }

                    timer1.Start();

                    while (true)
                    {
                        string secretWord = PickSecretWord();
                        string hiddenSecretWord = CreateHiddenSecretWord(secretWord);

                        while (true)
                        {
                            bool correctGuess = false;

                            Console.WriteLine();

                            // Användaren gissar en bokstav
                            Console.WriteLine("Gissa en bokstav: ");
                            Console.WriteLine("Ditt ord ser ut såhär: " + hiddenSecretWord);
                            char guessedChar = Console.ReadKey().KeyChar;
                            Console.WriteLine();

                            // Kontrollerar om bokstaven finns i ordet
                            for (int i = 0; i < secretWord.Length; i++)
                            {
                                if (guessedChar == secretWord[i])
                                {
                                    hiddenSecretWord = ReplaceCharInString(hiddenSecretWord, i, guessedChar);
                                    correctGuess = true;
                                }
                            }

                            currentScore[currentPlayer]++;

                            // Om alla bokstäver är rätt vinner användaren
                            if (hiddenSecretWord == secretWord)
                            {
                                timer1.Stop();
                                timer1.Dispose();

                                Console.WriteLine("Ordet var: " + secretWord);
                                Console.WriteLine("Spelare " + name1 + " score blev: " + currentScore[currentPlayer]);
                                if (currentPlayer == 0)
                                {
                                    //Lägger till en ny spelare till highscorelistan
                                    //highScore.Add(new PlayerInfo(name1, currentScore[currentPlayer], Math.Round(currentTime[currentPlayer], 3)));
                                    AddToHighScore(currentPlayer, name1);

                                    Console.WriteLine("-------------------------------------------------");
                                    Console.WriteLine("Nu är det " + name2 + " som ska köra");
                                    Console.WriteLine("När " + name2 + " är redo, tryck på valfri tangent");
                                    Console.ReadKey();
                                    Console.Clear();
                                }
                                else
                                {
                                    // Lägger till en ny spelare till highscorelistan
                                    //highScore.Add(new PlayerInfo(name2, currentScore[currentPlayer], Math.Round(currentTime[currentPlayer], 3)));
                                    AddToHighScore(currentPlayer, name2);

                                    ShowResults(name1, name2);

                                    ResetScore();
                                    ResetTime();

                                    Console.WriteLine();
                                    Console.WriteLine("Tryck på space för att visa highscore");
                                    Console.WriteLine("Tryck på valfri tangent för ny runda");
                                    Console.WriteLine("Tryck på esc för att avsluta spelet");

                                    switch (Console.ReadKey().Key)
                                    {
                                        case ConsoleKey.Spacebar:
                                            Console.Clear();
                                            SortHighScore();
                                            ShowHighScore();
                                            Console.Clear();
                                            break;

                                        case ConsoleKey.Escape:
                                            newGame = false;
                                            return;

                                        default:
                                            Console.Clear();
                                            break;
                                    }
                                }
                                break;
                            }
                            else
                            {
                                // Om gissningen var rätt
                                if (correctGuess == true)
                                {
                                    Console.WriteLine("Rätt");
                                }
                                // Om gissningen var fel
                                else
                                {
                                    Console.WriteLine("Fel");
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return;
        }
    }
}