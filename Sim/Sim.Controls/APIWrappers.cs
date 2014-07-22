using System;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#pragma warning disable 1591

namespace Sim.Controls.WinAPI
{
 /// <summary>
 /// Класс враперов API функций.
 /// </summary>
 public static class APIWrappers
 {
  [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
  public static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);
 
  [DllImport("User32.dll", CharSet = CharSet.Auto)]
  public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hRegion, int flags);
  
  [DllImport("User32.dll", CharSet = CharSet.Auto)]
  public static extern IntPtr GetWindowDC(IntPtr hWnd);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
  
  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

  [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
  public static extern bool RedrawWindow(HandleRef hwnd, COMRECT rcUpdate, HandleRef hrgnUpdate, int flags);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

  [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
  public static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);
  
  [DllImport("User32.dll")]
  public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

  [DllImport("User32.dll")]
  public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern int SendMessage(IntPtr hWnd, uint wMsg, uint wParam, uint lParam);
  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern bool PostMessage(IntPtr hWnd, uint wMsg, uint wParam, uint lParam);

  [System.Security.SuppressUnmanagedCodeSecurity]
  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  private static extern int AnimateWindow(HandleRef windowHandle, int time, AnimationFlags flags);
  public static void AnimateWindow(Control control, int time, AnimationFlags flags)
  {
   AnimateWindow(new HandleRef(control, control.Handle), time, flags);
  }


  //-------------------------------------------------------------------------------------
  public static int HIWORD(int n) { return (short)((n >> 16) & 0xffff); }
  public static int HIWORD(IntPtr n) { return HIWORD(unchecked((int)(long)n)); }
  public static int LOWORD(int n) { return (short)(n & 0xffff); }
  public static int LOWORD(IntPtr n) { return LOWORD(unchecked((int)(long)n)); }
 }
 //**************************************************************************************
 [Flags()]
 public enum SetWindowPosFlags : uint
 {
  /// <summary>If the calling thread and the thread that owns the window are attached to different input queues,
  /// the system posts the request to the thread that owns the window. This prevents the calling thread from
  /// blocking its execution while other threads process the request.</summary>
  /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
  SynchronousWindowPosition = 0x4000,
  /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
  /// <remarks>SWP_DEFERERASE</remarks>
  DeferErase = 0x2000,
  /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
  /// <remarks>SWP_DRAWFRAME</remarks>
  DrawFrame = 0x0020,
  /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to
  /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE
  /// is sent only when the window's size is being changed.</summary>
  /// <remarks>SWP_FRAMECHANGED</remarks>
  FrameChanged = 0x0020,
  /// <summary>Hides the window.</summary>
  /// <remarks>SWP_HIDEWINDOW</remarks>
  HideWindow = 0x0080,
  /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the
  /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter
  /// parameter).</summary>
  /// <remarks>SWP_NOACTIVATE</remarks>
  DoNotActivate = 0x0010,
  /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid
  /// contents of the client area are saved and copied back into the client area after the window is sized or
  /// repositioned.</summary>
  /// <remarks>SWP_NOCOPYBITS</remarks>
  DoNotCopyBits = 0x0100,
  /// <summary>Retains the current position (ignores X and Y parameters).</summary>
  /// <remarks>SWP_NOMOVE</remarks>
  IgnoreMove = 0x0002,
  /// <summary>Does not change the owner window's position in the Z order.</summary>
  /// <remarks>SWP_NOOWNERZORDER</remarks>
  DoNotChangeOwnerZOrder = 0x0200,
  /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to
  /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent
  /// window uncovered as a result of the window being moved. When this flag is set, the application must
  /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
  /// <remarks>SWP_NOREDRAW</remarks>
  DoNotRedraw = 0x0008,
  /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
  /// <remarks>SWP_NOREPOSITION</remarks>
  DoNotReposition = 0x0200,
  /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
  /// <remarks>SWP_NOSENDCHANGING</remarks>
  DoNotSendChangingEvent = 0x0400,
  /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
  /// <remarks>SWP_NOSIZE</remarks>
  IgnoreResize = 0x0001,
  /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
  /// <remarks>SWP_NOZORDER</remarks>
  IgnoreZOrder = 0x0004,
  /// <summary>Displays the window.</summary>
  /// <remarks>SWP_SHOWWINDOW</remarks>
  ShowWindow = 0x0040,
 }
 //**************************************************************************************
 [StructLayout(LayoutKind.Sequential)]
 public struct NCCALCSIZE_PARAMS
 {
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
  public RECT[] rgc;
  public WINDOWPOS wndpos;
 }
 //**************************************************************************************
 [StructLayout(LayoutKind.Sequential)]
 public struct WINDOWPOS
 {
  public IntPtr hwnd;
  public IntPtr hwndAfter;
  public int x;
  public int y;
  public int cx;
  public int cy;
  public uint flags;
 }
 //**************************************************************************************
 [StructLayout(LayoutKind.Sequential)]
 public struct RECT
 {
  public int left;
  public int top;
  public int right;
  public int bottom;
  public RECT(int left, int top, int right, int bottom)
  {
   this.left = left;
   this.top = top;
   this.right = right;
   this.bottom = bottom;
  }

