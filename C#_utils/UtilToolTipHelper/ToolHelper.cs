using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Security.Principal;

namespace UtilToolTipHelper
{
    /// <summary>
    /// 提示信息
    /// </summary>
    internal class ToolTopMessage
    {
        /// <summary>
        /// 提示信息
        /// </summary>
        internal ToolTopMessage()
        {

        }
        private String _Title;
        /// <summary>
        /// 消息标题
        /// </summary>
        internal String Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _Content;
        /// <summary>
        /// 消息内容
        /// </summary>
        internal string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        private Int32 _Timeout = 5000;
        /// <summary>
        /// 消息默认显示时间(毫秒)
        /// 默认5000毫秒
        /// </summary>
        internal Int32 Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }
        private ToolTipIcon _balloonType = ToolTipIcon.Info;
        /// <summary>
        /// 显示消息的方式
        /// </summary>
        internal ToolTipIcon BalloonType
        {
            get { return _balloonType; }
            set { _balloonType = value; }
        }
    }
    /// <summary>
    /// Desktop
    /// </summary>
    internal class Desktop
    {
        private IntPtr m_hCurWinsta = IntPtr.Zero;
        private IntPtr m_hCurDesktop = IntPtr.Zero;
        private IntPtr m_hWinsta = IntPtr.Zero;
        private IntPtr m_hDesk = IntPtr.Zero;

        /// <summary>
        /// associate the current thread to the default desktop
        /// </summary>
        /// <returns></returns>
        internal bool BeginInteraction()
        {
            EndInteraction();
            m_hCurWinsta = User32DLL.GetProcessWindowStation();
            if (m_hCurWinsta == IntPtr.Zero)
                return false;

            m_hCurDesktop = User32DLL.GetDesktopWindow();
            if (m_hCurDesktop == IntPtr.Zero)
                return false;

            m_hWinsta = User32DLL.OpenWindowStation("winsta0", false,
                WindowStationAccessRight.WINSTA_ACCESSCLIPBOARD |
                WindowStationAccessRight.WINSTA_ACCESSGLOBALATOMS |
                WindowStationAccessRight.WINSTA_CREATEDESKTOP |
                WindowStationAccessRight.WINSTA_ENUMDESKTOPS |
                WindowStationAccessRight.WINSTA_ENUMERATE |
                WindowStationAccessRight.WINSTA_EXITWINDOWS |
                WindowStationAccessRight.WINSTA_READATTRIBUTES |
                WindowStationAccessRight.WINSTA_READSCREEN |
                WindowStationAccessRight.WINSTA_WRITEATTRIBUTES
                );
            if (m_hWinsta == IntPtr.Zero)
                return false;

            User32DLL.SetProcessWindowStation(m_hWinsta);

            m_hDesk = User32DLL.OpenDesktop("default", OpenDesktopFlag.DF_NONE, false,
                    DesktopAccessRight.DESKTOP_CREATEMENU |
                    DesktopAccessRight.DESKTOP_CREATEWINDOW |
                    DesktopAccessRight.DESKTOP_ENUMERATE |
                    DesktopAccessRight.DESKTOP_HOOKCONTROL |
                    DesktopAccessRight.DESKTOP_JOURNALPLAYBACK |
                    DesktopAccessRight.DESKTOP_JOURNALRECORD |
                    DesktopAccessRight.DESKTOP_READOBJECTS |
                    DesktopAccessRight.DESKTOP_SWITCHDESKTOP |
                    DesktopAccessRight.DESKTOP_WRITEOBJECTS
                    );
            if (m_hDesk == IntPtr.Zero)
                return false;

            User32DLL.SetThreadDesktop(m_hDesk);

            return true;
        }

        /// <summary>
        /// restore
        /// </summary>
        internal void EndInteraction()
        {
            if (m_hCurWinsta != IntPtr.Zero)
                User32DLL.SetProcessWindowStation(m_hCurWinsta);

            if (m_hCurDesktop != IntPtr.Zero)
                User32DLL.SetThreadDesktop(m_hCurDesktop);

            if (m_hWinsta != IntPtr.Zero)
                User32DLL.CloseWindowStation(m_hWinsta);

            if (m_hDesk != IntPtr.Zero)
                User32DLL.CloseDesktop(m_hDesk);
        }

        const int GENERIC_ALL_ACCESS = 0x10000000;
        /// <summary>
        /// 在用户桌面上启动一个进程 支持Win7及win ser 2008系统内核
        /// </summary>
        /// <param name="app">进程名称(全路径)</param>
        /// <param name="path">运行路径</param>
        /// <param name="parameter">启动参数</param>
        internal static bool CreateProcess(string app, string path, string parameter)
        {
            bool result;
            IntPtr hToken = WindowsIdentity.GetCurrent().Token;
            IntPtr hDupedToken = IntPtr.Zero;

            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            SECURITY_ATTRIBUTES sa = new SECURITY_ATTRIBUTES();
            sa.Length = Marshal.SizeOf(sa);

            STARTUPINFO si = new STARTUPINFO();
            si.cb = Marshal.SizeOf(si);

            int dwSessionID = Kernel32DLL.WTSGetActiveConsoleSessionId();
            result = WtsAPI32DLL.WTSQueryUserToken(dwSessionID, out hToken);
            if (!result)
                return false;

            result = AdvAPI32DLL.DuplicateTokenEx(
                  hToken,
                  GENERIC_ALL_ACCESS,
                  ref sa,
                  (int)SECURITY_IMPERSONATION_LEVEL.SecurityIdentification,
                  (int)TOKEN_TYPE.TokenPrimary,
                  ref hDupedToken
               );
            if (!result)
                return false;

            IntPtr lpEnvironment = IntPtr.Zero;
            result = UserENVDLL.CreateEnvironmentBlock(out lpEnvironment, hDupedToken, false);
            if (!result)
                return false;

            result = AdvAPI32DLL.CreateProcessAsUser(
                                 hDupedToken,
                                 app,
                                 parameter,
                                 ref sa, ref sa,
                                 false, 0, IntPtr.Zero,
                                 path, ref si, ref pi);
            if (!result)
                return false;

            if (pi.hProcess != IntPtr.Zero)
                Kernel32DLL.CloseHandle(pi.hProcess);
            if (pi.hThread != IntPtr.Zero)
                Kernel32DLL.CloseHandle(pi.hThread);
            if (hDupedToken != IntPtr.Zero)
                Kernel32DLL.CloseHandle(hDupedToken);

            return true;
        }
    }
    /// <summary>
    /// Kernel32DLL
    /// </summary>
    internal static class Kernel32DLL
    {
        /// <summary>
        /// WTSGetActiveConsoleSessionId
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int WTSGetActiveConsoleSessionId();
        /// <summary>
        /// CloseHandle
        /// </summary>
        /// <param name="handle">IntPtr</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true,
             CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool CloseHandle(IntPtr handle);
    }
    /// <summary>
    /// The wrapper class for User32.dll
    /// </summary>
    internal static class User32DLL
    {
        /// <summary>
        /// The GetDesktopWindow function returns a handle to the desktop window. The desktop window covers the entire screen. The desktop window is the area on top of which other windows are painted. 
        /// </summary>
        /// <returns>The return value is a handle to the desktop window. </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr GetDesktopWindow();
        /// <summary>
        /// Retrieves a handle to the current window station for the calling process.
        /// </summary>
        /// <returns>If the function succeeds, the return value is a handle to the window station.If the function fails, the return value is NULL.</returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr GetProcessWindowStation();
        /// <summary>
        /// Retrieves a handle to the desktop assigned to the specified thread.
        /// </summary>
        /// <param name="dwThread">[in] Handle to the thread for which to return the desktop handle.</param>
        /// <returns>If the function succeeds, the return value is a handle to the desktop associated with the specified thread. You do not need to call the CloseDesktop function to close the returned handle.If the function fails, the return value is NULL.</returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr GetThreadDesktop(uint dwThread);
        /// <summary>
        /// Opens the specified window station.
        /// </summary>
        /// <param name="lpszWinSta">Pointer to a null-terminated string specifying the name of the window station to be opened. Window station names are case-insensitive.This window station must belong to the current session.</param>
        /// <param name="fInherit">[in] If this value is TRUE, processes created by this process will inherit the handle. Otherwise, the processes do not inherit this handle. </param>
        /// <param name="dwDesiredAccess">[in] Access to the window station</param>
        /// <returns>If the function succeeds, the return value is the handle to the specified window station.If the function fails, the return value is NULL.</returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr OpenWindowStation(string lpszWinSta
            , bool fInherit
            , WindowStationAccessRight dwDesiredAccess
            );
        /// <summary>
        /// Assigns the specified window station to the calling process. This enables the process to access objects in the window station such as desktops, the clipboard, and global atoms. All subsequent operations on the window station use the access rights granted to hWinSta.
        /// </summary>
        /// <param name="hWinSta">[in] Handle to the window station to be assigned to the calling process</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr SetProcessWindowStation(IntPtr hWinSta);
        /// <summary>
        /// Closes an open window station handle.
        /// </summary>
        /// <param name="hWinSta">[in] Handle to the window station to be closed.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr CloseWindowStation(IntPtr hWinSta);
        /// <summary>
        /// Opens the specified desktop object.
        /// </summary>
        /// <param name="lpszDesktop">[in] Pointer to null-terminated string specifying the name of the desktop to be opened. Desktop names are case-insensitive.
        /// This desktop must belong to the current window station.</param>
        /// <param name="dwFlags">[in] This parameter can be zero or DF_ALLOWOTHERACCOUNTHOOK=0x0001</param>
        /// <param name="fInherit">[in] If this value is TRUE, processes created by this process will inherit the handle. Otherwise, the processes do not inherit this handle. </param>
        /// <param name="dwDesiredAccess">[in] Access to the desktop. For a list of access rights</param>
        /// <returns>If the function succeeds, the return value is a handle to the opened desktop. When you are finished using the handle, call the CloseDesktop function to close it.
        /// If the function fails, the return value is NULL.
        /// </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr OpenDesktop(string lpszDesktop
            , OpenDesktopFlag dwFlags
            , bool fInherit
            , DesktopAccessRight dwDesiredAccess
            );
        /// <summary>
        /// Closes an open handle to a desktop object.
        /// </summary>
        /// <param name="hDesktop">[in] Handle to the desktop to be closed.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr CloseDesktop(IntPtr hDesktop);
        /// <summary>
        /// Assigns the specified desktop to the calling thread. All subsequent operations on the desktop use the access rights granted to the desktop.
        /// </summary>
        /// <param name="hDesktop">[in] Handle to the desktop to be assigned to the calling thread.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetThreadDesktop(IntPtr hDesktop);
    }
    /// <summary>
    /// REF MSDN:Window Station Security and Access Rights
    /// ms-help://MS.MSDN.vAug06.en/dllproc/base/window_station_security_and_access_rights.htm
    /// </summary>
    [FlagsAttribute]
    internal enum WindowStationAccessRight : uint
    {
        /// <summary>
        /// All possible access rights for the window station.
        /// </summary>
        WINSTA_ALL_ACCESS = 0x37F,

        /// <summary>
        /// Required to use the clipboard.
        /// </summary>
        WINSTA_ACCESSCLIPBOARD = 0x0004,

        /// <summary>
        /// Required to manipulate global atoms.
        /// </summary>
        WINSTA_ACCESSGLOBALATOMS = 0x0020,

        /// <summary>
        /// Required to create new desktop objects on the window station.
        /// </summary>
        WINSTA_CREATEDESKTOP = 0x0008,

        /// <summary>
        /// Required to enumerate existing desktop objects.
        /// </summary>
        WINSTA_ENUMDESKTOPS = 0x0001,

        /// <summary>Required for the window station to be enumerated.</summary>
        WINSTA_ENUMERATE = 0x0100,

        /// <summary>
        /// Required to successfully call the ExitWindows or ExitWindowsEx function. Window stations can be shared by users and this access type can prevent other users of a window station from logging off the window station owner.
        /// </summary>
        WINSTA_EXITWINDOWS = 0x0040,

        /// <summary>
        /// Required to read the attributes of a window station object. This attribute includes color settings and other global window station properties.
        /// </summary>
        WINSTA_READATTRIBUTES = 0x0002,

        /// <summary>
        /// Required to access screen contents.
        /// </summary>
        WINSTA_READSCREEN = 0x0200,
        /// <summary>
        /// Required to modify the attributes of a window station object. The attributes include color settings and other global window station properties.
        /// </summary>
        WINSTA_WRITEATTRIBUTES = 0x0010,
    }
    /// <summary>
    /// OpenDesktop 2nd param
    /// </summary>
    internal enum OpenDesktopFlag : uint
    {
        /// <summary>
        /// Default value
        /// </summary>
        DF_NONE = 0x0000,
        /// <summary>
        /// Allows processes running in other accounts on the desktop to set hooks in this process.
        /// </summary>
        DF_ALLOWOTHERACCOUNTHOOK = 0x0001,
    }
    /// <summary>
    /// REF MSDN:Desktop Security and Access Rights
    /// ms-help://MS.MSDN.vAug06.en/dllproc/base/desktop_security_and_access_rights.htm
    /// </summary>
    [FlagsAttribute]
    internal enum DesktopAccessRight : uint
    {
        /// <summary>Required to create a menu on the desktop. </summary>
        DESKTOP_CREATEMENU = 0x0004,
        /// <summary>
        /// Required to create a window on the desktop. 
        /// </summary>
        DESKTOP_CREATEWINDOW = 0x0002,
        /// <summary>
        /// Required for the desktop to be enumerated. 
        /// </summary>
        DESKTOP_ENUMERATE = 0x0040,
        /// <summary>
        /// Required to establish any of the window hooks. 
        /// </summary>
        DESKTOP_HOOKCONTROL = 0x0008,
        /// <summary>
        /// Required to perform journal playback on a desktop. 
        /// </summary>
        DESKTOP_JOURNALPLAYBACK = 0x0020,
        /// <summary>
        /// Required to perform journal recording on a desktop. 
        /// </summary>
        DESKTOP_JOURNALRECORD = 0x0010,
        /// <summary>
        /// Required to read objects on the desktop. 
        /// </summary>
        DESKTOP_READOBJECTS = 0x0001,
        /// <summary>
        /// Required to activate the desktop using the SwitchDesktop function. 
        /// </summary>
        DESKTOP_SWITCHDESKTOP = 0x0100,
        /// <summary>
        /// Required to write objects on the desktop. 
        /// </summary>
        DESKTOP_WRITEOBJECTS = 0x0080,
    }
    /// <summary>
    /// WtsAPI32DLL
    /// </summary>
    internal static class WtsAPI32DLL
    {
        /// <summary>
        /// Obtains the primary access token of the logged-on user specified by the session ID
        /// </summary>
        /// <param name="sessionId">A Remote Desktop Services session identifier. Any program running in the context of a service will have a session identifier of zero (0).</param>
        /// <param name="Token">If the function succeeds, receives a pointer to the token handle for the logged-on user</param>
        /// <returns></returns>
        [DllImport("wtsapi32.dll", SetLastError = true)]
        internal static extern bool WTSQueryUserToken(
            Int32 sessionId,
            out IntPtr Token);
    }
    /// <summary>
    /// AdvAPI32DLL
    /// </summary>
    internal static class AdvAPI32DLL
    {
        /// <summary>
        /// Creates a new process and its primary thread. The new process runs in the security context of the user represented by the specified token.
        /// </summary>
        /// <param name="hToken">A handle to the primary token that represents a user</param>
        /// <param name="lpApplicationName">The name of the module to be executed. </param>
        /// <param name="lpCommandLine">The command line to be executed</param>
        /// <param name="lpProcessAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for the new process object and determines whether child processes can inherit the returned handle to the process</param>
        /// <param name="lpThreadAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for the new thread object and determines whether child processes can inherit the returned handle to the thread</param>
        /// <param name="bInheritHandle">If this parameter is TRUE, each inheritable handle in the calling process is inherited by the new process. If the parameter is FALSE, the handles are not inherited. Note that inherited handles have the same value and access rights as the original handles</param>
        /// <param name="dwCreationFlags">The flags that control the priority class and the creation of the process</param>
        /// <param name="lpEnvrionment">A pointer to an environment block for the new process</param>
        /// <param name="lpCurrentDirectory">The full path to the current directory for the process. The string can also specify a UNC path</param>
        /// <param name="lpStartupInfo">A pointer to a STARTUPINFO or STARTUPINFOEX structure.</param>
        /// <param name="lpProcessInformation">A pointer to a PROCESS_INFORMATION structure that receives identification information about the new process</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true,
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool CreateProcessAsUser(
            IntPtr hToken,
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandle,
            Int32 dwCreationFlags,
            IntPtr lpEnvrionment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            ref PROCESS_INFORMATION lpProcessInformation);

        /// <summary>
        /// DuplicateTokenEx
        /// </summary>
        /// <param name="hExistingToken">IntPtr</param>
        /// <param name="dwDesiredAccess">Int32</param>
        /// <param name="lpThreadAttributes">SECURITY_ATTRIBUTES</param>
        /// <param name="ImpersonationLevel">Int32</param>
        /// <param name="dwTokenType">Int32</param>
        /// <param name="phNewToken">IntPtr</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool DuplicateTokenEx(
            IntPtr hExistingToken,
            Int32 dwDesiredAccess,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            Int32 ImpersonationLevel,
            Int32 dwTokenType,
            ref IntPtr phNewToken);
    }
    /// <summary>
    /// UserENVDLL
    /// </summary>
    internal static class UserENVDLL
    {
        /// <summary>
        /// CreateEnvironmentBlock
        /// </summary>
        /// <param name="lpEnvironment">IntPtr</param>
        /// <param name="hToken">IntPtr</param>
        /// <param name="bInherit">bool</param>
        /// <returns></returns>
        [DllImport("userenv.dll", SetLastError = true)]
        internal static extern bool CreateEnvironmentBlock(
             out IntPtr lpEnvironment,
             IntPtr hToken,
             bool bInherit);
    }
    /// <summary>
    /// test
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct STARTUPINFO
    {
        /// <summary>
        /// test
        /// </summary>
        internal Int32 cb;
        /// <summary>
        /// test
        /// </summary>
        internal string lpReserved;
        /// <summary>
        /// test
        /// </summary>
        internal string lpDesktop;
        /// <summary>
        /// test
        /// </summary>
        internal string lpTitle;
        /// <summary>
        /// test
        /// </summary>
        internal Int32 dwX;
        /// <summary>
        /// test
        /// </summary>
        internal Int32 dwY;
        /// <summary>
        /// test
        /// </summary>
        internal Int32 dwXSize;
        /// <summary>
        /// test
        /// </summary>
        internal Int32 dwXCountChars;
        /// <summary>
        /// test
        /// </summary>
        internal Int32 dwYCountChars;
        /// <summary>
        /// test
        /// </summary>
        internal Int32 dwFillAttribute;
        /// <summary>
        /// test
        /// </summary>
        internal Int32 dwFlags;
        /// <summary>
        /// test
        /// </summary>
        internal Int16 wShowWindow;
        /// <summary>
        /// test
        /// </summary>
        internal Int16 cbReserved2;
        /// <summary>
        /// test
        /// </summary>
        internal IntPtr lpReserved2;
        /// <summary>
        /// test
        /// </summary>
        internal IntPtr hStdInput;
        /// <summary>
        /// test
        /// </summary>
        internal IntPtr hStdOutput;
        /// <summary>
        /// test
        /// </summary>
        internal IntPtr hStdError;
    }
    /// <summary>
    /// struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESS_INFORMATION
    {
        /// <summary>
        /// hProcess
        /// </summary>
        internal IntPtr hProcess;
        /// <summary>
        /// hThread
        /// </summary>
        internal IntPtr hThread;
        /// <summary>
        /// dwProcessID
        /// </summary>
        internal Int32 dwProcessID;
        /// <summary>
        /// dwThreadID
        /// </summary>
        internal Int32 dwThreadID;
    }
    /// <summary>
    /// SECURITY_ATTRIBUTES
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SECURITY_ATTRIBUTES
    {
        /// <summary>
        /// Length
        /// </summary>
        internal Int32 Length;
        /// <summary>
        /// lpSecurityDescriptor
        /// </summary>
        internal IntPtr lpSecurityDescriptor;
        /// <summary>
        /// bInheritHandle
        /// </summary>
        internal bool bInheritHandle;
    }
    /// <summary>
    /// SECURITY_IMPERSONATION_LEVEL
    /// </summary>
    internal enum SECURITY_IMPERSONATION_LEVEL
    {
        /// <summary>
        /// SecurityAnonymous
        /// </summary>
        SecurityAnonymous,
        /// <summary>
        /// SecurityIdentification
        /// </summary>
        SecurityIdentification,
        /// <summary>
        /// SecurityImpersonation
        /// </summary>
        SecurityImpersonation,
        /// <summary>
        /// SecurityDelegation
        /// </summary>
        SecurityDelegation
    }
    /// <summary>
    /// TOKEN_TYPE
    /// </summary>
    internal enum TOKEN_TYPE
    {
        /// <summary>
        /// TokenPrimary
        /// </summary>
        TokenPrimary = 1,
        /// <summary>
        /// TokenImpersonation
        /// </summary>
        TokenImpersonation
    }
}
