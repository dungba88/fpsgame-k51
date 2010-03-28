using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using FPSGame.Engine;
using Microsoft.Xna.Framework;

namespace FPSGame.Core
{
    public class EffectUtils
    {
        private static EffectUtils instance = new EffectUtils();

        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;
        private Dictionary<String, Cue> cues;

        public static EffectUtils GetInstance()
        {
            return instance;
        }

        private EffectUtils()
        {
            LoadResource();
        }

        public void Update(GameTime gameTime)
        {
            audioEngine.Update();
        }
        
        private void LoadResource()
        {
            audioEngine = new AudioEngine("Content/Sounds/xactProject.xgs");
            waveBank = new WaveBank(audioEngine, "Content/Sounds/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content/Sounds/Sound Bank.xsb");

            ResourceManager.RegisterResource(ResourceManager.OPERA_THEME_SONG, "opera");
            ResourceManager.RegisterResource(ResourceManager.PLAYER_GUN_SND, "Machine gun 2");
            ResourceManager.RegisterResource(ResourceManager.PLAYER_WALKING_SND, "footsteps-3");

            cues = new Dictionary<string, Cue>();
            cues.Add(ResourceManager.PLAYER_WALKING_SND, soundBank.GetCue(ResourceManager.GetResource<String>(ResourceManager.PLAYER_WALKING_SND)));
            cues.Add(ResourceManager.OPERA_THEME_SONG, soundBank.GetCue(ResourceManager.GetResource<String>(ResourceManager.OPERA_THEME_SONG)));
            cues.Add(ResourceManager.PLAYER_GUN_SND, soundBank.GetCue(ResourceManager.GetResource<String>(ResourceManager.PLAYER_GUN_SND)));
        }

        public void PlaySound(String sound, bool interrupt)
        {
            Cue cue = cues[sound];
            if (cue.IsPlaying)
            {
                if (interrupt)
                {
                    StopSound(sound);
                    cues.Remove(sound);
                    cues.Add(sound, soundBank.GetCue(ResourceManager.GetResource<String>(sound)));
                    cue = cues[sound];
                }
                else
                {
                    return;
                }
            }
            if (cue.IsStopped)
            {
                cues.Remove(sound);
                cues.Add(sound, soundBank.GetCue(ResourceManager.GetResource<String>(sound)));
                cue = cues[sound];
            }
            cue.Play();
        }

        public void StopSound(String sound)
        {
            cues[sound].Stop(AudioStopOptions.Immediate);
        }
    }
}