  public RECT(Rectangle r)
  {
   this.left = r.Left;
   this.top = r.Top;
   this.right = r.Right;
   this.bottom = r.Bottom;
  }

  public static RECT FromXYWH(int x, int y, int width, int height)
  {
   return new RECT(x, y, x + width, y + height);
  }

  public Size Size
  {
   get
   {
    return new Size(this.right - this.left, this.bottom - this.top);
   }
  }

  public override string ToString()
  {
   return String.Format("left:{0},top:{1},right:{2},bottom:{3}", left, top,right,bottom);
  }
 }
 //**************************************************************************************
 public enum GetDCExFlags : int
 {
  DCX_WINDOW = 0x00000001,
  DCX_CACHE = 0x00000002,
  DCX_NORESETATTRS = 0x00000004,
  DCX_CLIPCHILDREN = 0x00000008,
  DCX_CLIPSIBLINGS = 0x00000010,
  DCX_PARENTCLIP = 0x00000020,
  DCX_EXCLUDERGN = 0x00000040,
  DCX_INTERSECTRGN = 0x00000080,
  DCX_EXCLUDEUPDATE = 0x00000100,
  DCX_INTERSECTUPDATE = 0x00000200,
  DCX_LOCKWINDOWUPDATE = 0x00000400,
  DCX_VALIDATE = 0x00200000
 }
 //**************************************************************************************
 [StructLayout(LayoutKind.Sequential)]
 public class COMRECT
 {
  public int left;
  public int top;
  public int right;
  public int bottom;
  
  public COMRECT()
  {
  }

  public COMRECT(Rectangle r)
  {
   this.left = r.X;
   this.top = r.Y;
   this.right = r.Right;
   this.bottom = r.Bottom;
  }

  public COMRECT(int left, int top, int right, int bottom)
  {
   this.left = left;
   this.top = top;
   this.right = right;
   this.bottom = bottom;
  }

  public static COMRECT FromXYWH(int x, int y, int width, int height)
  {
   return new COMRECT(x, y, x + width, y + height);
  }

  public override string ToString()
  {
   return string.Concat(new object[] { "Left = ", this.left, " Top ", this.top, " Right = ", this.right, " Bottom = ", this.bottom });
  }
 }
 //**************************************************************************************
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
 //**************************************************************************************
 /// <summary>
 /// 
 /// </summary>
 [StructLayout(LayoutKind.Sequential)]
 public struct IconInfo
 {
  public bool fIcon;
  public int xHotspot;
  public int yHotspot;
  public IntPtr hbmMask;
  public IntPtr hbmColor;

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

  [DllImport("user32.dll")]
  public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

