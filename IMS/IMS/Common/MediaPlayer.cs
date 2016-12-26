using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;

namespace IMS.Common
{
    public class MediaPlayer
    {
        public static System.Media.SoundPlayer sp = new SoundPlayer();
        public static void PlaySound(string path)
        {
            sp.SoundLocation = path;
            sp.Play();
        }
    }
}
