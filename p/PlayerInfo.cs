using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    public class PlayerInfo
    {
        public string name;
        public int score;
        public double time;

        public PlayerInfo(string name, int score, double time)
        {
            this.name = name;
            this.score = score;
            this.time = time;
        }
    }
}
