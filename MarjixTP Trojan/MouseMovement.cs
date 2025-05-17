using System.Threading;
using System.Windows.Forms;
using static marjtp.DLLImports.MainClass;
using static marjtp.Variables.Variables;

namespace marjtp.Mouse
{
    public class MouseMovement
    {
        public static bool mouseEnabled = true;
        public static void EditMouse()
        {
            if (currentMode == updateMode & mouseEnabled)
            {
                int mainMouseX = Cursor.Position.X, mainMouseY = Cursor.Position.Y;
                int cursx14 = 0, cursy14 = 0;
                switch (currentMode)
                {
                    case 0:
                        SetCursorPos(r.Next(x), r.Next(y));
                        break;
                    case 1:
                        SetCursorPos(mainMouseX + r.Next(-25, 25), mainMouseY + r.Next(-25, 25));
                        break;
                    case 2:
                        SetCursorPos(mainMouseX, mainMouseY + 10);
                        break;
                    case 3:
                        SetCursorPos(0, 0);
                        break;
                    case 4:
                        SetCursorPos(case4Right, case4Down);
                        break;
                    case 5:
                        SetCursorPos(x / 2 + r.Next(-50, 50), y / 2 + r.Next(-50, 50));
                        break;
                    case 6:
                        SetCursorPos(r.Next(x), r.Next(y));
                        break;
                    case 7:
                        SetCursorPos(x / 2, mainMouseY);
                        break;
                    case 8:
                        SetCursorPos(-mainMouseX + x, -mainMouseY + y);
                        break;
                    case 9:
                        SetCursorPos(mainMouseY, mainMouseX);
                        break;
                    case 10:
                        SetCursorPos(0, 0);
                        Thread.Sleep(2);
                        SetCursorPos(x, 0);
                        Thread.Sleep(2);
                        SetCursorPos(x, y);
                        Thread.Sleep(2);
                        SetCursorPos(0, y);
                        break;
                    case 12:
                        SetCursorPos(x, y);
                        break;
                    case 13:
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
                        break;
                }
            }
        }
    }
}