  public static Cursor CreateCursorFromBitmap(Bitmap bmp, int xHotSpot, int yHotSpot)
  {
   IntPtr ptr = bmp.GetHicon();
   IconInfo tmp = new IconInfo();
   IconInfo.GetIconInfo(ptr, ref tmp);
   tmp.xHotspot = xHotSpot;
   tmp.yHotspot = yHotSpot;
   tmp.fIcon = false;
   ptr = IconInfo.CreateIconIndirect(ref tmp);
   return new Cursor(ptr);
  }
 }
 //**************************************************************************************
 [StructLayout(LayoutKind.Sequential)]
 public struct PAINTSTRUCT
 {
  public IntPtr hdc;
  public int fErase;
  public RECT rcPaint;
  public int fRestore;
  public int fIncUpdate;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
  public byte[] rgbReserved;
 }
 //**************************************************************************************
 #region << public enum AnimationFlags >>
 /// <summary>
 /// Types of animation of the pop-up window.
 /// </summary>
 [Flags]
 public enum AnimationFlags : int
 {
  /// <summary>
  ///  Uses a roll animation.
  /// </summary>
  Roll = 0x0000,
  /// <summary>
  /// Animates the window from left to right. 
  /// This flag can be used with roll or slide animation.
  /// </summary>
  HorizontalPositive = 0x00001,
  /// <summary>
  /// Animates the window from right to left.
  /// This flag can be used with roll or slide animation.
  /// </summary>
  HorizontalNegative = 0x00002,
  /// <summary>
  /// Animates the window from top to bottom.
  /// This flag can be used with roll or slide animation.
  /// </summary>
  VerticalPositive = 0x00004,
  /// <summary>
  /// Animates the window from bottom to top.
  /// This flag can be used with roll or slide animation.
  /// </summary>
  VerticalNegative = 0x00008,
  /// <summary>
  /// Makes the window appear to collapse inward if Hide is used or expand outward if the Hide is not used.
  /// </summary>
  Center = 0x00010,
  /// <summary>
  /// Hides the window. By default, the window is shown.
  /// </summary>
  Hide = 0x10000,
  /// <summary>
  /// Activates the window.
  /// </summary>
  Activate = 0x20000,
  /// <summary>
  /// Uses a slide animation. By default, roll animation is used.
  /// </summary>
  Slide = 0x40000,
  /// <summary>
  /// Uses a fade effect. This flag can be used only with a top-level window.
  /// </summary>
  Blend = 0x80000,
  /// <summary>
  /// Mask = 0xfffff
  /// </summary>
  Mask = 0xfffff
 }
 #endregion << public enum AnimationFlags >>
 //**************************************************************************************
 /// <summary>
 /// Перечисление стилей окна Windows.
 /// </summary>
 [Flags]
 public enum WindowStyles : uint
 {
  /// <summary>
  /// Creates an overlapped window. An overlapped window has a title bar and a border.
  /// Same as the WS_TILED style.
  /// </summary>
  WS_OVERLAPPED = 0x00000000,
  /// <summary>
  /// Creates a pop-up window. This style cannot be used with the WS_CHILD style.
  /// </summary>
  WS_POPUP = 0x80000000,
  /// <summary>
  /// (Aliace WS_CHILDWINDOW) Creates a child window. A window with this style cannot
  /// have a menu bar. This style cannot be used with the WS_POPUP style.
  /// </summary>
  WS_CHILD = 0x40000000,
  /// <summary>
  /// Creates a window that is initially minimized. Same as the WS_ICONIC style.
  /// </summary>
  WS_MINIMIZE = 0x20000000,
  /// <summary>
  /// Creates a window that is initially visible. 
  /// </summary>
  WS_VISIBLE = 0x10000000,
  /// <summary>
  /// Creates a window that is initially disabled. A disabled window cannot receive input
  /// from the user. To change this after a window has been created, use EnableWindow. 
  /// </summary>
  WS_DISABLED = 0x08000000,
  /// <summary>
  /// Clips child windows relative to each other; that is, when a particular child window
  /// receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping
  /// child windows out of the region of the child window to be updated.
  /// If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when
  /// drawing within the client area of a child window, to draw within the client area of
  /// a neighboring child window.
  /// </summary>
  WS_CLIPSIBLINGS = 0x04000000,
  /// <summary>
  /// Excludes the area occupied by child windows when drawing occurs within the parent
  /// window. This style is used when creating the parent window.
  /// </summary> 
  WS_CLIPCHILDREN = 0x02000000,
  /// <summary>
  /// Creates a window that is initially maximized.
  /// </summary>
  WS_MAXIMIZE = 0x01000000,
  /// <summary>
  /// (WS_BORDER | WS_DLGFRAME) Creates a window that has a title bar.
  /// </summary>
  WS_CAPTION = 0x00C00000,
  /// <summary>
  /// Creates a window that has a thin-line border.
  /// </summary>
  WS_BORDER = 0x00800000,
  /// <summary>
  /// Creates a window that has a border of a style typically used with dialog boxes.
  /// A window with this style cannot have a title bar.
  /// </summary>
  WS_DLGFRAME = 0x00400000,
  /// <summary>
  /// Creates a window that has a vertical scroll bar.
  /// </summary>
  WS_VSCROLL = 0x00200000,
  /// <summary>
  /// Creates a window that has a horizontal scroll bar.
  /// </summary>
  WS_HSCROLL = 0x00100000,
  /// <summary>
  /// Creates a window that has a window menu on its title bar. The WS_CAPTION style must
  /// also be specified.
  /// </summary>
  WS_SYSMENU = 0x00080000,
  /// <summary>
  /// Creates a window that has a sizing border. Same as the WS_SIZEBOX style.
  /// </summary>
  WS_THICKFRAME = 0x00040000,
  /// <summary>
  /// Specifies the first control of a group of controls. The group consists of this first
  /// control and all controls defined after it, up to the next control with the WS_GROUP
  /// style. The first control in each group usually has the WS_TABSTOP style so that the
  /// user can move from group to group. The user can subsequently change the keyboard focus
  /// from one control in the group to the next control in the group by using the direction
  /// keys. You can turn this style on and off to change dialog box navigation. To change
  /// this style after a window has been created, use SetWindowLong.
  /// </summary>
  WS_GROUP = 0x00020000,
  /// <summary>
  /// Specifies a control that can receive the keyboard focus when the user presses
  /// the TAB key. Pressing the TAB key changes the keyboard focus to the next control
  /// with the WS_TABSTOP style. You can turn this style on and off to change dialog
  /// box navigation. To change this style after a window has been created, use SetWindowLong.
  /// </summary>
  WS_TABSTOP = 0x00010000,
  /// <summary>
  /// Creates a window that has a minimize button. Cannot be combined with the
  /// WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified. 
  /// </summary>
  WS_MINIMIZEBOX = 0x00020000,
  /// <summary>
  /// Creates a window that has a maximize button. Cannot be combined with the
  /// WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified. 
  /// </summary>
  WS_MAXIMIZEBOX = 0x00010000,
  /// <summary>
  /// Creates an overlapped window. An overlapped window has a title bar and a border.
  /// Same as the WS_OVERLAPPED style. 
  /// </summary>
  WS_TILED = 0x00000000,
  /// <summary>
  /// Creates a window that is initially minimized. Same as the WS_MINIMIZE style.
  /// </summary>
  WS_ICONIC = 0x20000000,
  /// <summary>
  /// Creates a window that has a sizing border. Same as the WS_THICKFRAME style.
  /// </summary>
  WS_SIZEBOX = 0x00040000,
  /// <summary>
  /// Creates an overlapped window with the WS_OVERLAPPED, WS_CAPTION, WS_SYSMENU,
  /// WS_THICKFRAME, WS_MINIMIZEBOX, and WS_MAXIMIZEBOX styles. Same as the
  /// WS_OVERLAPPEDWINDOW style. 
  /// </summary>
  WS_TILEDWINDOW = 0x00CF0000,
  /// <summary>
  /// Creates an overlapped window with the WS_OVERLAPPED, WS_CAPTION, WS_SYSMENU,
  /// WS_THICKFRAME, WS_MINIMIZEBOX, and WS_MAXIMIZEBOX styles. Same as the WS_TILEDWINDOW
  /// style. 
  /// </summary>
  WS_OVERLAPPEDWINDOW = 0x00CF0000,
  /// <summary>
  /// Creates a pop-up window with WS_BORDER, WS_POPUP, and WS_SYSMENU styles.
  /// The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu
  /// visible.
  /// </summary>
  WS_POPUPWINDOW = 0x80880000,
  /// <summary>
  /// Same as WS_CHILD.
  /// </summary>
  WS_CHILDWINDOW = 0x40000000
 }
 //**************************************************************************************
 /// <summary>
 /// Перечисление дополнительных стилей окна Windows.
 /// </summary>
 [Flags]
 public enum ExWindowStyles : int
 {
  /// <summary>
  /// Creates a window that has a double border; the window can, optionally, be created
  /// with a title bar by specifying the WS_CAPTION style in the dwStyle parameter.
  /// </summary>
  WS_EX_DLGMODALFRAME = 0x00000001,
  /// <summary>
  /// Specifies that a child window created with this style does not send the
  /// WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
  /// </summary>
  WS_EX_NOPARENTNOTIFY = 0x00000004,
  /// <summary>
  /// Specifies that a window created with this style should be placed above all
  /// non-topmost windows and should stay above them, even when the window is deactivated.
  /// To add or remove this style, use the SetWindowPos function.
  /// </summary>
  WS_EX_TOPMOST = 0x00000008,
  /// <summary>
  /// Specifies that a window created with this style accepts drag-drop files.
  /// </summary>
  WS_EX_ACCEPTFILES = 0x00000010,
  /// <summary>
  /// Specifies that a window created with this style should not be painted until siblings
  /// beneath the window (that were created by the same thread) have been painted.
  /// The window appears transparent because the bits of underlying sibling windows have
  /// already been painted. To achieve transparency without these restrictions, use the
  /// SetWindowRgn function.
  /// </summary>
  WS_EX_TRANSPARENT = 0x00000020,
  /// <summary>
  /// Creates a multiple-document interface (MDI) child window.
  /// </summary>
  WS_EX_MDICHILD = 0x00000040,
  /// <summary>
  /// Creates a tool window; that is, a window intended to be used as a floating toolbar.
  /// A tool window has a title bar that is shorter than a normal title bar, and the
  /// window title is drawn using a smaller font. A tool window does not appear in the
  /// taskbar or in the dialog that appears when the user presses ALT+TAB. If a tool window
  /// has a system menu, its icon is not displayed on the title bar. However, you can
  /// display the system menu by right-clicking or by typing ALT+SPACE. 
  /// </summary>
  WS_EX_TOOLWINDOW = 0x00000080,
  /// <summary>
  /// Specifies that a window has a border with a raised edge.
  /// </summary>
  WS_EX_WINDOWEDGE = 0x00000100,
  /// <summary>
  /// Specifies that a window has a border with a sunken edge.
  /// </summary>
  WS_EX_CLIENTEDGE = 0x00000200,
  /// <summary>
  /// Includes a question mark in the title bar of the window. When the user clicks the
  /// question mark, the cursor changes to a question mark with a pointer. If the user
  /// then clicks a child window, the child receives a WM_HELP message. The child window
  /// should pass the message to the parent window procedure, which should call the
  /// WinHelp function using the HELP_WM_HELP command. The Help application displays a
  /// pop-up window that typically contains help for the child window. WS_EX_CONTEXTHELP
  /// cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
  /// </summary>
  WS_EX_CONTEXTHELP = 0x00000400,
  /// <summary>
  /// The window has generic "right-aligned" properties. This depends on the window class.
  /// This style has an effect only if the shell language is Hebrew, Arabic, or another
  /// language that supports reading-order alignment; otherwise, the style is ignored.
  /// Using the WS_EX_RIGHT style for static or edit controls has the same effect as
  /// using the SS_RIGHT or ES_RIGHT style, respectively. Using this style with button
  /// controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles. 
  /// </summary>
  WS_EX_RIGHT = 0x00001000,
  /// <summary>
  /// Creates a window that has generic left-aligned properties. This is the default.
  /// </summary>
  WS_EX_LEFT = 0x00000000,
  /// <summary>
  /// If the shell language is Hebrew, Arabic, or another language that supports
  /// reading-order alignment, the window text is displayed using right-to-left
  /// reading-order properties. For other languages, the style is ignored.
  /// </summary>
  WS_EX_RTLREADING = 0x00002000,
  /// <summary>
  /// The window text is displayed using left-to-right reading-order properties. This is
  /// the default.
  /// </summary>
  WS_EX_LTRREADING = 0x00000000,
  /// <summary>
  /// If the shell language is Hebrew, Arabic, or another language that supports reading
  /// order alignment, the vertical scroll bar (if present) is to the left of the client
  /// area. For other languages, the style is ignored.
  /// </summary>
  WS_EX_LEFTSCROLLBAR = 0x00004000,
  /// <summary>
  /// Vertical scroll bar (if present) is to the right of the client area. This is the default.
  /// </summary>
  WS_EX_RIGHTSCROLLBAR = 0x00000000,
  /// <summary>
  /// The window itself contains child windows that should take part in dialog box
  /// navigation. If this style is specified, the dialog manager recurses into children
  /// of this window when performing navigation operations such as handling the TAB key,
  /// an arrow key, or a keyboard mnemonic.
  /// </summary>
  WS_EX_CONTROLPARENT = 0x00010000,
  /// <summary>
  /// Creates a window with a three-dimensional border style intended to be used for items
  /// that do not accept user input.
  /// </summary>
  WS_EX_STATICEDGE = 0x00020000,
  /// <summary>
  /// Forces a top-level window onto the taskbar when the window is visible.
  /// </summary>
  WS_EX_APPWINDOW = 0x00040000,
  /// <summary>
  /// Combines the WS_EX_CLIENTEDGE and WS_EX_WINDOWEDGE styles.
  /// </summary>
  WS_EX_OVERLAPPEDWINDOW = 0x00000300,
  /// <summary>
  /// Combines the WS_EX_WINDOWEDGE, WS_EX_TOOLWINDOW, and WS_EX_TOPMOST styles.
  /// </summary>
  WS_EX_PALETTEWINDOW = 0x00000188,
  /// <summary>
  /// Windows 2000/XP: Creates a layered window. Note that this cannot be used for child
  /// windows. Also, this cannot be used if the window has a class style of either
  /// CS_OWNDC or CS_CLASSDC. 
  /// </summary>
  WS_EX_LAYERED = 0x00080000,
  /// <summary>
  /// Windows 2000/XP: A window created with this style does not pass its window layout to
  /// its child windows. Disable inheritence of mirroring by children.
  /// </summary>
  WS_EX_NOINHERITLAYOUT = 0x00100000,
  /// <summary>
  /// Arabic and Hebrew versions of Windows 98/Me, Windows 2000/XP: Creates a window whose
  /// horizontal origin is on the right edge. Increasing horizontal values advance to the
  /// left (Right to left mirroring).
  /// </summary>
  WS_EX_LAYOUTRTL = 0x00400000,
  /// <summary>
  /// Windows XP: Paints all descendants of a window in bottom-to-top painting order using
  /// double-buffering. For more information, see Remarks. This cannot be used if the window
  /// has a class style of either CS_OWNDC or CS_CLASSDC. 
  /// </summary>
  WS_EX_COMPOSITED = 0x02000000,
  /// <summary>
  /// Windows 2000/XP: A top-level window created with this style does not become the
  /// foreground window when the user clicks it. The system does not bring this window
  /// to the foreground when the user minimizes or closes the foreground window. To activate
  /// the window, use the SetActiveWindow or SetForegroundWindow function. The window does
  /// not appear on the taskbar by default. To force the window to appear on the taskbar,
  /// use the WS_EX_APPWINDOW style.
  /// </summary>
  WS_EX_NOACTIVATE = 0x08000000
 }
 //**************************************************************************************
 /// <summary>
 /// Перечисление стилей класса окна Windows.
 /// </summary>
 [Flags]
 public enum ClassStyles : uint
 {
  /// <summary>
  /// Redraws the entire window if a movement or size adjustment changes the height of the
  /// client area.
  /// </summary>
  CS_VREDRAW = 0x0001,
  /// <summary>
  /// Redraws the entire window if a movement or size adjustment changes the width of the 
  /// client area.
  /// </summary>
  CS_HREDRAW = 0x0002,
  /// <summary>
  /// Sends a double-click message to the window procedure when the user double-clicks
  /// the mouse while the cursor is within a window belonging to the class. 
  /// </summary>
  CS_DBLCLKS = 0x0008,
  /// <summary>
  /// Allocates a unique device context for each window in the class.
  /// </summary>
  CS_OWNDC = 0x0020,
  /// <summary>
  /// Allocates one device context to be shared by all windows in the class. Because window
  /// classes are process specific, it is possible for multiple threads of an application
  /// to create a window of the same class. It is also possible for the threads to attempt
  /// to use the device context simultaneously. When this happens, the system allows only
  /// one thread to successfully finish its drawing operation. 
  /// </summary>
  CS_CLASSDC = 0x0040,
  /// <summary>
  /// Sets the clipping rectangle of the child window to that of the parent window so that
  /// the child can draw on the parent. A window with the CS_PARENTDC style bit receives a
  /// regular device context from the system's cache of device contexts. It does not give
  /// the child the parent's device context or device context settings. Specifying
  /// CS_PARENTDC enhances an application's performance. 
  /// </summary>
  CS_PARENTDC = 0x0080,
  /// <summary>
  /// Disables Close on the window menu.
  /// </summary>
  CS_NOCLOSE = 0x0200,
  /// <summary>
  /// Saves, as a bitmap, the portion of the screen image obscured by a window of this class.
  /// When the window is removed, the system uses the saved bitmap to restore the screen
  /// image, including other windows that were obscured. Therefore, the system does not
  /// send WM_PAINT messages to windows that were obscured if the memory used by the bitmap
  /// has not been discarded and if other screen actions have not invalidated the stored image.
  /// This style is useful for small windows (for example, menus or dialog boxes) that are
  /// displayed briefly and then removed before other screen activity takes place. This style
  /// increases the time required to display the window, because the system must first allocate
  /// memory to store the bitmap.
  /// </summary>
  CS_SAVEBITS = 0x0800,
  /// <summary>
  /// Aligns the window's client area on a byte boundary (in the x direction). This style
  /// affects the width of the window and its horizontal placement on the display.
  /// </summary>
  CS_BYTEALIGNCLIENT = 0x1000,
  /// <summary>
  /// Aligns the window on a byte boundary (in the x direction). This style affects the
  /// width of the window and its horizontal placement on the display.
  /// </summary>
  CS_BYTEALIGNWINDOW = 0x2000,
  /// <summary>
  /// Specifies that the window class is an application global class. For more information,
  /// see Application Global Classes.
  /// </summary>
  CS_GLOBALCLASS = 0x4000,
  /// <summary>
  /// 
  /// </summary>
  CS_IME = 0x00010000,
  /// <summary>
  /// Windows XP: Enables the drop shadow effect on a window. The effect is turned on and
  /// off through SPI_SETDROPSHADOW. Typically, this is enabled for small, short-lived windows
  /// such as menus to emphasize their Z order relationship to other windows.
  /// </summary>
  CS_DROPSHADOW = 0x00020000
 }
 //**************************************************************************************
 /// <summary>
 /// Сообщения Windows
 /// </summary>
 public struct WM
 {
#pragma warning disable 1591
  public const uint NULL = 0x0000;
  public const uint CREATE = 0x0001;
  public const uint DESTROY = 0x0002;
  public const uint MOVE = 0x0003;
  public const uint SIZE = 0x0005;
  public const uint ACTIVATE = 0x0006;
  public const uint SETFOCUS = 0x0007;
  public const uint KILLFOCUS = 0x0008;
  public const uint ENABLE = 0x000A;
  public const uint SETREDRAW = 0x000B;
  public const uint SETTEXT = 0x000C;
  public const uint GETTEXT = 0x000D;
  public const uint GETTEXTLENGTH = 0x000E;
  public const uint PAINT = 0x000F;
  public const uint CLOSE = 0x0010;
  public const uint QUERYENDSESSION = 0x0011;
  public const uint QUERYOPEN = 0x0013;
  public const uint ENDSESSION = 0x0016;
  public const uint QUIT = 0x0012;
  public const uint ERASEBKGND = 0x0014;
  public const uint SYSCOLORCHANGE = 0x0015;
  public const uint SHOWWINDOW = 0x0018;
  public const uint WININICHANGE = 0x001A;
  public const uint SETTINGCHANGE = 0x001A;
  public const uint DEVMODECHANGE = 0x001B;
  public const uint ACTIVATEAPP = 0x001C;
  public const uint FONTCHANGE = 0x001D;
  public const uint TIMECHANGE = 0x001E;
  public const uint CANCELMODE = 0x001F;
  public const uint SETCURSOR = 0x0020;
  public const uint MOUSEACTIVATE = 0x0021;
  public const uint CHILDACTIVATE = 0x0022;
  public const uint QUEUESYNC = 0x0023;
  public const uint GETMINMAXINFO = 0x0024;
  public const uint PAINTICON = 0x0026;
  public const uint ICONERASEBKGND = 0x0027;
  public const uint NEXTDLGCTL = 0x0028;
  public const uint SPOOLERSTATUS = 0x002A;
  public const uint DRAWITEM = 0x002B;
  public const uint MEASUREITEM = 0x002C;
  public const uint DELETEITEM = 0x002D;
  public const uint VKEYTOITEM = 0x002E;
  public const uint CHARTOITEM = 0x002F;
  public const uint SETFONT = 0x0030;
  public const uint GETFONT = 0x0031;
  public const uint SETHOTKEY = 0x0032;
  public const uint GETHOTKEY = 0x0033;
  public const uint QUERYDRAGICON = 0x0037;
  public const uint COMPAREITEM = 0x0039;
  public const uint GETOBJECT = 0x003D;
  public const uint COMPACTING = 0x0041;
  /// <summary>
  /// no longer suported
  /// </summary>
  public const uint COMMNOTIFY = 0x0044;
  public const uint WINDOWPOSCHANGING = 0x0046;
  public const uint WINDOWPOSCHANGED = 0x0047;
  public const uint POWER = 0x0048;
  public const uint COPYDATA = 0x004A;
  public const uint CANCELJOURNAL = 0x004B;
  public const uint NOTIFY = 0x004E;
  public const uint INPUTLANGCHANGEREQUEST = 0x0050;
  public const uint INPUTLANGCHANGE = 0x0051;
  public const uint TCARD = 0x0052;
  public const uint HELP = 0x0053;
  public const uint USERCHANGED = 0x0054;
  public const uint NOTIFYFORMAT = 0x0055;
  public const uint CONTEXTMENU = 0x007B;
  public const uint STYLECHANGING = 0x007C;
  public const uint STYLECHANGED = 0x007D;
  public const uint DISPLAYCHANGE = 0x007E;
  public const uint GETICON = 0x007F;
  public const uint SETICON = 0x0080;
  public const uint NCCREATE = 0x0081;
  public const uint NCDESTROY = 0x0082;
  public const uint NCCALCSIZE = 0x0083;
  public const uint NCHITTEST = 0x0084;
  public const uint NCPAINT = 0x0085;
  public const uint NCACTIVATE = 0x0086;
  public const uint GETDLGCODE = 0x0087;
  public const uint SYNCPAINT = 0x0088;
  public const uint NCMOUSEMOVE = 0x00A0;
  public const uint NCLBUTTONDOWN = 0x00A1;
  public const uint NCLBUTTONUP = 0x00A2;
  public const uint NCLBUTTONDBLCLK = 0x00A3;
  public const uint NCRBUTTONDOWN = 0x00A4;
  public const uint NCRBUTTONUP = 0x00A5;
  public const uint NCRBUTTONDBLCLK = 0x00A6;
  public const uint NCMBUTTONDOWN = 0x00A7;
  public const uint NCMBUTTONUP = 0x00A8;
  public const uint NCMBUTTONDBLCLK = 0x00A9;
  public const uint NCXBUTTONDOWN = 0x00AB;
  public const uint NCXBUTTONUP = 0x00AC;
  public const uint NCXBUTTONDBLCLK = 0x00AD;
  public const uint INPUT = 0x00FF;
  public const uint KEYFIRST = 0x0100;
  public const uint KEYDOWN = 0x0100;
  public const uint KEYUP = 0x0101;
  public const uint CHAR = 0x0102;
  public const uint DEADCHAR = 0x0103;
  public const uint SYSKEYDOWN = 0x0104;
  public const uint SYSKEYUP = 0x0105;
  public const uint SYSCHAR = 0x0106;
  public const uint SYSDEADCHAR = 0x0107;
  public const uint KEYLAST = 0x0108;
  public const uint UNICHAR = 0x0109;
  public const uint IME_STARTCOMPOSITION = 0x010D;
  public const uint IME_ENDCOMPOSITION = 0x010E;
  public const uint IME_COMPOSITION = 0x010F;
  public const uint IME_KEYLAST = 0x010F;
  public const uint INITDIALOG = 0x0110;
  public const uint COMMAND = 0x0111;
  public const uint SYSCOMMAND = 0x0112;
  public const uint TIMER = 0x0113;
  public const uint HSCROLL = 0x0114;
  public const uint VSCROLL = 0x0115;
  public const uint INITMENU = 0x0116;
  public const uint INITMENUPOPUP = 0x0117;
  public const uint MENUSELECT = 0x011F;
  public const uint MENUCHAR = 0x0120;
  public const uint ENTERIDLE = 0x0121;
  public const uint MENURBUTTONUP = 0x0122;
  public const uint MENUDRAG = 0x0123;
  public const uint MENUGETOBJECT = 0x0124;
  public const uint UNINITMENUPOPUP = 0x0125;
  public const uint MENUCOMMAND = 0x0126;
  public const uint CHANGEUISTATE = 0x0127;
  public const uint UPDATEUISTATE = 0x0128;
  public const uint QUERYUISTATE = 0x0129;
  public const uint CTLCOLORMSGBOX = 0x0132;
  public const uint CTLCOLOREDIT = 0x0133;
  public const uint CTLCOLORLISTBOX = 0x0134;
  public const uint CTLCOLORBTN = 0x0135;
  public const uint CTLCOLORDLG = 0x0136;
  public const uint CTLCOLORSCROLLBAR = 0x0137;
  public const uint CTLCOLORSTATIC = 0x0138;
  public const uint MN_GETHMENU = 0x01E1;
  public const uint MOUSEFIRST = 0x0200;
  public const uint MOUSEMOVE = 0x0200;
  public const uint LBUTTONDOWN = 0x0201;
  public const uint LBUTTONUP = 0x0202;
  public const uint LBUTTONDBLCLK = 0x0203;
  public const uint RBUTTONDOWN = 0x0204;
  public const uint RBUTTONUP = 0x0205;
  public const uint RBUTTONDBLCLK = 0x0206;
  public const uint MBUTTONDOWN = 0x0207;
  public const uint MBUTTONUP = 0x0208;
  public const uint MBUTTONDBLCLK = 0x0209;
  public const uint MOUSEWHEEL = 0x020A;
  public const uint XBUTTONDOWN = 0x020B;
  public const uint XBUTTONUP = 0x020C;
  public const uint XBUTTONDBLCLK = 0x020D;
  public const uint MOUSELAST = 0x020D;
  public const uint PARENTNOTIFY = 0x0210;
  public const uint ENTERMENULOOP = 0x0211;
  public const uint EXITMENULOOP = 0x0212;
  public const uint NEXTMENU = 0x0213;
  public const uint SIZING = 0x0214;
  public const uint CAPTURECHANGED = 0x0215;
  public const uint MOVING = 0x0216;
  public const uint POWERBROADCAST = 0x0218;
  public const uint DEVICECHANGE = 0x0219;
  public const uint MDICREATE = 0x0220;
  public const uint MDIDESTROY = 0x0221;
  public const uint MDIACTIVATE = 0x0222;
  public const uint MDIRESTORE = 0x0223;
  public const uint MDINEXT = 0x0224;
  public const uint MDIMAXIMIZE = 0x0225;
  public const uint MDITILE = 0x0226;
  public const uint MDICASCADE = 0x0227;
  public const uint MDIICONARRANGE = 0x0228;
  public const uint MDIGETACTIVE = 0x0229;
  public const uint MDISETMENU = 0x0230;
  public const uint ENTERSIZEMOVE = 0x0231;
  public const uint EXITSIZEMOVE = 0x0232;
  public const uint DROPFILES = 0x0233;
  public const uint MDIREFRESHMENU = 0x0234;
  public const uint IME_SETCONTEXT = 0x0281;
  public const uint IME_NOTIFY = 0x0282;
  public const uint IME_CONTROL = 0x0283;
  public const uint IME_COMPOSITIONFULL = 0x0284;
  public const uint IME_SELECT = 0x0285;
  public const uint IME_CHAR = 0x0286;
  public const uint IME_REQUEST = 0x0288;
  public const uint IME_KEYDOWN = 0x0290;
  public const uint IME_KEYUP = 0x0291;
  public const uint MOUSEHOVER = 0x02A1;
  public const uint MOUSELEAVE = 0x02A3;
  public const uint NCMOUSEHOVER = 0x02A0;
  public const uint NCMOUSELEAVE = 0x02A2;
  public const uint WTSSESSION_CHANGE = 0x02B1;
  public const uint TABLET_FIRST = 0x02c0;
  public const uint TABLET_LAST = 0x02df;
  public const uint CUT = 0x0300;
  public const uint COPY = 0x0301;
  public const uint PASTE = 0x0302;
  public const uint CLEAR = 0x0303;
  public const uint UNDO = 0x0304;
  public const uint RENDERFORMAT = 0x0305;
  public const uint RENDERALLFORMATS = 0x0306;
  public const uint DESTROYCLIPBOARD = 0x0307;
  public const uint DRAWCLIPBOARD = 0x0308;
  public const uint PAINTCLIPBOARD = 0x0309;
  public const uint VSCROLLCLIPBOARD = 0x030A;
  public const uint SIZECLIPBOARD = 0x030B;
  public const uint ASKCBFORMATNAME = 0x030C;
  public const uint CHANGECBCHAIN = 0x030D;
  public const uint HSCROLLCLIPBOARD = 0x030E;
  public const uint QUERYNEWPALETTE = 0x030F;
  public const uint PALETTEISCHANGING = 0x0310;
  public const uint PALETTECHANGED = 0x0311;
  public const uint HOTKEY = 0x0312;
  public const uint PRINT = 0x0317;
  public const uint PRINTCLIENT = 0x0318;
  public const uint APPCOMMAND = 0x0319;
  public const uint THEMECHANGED = 0x031A;
  public const uint HANDHELDFIRST = 0x0358;
  public const uint HANDHELDLAST = 0x035F;
  public const uint AFXFIRST = 0x0360;
  public const uint AFXLAST = 0x037F;
  public const uint PENWINFIRST = 0x0380;
  public const uint PENWINLAST = 0x038F;
  public const uint APP = 0x8000;
  public const uint USER = 0x0400;
  public const uint REFLECT = USER + 0x1C00;
#pragma warning restore 1591
 }                                                          

}
#pragma warning restore 1591
