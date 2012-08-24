using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Win32 {

    /// <summary>
    ///   The Console class can change several attributes of your console window.
    /// </summary>
    /// <remarks></remarks>
    public class ConsoleEx {

        /// <summary>Enumerates all available colors for the forecolor or the backcolor of the console.</summary>
        public enum Colors : int {
            /// <summary>Black</summary>
            Black = 0,
            /// <summary>Red</summary>
            Red = 1,
            /// <summary>Light red</summary>
            LightRed = 2,
            /// <summary>Green</summary>
            Green = 3,
            /// <summary>Light green</summary>
            LightGreen = 4,
            /// <summary>Blue</summary>
            Blue = 5,
            /// <summary>Light blue</summary>
            LightBlue = 6,
            /// <summary>Gold</summary>
            Gold = 7,
            /// <summary>Yellow</summary>
            Yellow = 8,
            /// <summary>Cyan</summary>
            Cyan = 9,
            /// <summary>Light cyan</summary>
            LightCyan = 10,
            /// <summary>Purple</summary>
            Purple = 11,
            /// <summary>Light purple</summary>
            LightPurple = 12,
            /// <summary>Gray</summary>
            Gray = 13,
            /// <summary>White</summary>
            White = 14
        }

        // Private constants


        private const int InvalidHandleValue = -1;
        /// <summary>Standard output handle.</summary>
        private const int StandardOutputHandle = -11;
        /// <summary>Standard input handle.</summary>
        private const int StandardInputHandle = -10;
        /// <summary>Characters read by the ReadFile or ReadConsole function are written to the active screen buffer as they are read. This mode can be used only if the ENABLE_LINE_INPUT mode is also enabled.</summary>

        private const int EnableEchoInput = 0x4;
        //Private variables

        /// <summary>Holds the forecolor of the console window.</summary>
        private static Colors m_foreColor = Colors.Gray;
        /// <summary>Holds the backcolor of the console window.</summary>
        private static Colors m_backColor = Colors.Black;
        /// <summary>Holds the value of the CursorVisible property.</summary>
        private static bool m_cursorVisible = true;
        /// <summary>Holds the value of the OvrMode property.</summary>
        private static bool m_overwriteMode = false;
        /// <summary>Holds the value of the EchoInput property.</summary>

        private static bool m_echoInput = true;
        /// <summary>
        /// Lists all the possible background color values.
        /// </summary>
        private static int[] m_backgroundColors = {
			0x0,
			0x40,
			0x80 | 0x40,
			0x20,
			0x80 | 0x20,
			0x10,
			0x80 | 0x10,
			0x40 | 0x20,
			0x80 | 0x40 | 0x20,
			0x20 | 0x10,
			0x80 | 0x20 | 0x10,
			0x40 | 0x10,
			0x80 | 0x40 | 0x10,
			0x40 | 0x20 | 0x10,
			0x80 | 0x40 | 0x20 | 0x10
		};
        /// <summary>
        /// Lists all the possible foreground color values.
        /// </summary>
        private static int[] m_foregroundColors = {
			0x0,
			0x4,
			0x8 | 0x4,
			0x2,
			0x8 | 0x2,
			0x1,
			0x8 | 0x1,
			0x4 | 0x2,
			0x8 | 0x4 | 0x2,
			0x2 | 0x1,
			0x8 | 0x2 | 0x1,
			0x4 | 0x1,
			0x8 | 0x4 | 0x1,
			0x4 | 0x2 | 0x1,
			0x8 | 0x4 | 0x2 | 0x1

		};
        #region "Win32 Structures"

        /// <summary>
        /// The CONSOLE_CURSOR_INFO structure contains information about the console 
        /// cursor.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct ConsoleCursorInfo {
            /// <summary>Specifies a number between 1 and 100, indicating the percentage of the character cell that is filled by the cursor. The cursor appearance varies, ranging from completely filling the cell to showing up as a horizontal line at the bottom of the cell.</summary>
            public int Size;
            /// <summary>Specifies the visibility of the cursor. If the cursor is visible, this member is TRUE (nonzero).</summary>
            public int Visible;
        }

        /// <summary>
        ///   The COORD structure defines the coordinates of a character cell in a 
        ///   console screen buffer. The origin of the coordinate system (0,0) is at 
        ///   the top, left cell of the buffer.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct Coord {
            /// <summary>Horizontal or column value.</summary>
            public short X;
            /// <summary>Vertical or row value.</summary>
            public short Y;
        }

        /// <summary>
        /// The SMALL_RECT structure defines the coordinates of the upper left and lower right corners of a rectangle.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct SmallRectangle {
            /// <summary>Specifies the x-coordinate of the upper left corner of the rectangle.</summary>
            public short Left;
            /// <summary>Specifies the y-coordinate of the upper left corner of the rectangle.</summary>
            public short Top;
            /// <summary>Specifies the x-coordinate of the lower right corner of the rectangle.</summary>
            public short Right;
            /// <summary>Specifies the y-coordinate of the lower right corner of the rectangle.</summary>
            public short Bottom;
        }

        ///' <summary>
        ///' The ConsoleScreenBufferInfo structure contains information about a console screen buffer.
        ///' </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct ScreenBufferInfo {
            /// <summary>
            ///   Specifies the size, in character columns and rows, of the screen buffer.
            /// </summary>
            public Coord Size;
            /// <summary>
            ///   Specifies the column and row coordinates of the cursor in the screen 
            ///   buffer.
            /// </summary>
            public Coord CursorPosition;
            /// <summary> 
            ///   Specifies the foreground (text) and background color attributes to be 
            ///   used for characters that are written to a screen buffer by the 
            ///   WriteFile and WriteConsole functions, or echoed to a screen buffer by 
            ///   the ReadFile and ReadConsole functions. The attribute values are some 
            ///   combination of the following values: 
            ///     FOREGROUND_BLUE, 
            ///     FOREGROUND_GREEN, 
            ///     FOREGROUND_RED, 
            ///     FOREGROUND_INTENSITY, 
            ///     BACKGROUND_BLUE, 
            ///     BACKGROUND_GREEN, 
            ///     BACKGROUND_RED, 
            ///     BACKGROUND_INTENSITY.
            /// </summary>
            public short Attributes;
            /// <summary>
            ///   Specifies a SMALL_RECT structure that contains the screen buffer 
            ///   coordinates of the upper-left and lower-right corners of the display 
            ///   window.
            /// </summary>
            public SmallRectangle Window;
            /// <summary>
            ///   Specifies the maximum size of the console window, given the current 
            ///   screen buffer size and font and the screen size.
            /// </summary>
            public Coord MaximumWindowSize;
        }
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        #endregion

        #region "Win32 API"

        /// <summary>
        /// The SetConsoleTextAttribute function sets the foreground (text) and background color attributes of characters written to the screen buffer by the WriteFile or WriteConsole function, or echoed by the ReadFile or ReadConsole function. This function affects only text written after the function call.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. The handle must have GENERIC_READ access.</param>
        /// <param name="wAttributes">Specifies the foreground and background color attributes. Any combination of the following values can be specified: FOREGROUND_BLUE, FOREGROUND_GREEN, FOREGROUND_RED, FOREGROUND_INTENSITY, BACKGROUND_BLUE, BACKGROUND_GREEN, BACKGROUND_RED, and BACKGROUND_INTENSITY.</param>
        /// <returns>If the function succeeds, the return value is nonzero.<br></br><br>If the function fails, the return value is zero. To get extended error information, call GetLastError.</br></returns>
        private static extern int SetConsoleTextAttribute(int hConsoleOutput, int wAttributes);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        /// <summary>
        /// The GetStdHandle function returns a handle for the standard input, standard output, or standard error device.
        /// </summary>
        /// <param name="nStdHandle">Specifies the device for which to return the handle. This parameter can have one of the following values:
        /// <list type="bullet"> 
        ///		<listheader>
        ///			<value>Value</value>
        ///			<meaning>Meaning</meaning>
        ///		</listheader>
        ///		<item>
        ///			<value>STD_INPUT_HANDLE</value>
        ///			<meaning>Standard input handle.</meaning>
        ///		</item>
        ///		<item>
        ///			<value>STD_OUTPUT_HANDLE</value>
        ///			<meaning>Standard output handle.</meaning>
        ///		</item>
        ///		<item>
        ///			<value>STD_ERROR_HANDLE</value>
        ///			<meaning>Standard error handle.</meaning>
        ///		</item>
        /// </list>
        /// </param>
        /// <returns>If the function succeeds, the return value is a handle to the specified device.<br></br><br>If the function fails, the return value is the INVALID_HANDLE_VALUE flag. To get extended error information, call GetLastError.</br></returns>
        private static extern int GetStdHandle(int nStdHandle);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        /// <summary>
        /// The SetConsoleCursorInfo function sets the size and visibility of the cursor for the specified console screen buffer.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. The handle must have GENERIC_WRITE access.</param>
        /// <param name="lpConsoleCursorInfo">Pointer to a CONSOLE_CURSOR_INFO structure containing the new specifications for the screen buffer's cursor.</param>
        /// <returns>If the function succeeds, the return value is nonzero.<br></br><br>If the function fails, the return value is zero. To get extended error information, call GetLastError.</br></returns>
        private static extern int SetConsoleCursorInfo(int hConsoleOutput, ref ConsoleCursorInfo lpConsoleCursorInfo);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        /// <summary>
        /// The GetConsoleMode function reports the current input mode of a console's input buffer or the current output mode of a console screen buffer.
        /// </summary>
        /// <param name="hConsoleHandle">Handle to a console input buffer or a screen buffer. The handle must have GENERIC_READ access.</param>
        /// <param name="lpConsoleCursorInfo">
        /// Pointer to a 32-bit variable that indicates the current mode of the specified buffer.<br>If the hConsoleHandle parameter is an input handle, the mode can be a combination of the following values. When a console is created, all input modes except ENABLE_WINDOW_INPUT are enabled by default.</br>
        /// <list type="bullet">
        /// 	<listheader>
        /// 		<value>Value</value>
        /// 		<meaning>Meaning</meaning>
        /// 	</listheader>
        /// 	<item>
        /// 		<value>ENABLE_LINE_INPUT</value>
        /// 		<meaning>The ReadFile or ReadConsole function returns only when a carriage return character is read. If this mode is disabled, the functions return when one or more characters are available.</meaning>
        /// 	</item>
        /// 	<item>
        /// 		<value>ENABLE_ECHO_INPUT</value>
        /// 		<meaning>Characters read by the ReadFile or ReadConsole function are written to the active screen buffer as they are read. This mode can be used only if the ENABLE_LINE_INPUT mode is also enabled.</meaning>
        /// 	</item>
        /// 	<item>
        /// 		<value>ENABLE_PROCESSED_INPUT</value>
        /// 		<meaning>ctrl+c is processed by the system and is not placed in the input buffer. If the input buffer is being read by ReadFile or ReadConsole, other control keys are processed by the system and are not returned in the ReadFile or ReadConsole buffer. If the ENABLE_LINE_INPUT mode is also enabled, backspace, carriage return, and linefeed characters are handled by the system.</meaning>
        /// 	</item>
        /// 	<item>
        /// 		<value>ENABLE_WINDOW_INPUT</value>
        /// 		<meaning>User interactions that change the size of the console screen buffer are reported in the console's input buffer. Information about these events can be read from the input buffer by applications using the ReadConsoleInput function, but not by those using ReadFile or ReadConsole.</meaning>
        /// 	</item>
        /// 	<item>
        /// 		<value>ENABLE_MOUSE_INPUT</value>
        /// 		<meaning>If the mouse pointer is within the borders of the console window and the window has the keyboard focus, mouse events generated by mouse movement and button presses are placed in the input buffer. These events are discarded by ReadFile or ReadConsole, even when this mode is enabled.</meaning>
        /// 	</item>
        /// </list>
        /// If the hConsoleHandle parameter is a screen buffer handle, the mode can be a combination of the following values. When a screen buffer is created, both output modes are enabled by default.
        /// <list type="bullet">
        /// 	<listheader>
        /// 		<value>Value</value>
        /// 		<meaning>Meaning</meaning>
        /// 	</listheader>
        /// 	<item>
        /// 		<value>ENABLE_PROCESSED_OUTPUT</value>
        /// 		<meaning>Characters written by the WriteFile or WriteConsole function or echoed by the ReadFile or ReadConsole function are parsed for ASCII control sequences, and the correct action is performed. Backspace, tab, bell, carriage return, and linefeed characters are processed.</meaning>
        /// 	</item>
        /// 	<item>
        /// 		<value>ENABLE_WRAP_AT_EOL_OUTPUT</value>
        /// 		<meaning>When writing with WriteFile or WriteConsole or echoing with ReadFile or ReadConsole, the cursor moves to the beginning of the next row when it reaches the end of the current row. This causes the rows displayed in the console window to scroll up automatically when the cursor advances beyond the last row in the window. It also causes the contents of the screen buffer to scroll up (discarding the top row of the screen buffer) when the cursor advances beyond the last row in the screen buffer. If this mode is disabled, the last character in the row is overwritten with any subsequent characters.</meaning>
        /// 	</item>
        /// </list>
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.<br></br><br>If the function fails, the return value is zero. To get extended error information, call GetLastError.</br></returns>
        private static extern int GetConsoleMode(int hConsoleHandle, ref int lpConsoleCursorInfo);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        /// <summary>
        /// The SetConsoleMode function sets the input mode of a console's input buffer or the output mode of a console screen buffer.
        /// </summary>
        /// <param name="hConsoleHandle">Handle to a console input buffer or a screen buffer. The handle must have GENERIC_WRITE access.</param>
        /// <param name="lpConsoleCursorInfo">
        /// Pointer to a 32-bit variable that indicates the current mode of the specified buffer.<br>If the hConsoleHandle parameter is an input handle, the mode can be a combination of the following values. When a console is created, all input modes except ENABLE_WINDOW_INPUT are enabled by default.</br>
        /// <list type="bullet">
        /// 	<listheader>
        /// 		<value>Value</value>
        /// 		<meaning>Meaning</meaning>
        /// 	</listheader>
        /// 	<item>
        /// 		<value>ENABLE_LINE_INPUT</value>
        /// 		<meaning>The ReadFile or ReadConsole function returns only when a carriage return character is read. If this mode is disabled, the functions return when one or more characters are available.</meaning>
        /// 	</item>
        /// 	<item>
        /// 		<value>ENABLE_ECHO_INPUT</value>
        /// 		<meaning>Characters read by the ReadFile or ReadConsole function are written to the active screen buffer as they are read. This mode can be used only if the ENABLE_LINE_INPUT mode is also enabled.</meaning>
        /// 	</item>
        /// 	<item>
        /// 		<value>ENABLE_PROCESSED_INPUT</value>
        /// 		<meaning>ctrl+c is processed by the system and is not placed in the input buffer. If the input buffer is being read by ReadFile or ReadConsole, other control keys are processed by the system and are not returned in the ReadFile or ReadConsole buffer. If the ENABLE_LINE_INPUT mode is also enabled, backspace, carriage return, and linefeed characters are handled by the system.</meaning>
        /// 	</item>
        /// 	<item>
        /// 		<value>ENABLE_WINDOW_INPUT</value>
        /// 		<meaning>User interactions that change the size of the console screen buffer are reported in the console's input buffer. Information about these events can be read from the input buffer by applications using the ReadConsoleInput function, but not by those using ReadFile or ReadConsole.</meaning>
        /// 	</item>
        /// 	<item>
        /// 		<value>ENABLE_MOUSE_INPUT</value>
        /// 		<meaning>If the mouse pointer is within the borders of the console window and the window has the keyboard focus, mouse events generated by mouse movement and button presses are placed in the input buffer. These events are discarded by ReadFile or ReadConsole, even when this mode is enabled.</meaning>
        /// 	</item>
        /// </list>
        /// If the hConsoleHandle parameter is a screen buffer handle, the mode can be a combination of the following values. When a screen buffer is created, both output modes are enabled by default.
        /// <list type="bullet">
        /// 	<listheader>
        /// 		<value>Value</value>
        /// 		<meaning>Meaning</meaning>
        /// 	</listheader>
        /// 	<item>
        /// 		<value>ENABLE_PROCESSED_OUTPUT</value>
        /// 		<meaning>Characters written by the WriteFile or WriteConsole function or echoed by the ReadFile or ReadConsole function are parsed for ASCII control sequences, and the correct action is performed. Backspace, tab, bell, carriage return, and linefeed characters are processed.</meaning>
        /// 	</item>
        /// 	<item>
        /// 		<value>ENABLE_WRAP_AT_EOL_OUTPUT</value>
        /// 		<meaning>When writing with WriteFile or WriteConsole or echoing with ReadFile or ReadConsole, the cursor moves to the beginning of the next row when it reaches the end of the current row. This causes the rows displayed in the console window to scroll up automatically when the cursor advances beyond the last row in the window. It also causes the contents of the screen buffer to scroll up (discarding the top row of the screen buffer) when the cursor advances beyond the last row in the screen buffer. If this mode is disabled, the last character in the row is overwritten with any subsequent characters.</meaning>
        /// 	</item>
        /// </list>
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.<br></br><br>If the function fails, the return value is zero. To get extended error information, call GetLastError.</br></returns>
        private static extern int SetConsoleMode(int hConsoleHandle, int lpConsoleCursorInfo);
        [DllImport("kernel32", EntryPoint = "SetConsoleTitleA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        /// <summary>
        /// The SetConsoleTitle function sets the title bar string for the current console window.
        /// </summary>
        /// <param name="lpConsoleTitle">Pointer to a null-terminated string that contains the string to appear in the title bar of the console window.</param>
        /// <returns>If the function succeeds, the return value is nonzero.<br></br><br>If the function fails, the return value is zero. To get extended error information, call GetLastError.</br></returns>
        private static extern int SetConsoleTitle(string lpConsoleTitle);
        [DllImport("kernel32", EntryPoint = "GetConsoleTitleA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        /// <summary>
        /// The GetConsoleTitle function retrieves the title bar string for the current console window.
        /// </summary>
        /// <param name="lpConsoleTitle">Pointer to a buffer that receives a null-terminated string containing the text that appears in the title bar of the console window.</param>
        /// <param name="nSize">Specifies the size, in characters, of the buffer pointed to by the lpConsoleTitle parameter.</param>
        /// <returns>If the function succeeds, the return value is the length, in characters, of the string copied to the buffer.<br></br><br>If the function fails, the return value is zero. To get extended error information, call GetLastError.</br></returns>
        private static extern int GetConsoleTitle(StringBuilder lpConsoleTitle, int nSize);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        /// <summary>
        /// The GetConsoleScreenBufferInfo function retrieves information about the specified console screen buffer.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. The handle must have GENERIC_READ access.</param>
        /// <param name="lpConsoleScreenBufferInfo">Pointer to a CONSOLE_SCREEN_BUFFER_INFO structure in which the screen buffer information is returned.</param>
        /// <returns>If the function succeeds, the return value is nonzero.<br></br><br>If the function fails, the return value is zero. To get extended error information, call GetLastError.</br></returns>
        private static extern int GetConsoleScreenBufferInfo(int hConsoleOutput, ref ScreenBufferInfo lpConsoleScreenBufferInfo);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        /// <summary>
        /// The SetConsoleCursorPosition function sets the cursor position in the specified console screen buffer.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. The handle must have GENERIC_WRITE access.</param>
        /// <param name="dwCursorPosition">Specifies a COORD structure containing the new cursor position. The coordinates are the column and row of a screen buffer character cell. The coordinates must be within the boundaries of the screen buffer.</param>
        /// <returns>If the function succeeds, the return value is nonzero.<br></br><br>If the function fails, the return value is zero. To get extended error information, call GetLastError.</br></returns>
        private static extern int SetConsoleCursorPosition(int hConsoleOutput, Coord dwCursorPosition);

        #endregion

        /// <summary>Gets or sets the color of the console font.</summary>
        /// <value>A value of the ConsoleColor enum that specifies the color of the console font.</value>
        public static Colors ForeColor {
            get { return m_foreColor; }
            set {
                m_foreColor = value;
                SetConsoleTextAttribute(GetHandle(), m_foregroundColors[(int)m_foreColor] | m_backgroundColors[(int)m_backColor]);
            }
        }

        /// <summary>Gets or sets the color of the console background.</summary>
        /// <value>A value of the ConsoleColor enum that specifies the color of the console background.</value>
        public static Colors BackColor {
            get { return m_backColor; }
            set {
                m_backColor = value;
                SetConsoleTextAttribute(GetHandle(), m_foregroundColors[(int)m_foreColor] | m_backgroundColors[(int)m_backColor]);
            }
        }


        public static void Cls() {
            for (int row = 0; row <= Window.Height; row++) {
                Cursor.Move(0, row);
                Console.Write(new string(' ', ConsoleEx.Window.Width));
            }
            Cursor.Move(0, 0);

        }

        /// <summary>Gets or sets whether the console must echo the input or not.</summary>
        /// <value>A boolean value that specifies the console must echo the input or not.</value>
        /// <remarks>EchoInput is often turned off when the program asks the user to type in a password.</remarks>
        public static bool Echo {
            get { return m_echoInput; }

            set {
                int result = 0;

                m_echoInput = value;
                GetConsoleMode(GetHandle(), ref result);
                if (m_echoInput) {
                    result = result | EnableEchoInput;
                } else {
                    result = result & -EnableEchoInput;
                }
                SetConsoleMode(GetHandle(), result);

            }
        }

        public static int QPrint(string text, int x, int y) {
            return QPrint(text, x, y, ConsoleEx.ForeColor, ConsoleEx.BackColor);
        }

        public static int QPrint(string text, int x, int y, Colors foreColor) {
            return QPrint(text, x, y, foreColor, ConsoleEx.BackColor);
        }

        ///-----------------------------------------------------------------------------
        /// <summary>
        ///   Allows for a single call to output text at a specific location and colors.
        /// </summary>
        /// <param name="text">Output text value.</param>
        /// <param name="x">Column value (0 based).</param>
        /// <param name="y">Row value (0 based).</param>
        /// <param name="foreColor">Foreground color.</param>
        /// <param name="backColor">Background color.</param>
        /// <returns>0=Success, other value failure.</returns>
        /// <remarks></remarks>
        /// <history>
        /// 	[Cory Smith] 	06/20/2003	Created
        /// </history>
        ///-----------------------------------------------------------------------------
        public static int QPrint(string text, int x, int y, Colors foreColor, Colors backColor) {
            ConsoleEx.Cursor.Move(x, y);
            ConsoleEx.ForeColor = foreColor;
            ConsoleEx.BackColor = backColor;
            Console.Write(text);
            return 0;
        }

        public static int QPrint(string text, bool centered, int y, Colors foreColor, Colors backColor) {
            return QPrint(text, (ConsoleEx.Window.Width - text.Length) / 2, y, foreColor, backColor);
        }

        public static int QPrint(string text, bool centered, int y, Colors foreColor) {
            return QPrint(text, centered, y, foreColor, ConsoleEx.BackColor);
        }

        public static int QPrint(string text, bool centered, int y) {
            return QPrint(text, centered, y, ConsoleEx.ForeColor, ConsoleEx.BackColor);
        }

        public static int QPrint(string text, bool centered) {
            return QPrint(text, centered, ConsoleEx.Cursor.Y, ConsoleEx.ForeColor, ConsoleEx.BackColor);
        }

        private static int GetHandle() {
            int handle = GetStdHandle(StandardOutputHandle);
            if (handle == InvalidHandleValue) {
                throw new Exception("GetStdHandle resulted in error #" + Marshal.GetLastWin32Error() + ".");
            } else {
                return handle;
            }
        }

        public class Cursor {

            /// <summary>Gets or sets whether the cursor is visible or not.</summary>
            /// <value>A boolean value that specifies the visibility of the cursor.</value>
            public static bool Visible {
                get { return ConsoleEx.m_cursorVisible; }
                set {
                    ConsoleEx.m_cursorVisible = value;
                    ChangeCursor();
                }
            }

            /// <summary>Gets or sets whether the cursor is in overwrite-mode or not.</summary>
            /// <value>A boolean value that specifies the mode of the cursor.</value>
            /// <remarks>In overwrite mode, the cursor size will be 50% of the character space instead of 25% in normal mode</remarks>
            public static bool OverwriteMode {
                get { return ConsoleEx.m_overwriteMode; }
                set {
                    ConsoleEx.m_overwriteMode = value;
                    ChangeCursor();
                }
            }

            /// <summary>Applies the current cursor settings.</summary>
            /// <remarks>This method applies changes in the CursorVisible and OvrMode properties.</remarks>
            private static void ChangeCursor() {
                ConsoleCursorInfo cci = default(ConsoleCursorInfo);
                cci.Visible = Convert.ToInt32(ConsoleEx.m_cursorVisible);
                if (ConsoleEx.m_overwriteMode) {
                    cci.Size = 50;
                } else {
                    cci.Size = 25;
                }
                ConsoleEx.SetConsoleCursorInfo(GetHandle(), ref cci);
            }

            /// <summary>Gets or sets the current cursos position on the x axis in the console.</summary>
            /// <value>A value that specifies the current cursos position on the x axis in the console.</value>
            public static int X {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.CursorPosition.X;
                }
                set {
                    if (value > short.MaxValue) {
                        throw new ArgumentException("Value must be less than or equal to " + short.MaxValue + ".");
                    }
                    Move(value, Cursor.Y);
                }
            }

            /// <summary>Gets or sets the current cursos position on the y axis in the console.</summary>
            /// <value>A value that specifies the current cursos position on the y axis in the console.</value>
            public static int Y {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.CursorPosition.Y;
                }
                set {
                    if (value > short.MaxValue) {
                        throw new ArgumentException("Value must be less than or equal to " + short.MaxValue + ".");
                    }
                    Move(Cursor.X, value);
                }
            }

            /// <summary>Moves the cursor to the specified location.</summary>
            /// <param name="x">Specifies the x value of the new location.</param>
            /// <param name="y">Specifies the y value of the new location.</param>

            public static void Move(int x, int y) {
                int result = 0;

                if (x > short.MaxValue) {
                    throw new ArgumentException("The value of x must be less than or equal to " + short.MaxValue + ".");
                } else if (y > short.MaxValue) {
                    throw new ArgumentException("The value of y must be less than or equal to " + short.MaxValue + ".");
                }

                Coord c = default(Coord);
                c.X = Convert.ToInt16(x);
                c.Y = Convert.ToInt16(y);

                result = ConsoleEx.SetConsoleCursorPosition(GetHandle(), c);
                if (result == 0) {
                    //throw new Exception("SetConsoleCursorPosition resulted in error #" + Marshal.GetLastWin32Error() + ".");
                }

            }

        }

        public class Window {

            /// <summary>Gets or sets the caption of the console.</summary>
            /// <value>A String that specifies the caption of the console.</value>
            public static string Text {
                get {
                    StringBuilder sb = new StringBuilder(256);
                    ConsoleEx.GetConsoleTitle(sb, 256);
                    return sb.ToString();
                }
                set { ConsoleEx.SetConsoleTitle(value); }
            }

            /// <summary>Gets the width (in characters) of the console window.</summary>
            /// <value>An integer that holds the width of the console window in characters.</value>
            public static int Width {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.Window.Right - sbi.Window.Left + 1;
                }
            }

            /// <summary>Gets the height (in characters) of the console window.</summary>
            /// <value>An integer that holds the height of the console window in characters.</value>
            public static int Height {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.Window.Bottom - sbi.Window.Top + 1;
                }
            }

            public static int SizeX {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.Size.X;
                }
            }

            public static int SizeY {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.Size.Y;
                }
            }

            public static int MaximumHeight {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.MaximumWindowSize.Y;
                }
            }

            public static int MaximumWidth {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.MaximumWindowSize.X;
                }
            }

            public static int Top {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.Window.Top;
                }
            }

            public static int Left {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.Window.Left;
                }
            }

            public static int Right {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.Window.Right;
                }
            }

            public static int Bottom {
                get {
                    ScreenBufferInfo sbi = new ScreenBufferInfo();
                    ConsoleEx.GetConsoleScreenBufferInfo(GetHandle(), ref sbi);
                    return sbi.Window.Bottom;
                }
            }

        }

    }

}
