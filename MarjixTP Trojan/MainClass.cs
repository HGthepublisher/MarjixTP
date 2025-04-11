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

namespace MarjixTP_Trojan
{
    public class MainClass
    {
        [DllImport("gdi32.dll")]
        static extern IntPtr CreatePen(PenStyle fnPenStyle, int nWidth, uint crColor);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("gdi32.dll")]
        static extern bool MoveToEx(IntPtr hdc, int X, int Y, IntPtr lpPoint);
        [DllImport("gdi32.dll", SetLastError = true)]
        static extern bool MaskBlt(IntPtr hdcDest, int xDest, int yDest, int width, int height, IntPtr hdcSrc, int xSrc, int ySrc, IntPtr hbmMask, int xMask, int yMask, uint rop);
        [DllImport("gdi32.dll")]
        static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);
        [DllImport("gdi32.dll")]
        static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest,
        IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
        TernaryRasterOperations dwRop);
        [DllImport("gdi32.dll")]
        static extern bool PlgBlt(IntPtr hdcDest, POINT[] lpPoint, IntPtr hdcSrc,
        int nXSrc, int nYSrc, int nWidth, int nHeight, IntPtr hbmMask, int xMask,
        int yMask);
        [DllImport("gdi32.dll")]
        static extern bool PatBlt(IntPtr hdc, int nXLeft, int nYLeft, int nWidth, int nHeight, TernaryRasterOperations dwRop);
        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        static extern IntPtr Ellipse(IntPtr hdc, int nLeftRect, int nTopRect,
        int nRightRect, int nBottomRect);
        [DllImport("gdi32.dll", EntryPoint = "GdiAlphaBlend")]
        public static extern bool AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest,
        int nWidthDest, int nHeightDest,
        IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
        BLENDFUNCTION blendFunction);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateSolidBrush(uint crColor);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateBitmap(int nWidth, int nHeight, uint cPlanes, uint cBitsPerPel, IntPtr lpvBits);
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern bool FloodFill(IntPtr hdc, int nXStart, int nYStart, uint crFill);
        [DllImport("gdi32.dll", EntryPoint = "GdiGradientFill", ExactSpelling = true)]
        public static extern bool GradientFill(
        IntPtr hdc,           // handle to DC
        TRIVERTEX[] pVertex,    // array of vertices
        uint dwNumVertex,     // number of vertices
        GRADIENT_RECT[] pMesh, // array of gradient triangles, that each one keeps three indices in pVertex array, to determine its bounds
        uint dwNumMesh,       // number of gradient triangles to draw
        GRADIENT_FILL dwMode);           // Use only GRADIENT_FILL.TRIANGLE. Both values GRADIENT_FILL.RECT_H and GRADIENT_FILL.RECT_V are wrong in this overload!

        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);
        [DllImport("User32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);
        [DllImport("gdi32.dll")]
        static extern bool FillRgn(IntPtr hdc, IntPtr hrgn, IntPtr hbr);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect,
        int nBottomRect);
        [DllImport("gdi32.dll")]
        static extern bool Pie(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect,
        int nBottomRect, int nXRadial1, int nYRadial1, int nXRadial2, int nYRadial2);
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        static extern bool Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
        [DllImport("gdi32.dll")]
        static extern uint SetPixel(IntPtr hdc, int X, int Y, int crColor);
        [DllImport("gdi32.dll")]
        static extern IntPtr GetPixel(IntPtr hdc, int nXPos, int nYPos);
        [DllImport("gdi32.dll")]
        static extern bool AngleArc(IntPtr hdc, int X, int Y, uint dwRadius,
        float eStartAngle, float eSweepAngle);
        [DllImport("gdi32.dll")]
        static extern bool RoundRect(IntPtr hdc, int nLeftRect, int nTopRect,
        int nRightRect, int nBottomRect, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        static extern bool DeleteMetaFile(IntPtr hmf);
        [DllImport("gdi32.dll")]
        static extern bool CancelDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern bool Polygon(IntPtr hdc, POINT[] lpPoints, int nCount);
        [DllImport("gdi32.dll")]

        static extern int SetBitmapBits(IntPtr hbmp, int cBytes, RGBQUAD[] lpBits);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Beep(uint dwFreq, uint dwDuration);

        [DllImport("user32.dll")]
        private static extern bool BlockInput(bool block);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType,
        int cxDesired, int cyDesired, uint fuLoad);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int DestroyIcon(IntPtr hIcon);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibraryEx(IntPtr lpFileName, IntPtr hFile, LoadLibraryFlags dwFlags);

        [DllImport("user32.dll")]
        static extern IntPtr LoadBitmap(IntPtr hInstance, string lpBitmapName);

        [DllImport("user32.dll")]
        static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        static extern bool EndPaint(IntPtr hWnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("gdi32.dll")]
        static extern int SetStretchBltMode(IntPtr hdc, StretchBltMode iStretchMode);

        [DllImport("gdi32.dll")]
        static extern int StretchDIBits(IntPtr hdc, int XDest, int YDest,
        int nDestWidth, int nDestHeight, int XSrc, int YSrc, int nSrcWidth,
        int nSrcHeight, RGBQUAD rgbq, [In] ref BITMAPINFO lpBitsInfo, DIB_Color_Mode dib_mode,
        TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll")]
        public static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [DllImport("Gdi32", EntryPoint = "GetBitmapBits")]
        private extern static long GetBitmapBits([In] IntPtr hbmp, [In] int cbBuffer, RGBQUAD[] lpvBits);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateHatchBrush(int iHatch, uint Color);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreatePatternBrush(IntPtr hbmp);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateDIBitmap(IntPtr hdc, [In] ref BITMAPINFOHEADER
        lpbmih, uint fdwInit, byte[] lpbInit, [In] ref BITMAPINFO lpbmi,
        uint fuUsage);

        [DllImport("gdi32.dll")]
        static extern int SetDIBitsToDevice(IntPtr hdc, int XDest, int YDest, uint
        dwWidth, uint dwHeight, int XSrc, int YSrc, uint uStartScan, uint cScanLines,
        byte[] lpvBits, [In] ref BITMAPINFO lpbmi, uint fuColorUse);

        [DllImport("gdi32.dll")]
        static extern IntPtr SetDIBits(IntPtr hdc, IntPtr hbm, uint start, int line, int lpBits, [In] ref BITMAPINFO lpbmi, DIB_Color_Mode ColorUse);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Blue;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            /// <summary>
            /// A BITMAPINFOHEADER structure that contains information about the dimensions of color format.
            /// </summary>
            public BITMAPINFOHEADER bmiHeader;

            /// <summary>
            /// An array of RGBQUAD. The elements of the array that make up the color table.
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
            public RGBQUAD[] bmiColors;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
            public uint biCompression;

            public void Init()
            {
                biSize = (uint)Marshal.SizeOf(this);
            }
        }

        enum BitmapCompressionMode : uint
        {
            BI_RGB = 0,
            BI_RLE8 = 1,
            BI_RLE4 = 2,
            BI_BITFIELDS = 3,
            BI_JPEG = 4,
            BI_PNG = 5
        }

        enum DIB_Color_Mode : uint
        {
            DIB_RGB_COLORS = 0,
            DIB_PAL_COLORS = 1
        }

        private enum StretchBltMode : int
        {
            STRETCH_ANDSCANS = 1,
            STRETCH_ORSCANS = 2,
            STRETCH_DELETESCANS = 3,
            STRETCH_HALFTONE = 4,
        }

        /// <summary>
        /// The default flag; it does nothing. All it means is "not LR_MONOCHROME".
        /// </summary>
        public const int LR_DEFAULTCOLOR = 0x0000;

        /// <summary>
        /// Loads the image in black and white.
        /// </summary>
        public const int LR_MONOCHROME = 0x0001;

        /// <summary>
        /// Returns the original hImage if it satisfies the criteria for the copy—that is, correct dimensions and color depth—in
        /// which case the LR_COPYDELETEORG flag is ignored. If this flag is not specified, a new object is always created.
        /// </summary>
        public const int LR_COPYRETURNORG = 0x0004;

        /// <summary>
        /// Deletes the original image after creating the copy.
        /// </summary>
        public const int LR_COPYDELETEORG = 0x0008;

        /// <summary>
        /// Specifies the image to load. If the hinst parameter is non-NULL and the fuLoad parameter omits LR_LOADFROMFILE,
        /// lpszName specifies the image resource in the hinst module. If the image resource is to be loaded by name,
        /// the lpszName parameter is a pointer to a null-terminated string that contains the name of the image resource.
        /// If the image resource is to be loaded by ordinal, use the MAKEINTRESOURCE macro to convert the image ordinal
        /// into a form that can be passed to the LoadImage function.
        ///  
        /// If the hinst parameter is NULL and the fuLoad parameter omits the LR_LOADFROMFILE value,
        /// the lpszName specifies the OEM image to load. The OEM image identifiers are defined in Winuser.h and have the following prefixes.
        ///
        /// OBM_ OEM bitmaps
        /// OIC_ OEM icons
        /// OCR_ OEM cursors
        ///
        /// To pass these constants to the LoadImage function, use the MAKEINTRESOURCE macro. For example, to load the OCR_NORMAL cursor,
        /// pass MAKEINTRESOURCE(OCR_NORMAL) as the lpszName parameter and NULL as the hinst parameter.
        ///
        /// If the fuLoad parameter includes the LR_LOADFROMFILE value, lpszName is the name of the file that contains the image.
        /// </summary>
        public const int LR_LOADFROMFILE = 0x0010;

        /// <summary>
        /// Retrieves the color value of the first pixel in the image and replaces the corresponding entry in the color table
        /// with the default window color (COLOR_WINDOW). All pixels in the image that use that entry become the default window color.
        /// This value applies only to images that have corresponding color tables.
        /// Do not use this option if you are loading a bitmap with a color depth greater than 8bpp.
        ///
        /// If fuLoad includes both the LR_LOADTRANSPARENT and LR_LOADMAP3DCOLORS values, LRLOADTRANSPARENT takes precedence.
        /// However, the color table entry is replaced with COLOR_3DFACE rather than COLOR_WINDOW.
        /// </summary>
        public const int LR_LOADTRANSPARENT = 0x0020;

        /// <summary>
        /// Uses the width or height specified by the system metric values for cursors or icons,
        /// if the cxDesired or cyDesired values are set to zero. If this flag is not specified and cxDesired and cyDesired are set to zero,
        /// the function uses the actual resource size. If the resource contains multiple images, the function uses the size of the first image.
        /// </summary>
        public const int LR_DEFAULTSIZE = 0x0040;

        /// <summary>
        /// Uses true VGA colors.
        /// </summary>
        public const int LR_VGACOLOR = 0x0080;

        /// <summary>
        /// Searches the color table for the image and replaces the following shades of gray with the corresponding 3-D color: Color Replaced with
        /// Dk Gray, RGB(128,128,128) COLOR_3DSHADOW
        /// Gray, RGB(192,192,192) COLOR_3DFACE
        /// Lt Gray, RGB(223,223,223) COLOR_3DLIGHT
        /// Do not use this option if you are loading a bitmap with a color depth greater than 8bpp.
        /// </summary>
        public const int LR_LOADMAP3DCOLORS = 0x1000;

        /// <summary>
        /// When the uType parameter specifies IMAGE_BITMAP, causes the function to return a DIB section bitmap rather than a compatible bitmap.
        /// This flag is useful for loading a bitmap without mapping it to the colors of the display device.
        /// </summary>
        public const int LR_CREATEDIBSECTION = 0x2000;

        /// <summary>
        /// Tries to reload an icon or cursor resource from the original resource file rather than simply copying the current image.
        /// This is useful for creating a different-sized copy when the resource file contains multiple sizes of the resource.
        /// Without this flag, CopyImage stretches the original image to the new size. If this flag is set, CopyImage uses the size
        /// in the resource file closest to the desired size. This will succeed only if hImage was loaded by LoadIcon or LoadCursor,
        /// or by LoadImage with the LR_SHARED flag.
        /// </summary>
        public const int LR_COPYFROMRESOURCE = 0x4000;

        /// <summary>
        /// Shares the image handle if the image is loaded multiple times. If LR_SHARED is not set, a second call to LoadImage for the
        /// same resource will load the image again and return a different handle.
        /// When you use this flag, the system will destroy the resource when it is no longer needed.
        ///
        /// Do not use LR_SHARED for images that have non-standard sizes, that may change after loading, or that are loaded from a file.
        ///
        /// When loading a system icon or cursor, you must use LR_SHARED or the function will fail to load the resource.
        ///
        /// Windows 95/98/Me: The function finds the first image with the requested resource name in the cache, regardless of the size requested.
        /// </summary>
        public const int LR_SHARED = 0x8000;

        [StructLayout(LayoutKind.Sequential)]
        struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public byte[] rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }

            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }

        private enum LoadLibraryFlags : uint
        {
            DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
            LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
            LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
            LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
            LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
            LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
        }

        private enum PenStyle : int
        {
            PS_SOLID = 0, //The pen is solid.
            PS_DASH = 1, //The pen is dashed.
            PS_DOT = 2, //The pen is dotted.
            PS_DASHDOT = 3, //The pen has alternating dashes and dots.
            PS_DASHDOTDOT = 4, //The pen has alternating dashes and double dots.
            PS_NULL = 5, //The pen is invisible.
            PS_INSIDEFRAME = 6,// Normally when the edge is drawn, it’s centred on the outer edge meaning that half the width of the pen is drawn
                               // outside the shape’s edge, half is inside the shape’s edge. When PS_INSIDEFRAME is specified the edge is drawn
                               //completely inside the outer edge of the shape.
            PS_USERSTYLE = 7,
            PS_ALTERNATE = 8,
            PS_STYLE_MASK = 0x0000000F,

            PS_ENDCAP_ROUND = 0x00000000,
            PS_ENDCAP_SQUARE = 0x00000100,
            PS_ENDCAP_FLAT = 0x00000200,
            PS_ENDCAP_MASK = 0x00000F00,

            PS_JOIN_ROUND = 0x00000000,
            PS_JOIN_BEVEL = 0x00001000,
            PS_JOIN_MITER = 0x00002000,
            PS_JOIN_MASK = 0x0000F000,

            PS_COSMETIC = 0x00000000,
            PS_GEOMETRIC = 0x00010000,
            PS_TYPE_MASK = 0x000F0000
        };
        public enum TernaryRasterOperations : uint
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020,
            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086,
            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6,
            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046,
            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328,
            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008,
            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6,
            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA,
            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226,
            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021,
            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09,
            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049,
            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009,
            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042,
            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062,
            /// <summary>
            /// Capture window as seen on screen.  This includes layered windows
            /// such as WPF windows with AllowsTransparency="true"
            /// </summary>
            CAPTUREBLT = 0x40000000,
            CUSTOM = 0x00100C85
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            byte BlendOp;
            byte BlendFlags;
            byte SourceConstantAlpha;
            byte AlphaFormat;

            public BLENDFUNCTION(byte op, byte flags, byte alpha, byte format)
            {
                BlendOp = op;
                BlendFlags = flags;
                SourceConstantAlpha = alpha;
                AlphaFormat = format;
            }
        }

        //
        // currently defined blend operation
        //
        const int AC_SRC_OVER = 0x00;

        //
        // currently defined alpha format
        //
        const int AC_SRC_ALPHA = 0x01;

        [StructLayout(LayoutKind.Sequential)]
        public struct GRADIENT_RECT
        {
            public uint UpperLeft;
            public uint LowerRight;

            public GRADIENT_RECT(uint upLeft, uint lowRight)
            {
                this.UpperLeft = upLeft;
                this.LowerRight = lowRight;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TRIVERTEX
        {
            public int x;
            public int y;
            public ushort Red;
            public ushort Green;
            public ushort Blue;
            public ushort Alpha;

            public TRIVERTEX(int x, int y, ushort red, ushort green, ushort blue, ushort alpha)
            {
                this.x = x;
                this.y = y;
                this.Red = red;
                this.Green = green;
                this.Blue = blue;
                this.Alpha = alpha;
            }
        }
        public enum GRADIENT_FILL : uint
        {
            RECT_H = 0,
            RECT_V = 1,
            TRIANGLE = 2,
            OP_FLAG = 0xff
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct GRADIENT_TRIANGLE
        {
            public uint Vertex1;
            public uint Vertex2;
            public uint Vertex3;

            public GRADIENT_TRIANGLE(uint vertex1, uint vertex2, uint vertex3)
            {
                this.Vertex1 = vertex1;
                this.Vertex2 = vertex2;
                this.Vertex3 = vertex3;
            }
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        public static SoundPlayer soundP;
        public static void ByteBeat(int currentBeat, int freq, bool stop)
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

        public static void MTPEndPart()
        {
            MessageBox.Show("Alright, I'm done now. I must go, use your computer wisely. Peace out!!", "Thanks for using again!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static int currentMode = 0;
        public static bool ending = false;
        public static int time = 25;
        public static void ModeTypes()
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
                        Thread.Sleep(25);
                        break;
                    case 1:
                        SelectObject(hdc, Brush);
                        StretchBlt(hdc, 0, 0, x, y, hdc, 0, 0, x - 5, y + 5, TernaryRasterOperations.SRCCOPY);
                        DeleteObject(Brush);
                        DeleteDC(hdc);
                        Thread.Sleep(5);
                        break;
                    case 2:
                        SelectObject(hdc, Brush);
                        BitBlt(hdc, 0, 0, x, y, hdc, 0, -10, TernaryRasterOperations.SRCCOPY);
                        DeleteObject(Brush);
                        DeleteDC(hdc);
                        Thread.Sleep(1);
                        break;
                    case 3:
                        SelectObject(hdc, Brush);
                        BitBlt(hdc, 0, 0, x, y, hdc, 0, -10, TernaryRasterOperations.SRCCOPY);
                        DeleteObject(Brush);
                        DeleteDC(hdc);
                        Thread.Sleep(1);
                        break;
                    case 4:
                        SelectObject(hdc, Brush);
                        BitBlt(hdc, 0, 0, x, y, hdc, 0, -10, TernaryRasterOperations.SRCCOPY);
                        DeleteObject(Brush);
                        DeleteDC(hdc);
                        Thread.Sleep(1);
                        break;
                    case 5:
                        SelectObject(hdc, Brush);
                        BitBlt(hdc, 0, 0, x, y, hdc, 0, -10, TernaryRasterOperations.SRCCOPY);
                        DeleteObject(Brush);
                        DeleteDC(hdc);
                        Thread.Sleep(1);
                        break;
                }
            }
            ByteBeat(0, 0, true);

            Thread.Sleep(800);

            MTPEndPart();
        }

        public static void Main()
        {
            WindowsPrincipal winPrin = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdmin = winPrin.IsInRole(WindowsBuiltInRole.Administrator);
            if (isAdmin)
            {
                MessageBox.Show("Why'd you run this as admin? I don't feel like destroying your computer. Instead, run this normally. (Without admin!)", "Hold on.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                MessageBox.Show("If you got this virus on any other platform but the official Github site from HGThePublisher, this might be a real destructive virus!!! More info is on the Github repository.", "HEADS UP!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult message1 = MessageBox.Show("Hi, I've seen that you're currently running something I, HGThePublisher, has created. If you want to continue, click yes. This isn't a real virus and won't do anything but play funky sounds and make cool visuals.", "Until you run...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (message1 == DialogResult.Yes)
                {
                    MessageBox.Show("Alright, well, lets get started!! (Click OK to continue.) Thanks for using!!", "Let's go!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    MTPMain();
                }
                else
                {
                    MessageBox.Show("I'm quitting then, you do you. Come back anytime if you want!", "Bye bye!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}