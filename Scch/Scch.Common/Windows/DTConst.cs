namespace Scch.Common.Windows
{
    /// <summary>
    /// Const for DrawText.
    /// </summary>
    public static class DTConst
    {
        /// <summary>
        /// Aligns text to the left.
        /// </summary>
        public const int DT_LEFT = 0x00000000;
        /// <summary>
        /// Justifies the text to the top of the rectangle.
        /// </summary>
        public const int DT_TOP = 0x00000000;
        /// <summary>
        /// Centers text horizontally in the rectangle.
        /// </summary>
        public const int DT_CENTER = 0x00000001;
        /// <summary>
        /// Aligns text to the right.
        /// </summary>
        public const int DT_RIGHT = 0x00000002;
        /// <summary>
        /// Centers text vertically. This value is used only with the DT_SINGLELINE value.
        /// </summary>
        public const int DT_VCENTER = 0x00000004;
        /// <summary>
        /// Breaks words. Lines are automatically broken between words if a word would extend past the edge of the rectangle specified by the lpRect parameter. 
        /// A carriage return-line feed sequence also breaks the line. If this is not specified, output is on one line. 
        /// </summary>
        public const int DT_WORDBREAK = 0x00000010;
        /// <summary>
        /// Displays text on a single line only. Carriage returns and line feeds do not break the line.
        /// </summary>
        public const int DT_SINGLELINE = 0x00000020;
        /// <summary>
        /// Determines the width and height of the rectangle. If there are multiple lines of text, DrawText uses the width of the rectangle pointed to by the lpRect 
        /// parameter and extends the base of the rectangle to bound the last line of text. If the largest word is wider than the rectangle, the width is expanded. 
        /// If the text is less than the width of the rectangle, the width is reduced. If there is only one line of text, DrawText modifies the right side of the 
        /// rectangle so that it bounds the last character in the line. In either case, DrawText returns the height of the formatted text but does not draw the text.
        /// </summary>
        public const int DT_CALCRECT = 0x00000400;
        /// <summary>
        /// Turns off processing of prefix characters. Normally, DrawText interprets the mnemonic-prefix character &amp; as a directive to underscore the character 
        /// that follows, and the mnemonic-prefix characters &amp;&amp; as a directive to print a single &amp;. By specifying DT_NOPREFIX, this processing is turned off.
        /// </summary>
        public const int DT_NOPREFIX = 0x00000800;
        /// <summary>
        /// Duplicates the text-displaying characteristics of a multiline edit control. Specifically, the average character width is calculated in the same manner as for an edit control, and the function does not display a partially visible last line.
        /// </summary>
        public const int DT_EDITCONTROL = 0x00002000;
        /// <summary>
        /// For displayed text, if the end of a string does not fit in the rectangle, it is truncated and ellipses are added. If a word that is not at the end of the string goes beyond the limits of the rectangle, it is truncated without ellipses. 
        /// The string is not modified unless the DT_MODIFYSTRING flag is specified. Compare with DT_PATH_ELLIPSIS and DT_WORD_ELLIPSIS.
        /// </summary>
        public const int DT_END_ELLIPSIS = 0x00008000;
        /// <summary>
        /// Layout in right-to-left reading order for bi-directional text when the font selected into the hdc is a Hebrew or Arabic font. The default reading order for all text is left-to-right.
        /// </summary>
        public const int DT_RTLREADING = 0x00020000;
        /// <summary>
        /// Ignores the ampersand (&amp;) prefix character in the text. The letter that follows will not be underlined, but other mnemonic-prefix characters are still processed.
        /// </summary>
        public const int DT_HIDEPREFIX = 0x00100000;
        /// <summary>
        /// Justifies the text to the bottom of the rectangle. This value is used only with the DT_SINGLELINE value.
        /// </summary>
        public const int DT_BOTTOM = 0x8;
    }
}
