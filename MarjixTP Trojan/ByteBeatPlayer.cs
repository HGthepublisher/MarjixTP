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

                    writer.Write("RIFF".ToCharArray());
                    writer.Write((UInt32)0);
                    writer.Write("WAVE".ToCharArray());

                    writer.Write("fmt ".ToCharArray());
                    writer.Write((UInt32)16);
                    writer.Write((UInt16)1);

                    SetT(0);

                    var channels = 1;
                    var lowSampleRate = byteBeats[currentMode].freq;
                    var highSampleRate = 48000;
                    var sample_rate = highSampleRate;
                    var bits_per_sample = 8;

                    writer.Write((UInt16)channels);
                    writer.Write((UInt32)sample_rate);
                    writer.Write((UInt32)(sample_rate * channels * bits_per_sample / 8));
                    writer.Write((UInt16)(channels * bits_per_sample / 8));
                    writer.Write((UInt16)bits_per_sample);

                    writer.Write("data".ToCharArray());
                    var seconds = time;
                    var data = new byte[highSampleRate * seconds];

                    for (int i = 0; i < data.Length; i++)
                    {
                        double lowT = i * (lowSampleRate / (double)highSampleRate);
                        int tInt = (int)lowT;
                        SetT(tInt);
                        data[i] = byteBeats[currentMode].customByte;
                    }

                    writer.Write((UInt32)(data.Length * channels * bits_per_sample / 8));

                    foreach (var elt in data) writer.Write(elt);

                    writer.Seek(4, SeekOrigin.Begin);
                    writer.Write((UInt32)(writer.BaseStream.Length - 8));

                    stream.Seek(0, SeekOrigin.Begin);

                    soundP = new SoundPlayer(stream);
                    soundP.Play();
                }
            }
        }
    }
}