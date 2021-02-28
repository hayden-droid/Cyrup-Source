using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using Cyrup.Properties;
using DiscordRPC;
using System.Diagnostics;
using KrnlAPI;
using WeAreDevs_API;
using EasyExploits;

namespace Cyrup
{
    class Program
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        private static IntPtr MyConsole = GetConsoleWindow();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int x,
        int y,
        int cx,
        int cy,
        int uFlags);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        private const int HWND_TOPMOST = -1;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Getting console window...");
            var handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            Console.WriteLine("Disabling maximize and resizing...");
            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }

            int xpos = 300;
            int ypos = 150;
            Console.WriteLine("Setting window position and size...");
            Console.SetWindowSize(80, 20);
            SetWindowPos(MyConsole, 0, xpos, ypos, 0, 0, SWP_NOSIZE);
            if (Directory.Exists(Environment.CurrentDirectory + "\\autoexec") == false)
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\autoexec");
            }
            Console.WriteLine("Checking version...");
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                System.Net.WebClient wc = new System.Net.WebClient();
                var version = Convert.ToString(wc.DownloadString("https://pastebin.com/raw/rPkz0Nc8"));
                string appver = "4.1.3";
                if (version != appver)
                {
                    System.Windows.Forms.MessageBox.Show("This version of Cyrup is outdated. Please run the bootstrapper again.", "Notice");
                    Environment.Exit(0);
                }
            }
            catch
            {
                Console.WriteLine("Operation failed. Could not check version.");
            }
            if (File.Exists(Environment.CurrentDirectory + "\\bin\\changelog.log") == false)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                string changelog = @"~~Changelog~~

Added changelog
Version check no longer required (you won't be prompted though)
Bootstrapper no longer disables firewall
Injection now displays debug console
Other bug fixes";
                Console.WriteLine(changelog);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                using (StreamWriter sw = File.CreateText(Environment.CurrentDirectory + "\\bin\\changelog.log"))
                {
                    sw.WriteLine();
                }
                Console.Clear();
            }
            Console.WriteLine("Drawing GUI...");
            if (args.Length > 0)
            {
                if (Convert.ToString(args[0]) == "msdos")
                {
                    Console.WriteLine("Looks like you're using the special colour scheme ;)");
                    Thread.Sleep(1000);
                    Console.Clear();
                    Draw("msdos");
                }
            }
            Console.Clear();
            Draw("nah");
        }

        [STAThread]
        static void Draw(string color)
        {
            DiscordRpcClient client = new DiscordRpcClient("797124617268625408");
            client.SetPresence(new RichPresence()
            {
                Details = "Level 7 Roblox Lua Executor | TRIPLE API",
                State = "Using Cyrup v4",
                Assets = new Assets()
                {
                    LargeImageKey = "untitled_7_",
                }
            });

            if (Settings.Default.RPC == true)
            {
                client.Initialize();
            }
            Console.SetWindowSize(80, 20);
            Console.Title = "Coco Z";
            EasyExploits.Module easy = new EasyExploits.Module();
            ExploitAPI wrd = new ExploitAPI();
            Terminal.Gui.Application.Init();
            var top = Terminal.Gui.Application.Top;
            var win = new FrameView(new Rect(0, 0, top.Frame.Width - 68, top.Frame.Height), "Cyrup");
            var win2 = new FrameView(new Rect(12, 0, top.Frame.Width - 12, top.Frame.Height), "");
            if (color == "msdos")
            {
                Colors.Base.Normal = Terminal.Gui.Application.Driver.MakeAttribute(Color.BrightYellow, Color.Blue);
                Colors.Menu.Normal = Terminal.Gui.Application.Driver.MakeAttribute(Color.White, Color.Blue);
                Colors.Dialog.Normal = Terminal.Gui.Application.Driver.MakeAttribute(Color.BrightYellow, Color.BrightBlue);
            }
            else
            {
                Colors.Base.Normal = Terminal.Gui.Application.Driver.MakeAttribute(Color.BrightMagenta, Color.Black);
                Colors.Menu.Normal = Terminal.Gui.Application.Driver.MakeAttribute(Color.Cyan, Color.Black);
                Colors.Dialog.Normal = Terminal.Gui.Application.Driver.MakeAttribute(Color.Magenta, Color.Black);
            }
            win.ColorScheme = Colors.Base;
            win2.ColorScheme = Colors.Base;
            top.Add(win);
            top.Add(win2);

            var exec = new Terminal.Gui.Button(1, 3, "Exec");
            var clr = new Terminal.Gui.Button(1, 5, "Clr ");
            var open = new Terminal.Gui.Button(1, 7, "Open");
            var save = new Terminal.Gui.Button(1, 9, "Save");
            var inj = new Terminal.Gui.Button(1, 1, "Inj ");
            var opt = new Terminal.Gui.Button(1, 16, "Opt ");

            var editor = new TextView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ColorScheme = Colors.Menu
            };

            string credits = @"--[[
