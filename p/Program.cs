using System;
using System.Timers;

namespace Hangman
{
    internal class Program
    {
        private static double[] time = { 0, 0 };

        private static void OnTimedEvent1(object source, ElapsedEventArgs e)
        {
            time[0] += 0.1;
        }

        private static void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            time[1] += 0.1;
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

        private static void Main(string[] args)
        {
            Timer TimerP1 = new Timer();
            Timer TimerP2 = new Timer();

            int[] score = { 0, 0 };

            bool omstart = true;

            for (int player = 0; player < 2; player++)
            {
                if (player == 0)
                {
                    TimerP1.Elapsed += new ElapsedEventHandler(OnTimedEvent1);
                    TimerP1.Interval = 100;
                    TimerP1.Enabled = true;
                }
                else
                {
                    TimerP2.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
                    TimerP2.Interval = 100;
                    TimerP2.Enabled = true;
                }

                while (omstart == true)
                {
                    string nyttOrd = "";

                    //// Användaren skriver in det valda ordet
                    //Console.WriteLine("Välkommen till Hangman the game. Kom på ett ord som din kompis ska gissa");
                    //Console.Write("Skriv in det ord som ska gissas: ");
                    //string text = Console.ReadLine();
                    //// Töm konsolen
                    //Console.Clear();

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

                        // Användaren gissar en bokstav
                        Console.WriteLine("Gissa en bokstav: ");
                        Console.WriteLine("Ditt ord ser ut såhär: " + nyttOrd);
                        char gissadBokstav = char.Parse(Console.ReadLine());

                        // Kontrollerar om bokstaven finns i ordet
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (gissadBokstav == text[i])
                            {
                                nyttOrd = ReplaceCharInString(nyttOrd, i, gissadBokstav);
                                rättSvar = true;
                            }
                        }

                        score[player]++;

                        // Om alla bokstäver är rätt vinner användaren
                        if (text == nyttOrd)
                        {
                            Console.WriteLine("Ordet var: " + text);
                            Console.WriteLine("Spelare " + (player + 1) + " score blev: " + score[player]);
                            if (player == 0)
                            {
                                TimerP1.Enabled = false;
                                Console.WriteLine("------------------");
                                Console.WriteLine("Nu är det spelare 2 som ska köra");
                                Console.WriteLine("När spelare 2 är reda tryck på valfri tangent");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                Console.WriteLine("------------------");
                                Console.WriteLine("Spelare 1: Score: " + score[0] + " Tid: " + Math.Round(time[0], 2));
                                Console.WriteLine();
                                Console.WriteLine("Spelare 2: Score: " + score[1] + " Tid: " + Math.Round(time[1], 2));
                                Console.WriteLine();

                                if (score[0] > score[1])
                                {
                                    Console.WriteLine("Vinnare: Spelare 2");
                                }
                                else if (score[0] < score[1])
                                {
                                    Console.WriteLine("Vinnare: Spelare 1");
                                }
                                else
                                {
                                    if (time[0] < time[1])
                                    {
                                        Console.WriteLine("Vinnare: Spelare 1");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Vinnare: Spelare 2");
                                    }
                                }

                                Console.ReadKey();
                            }

                            break;
                            //avsluta = true;
                        }
                        else
                        {
                            // Om gissningen var rätt skriv rätt gissat
                            if (rättSvar == true)
                            {
                                Console.WriteLine("Rätt gissat");
                            }

                            // Om bokstaven var fel dras ett liv
                            else
                            {
                                Console.WriteLine("Fel gissat");
                                //ScoreP1++;
                            }
                        }
                    }
                    break;
                    // Programmet avslutas
                    //return;
                }
            }
        }
    }
}