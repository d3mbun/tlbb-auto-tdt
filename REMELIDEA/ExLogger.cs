/*
 * Created by SharpDevelop.
 * User: Bogdan
 * Date: 09.12.2010
 * Time: 15:33
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;

namespace _i.REMELIDEA {

  
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    internal class ExLogger {

        internal static void Exec(string path = null, bool isRun = true) {
            string assemblyname = path;
            if (assemblyname != "" && File.Exists(assemblyname)) {
                AppDomain appDomain = null;
                asmpath = Path.GetDirectoryName(assemblyname);

                // save the .code section of mscorjit.dll
                int Length = 0;
                byte[] mscorjitcode = null;
                IntPtr mscorjith = GetModuleHandle("mscorjit.dll");
                if (mscorjith != IntPtr.Zero) {
                    int PEOffset = Marshal.ReadInt32(mscorjith, 0x3C);
                    int SizeOfCode = Marshal.ReadInt32(mscorjith, PEOffset + 0x1C);
                    int BaseOfCode = Marshal.ReadInt32(mscorjith, PEOffset + 0x2C);
                    mscorjith = (IntPtr)((long)mscorjith + BaseOfCode);
                    mscorjitcode = new byte[SizeOfCode];
                    ReadProcessMemory(-1, mscorjith, mscorjitcode, mscorjitcode.Length, ref Length);
                }

                // Create the new AppDomain
                Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
                Evidence childEvidence = new Evidence(baseEvidence);
                appDomain = AppDomain.CreateDomain(Path.GetFileName(assemblyname), AppDomain.CurrentDomain.Evidence);
                //appDomain.e
                if (appDomain != null) {
                    appDomain.UnhandledException +=
                    new UnhandledExceptionEventHandler(Domain_UnhandledException);
                    Application.ThreadException +=
                    new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                }

                if (isRun) {
                    appDomain.ExecuteAssembly(assemblyname);
                }
                else {
                    appDomain.SetData("assembly", assemblyname);

                    CrossAppDomainDelegate del = new CrossAppDomainDelegate(loadAssembly);
                    // do the callback
                    appDomain.DoCallBack(del);
                }

                try {
                    // restore the .code section of mscorjit.dll
                    if (mscorjith != IntPtr.Zero) {
                        WriteProcessMemory(-1, mscorjith, mscorjitcode, mscorjitcode.Length, ref Length);
                    }
                }
                catch {
                }

                if (appDomain != null) {
                    try {
                        appDomain.UnhandledException -=
                        new UnhandledExceptionEventHandler(Domain_UnhandledException);
                        Application.ThreadException -=
                        new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

                        CrossAppDomainDelegate del = new CrossAppDomainDelegate(Shutdown);
                        appDomain.DoCallBack(del);
                        AppDomain.Unload(appDomain);

                        appDomain = null; // why not
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    catch {
                    }
                }
            }
        }

        [Flags]
        enum ThreadAccess : int {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        internal string ExecPath { get; set; } = "hello";
        // use to get memory available
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        class MEMORYSTATUSEX {
            internal uint dwLength;
            internal uint dwMemoryLoad;
            internal ulong ullTotalPhys;
            internal ulong ullAvailPhys;
            internal ulong ullTotalPageFile;
            internal ulong ullAvailPageFile;
            internal ulong ullTotalVirtual;
            internal ulong ullAvailVirtual;
            internal ulong ullAvailExtendedVirtual;

            internal MEMORYSTATUSEX() {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }
        #region Private

        private static Hashtable alreadyLoaded;

        private static string asmpath;

        private static string MissingAssemblies;

        private string DirectoryName = "";

        private delegate void LogExceptionDelegate(Exception e);

        private static string suplexception { get; set; } = "";

        private static void Application_ThreadException
        (object sender, System.Threading.ThreadExceptionEventArgs e) {
            Exception exc = e.Exception;

            LogExceptionDelegate log = new LogExceptionDelegate(LogException);
            AsyncCallback cb = new AsyncCallback(LogCallBack);
            IAsyncResult ar = log.BeginInvoke(exc, cb, null);
        }

        private static void Domain_UnhandledException
        (object sender, UnhandledExceptionEventArgs e) {
            Exception exc = (Exception)e.ExceptionObject;

            LogExceptionDelegate log = new LogExceptionDelegate(LogException);
            AsyncCallback cb = new AsyncCallback(LogCallBack);
            IAsyncResult ar = log.BeginInvoke(exc, cb, null);
        }

        private static void DumpAssembly(Assembly assm) {
            AssemblyName fqn = assm.GetName();
            if (alreadyLoaded.Contains(fqn.FullName)) {
                return;
            }
            alreadyLoaded.Add(fqn.FullName, fqn);

            Assembly referenced = null;
            foreach (AssemblyName name in assm.GetReferencedAssemblies()) {
                referenced = null;
                try {
                    referenced = AppDomain.CurrentDomain.Load(name);
                }
                catch {
                    string path = Path.Combine(asmpath, name.Name + ".dll");
                    if (File.Exists(path)) {
                        try {
                            byte[] asmdata = File.ReadAllBytes(path);
                            referenced = Assembly.ReflectionOnlyLoad(asmdata);
                            asmdata = null;
                        }
                        catch {
                        }
                    }
                }
                if (referenced == null)
                    MissingAssemblies = MissingAssemblies + name.FullName + "\r\n";
                else
                    DumpAssembly(referenced);
            }
        }

        private static string GetExceptionCallStack(Exception e) {
            if (e.InnerException != null) {
                StringBuilder message = new StringBuilder();
                message.AppendLine(GetExceptionCallStack(e.InnerException));
                message.AppendLine("--- Next Call Stack:");
                message.AppendLine(e.StackTrace);
                return (message.ToString());
            }
            else {
                return e.StackTrace;
            }
        }

        private static string GetExceptionMessageStack(Exception e) {
            if (e.InnerException != null) {
                StringBuilder message = new StringBuilder();
                message.AppendLine(GetExceptionMessageStack(e.InnerException));
                message.AppendLine("   " + e.Message);
                return (message.ToString());
            }
            else {
                return "   " + e.Message;
            }
        }

        private static string GetExceptionTypeStack(Exception e) {
            if (e.InnerException != null) {
                StringBuilder message = new StringBuilder();
                message.AppendLine(GetExceptionTypeStack(e.InnerException));
                message.AppendLine("   " + e.GetType().ToString());
                return (message.ToString());
            }
            else {
                return "   " + e.GetType().ToString();
            }
        }

        private static TimeSpan GetSystemUpTime() {
            return TimeSpan.FromSeconds((uint)Environment.TickCount);
        }

        private static void loadAssembly() {
            string name = (string)AppDomain.CurrentDomain.GetData("assembly");
            MissingAssemblies = "";
            asmpath = Path.GetDirectoryName(name);

            byte[] asmdata = File.ReadAllBytes(name);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.ReflectionOnlyLoad(asmdata);

            asmdata = null;
            alreadyLoaded = new Hashtable();

            DumpAssembly(assembly);

            if (MissingAssemblies != "") {
                // The error dialog
                Form errorForm = new Form();
                if (Application.OpenForms.Count > 0) {
                    errorForm.StartPosition = FormStartPosition.CenterScreen;
                    errorForm.WindowState = FormWindowState.Maximized;
                    errorForm.TopLevel = true;
                    errorForm.TopMost = true;
                }
                else {
                    errorForm.WindowState = FormWindowState.Maximized;
                    errorForm.StartPosition = FormStartPosition.CenterScreen;
                }

                errorForm.Text = "Missing assembly finded!";

                RichTextBox errorBox = new RichTextBox();
                errorForm.Controls.Add(errorBox);
                errorBox.Top = 10;
                errorBox.Left = 5;
                errorBox.Width = errorForm.Width - 20;
                errorBox.Height = errorForm.ClientRectangle.Height - 30 - errorBox.Top;
                errorBox.Text = "Missing assembly list:" + "\r\n" + MissingAssemblies;
                errorBox.Font = new System.Drawing.Font("Courier New", 10);
                errorBox.ReadOnly = true;
                errorBox.WordWrap = false;
                errorBox.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
                errorBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                Button errorOk = new Button();
                errorForm.Controls.Add(errorOk);
                errorOk.Top = errorForm.ClientRectangle.Height - 25;
                errorOk.Left = errorForm.ClientRectangle.Width - 5 - errorOk.Width;
                errorOk.Text = "&OK";
                errorOk.DialogResult = DialogResult.Cancel;
                errorOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                errorOk.FlatStyle = FlatStyle.System;
                errorForm.CancelButton = errorOk;
                errorForm.AcceptButton = errorOk;

                errorForm.ShowDialog();
            }
        }

        private static void LogCallBack(IAsyncResult ar) {
            LogExceptionDelegate del = (LogExceptionDelegate)((AsyncResult)ar).AsyncDelegate;
            del.EndInvoke(ar);
        }

        /// <summary>writes exception details to the registered loggers</summary>
        /// <param name="exception">The exception to log.</param>
        private static void LogException(Exception exception) {
            StringBuilder error = new StringBuilder();

            error.AppendLine("Application:       " + Application.ProductName);

            error.AppendLine("Version:           " + Application.ProductVersion);
            error.AppendLine("Date:              " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            error.AppendLine("Computer name:     " + SystemInformation.ComputerName);
            error.AppendLine("User name:         " + SystemInformation.UserName);
            error.AppendLine("OS:                " + Environment.OSVersion.ToString());
            error.AppendLine("Culture:           " + CultureInfo.CurrentCulture.Name);
            error.AppendLine("Resolution:        " + SystemInformation.PrimaryMonitorSize.ToString());

            error.AppendLine("System up time:    " + GetSystemUpTime());
            error.AppendLine("App up time:       " +
              (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString());

            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
            if (GlobalMemoryStatusEx(memStatus)) {
                error.AppendLine("Total memory:      " + memStatus.ullTotalPhys / (1024 * 1024) + "Mb");
                error.AppendLine("Available memory:  " + memStatus.ullAvailPhys / (1024 * 1024) + "Mb");
            }

            error.AppendLine("");

            error.AppendLine("Exception classes:   ");
            error.Append(GetExceptionTypeStack(exception));
            error.AppendLine("");
            error.AppendLine("Exception messages: ");
            error.Append(GetExceptionMessageStack(exception));

            suplexception = "";
            TypeLoadexceptions(exception);
            error.Append(suplexception);

            error.AppendLine("");
            error.AppendLine("Stack Traces:");
            error.Append(GetExceptionCallStack(exception));
            error.AppendLine("");
            error.AppendLine("Loaded Modules:");
            Process thisProcess = Process.GetCurrentProcess();
            foreach (ProcessModule module in thisProcess.Modules) {
                error.AppendLine(module.FileName + " " + module.FileVersionInfo.FileVersion);
            }

            error.AppendLine("");
            error.AppendLine("Loaded Assemblies:");
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in assemblies) {
                error.AppendLine(asm.Location + "  (" + asm.FullName + ")");
            }

            //for (int i = 0; i < loggers.Count; i++)
            //{
            //  loggers[i].LogError(error.ToString());
            //}

            // The error dialog
            Form errorForm = new Form();
            if (Application.OpenForms.Count > 0) {
                errorForm.StartPosition = FormStartPosition.CenterScreen;
                errorForm.WindowState = FormWindowState.Maximized;
                errorForm.TopLevel = true;
                errorForm.TopMost = true;
            }
            else {
                errorForm.WindowState = FormWindowState.Maximized;
                errorForm.StartPosition = FormStartPosition.CenterScreen;
            }

            errorForm.Text = "Unhandled exception reached!";

            RichTextBox errorBox = new RichTextBox();
            errorForm.Controls.Add(errorBox);
            errorBox.Top = 10;
            errorBox.Left = 5;
            errorBox.Width = errorForm.Width - 20;
            errorBox.Height = errorForm.ClientRectangle.Height - 30 - errorBox.Top;
            errorBox.Text = error.ToString();
            errorBox.Font = new System.Drawing.Font("Courier New", 10);
            errorBox.ReadOnly = true;
            errorBox.WordWrap = false;
            errorBox.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            errorBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            Button errorOk = new Button();
            errorForm.Controls.Add(errorOk);
            errorOk.Top = errorForm.ClientRectangle.Height - 25;
            errorOk.Left = errorForm.ClientRectangle.Width - 5 - errorOk.Width;
            errorOk.Text = "&OK";
            errorOk.DialogResult = DialogResult.Cancel;
            errorOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            errorOk.FlatStyle = FlatStyle.System;
            errorForm.CancelButton = errorOk;
            errorForm.AcceptButton = errorOk;

            errorForm.ShowDialog();
        }

        private static void Shutdown() {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.InvokeShutdown();
        }

        private static void TypeLoadexceptions(Exception ex) {
            ReflectionTypeLoadException refexception = ex as ReflectionTypeLoadException;
            if (refexception != null) {
                suplexception = suplexception + "\r\n" + "\r\n" + "Details of ReflectionTypeLoadException: ";
                suplexception = suplexception + "\r\n" + refexception.Message;
                for (int i = 0; i < refexception.LoaderExceptions.Length; i++) {
                    if (refexception.LoaderExceptions[i] != null) {
                        suplexception = suplexception + "\r\n" + refexception.LoaderExceptions[i].Message;
                        if (refexception.LoaderExceptions[i].StackTrace != "")
                            suplexception = suplexception + "\r\n" + " Stack:" + refexception.LoaderExceptions[i].StackTrace;
                        TypeLoadexceptions(refexception.LoaderExceptions[i]);
                    }
                }
            }

            TypeLoadException TypeLoadexception = ex as TypeLoadException;
            if (TypeLoadexception != null) {
                suplexception = suplexception + refexception.Message + "\r\n";
            }
        }

        private void Exit(object sender, EventArgs e) {
            Application.Exit();
        }

        private void SetPath() {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Browse for target assembly";
            fdlg.InitialDirectory = @"c:\";
            if (DirectoryName != "")
                fdlg.InitialDirectory = DirectoryName;
            fdlg.Filter = "All files (*.exe)|*.exe";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK) {
                string FileName = fdlg.FileName;
                ExecPath = FileName;
                //textBox1.Text = FileName;
                int lastslash = FileName.LastIndexOf("\\");
                if (lastslash != -1)
                    DirectoryName = FileName.Remove(lastslash, FileName.Length - lastslash);
                if (DirectoryName.Length == 2)
                    DirectoryName = DirectoryName + "\\";
            }
        }

        //static string Path { get; set; }

        private void TextBox1DragDrop(object sender, DragEventArgs e) {
            try {
                Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                if (a != null) {
                    string s = a.GetValue(0).ToString();
                    int lastoffsetpoint = s.LastIndexOf(".");
                    if (lastoffsetpoint != -1) {
                        string Extension = s.Substring(lastoffsetpoint);
                        Extension = Extension.ToLower();
                        if (Extension == ".exe") {
                            //this.Activate();
                            //textBox1.Text = s;
                            int lastslash = s.LastIndexOf("\\");
                            if (lastslash != -1)
                                DirectoryName = s.Remove(lastslash, s.Length - lastslash);
                        }
                    }
                }
            }
            catch {
            }
        }

        private void TextBox1DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        #endregion Private

        #region DllImport

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr IntPtr_Module);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string csFileName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle,
           uint dwThreadId);

        [DllImport("kernel32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ReadProcessMemory(int hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ReadProcessMemory(int hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool TerminateThread(IntPtr hThread, uint dwExitCode);

        [DllImport("kernel32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int WriteProcessMemory(int hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, ref int lpNumberOfBytesWritten);

        #endregion DllImport
    }
}