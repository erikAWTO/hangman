using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace Hangman
{
    public class Program
    {
        //Håller koll på tid för respektive spelare
        private static double[] time = { 0, 0 };

        //Anropas när Timer för spelare 1 överflödas
        private static void OnTimedEvent1(object source, ElapsedEventArgs e)
        {
            time[0] += 0.01;
        }

        //Anropas när Timer för spelare 2 överflödas
        private static void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            time[1] += 0.01;
        }

        //Lambda för att sortera lista
        private static int CompareValue(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
        {
            return a.Value.CompareTo(b.Value);
        }

        private static string[] ordLista =
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

        private static string ReplaceCharInString(string text, int position, char letter)
        {
            text = text.Remove(position, 1);
            text = text.Insert(position, letter.ToString());

            return text;
        }

        private static void ReadHighScore()
        {
        }

        private static void Main(string[] args)
        {
            Timer TimerP1 = new Timer();
            Timer TimerP2 = new Timer();

            int[] currentScore = { 0, 0 };

            //SortedList<string, int> highScore = new SortedList<string, int>();
            List<Tuple<string, int>> highScore = new List<Tuple<string, int>>();
            //int[] totalScore = { 0, 0, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 };

            string path = @"c:\temp\notHighScore.txt";

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path)) ;
            }

            using (StreamReader sr = File.OpenText(path))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var values = line.Split(',');

                    var val1 = values[0].ToString();
                    var val2 = int.Parse(values[1], System.Globalization.NumberStyles.Float);

                    Tuple<string, int> highScoreTuple = Tuple.Create<string, int>(val1, val2);

                    highScore.Add(highScoreTuple);
                }
            }

            bool nyRunda = true;
            bool omstart = true;

            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Hello");
                    sw.WriteLine("And");
                    sw.WriteLine("Welcome");
                }
            }

            // Open the file to read from.

            while (omstart == true)
            {
                Console.WriteLine("Skriv in namn på spelare 1: ");
                string namn1 = Console.ReadLine();

                Console.WriteLine("Skriv in namn på spelare 2: ");
                string namn2 = Console.ReadLine();

                for (int player = 0; player < 2; player++)
                {
                    if (player == 0)
                    {
                        TimerP1.Elapsed += new ElapsedEventHandler(OnTimedEvent1);
                        TimerP1.Interval = 10;
                        TimerP1.Enabled = true;
                    }
                    else
                    {
                        TimerP2.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
                        TimerP2.Interval = 10;
                        TimerP2.Enabled = true;
                    }

                    while (nyRunda == true)
                    {
                        string nyttOrd = "";

                        Random rand = new Random(Environment.TickCount);

                        string text = ordLista[rand.Next(ordLista.Length)];

                        // Skapar ett nytt ord med lika många understräck
                        for (int i = 0; i < text.Length; i++)
                        {
                            nyttOrd += "_";
                        }

                        // Användaren har 5 liv

                        bool avsluta = false;

                        while (avsluta == false)
                        {
                            Console.WriteLine();
                            bool rättSvar = false;

                            //Användaren gissar en bokstav
                            Console.WriteLine("Gissa en bokstav: ");
                            Console.WriteLine("Ditt ord ser ut såhär: " + nyttOrd);
                            char gissadBokstav = Console.ReadKey().KeyChar;
                            Console.WriteLine();

                            // Kontrollerar om bokstaven finns i ordet
                            for (int i = 0; i < text.Length; i++)
                            {
                                if (gissadBokstav == text[i])
                                {
                                    nyttOrd = ReplaceCharInString(nyttOrd, i, gissadBokstav);
                                    rättSvar = true;
                                }
                            }

                            currentScore[player]++;

                            // Om alla bokstäver är rätt vinner användaren
                            if (text == nyttOrd)
                            {
                                Console.WriteLine("Ordet var: " + text);
                                Console.WriteLine("Spelare " + namn1 + " score blev: " + currentScore[player]);
                                if (player == 0)
                                {
                                    TimerP1.Enabled = false;

                                    highScore.Add(new Tuple<string, int>(namn1, currentScore[0]));

                                    Console.WriteLine("------------------");
                                    Console.WriteLine("Nu är det " + namn2 + " som ska köra");
                                    Console.WriteLine("När " + namn2 + " är reda tryck på valfri tangent");
                                    Console.ReadKey();
                                    Console.Clear();
                                }
                                else
                                {
                                    TimerP2.Enabled = false;

                                    highScore.Add(new Tuple<string, int>(namn2, currentScore[1]));

                                    Console.WriteLine("------------------");
                                    Console.WriteLine(namn1 + " Score: " + currentScore[0] + " Tid: " + Math.Round(time[0], 3) + "s");
                                    Console.WriteLine();
                                    Console.WriteLine(namn2 + " Score: " + currentScore[1] + " Tid: " + Math.Round(time[1], 3) + "s");
                                    Console.WriteLine();

                                    if (currentScore[0] > currentScore[1])
                                    {
                                        Console.WriteLine("Vinnare: " + namn2);
                                    }
                                    else if (currentScore[0] < currentScore[1])
                                    {
                                        Console.WriteLine("Vinnare: " + namn1);
                                    }
                                    else
                                    {
                                        if (time[0] < time[1])
                                        {
                                            Console.WriteLine("Vinnare: " + namn1);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Vinnare: " + namn2);
                                        }
                                    }
                                    Console.WriteLine("Tryck på valfri tangent för att visa highscore");

                                    //Sortera listan stigande beroende på score, 1 2 3 4 ....
                                    highScore.Sort((a, b) => a.Item2.CompareTo(b.Item2));

                                   
                                    using (StreamWriter sw = File.AppendText(path))
                                    {
                                        foreach (var pair in highScore)
                                        {
                                            sw.WriteLine(pair.Item1 + "," + pair.Item2);
                                        }
                                    }
                                    

                                    Console.ReadKey();
                                    Console.Clear();
                                    Console.WriteLine("High Score");
                                    Console.WriteLine();

                                    foreach (var pair in highScore)
                                    {
                                        Console.WriteLine("Namn: " + pair.Item1 + " Score: " + pair.Item2);
                                        Console.WriteLine();
                                    }

                                    Console.WriteLine("Starta om? Tryck på space");

                                    if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                                    {
                                        omstart = true;

                                        currentScore[0] = 0;
                                        currentScore[1] = 0;

                                        Console.Clear();
                                    }
                                    else
                                    {
                                        omstart = false;
                                    }
                                }

                                break;
                            }
                            else
                            {
                                // Om gissningen var rätt
                                if (rättSvar == true)
                                {
                                    Console.WriteLine("Rätt gissat");
                                }

                                // Om bokstaven var fel
                                else
                                {
                                    Console.WriteLine("Fel gissat");
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