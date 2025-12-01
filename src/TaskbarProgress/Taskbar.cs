using System;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace TaskbarProgress
{
    public enum TaskbarProgressState
    {
        NoProgress    = 0x0,
        Indeterminate = 0x1,
        Normal        = 0x2,
        Error         = 0x4,
        Paused        = 0x8
    }

    [ComImportAttribute()]
    [GuidAttribute("EA1AFB91-9E28-4B86-90E9-9E9F8A5EEFAF")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    interface ITaskbarList3
    {
        // ITaskbarList
        [PreserveSig]
        void HrInit();
        [PreserveSig]
        void AddTab(IntPtr hwnd);
        [PreserveSig]
        void DeleteTab(IntPtr hwnd);
        [PreserveSig]
        void ActivateTab(IntPtr hwnd);
        [PreserveSig]
        void SetActiveAlt(IntPtr hwnd);

        // ITaskbarList2
        [PreserveSig]
        void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool full);

        // ITaskbarList3
        [PreserveSig]
        void SetProgressValue(IntPtr hwnd, UInt64 completed, UInt64 total);
        [PreserveSig]
        void SetProgressState(IntPtr hwnd, TaskbarProgressState state);
    }

    [GuidAttribute("56FDF344-FD6D-11D0-958A-006097C9A090")]
    [ClassInterfaceAttribute(ClassInterfaceType.None)]
    [ComImportAttribute()]
    class TaskbarInstance
    {

    }

    public static class Taskbar
    {
        #region Fields
        private static bool                 _Supported = Environment.OSVersion.Version >= new Version(6, 1);
        private static ITaskbarList3        _Instance;
        private static IntPtr               _Handle    = IntPtr.Zero;
        private static TaskbarProgressState _State;
        private static int                  _Maximmum;
        private static int                  _Value;
        #endregion

        #region Properties
        public static bool IsSupported => _Supported;
        
        public static TaskbarProgressState ProgressState
        {
            get
            {
                return _State;
            }

            set
            {
                if (_Supported && _Handle != IntPtr.Zero && value != _State)
                {
                    _State = value;
                    _Instance.SetProgressState(_Handle, _State);
                }
            }
        }

        public static int ProgressMaximmum
        {
            get
            {
                return _Maximmum;
            }

            set
            {
                if (_Supported && _Handle != IntPtr.Zero && value != _Maximmum)
                {
                    _Maximmum = value;
                    _Instance.SetProgressValue(_Handle, (ulong)_Value, (ulong)_Maximmum);
                }
            }
        }

        public static int ProgressValue
        {
            get
            {
                return _Value;
            }

            set
            {
                if (_Supported && _Handle != IntPtr.Zero && value != _Value)
                {
                    _Value = value;
                    _Instance.SetProgressValue(_Handle, (ulong)_Value, (ulong)_Maximmum);
                }
            }
        }
        #endregion

        #region Methods
        public static void ProgressInit()
        {
            if (_Supported)
            {
                _Instance = (ITaskbarList3)(new TaskbarInstance());
                _Handle   = Process.GetCurrentProcess().MainWindowHandle;
                _State    = TaskbarProgressState.NoProgress;
                _Maximmum = 100;
                _Value    = 0;
            }
        }

        public static void ProgressReset()
        {
            if (_Supported && _Handle != IntPtr.Zero)
            {
                _State    = TaskbarProgressState.NoProgress;
                _Maximmum = 0;
                _Value    = 0;
                _Instance.SetProgressState(_Handle, _State);
                _Instance.SetProgressValue(_Handle, (ulong)_Value, (ulong)_Maximmum);
            }
        }
        #endregion
    }
}