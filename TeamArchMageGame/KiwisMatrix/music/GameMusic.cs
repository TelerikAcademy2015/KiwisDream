using System;
using System.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace music
{
    class GameMusic
    {
        static void Main()
        {
            while (true)
            {
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = @"C:\Users\Yumo\Desktop\Final2.wav";
                player.PlaySync();
            }

        }
    }
}
