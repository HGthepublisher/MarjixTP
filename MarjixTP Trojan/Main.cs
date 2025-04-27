using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using static marjtp.DLLImports.MainClass;
using static marjtp.Variables.Variables;
using static marjtp.BB.Player;
using static marjtp.Mouse.MouseMovement;

namespace marjtp.Main
{
    public class MainClass
    {
        private static void ModeTypes()
        {
            while (!ending)
            {
                if (currentMode <= 15)
                {
                    Thread.Sleep(time * 1000);
                    currentMode += 1;
                    mouseEnabled = false;
                }
                else
                {
                    ending = true;
                    mouseEnabled = false;
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
                        ByteBeat(true);
                        mouseEnabled = false;
                        currentByteBeat = currentMode;
                        if (currentMode != updateMode1)
                        {
                            updateMode1 = currentMode;
                            Thread.Sleep(modeDelay * 1000);
                        }
                        if (currentMode <= 15)
                        {
                            ByteBeat(false);
                            mouseEnabled = true;
                        }
                        else
                        {
                            ByteBeat(true);
                            mouseEnabled = false;
                        }
                    }
                }
            });

            Task.Run(() =>
            {
                while (currentMode <= 15)
                {
                    EditMouse();
                    Thread.Sleep(1);
                }
            });

            List<IntPtr> iconBytes = new List<IntPtr>();
            for (var i = 0; i < 200; i++)
            {
                Icon ic = ExtractIcon("imageres.dll", r.Next(200), true);
                if (ic != null)
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
            var marjImgBan = GetImageFromLinkAsync("https://raw.githubusercontent.com/HGthepublisher/MarjixTP/refs/heads/main/marjtpbanner.png");
            if (!marjImgBan.IsCanceled & !marjImgBan.IsFaulted)
            {
                couldGetMarjImages = true;
            }
            updateMode = currentMode;
            int x8 = x;
            bool rev8 = false;
            int x9 = 0;
            bool rev9 = false;
            IntPtr[] hBitmaps12 =
            {
                iconBytes[r.Next(iconBytes.Count)],
                iconBytes[r.Next(iconBytes.Count)],
                iconBytes[r.Next(iconBytes.Count)],
                iconBytes[r.Next(iconBytes.Count)],
                iconBytes[r.Next(iconBytes.Count)],
                iconBytes[r.Next(iconBytes.Count)],
                iconBytes[r.Next(iconBytes.Count)],
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
            while (!ending)
            {
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
                            Thread.Sleep(3);
                            break;
                        case 1:
                            SelectObject(hdc, Brush);
                            StretchBlt(hdc, 0, 0, x, y, hdc, 0, 0, x - 4, y + 3, TernaryRasterOperations.SRCCOPY);
                            DeleteObject(Brush);
                            DeleteDC(hdc);
                            Thread.Sleep(3);
                            break;
                        case 2:
                            SelectObject(hdc, Brush);
                            BitBlt(hdc, 0, 0, x, y, hdc, 0, -10, TernaryRasterOperations.SRCCOPY);
                            DeleteObject(Brush);
                            DeleteDC(hdc);
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
                            break;
                        case 10:
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
                            PatBlt(hdc, r.Next(-x, x), r.Next(-y, y), x, y, TernaryRasterOperations.PATCOPY);
                            Thread.Sleep(2);
                            SelectObject(hdc, Brush);
                            if (r.Next(2) == 1)
                            {
                                BitBlt(hdc, 0, 0, x, y, hdc, r.Next(-35, 35), r.Next(-20, 20), TernaryRasterOperations.MERGECOPY);
                            }
                            else
                            {
                                BitBlt(hdc, 0, 0, x, y, hdc, 0, 0, TernaryRasterOperations.PATINVERT);
                            }
                            DeleteDC(hdc);
                            DeleteObject(brush12);
                            DeleteObject(Brush);
                            Thread.Sleep(2);
                            break;
                        case 13:
                            SelectObject(hdc, Brush);
                            int r2 = r.Next(x);
                            BitBlt(hdc, r2, r.Next(20), r.Next(100), y, hdc, r2, 0, TernaryRasterOperations.SRCCOPY);
                            DeleteObject(Brush);
                            DeleteDC(hdc);
                            break;
                        case 14:
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
                            if (couldGetMarjImages)
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
            ByteBeat(true);

            Thread.Sleep(800);

            MessageBox.Show("Alright, I'm done now. I must go, use your computer wisely. Peace out!!", "Thanks for using again!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}