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
            //(new SoundPlayer(@"C:\Users\Yumo\Desktop\kiviwav.wav")).PlayLooping();
            //(new SoundPlayer(@"C:\Users\Yumo\Desktop\kiviwav.wav")).SoundLocation(C:\Users\Yumo\Desktop\kiviwav.wav);
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = @"C:\Users\Yumo\Desktop\kiviwav.wav";
            player.PlaySync();
            player.PlayLooping();
        }
    }
}
