using OWML.Common;
using OWML.ModHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EuropeTimes
{
    public class EuropeTimes : ModBehaviour
    {
        private static EuropeTimes instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            ModHelper.Console.WriteLine(nameof(EuropeTimes) + " has loaded!");

            GetClip(); //load audio

            ModHelper.HarmonyHelper.AddPostfix<AudioManager>(nameof(AudioManager.Awake), typeof(EuropeTimes), nameof(EuropeTimes.ReplaceInAudioManager));
            ModHelper.HarmonyHelper.AddPostfix<AudioLibrary>(nameof(AudioLibrary.BuildAudioEntryDictionary), typeof(EuropeTimes), nameof(EuropeTimes.ReplaceInAudioLibrary));
        }

        public const int EndOfTime = (int)AudioType.EndOfTime;

        public static void ReplaceInAudioManager(AudioManager __instance)
        {
            //The audio dictionary is a dictionary containing all of the sounds, matched to the int value of the AudioType enum
            if (__instance._audioLibraryDict.ContainsKey(EndOfTime))
                __instance._audioLibraryDict[EndOfTime] = new AudioLibrary.AudioEntry(AudioType.EndOfTime, GetClips(), 0.2f);
            else
                __instance._audioLibraryDict.Add(EndOfTime, new AudioLibrary.AudioEntry(AudioType.EndOfTime, GetClips(), 0.2f));
        }

        public static void ReplaceInAudioLibrary(ref Dictionary<int, AudioLibrary.AudioEntry> __result)
        {
            //The audio dictionary is a dictionary containing all of the sounds, matched to the int value of the AudioType enum
            if (__result.ContainsKey(EndOfTime))
                __result[EndOfTime] = new AudioLibrary.AudioEntry(AudioType.EndOfTime, GetClips(), 0.2f);
            else
                __result.Add(EndOfTime, new AudioLibrary.AudioEntry(AudioType.EndOfTime, GetClips(), 0.2f));
        }

        private static AudioClip clip;

        private static AudioClip[] GetClips() => new AudioClip[1] { GetClip() };
        private static AudioClip GetClip()
        {
            if (clip != null) return clip; //If it's already loaded, give the one in memory
            AudioClip audioClip = instance.ModHelper.Assets.GetAudio("Europe_Times.mp3"); //Else load it
            clip = audioClip;
            return audioClip;
        }
    }
}
