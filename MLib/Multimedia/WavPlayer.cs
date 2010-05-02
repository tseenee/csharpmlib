using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace MLib.Multimedia
{
    public static class WavPlayer
    {
        [DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true)]
        private static extern bool PlaySound(string szSound, System.IntPtr hMod, int flags);

        //  flag values for SoundFlags argument on PlaySound
        static int SND_SYNC = 0x0000;      // play synchronously
        // (default)
        static int SND_ASYNC = 0x0001;      // play asynchronously
        static int SND_PURGE = 0x0040;     // purge non-static
        
        // a memory file
        static int SND_LOOP = 0x0008;      // loop the sound until

        /*// if sound not found
        static int SND_MEMORY = 0x0004;      // pszSound points to
        // next sndPlaySound
        static int SND_NOSTOP = 0x0010;      // don't stop any
        // currently playing
        // sound
        static int SND_NODEFAULT = 0x0002;      // silence (!default)
        static int SND_NOWAIT = 0x00002000; // don't wait if the
        // driver is busy
        static int SND_ALIAS = 0x00010000; // name is a Registry
        // alias
        static int SND_ALIAS_ID = 0x00110000; // alias is a predefined
        // ID
        static int SND_FILENAME = 0x00020000; // name is file name
        static int SND_RESOURCE = 0x00040004; // name is resource name
        // or atom
        
        // events for task
        static int SND_APPLICATION = 0x0080;     // look for application-
        // specific association*/

        public static void Play(string Path, bool Loop, bool Asynchron)
        {
            byte[] bname = new Byte[256];    //Max path length
            bname = System.Text.Encoding.ASCII.GetBytes(Path);

            int flags = 0;

            if (Loop)
                flags |= SND_LOOP;
            if (Asynchron)
                flags |= SND_ASYNC;

            PlaySound(Path, new IntPtr(), flags);
        }
        public static void Stop()
        {
            PlaySound(null, new IntPtr(), SND_PURGE);
        }

    }
}
