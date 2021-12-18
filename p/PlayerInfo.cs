using System;

namespace Hangman
{
    public class PlayerInfo
    {
        public string name { get; set; }
        public int score { get; set; }
        public double time { get; set; }

        public PlayerInfo(string name, int score, double time)
        {
            this.name = name;
            this.score = score;
            this.time = time;
        }
    }
}