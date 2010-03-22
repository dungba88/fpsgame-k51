using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using FPSGame.Engine;

namespace FPSGame.Core
{
    public class EffectUtils
    {
        public static void PlaySound(String sound)
        {
            SoundEffect snd = ResourceManager.GetResource<SoundEffect>(sound);
            if (snd != null)
                snd.Play();
        }

        public static void PlaySound(String sound, bool loop)
        {
            SoundEffect snd = ResourceManager.GetResource<SoundEffect>(sound);
            if (snd != null)
                snd.Play(1.0f, 0.0f, 0.0f, loop);
        }

        public static void StopSound(String sound)
        {
            SoundEffect snd = ResourceManager.GetResource<SoundEffect>(sound);
            if (snd != null)
                snd.Dispose();
        }
    }
}
