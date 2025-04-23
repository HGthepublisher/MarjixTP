using System;
using System.IO;
using System.Media;
using static marjtp.Variables.Variables;

namespace marjtp.BB
{
    public class Player
    {
        public static SoundPlayer soundP;

        public static void ByteBeat(bool stop)
        {
            if (stop)
            {
                if (soundP != null)
                {
                    soundP.Stop();
                }
            }
            else
            {
                using (var stream = new MemoryStream())
                {
                    var writer = new BinaryWriter(stream);

                    writer.Write("RIFF".ToCharArray());  // chunk id
                    writer.Write((UInt32)0);             // chunk size
                    writer.Write("WAVE".ToCharArray());  // format

                    writer.Write("fmt ".ToCharArray());  // chunk id
                    writer.Write((UInt32)16);            // chunk size
                    writer.Write((UInt16)1);             // audio format

                    SetT(0);

                    var channels = 1;
                    var sample_rate = byteBeats[currentMode].freq;
                    var bits_per_sample = 8;

                    writer.Write((UInt16)channels);
                    writer.Write((UInt32)sample_rate);
                    writer.Write((UInt32)(sample_rate * channels * bits_per_sample / 8)); // byte rate
                    writer.Write((UInt16)(channels * bits_per_sample / 8));               // block align
                    writer.Write((UInt16)bits_per_sample);

                    writer.Write("data".ToCharArray());

                    var seconds = time;
                    var data = new byte[sample_rate * seconds];

                    for (var t = 0; t < data.Length; t++)
                    {
                        SetT(t);
                        data[t] = byteBeats[currentMode].customByte;
                    }

                    writer.Write((UInt32)(data.Length * channels * bits_per_sample / 8));

                    foreach (var elt in data) writer.Write(elt);

                    writer.Seek(4, SeekOrigin.Begin);                     // seek to header chunk size field
                    writer.Write((UInt32)(writer.BaseStream.Length - 8)); // chunk size

                    stream.Seek(0, SeekOrigin.Begin);

                    soundP = new SoundPlayer(stream);
                    soundP.Play();
                }
            }
        }
    }
}