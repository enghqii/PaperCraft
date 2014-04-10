using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace PaperCraft
{
    class SoundManager
    {
        private static Dictionary<string, SoundEffect> soundPool = new Dictionary<string,SoundEffect>();
        private static List<SoundEffectInstance> disposeList = new List<SoundEffectInstance>();

        public static void addSound(string index,SoundEffect inst) {
            soundPool.Add(index, inst);
        }

        public static SoundEffect getSound(string index)
        {
            if (soundPool.ContainsKey(index)) {
                return soundPool[index];
            }
            return null;
        }

        public static void playSound(string index)
        {
            //soundPool[index].Stop(true);
            SoundEffectInstance inst = soundPool[index].CreateInstance();
            inst.Play();
            //inst.Dispose();
        }
    }
}
