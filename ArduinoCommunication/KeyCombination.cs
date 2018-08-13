﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoCommunication
{
    public class KeyCombination
    {
        public int Delay { get; set; }
        public byte[] Modifier { get; set; } = new byte[0];
        public byte Key { get; set; }
        public Arduino Arduino { get; set; }

        public Task Play()
        {
            return Task.Run(async () => await PressKeys(Arduino, Modifier, true))
                .ContinueWith(async (t) => { await Task.Delay(Delay); }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (t) => { await PressKeys(Arduino, new byte[] { Key }, true); }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (t) => { await Task.Delay(Delay); }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (t) => { await PressKeys(Arduino, new byte[] { Key }, false); }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (t) => { await Task.Delay(Delay); }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (t) => { await PressKeys(Arduino, Modifier, false); }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private static async Task PressKeys(Arduino arduino, byte[] keys, bool pressKeys)
        {
            foreach (var key in keys)
            {
                if (pressKeys)
                    await arduino?.DepressKey(key);
                else
                    await arduino?.ReleaseKey(key);
            }
        }
    }
}