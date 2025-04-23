using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace marjtp.Variables
{
    public class ByteBeat
    {
        public byte customByte = (byte)(0);
        public int freq = 8000;
    }

    public class Variables
    {
        public static int currentMode = 0;
        public static bool ending = false;
        public static int time = 24;
        public static int modeDelay = 1;

        private static int t = 0;

        public static ByteBeat[] byteBeats = new ByteBeat[] { };

        public static void SetT(int i)
        {
            t = i;

            byteBeats = new ByteBeat[]
            {
                new ByteBeat { freq = 8000, customByte = (byte)((t & t * 5 | t >> 6 | ((t & 0xFFFF) > 0x8000 ? (-6 * t) / 7 : ((t & 0x1FFFF) > 0x10000 ? -9 * t & 100 : (-9 * (t & 100))) / 11)) * 10) },
                new ByteBeat { freq = 8000, customByte = (byte)(t * (((t & 4096) != 0 ? t % 65536 < 59392 ? 7 : t & 7 : 16) + (1 & t >> 14)) >> (3 & -t >> ((t & 2048) != 0 ? 2 : 10))) },
                new ByteBeat { freq = 8000, customByte = (byte)(t / 8 >> (t >> 9) * t / ((t >> 14 & 3) + 4)) },
                new ByteBeat { freq = 8000, customByte = (byte)(((10 * (t >> 6 | t | t >> (t >> 16 & 3)) + (7 & t >> 11)) % 256 * ((t & 3072) != 0 ? (2048 + t % 4096) % 3072 / 12 : 0) >> 8) + ((t & 3072) != 0 ? 0 : (t % 256 * (1024 - t % 1024) / 3 >> 6 & 128) >> (t >> 8 & 15))) },
                new ByteBeat { freq = 8000, customByte = (byte)(t + (t & t ^ t >> 6) - t * (t >> 9 & ((t % 16) != 0 ? 2 : 6) & t >> 9)) },
                new ByteBeat { freq = 11025, customByte = (byte)(((((t * ((t & 16384) != 0 ? 7 : 5) * (3 - (3 & (t >> 9)) + (3 & (t >> (((-t >> 20) & 1) != 0 ? 8 : 11)))) >> (3 & (-t >> ((t & ((-t & 57344) != 0 ? 4096 : 6144)) != 0 ? 2 : 16)))) | (((-t & 24576) != 0) ? ((3 * t >> 5) % 192) : ((t >> 4) % 192)) | (((t >> 20) & 1) != 0 ? (t >> 4) : (t >> (((-t >> 18) & 1) + 2)))) & 255) >> 1) - ((((t >> 18) & 1) != 0) ? ((((-t >> 1) * ((t & 16384) != 0 ? 7 : 5)) >> ((-t >> 10) & 3) & (t >> 4 & 255)) >> 1) : (((-t >> 2) * ((t & 16384) != 0 ? 7 : 5)) >> ((-t >> 10) & 3) & ((t >> 4 & 255) >> 1))) + (128 & (int)(40000 / (1 + (t & (((-t & 28672) != 0) ? 4095 : 2047))))) + ((((t >> 18) & 3) != 0) ? -(((t * (t ^ (t % 9))) & 255 & -(Convert.ToInt32((t >> ((t >> 11) & 31)) != 0 ? ((-t & 14336) != 0 ? 5 : 4) - Convert.ToInt32((-t & 28672) == 0) - Convert.ToInt32((-t & 122880) == 0) : 6)) << 2 & 255) >> 2) + 128 : 0)) },
                new ByteBeat { freq = 44100, customByte = (byte)(t * (-t >> (((t >> 13) % 8) + 2) & ((t >> 12) % 256)) / 4 + ((1048576 / ((t % 65536) | 1)) & 128)) },
                new ByteBeat { freq = 9500, customByte = (byte)((((t >> 5 | t >> 4 | (t % 42 * (t >> 4) | 357052691 - (t >> 4)) / ((t >> 16) | 1) ^ (t | t >> 4)) & 255) + ((t % 25 - (t >> 2 | ((t & 16384) != 0 ? 64 / 3 : 16) * t | t % 227) - t >> 3 | (t >> 5 & 1663 * (t << 5) | (t >> 3) % 1544) / ((t % 17 | t % 2048) | 1)) & 255) + 100 * ((t << 2 | t >> 5 | t ^ 63) & (t << 10 | t >> 11)) % 256) / 3) },
                new ByteBeat { freq = 8000, customByte = (byte)(t * t * 4 / (((5656 >> (t >> 12 & 14)) & 7) + 9) * (t >> 10 & 893) & t >> 4 ^ (((t >> 16 & 1) != 0 ? 0 : (t >> 10 & -t >> 14 & 3)) | (((t >> 9 & 1) + (t >> 12 & 7)) != 0 ? 0 : ((t % 4096) != 0 && (t / 9 & 8) == 0 ? 9001 / (t % 4096) : -1)))) },
                new ByteBeat { freq = 8000, customByte = (byte)(t * ((t >> 10 | t % 16 * t >> 8) & 8 * t >> 12 & 18)) },
                new ByteBeat { freq = 44100, customByte = (byte)(((t >> ((t >> 13) & 31)) & 128) + (((t & (t >> 12)) * t >> 12)) + (int)(300000.0 / (t % 16384 + 1))) },
                new ByteBeat { freq = 44100, customByte = (byte)((t & t / 2 & t / 4) * t / 4E3) },
                new ByteBeat { freq = 8000, customByte = (byte)(t * (t ^ t + (t >> 15 | 1) ^ (t - 1280 ^ t) >> 10)) },
                new ByteBeat { freq = 8000, customByte = (byte)((t >> 8 | t >> 16) != 0 && (t % (t >> 8 | t >> 16)) != 0 ? t / (t % (t >> 8 | t >> 16)) : 0) },
                new ByteBeat { freq = 8000, customByte = (byte)(((t * ((t & 8192) != 0 ? 7 : 5) * (6 - (3 & t >> 8) + (3 & t >> 9))) >> (3 & -t >> ((t & 2048) != 0 ? 2 : 11)))) },
                new ByteBeat { freq = 32000, customByte = (byte)((((2 * t & 255) * (-t >> 6 & t >> 7)) >> 8) | (t & t >> 1) | (t & t >> 1)) },
            };
        }

        public static int x = Screen.PrimaryScreen.Bounds.Width, y = Screen.PrimaryScreen.Bounds.Height;

        public static bool couldGetMarjImages = false;

        public static async Task<Image> GetImageFromLinkAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                byte[] imgData = await client.GetByteArrayAsync(url);
                using (MemoryStream imgMemoryStream = new MemoryStream(imgData))
                {
                    return Image.FromStream(imgMemoryStream);
                }
            }
        }

        public static int updateMode = currentMode;

        public static Random r = new Random();

        public static int case4Up = 0, case4Down = y, case4Left = 0, case4Right = x;
    }
}