Stop cumming in me stepdaddy oh your hot warm milk~
--]]";
            editor.Text = credits.Replace("\r\n", "\n");

            win2.KeyDown += (k) =>
            {
                if (k.KeyEvent.Key == Key.CtrlMask)
                {
                    string paste = System.Windows.Forms.Clipboard.GetText();
                    editor.Text = paste.Replace("\r\n", "\n");
                    k.Handled = true;
                }
            };

            inj.Clicked += () =>
            {
                if (Process.GetProcessesByName("RobloxPlayerBeta").Length < 1)
                {
                    Terminal.Gui.MessageBox.ErrorQuery(50, 5, "Error", "Please open Roblox before injecting!", "Okay");
                    return;
                }
                if (String.IsNullOrEmpty(Settings.Default.APIName))
                {
                    Terminal.Gui.MessageBox.ErrorQuery(50, 5, "Error", "Please select a DLL first!", "Okay");
                    return;
                }
                if (Settings.Default.APIName == "easy")
                {
                    if (easy.IsAttached())
                    {
                        Terminal.Gui.MessageBox.ErrorQuery(50, 5, "Error", "EasyExploits API is already injected!", "Okay");
                        return;
                    }
                    else
                    {
                        Process.Start(Environment.CurrentDirectory + "\\bin\\console.exe");
                        easy.LaunchExploit();
                        if (IsDirectoryEmpty(Environment.CurrentDirectory + "/autoexec") == false)
                        {
                            AutoExecEasy();
                        }
                        return;
                    }
                }
                if (Settings.Default.APIName == "wrd")
                {
                    if (wrd.isAPIAttached())
                    {
                        Terminal.Gui.MessageBox.ErrorQuery(50, 5, "Error", "WeAreDevs API is already injected!", "Okay");
                        return;
                    }
                    else
                    {
                        Process.Start(Environment.CurrentDirectory + "\\bin\\console.exe");
                        wrd.LaunchExploit();
                        if (IsDirectoryEmpty(Environment.CurrentDirectory + "/autoexec") == false)
                        {
                            AutoExecWRD();
                        }
                        return;
                    }
                }
                if (Settings.Default.APIName == "krnl")
                {
                    if (MainAPI.IsAttached())
                    {
                        Terminal.Gui.MessageBox.ErrorQuery(50, 5, "Error", "KRNL DLL is already injected!", "Okay");
                        return;
                    }
                    else
                    {
                        Process.Start(Environment.CurrentDirectory + "\\bin\\console.exe");
                        MainAPI.Inject();
                        if (IsDirectoryEmpty(Environment.CurrentDirectory + "/autoexec") == false)
                        {
                            AutoExecKRNL();
                        }
                        return;
                    }
                }
            };

            exec.Clicked += () =>
            {
                if (String.IsNullOrEmpty(Settings.Default.APIName))
                {
                    var error = Terminal.Gui.MessageBox.ErrorQuery(50, 5, "Error", "Please select a DLL first!", "Okay");
                    return;
                }
                if (Settings.Default.APIName == "easy")
                {
                    if (easy.IsAttached())
                    {
                        easy.ExecuteScript(Convert.ToString(editor.Text));
                        return;
                    }
                    else
                    {
                        Terminal.Gui.MessageBox.ErrorQuery(50, 5, "Error", "Exploit is not injected!", "Okay");
                        return;
                    }
                }
                if (Settings.Default.APIName == "wrd")
                {
                    if (wrd.isAPIAttached())
                    {
                        wrd.SendLuaScript(Convert.ToString(editor.Text));
                        return;
                    }
                    else
                    {
                        Terminal.Gui.MessageBox.ErrorQuery(50, 5, "Error", "Exploit is not injected!", "Okay");
                        return;
                    }
                }
                if (Settings.Default.APIName == "krnl")
                {
                    if (MainAPI.IsAttached())
                    {
                        MainAPI.Execute(Convert.ToString(editor.Text));
                        return;
                    }
                    else
                    {

                        Terminal.Gui.MessageBox.ErrorQuery(50, 5, "Error", "Exploit is not injected!", "Okay");
                        return;
                    }
                }
            };

            clr.Clicked += () =>
            {
                editor.Text = String.Empty;
            };

            save.Clicked += () =>
            {
                var saveChoice = Terminal.Gui.MessageBox.Query(50, 5, "Save Options", "Obfuscate script? (Lua only)", "Yes", "No");
                if (saveChoice == 0)
                {

                }
                if (saveChoice == 1)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog
                    {
                        Title = "Save File",

                        CheckPathExists = true,

                        DefaultExt = "txt",
                        Filter = "Text files (*.txt)|*.txt",
                        FilterIndex = 2,
                        RestoreDirectory = true
                    };
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fi = new FileInfo(saveFileDialog1.FileName);
                        using (StreamWriter sw = fi.CreateText())
                        {
                            sw.WriteLine(editor.Text.ToString());
                        }
                    }
                    return;
                }

            };

            open.Clicked += () =>
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    InitialDirectory = Environment.CurrentDirectory + "\\scripts",
                    Title = "Browse Text Files",

                    CheckFileExists = true,
                    CheckPathExists = true,

                    DefaultExt = "txt",
                    Filter = "Text files (*.txt)|*.txt",
                    FilterIndex = 2,
                    RestoreDirectory = true,

                    ReadOnlyChecked = true
                };

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string text = File.ReadAllText(openFileDialog1.FileName);
                    editor.Text = text.Replace("\r\n", "\n");
                }
            };

            opt.Clicked += () =>
            {
                var optMenu = Terminal.Gui.MessageBox.Query(50, 5, "Options", "Options Menu", "Select DLL", "Discord RPC", "Kill Roblox");

                if (optMenu == 2)
                {
                    var areyousure = Terminal.Gui.MessageBox.Query(50, 5, "Stop", "Are you sure?", "Yes kill Roblox", "No");
                    if (areyousure == 0)
                    {
                        foreach (var process in Process.GetProcessesByName("RobloxPlayerBeta"))
                        {
                            process.Kill();
                        }
                    }

                    if (areyousure == 1) return;
                }

                if (optMenu == 1)
                {
                    var _0 = Terminal.Gui.MessageBox.Query(50, 5, "Options Menu", "Discord RPC Settings", "Enable RPC", "Disable RPC");
                    if (_0 == 0)
                    {
                        Settings.Default.RPC = true;
                        Settings.Default.Save();
                        if (client.IsInitialized == false)
                        {
                            client.Initialize();
                            client.SetPresence(new RichPresence()
                            {
                                Details = "Level 7 Roblox Lua Executor | TRIPLE API",
                                State = "Using Cyrup v4",
                                Assets = new Assets()
                                {
                                    LargeImageKey = "untitled_7_",
                                }
                            });
                        }
                        Terminal.Gui.MessageBox.Query(50, 5, "Notice", "RPC has been enabled", "Okay");
                        return;
                    }

                    if (_0 == 1)
                    {
                        Settings.Default.RPC = false;
                        Settings.Default.Save();
                        client.Deinitialize();
                        Terminal.Gui.MessageBox.Query(50, 5, "Notice", "RPC has been disabled", "Okay");
                        return;
                    }
                }

                if (optMenu == 0)
                {
                    var dsec = Terminal.Gui.MessageBox.Query(50, 5, "Select DLL", "Select an API", "EasyExploits", "WeAreDevs", "KRNL DLL");
                    if (dsec == 0)
                    {
                        Settings.Default.APIName = "easy";
                        Settings.Default.Save();
                        Terminal.Gui.MessageBox.Query(50, 5, "Notice", "EasyExploits API has been selected", "Okay");
                        return;
                    }
                    if (dsec == 1)
                    {
                        Settings.Default.APIName = "wrd";
                        Settings.Default.Save();
                        Terminal.Gui.MessageBox.Query(50, 5, "Notice", "WeAreDevs API has been selected", "Okay");
                        return;
                    }
                    if (dsec == 2)
                    {
                        Settings.Default.APIName = "krnl";
                        Settings.Default.Save();
                        Terminal.Gui.MessageBox.Query(50, 5, "Notice", "KRNL DLL has been selected", "Okay");
                        return;
                    }
                    return;
                }
            };


            win.Add(
               inj,
               exec,
               clr,
               open,
               save,
               opt
               );

            win2.Add(
                editor
                );

            bool IsDirectoryEmpty(string path)
            {
                return !Directory.EnumerateFileSystemEntries(path).Any();
            }

            void AutoExecEasy()
            {
                if (easy.IsAttached())
                {
                    foreach (string file in Directory.EnumerateFiles(Environment.CurrentDirectory + "/autoexec", "*.*"))
                    {
                        string contents = File.ReadAllText(file);
                        easy.ExecuteScript(contents);
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    Thread.Sleep(500);
                    AutoExecEasy();
                }
            }

            void AutoExecWRD()
            {
                if (wrd.isAPIAttached())
                {
                    foreach (string file in Directory.EnumerateFiles(Environment.CurrentDirectory + "/autoexec", "*.*"))
                    {
                        string contents = File.ReadAllText(file);
                        wrd.SendLuaScript(contents);
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    Thread.Sleep(500);
                    AutoExecWRD();
                }
            }

            void AutoExecKRNL()
            {
                if (MainAPI.IsAttached())
                {
                    foreach (string file in Directory.EnumerateFiles(Environment.CurrentDirectory + "/autoexec", "*.*"))
                    {
                        string contents = File.ReadAllText(file);
                        MainAPI.Execute(contents);
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    Thread.Sleep(500);
                    AutoExecKRNL();
                }
            }

            Terminal.Gui.Application.Run();
        }
    }
}