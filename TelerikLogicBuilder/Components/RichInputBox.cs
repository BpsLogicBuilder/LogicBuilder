using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Native;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    //Modified From code written by Mav.Northwind (CodeProject)
    internal partial class RichInputBox : RichTextBox
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        public RichInputBox(IExceptionHelper exceptionHelper, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            //12/2006 Calling component sets DetectUrls property
            InitializeComponent();

            this.BackColor = ForeColorUtility.GetTextBoxBackColor(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);
            this.ForeColor = ForeColorUtility.GetTextBoxForeColor(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);

            Telerik.WinControls.ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.Disposed += RichInputBox_Disposed;
            //the font will be replaced (links erased) on theme changes if it hasn't been set
            this.Font = new Font(this.Font, this.Font.Style);
        }

        //12/2006 char array constant to distinguish between variables and functions
        #region Constants
        internal static readonly char[] BOUNDARYTEXTARRAY = new char[] { Strings.constructorVisibleTextBegin[0], Strings.constructorVisibleTextEnd[0], Strings.functionVisibleTextBegin[0], Strings.functionVisibleTextEnd[0], Strings.variableVisibleTextBegin[0], Strings.variableVisibleTextEnd[0] };
        #endregion Constants

        //12/2006 added fields to facilitate component functionality
        #region Variables
        private int selectionLengthOnKeyDown;
        private int suspendEventsRequested;
        private int suspendEventsSelectionStart;
        private int suspendEventsSelectionLength;
        private bool reverseSelection;
        private int charIndexOnMouseDown;
        private bool suspendTextChangedEvent;
        private bool denySpecialCharacters;
        #endregion Variables

        #region Properties
        [DefaultValue(false)]
        public new bool DetectUrls
        {
            get { return base.DetectUrls; }
            set { base.DetectUrls = value; }
        }
        //12/2006 new property
        [DefaultValue(false)]
        public new bool HideSelection
        {
            get { return base.HideSelection; }
            set { base.HideSelection = value; }
        }

        //12/2006 new property
        internal bool DenySpecialCharacters
        {
            get { return denySpecialCharacters; }
            set { denySpecialCharacters = value; }
        }
        #endregion Properties

        #region Interop-Defines
        [StructLayout(LayoutKind.Sequential)]
        private struct CHARFORMAT2_STRUCT
        {
            public UInt32 cbSize;
            public UInt32 dwMask;
            public UInt32 dwEffects;
            public Int32 yHeight;
            public Int32 yOffset;
            public Int32 crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szFaceName;
            public UInt16 wWeight;
            public UInt16 sSpacing;
            public int crBackColor; // Color.ToArgb() -> int
            public int lcid;
            public int dwReserved;
            public Int16 sStyle;
            public Int16 wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
            public byte bReserved1;
        }

        //12/2006 removed unused constants and added a few
        private const int WM_USER = 0x0400;
        private const int EM_GETCHARFORMAT = WM_USER + 58;
        private const int EM_SETCHARFORMAT = WM_USER + 68;
        private const int WM_SETREDRAW = 0x000B;
        private const int EM_GETEVENTMASK = (WM_USER + 59);
        private const int EM_SETEVENTMASK = (WM_USER + 69);
        private const int REDRAW_ON = 0x0001;

        private const int SCF_SELECTION = 0x0001;
        //private const int SCF_WORD = 0x0002;
        //private const int SCF_ALL = 0x0004;

        #region CHARFORMAT2 Flags
        private const UInt32 CFE_LINK = 0x0020;
        private const UInt32 CFM_LINK = 0x00000020;
        #endregion
        #endregion Interop-Defines

        #region Methods
        //12/2006 new method
        private IntPtr SuspendEvents()
        {
            suspendEventsRequested++;
            if (suspendEventsRequested > 1)
                return IntPtr.Zero;
            if (suspendEventsRequested < 1)
                throw _exceptionHelper.CriticalException("{38DDFDE4-2664-4BDD-B121-8A271D76A13D}");

            this.SuspendLayout();
            NativeMethods.SendMessage(this.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
            IntPtr eventMask = NativeMethods.SendMessage(this.Handle, EM_GETEVENTMASK, IntPtr.Zero, IntPtr.Zero);
            suspendEventsSelectionStart = this.SelectionStart;
            suspendEventsSelectionLength = this.SelectionLength;
            return eventMask;
        }

        //12/2006 new method
        private void ResumeEvents(IntPtr eventMask)
        {
            suspendEventsRequested--;
            if (suspendEventsRequested > 0)
                return;
            if (suspendEventsRequested < 0)
                throw _exceptionHelper.CriticalException("{118F2DC2-4BF3-4359-AC14-7CD6FAD108C5}");

            NativeMethods.SendMessage(this.Handle, EM_SETEVENTMASK, IntPtr.Zero, eventMask);
            if (reverseSelection)
                this.Select(suspendEventsSelectionStart + suspendEventsSelectionLength, -suspendEventsSelectionLength);
            else
                this.Select(suspendEventsSelectionStart, suspendEventsSelectionLength);
            NativeMethods.SendMessage(this.Handle, WM_SETREDRAW, new IntPtr(REDRAW_ON), IntPtr.Zero);
            this.ResumeLayout();
            this.Invalidate();
        }

        //12/2006 new method
        private void SetSelectionAsLinkBoundary()
        {
            if (this.SelectionLength != 1) return;
            this.SelectionProtected = false;
            FontStyle fontStyle = this.SelectionFont.Style;
            if (!this.SelectionFont.Underline)
                fontStyle |= FontStyle.Underline;
            
            this.SelectionFont = new Font(this.SelectionFont, fontStyle);
            this.SelectionColor = ForeColorUtility.GetLinkBoundaryColor(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);
            this.SelectionProtected = true;
        }

        //12/2006 new method
        private void ResetSelectionToNormal()
        {
            this.SelectionProtected = false;
            FontStyle fontStyle = this.SelectionFont.Style;
            if (this.SelectionFont.Underline)
                fontStyle &= ~FontStyle.Underline;

            this.SelectionFont = new Font(this.SelectionFont, fontStyle);
            this.SelectionColor = ForeColorUtility.GetTextBoxForeColor(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);
        }

        //12/2006 new method
        /// <summary>
        /// Insert a given text into the RichTextBox at the current insert position.
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        internal void InsertText(string text)
        {
            InsertText(text, this.SelectionStart);
        }

        //12/2006 new method
        /// <summary>
        /// Insert a given text at a given position 
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="position">Insert position</param>
        internal void InsertText(string text, int position)
        {
            if (position < 0 || position > this.Text.Length)
                throw new ArgumentOutOfRangeException(nameof(position));

            if (text == null)
                throw _exceptionHelper.CriticalException("{4FC2BBC9-DD1B-47D6-A1B3-A6EB1EE62847}");

            if (this.SelectionProtected)
                return;

            if (this.denySpecialCharacters)
            {
                text = text.Replace(FileConstants.DIRECTORYSEPARATOR, "&#92;");
                text = text.Replace("{", "&#123;");
                text = text.Replace("}", "&#125;");
            }

            text = text.Replace("&#13;", "\r");
            text = text.Replace("&#10;", "\n");

            this.SelectionStart = position;
            ResetSelectionToNormal();
            this.SelectedText = text;
            position += text.Length;
            this.Select(position, 0);
        }

        //12/2006 modified to distinguish between functions and variables
        /// <summary>
        /// Insert a given text at at the current input position as a link.
        /// The link text is followed by a hash (#) and the given hyperlink text, both of
        /// them invisible.
        /// When clicked on, the whole link text and hyperlink string are given in the
        /// LinkClickedEventArgs.
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
        internal void InsertLink(string text, string hyperlink, LinkType linkType)
        {
            InsertLink(text, hyperlink, this.SelectionStart, linkType);
        }

        //12/2006 modified to distinguish between functions and variables
        /// <summary>
        /// Insert a given text at a given position as a link. The link text is followed by
        /// a hash (#) and the given hyperlink text, both of them invisible.
        /// When clicked on, the whole link text and hyperlink string are given in the
        /// LinkClickedEventArgs.
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
        /// <param name="position">Insert position</param>
        internal void InsertLink(string text, string hyperlink, int position, LinkType linkType)
        {
            if (position < 0 || position > this.Text.Length)
                throw new ArgumentOutOfRangeException(nameof(position));

            if (text == null)
                throw _exceptionHelper.CriticalException("{6F22EA06-EACA-460F-87CE-A53ADA336C41}");

            if (hyperlink == null)
                throw _exceptionHelper.CriticalException("{AF87B840-0CEC-47EC-8FAD-BD20EB2252EF}");

            if (text.Length == 0)
                throw _exceptionHelper.CriticalException("{EB3A0D41-A255-4FE2-8ACE-BA7F128C3CAC}");

            if (hyperlink.Length == 0)
                throw _exceptionHelper.CriticalException("{F4CFF9D9-3042-423F-A0F2-3EB07162D9EC}");

            if (this.SelectionProtected)
                return;

            text = text.Replace(FileConstants.DIRECTORYSEPARATOR, "&#92;");
            hyperlink = hyperlink.Replace(FileConstants.DIRECTORYSEPARATOR, "&#92;");
            text = text.Replace("{", "&#123;");
            hyperlink = hyperlink.Replace("{", "&#123;");
            text = text.Replace("}", "&#125;");
            hyperlink = hyperlink.Replace("}", "&#125;");

            text = text.Replace("\r", "&#13;");
            hyperlink = hyperlink.Replace("\r", "&#13;");
            text = text.Replace("\n", "&#10;");
            hyperlink = hyperlink.Replace("\n", "&#10;");

            //Suspend Text Changed Event
            suspendTextChangedEvent = true;

            this.SelectionStart = position;
            this.SelectedText = GetVisibleTextBegin(linkType);
            this.Select(position, 1);
            SetSelectionAsLinkBoundary();

            position++;
            this.Select(position, 0);
            ResetSelectionToNormal();
            this.SelectedRtf = @"{\rtf1\ansi " + text + @"\v " + hyperlink + @"\v0}";
            this.Select(position, text.Length + hyperlink.Length);
            this.SetSelectionLink(true);
            this.SelectionProtected = true;

            position = position + text.Length + hyperlink.Length;
            this.Select(position, 0);
            ResetSelectionToNormal();
            this.SelectedText = GetVisibleTextEnd(linkType);
            this.Select(position, 1);
            SetSelectionAsLinkBoundary();
            position++;
            this.Select(position, 0);
            ResetSelectionToNormal();

            //Cancel Suspend Text Changed Event
            suspendTextChangedEvent = false;
            base.OnTextChanged(new EventArgs());
        }

        private string GetVisibleTextBegin(LinkType linkType)
        {
            return linkType switch
            {
                LinkType.Constructor => Strings.constructorVisibleTextBegin,
                LinkType.Function => Strings.functionVisibleTextBegin,
                LinkType.Variable => Strings.variableVisibleTextBegin,
                _ => throw _exceptionHelper.CriticalException("{93081791-F473-4564-A914-F19178E21A8A}"),
            };
        }

        private string GetVisibleTextEnd(LinkType linkType)
        {
            return linkType switch
            {
                LinkType.Constructor => Strings.constructorVisibleTextEnd,
                LinkType.Function => Strings.functionVisibleTextEnd,
                LinkType.Variable => Strings.variableVisibleTextEnd,
                _ => throw _exceptionHelper.CriticalException("{8054E7D4-BCA5-49E0-8F96-06CF34EFB0F4}"),
            };
        }

        /// <summary>
        /// Set the current selection's link style
        /// </summary>
        /// <param name="link">true: set link style, false: clear link style</param>
        private void SetSelectionLink(bool link)
        {
            SetSelectionStyle(CFM_LINK, link ? CFE_LINK : 0);
        }

        private void SetSelectionStyle(UInt32 mask, UInt32 effect)
        {
            CHARFORMAT2_STRUCT cf = new();
            cf.cbSize = (UInt32)Marshal.SizeOf(cf);
            cf.dwMask = mask;
            cf.dwEffects = effect;

            IntPtr wpar = new(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);

            NativeMethods.SendMessage(Handle, EM_SETCHARFORMAT, wpar, lpar);

            Marshal.FreeCoTaskMem(lpar);
        }

        /// <summary>
        /// Get the link style for the current selection
        /// </summary>
        /// <returns>0: link style not set, 1: link style set, -1: mixed</returns>
        private int GetSelectionLink()
        {
            return GetSelectionStyle(CFM_LINK, CFE_LINK);
        }

        private int GetSelectionStyle(UInt32 mask, UInt32 effect)
        {
            CHARFORMAT2_STRUCT cf = new();
            cf.cbSize = (UInt32)Marshal.SizeOf(cf);
            cf.szFaceName = new char[32];

            IntPtr wpar = new(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);

            NativeMethods.SendMessage(Handle, EM_GETCHARFORMAT, wpar, lpar);

            cf = (CHARFORMAT2_STRUCT)Marshal.PtrToStructure(lpar, typeof(CHARFORMAT2_STRUCT))!;/*lpar has a value - line 385*/

            int state;
            // dwMask holds the information which properties are consistent throughout the selection:
            if ((cf.dwMask & mask) == mask)
            {
                if ((cf.dwEffects & effect) == effect)
                    state = 1;
                else
                    state = 0;
            }
            else
            {
                state = -1;
            }

            Marshal.FreeCoTaskMem(lpar);
            return state;
        }

        //12/2006 new method
        private string GetVisibleLinkText(int position)
        {
            this.Select(position, 1);
            if (!this.SelectionProtected)
            {
                this.Select(position, 0);
                return string.Empty;
            }
            if (IsSelectionBoundaryPosition())
            {
                this.Select(position, 0);
                return string.Empty;
            }

            int start = GetFirstCharacterPosition(position);
            int finish = GetLastVisibleCharacterPosition(position);
            this.Select(finish + 1, 0);

            if (start == finish)
            {
                return string.Empty;
            }
            else
            {
                return this.Text.Substring(start, finish - start + 1);
            }
        }

        //12/2006 new method
        private int GetFirstCharacterPosition(int position)
        {
            this.Select(position, 1);
            while (!IsSelectionBoundaryPosition())
            {
                position--;
                this.Select(position, 1);
            }
            return position + 1;
        }

        //12/2006 new method
        private int GetLastVisibleCharacterPosition(int position)
        {
            this.Select(position, 1);
            while (this.SelectionType != (RichTextBoxSelectionTypes.Text | RichTextBoxSelectionTypes.MultiChar) && !IsSelectionBoundaryPosition())
            {
                position++;
                this.Select(position, 1);
            }
            return position - 1;
        }

        //12/2006 new method
        private int GetFirstHiddenCharacterPosition(int position)
        {
            this.Select(position, 1);
            while (this.SelectionType != (RichTextBoxSelectionTypes.Text | RichTextBoxSelectionTypes.MultiChar) && !IsSelectionBoundaryPosition())
            {
                position++;
                this.Select(position, 1);
            }
            if (IsSelectionBoundaryPosition())
            {
                this.Select(position, 0);
                return -1;
            }
            else
            {
                this.Select(position, 0);
                return position;
            }
        }

        //12/2006 new method
        private string GetHiddenLinkDocumentElement(int position)
        {
            string hiddenXml = GetHiddenLinkText(position);
            if (hiddenXml.Length == 0)
                return string.Empty;

            string elementName = _xmlDocumentHelpers.ToXmlElement(hiddenXml).Name;
            if (!new HashSet<string> 
                    {
                        XmlDataConstants.CONSTRUCTORELEMENT, 
                        XmlDataConstants.FUNCTIONELEMENT, 
                        XmlDataConstants.VARIABLEELEMENT 
                    }.Contains(elementName)
               )
            {
                throw _exceptionHelper.CriticalException("{6BC89391-9331-4468-918F-1D08F6DB79EC}");
            }

            return elementName;
        }

        //12/2006 new method
        internal string GetHiddenLinkText(int position)
        {
            IntPtr eventMask = this.SuspendEvents();

            this.Select(position, 0);
            if (!this.SelectionProtected)
            {
                this.ResumeEvents(eventMask);
                return string.Empty;
            }

            this.Select(position, 1);
            if (IsSelectionBoundaryPosition())
            {
                this.ResumeEvents(eventMask);
                return string.Empty;
            }

            LinkBoundaries? linkBoundaries = GetBoundary(position);
            if (linkBoundaries == null)
            {//selection is protected and not a boundary position so boundary cannot be null.
             //Also the position passed in should have been LinkBoundaries.Start + 1
                throw _exceptionHelper.CriticalException("{6E57FA50-9B06-4EAA-91F2-B2B56D4515F0}");
            }

            int start = GetFirstHiddenCharacterPosition(position);
            int finish = linkBoundaries.Finish - 1;
            this.Select(finish + 1, 0);

            if (start == -1 || start > finish)
            {
                this.ResumeEvents(eventMask);
                return string.Empty;
            }
            else
            {
                this.ResumeEvents(eventMask);
                return this.Text.Substring(start, finish - start + 1);
            }

        }

        //12/2006 new method
        private bool IsSelectionBoundaryPosition()
        {
            if (this.SelectionLength > 1) return false;
            if (this.SelectionType != RichTextBoxSelectionTypes.Empty
                && this.SelectionType != (RichTextBoxSelectionTypes.Text | RichTextBoxSelectionTypes.MultiChar)
                && this.SelectionProtected
                && new HashSet<char>(BOUNDARYTEXTARRAY).Contains(this.SelectedText[0])
                && this.GetSelectionLink() == 0
                && this.SelectionFont.Underline)
                return true;
            else
                return false;
        }

        //12/2006 new method
        private List<LinkBoundaries> GetBoundaryPositions()
        {
            string richText = this.Text;
            string[] stringArray = richText.Split(BOUNDARYTEXTARRAY);
            List<int> boundaryPositions = new();
            List<LinkBoundaries> boundaries = new();
            if (stringArray.Length < 2)
            {
                return boundaries;
            }

            int pos = -1;
            for (int i = 0; i < stringArray.Length; i++)
            {
                pos += stringArray[i].Length + 1;
                if (i != (stringArray.Length - 1))
                {
                    this.Select(pos, 1);
                    if (IsSelectionBoundaryPosition())
                        boundaryPositions.Add(pos);
                }
            }

            if (boundaryPositions.Count % 2 != 0)
                throw _exceptionHelper.CriticalException("{D8CABB27-CF80-48D9-BCFC-F1B418BE50E4}");

            for (int i = 0; i < boundaryPositions.Count; i++)
            {
                if (i % 2 != 0)
                    boundaries.Add(new LinkBoundaries(boundaryPositions[i - 1], boundaryPositions[i]));
            }

            return boundaries;
        }

        //12/2006 new method
        internal string GetMixedXml()
        {
            IntPtr eventMask = this.SuspendEvents();

            StringBuilder mixedXmlBuilder = new();
            using XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(mixedXmlBuilder);

            List<LinkBoundaries> boundaries = GetBoundaryPositions();
            if (boundaries.Count == 0)
            {
                this.ResumeEvents(eventMask);
                xmlTextWriter.WriteString(this.Text);
                return mixedXmlBuilder.ToString();
            }

            for (int i = 0; i < boundaries.Count; i++)
            {
                if (i == 0)
                    xmlTextWriter.WriteString(this.Text[..boundaries[i].Start]);
                else
                    xmlTextWriter.WriteString(this.Text.Substring(boundaries[i - 1].Finish + 1, boundaries[i].Start - boundaries[i - 1].Finish - 1));

                xmlTextWriter.WriteRaw(this.GetHiddenLinkText(boundaries[i].Start + 1));

                if (i == boundaries.Count - 1)
                    xmlTextWriter.WriteString(this.Text[(boundaries[i].Finish + 1)..]);
            }

            this.ResumeEvents(eventMask);

            return mixedXmlBuilder.ToString();
        }

        //12/2006 new method
        internal string GetVisibleText()
        {
            IntPtr eventMask = this.SuspendEvents();

            List<LinkBoundaries> boundaries = GetBoundaryPositions();
            if (boundaries.Count == 0)
            {
                this.ResumeEvents(eventMask);
                return this.Text;
            }

            StringBuilder stringBuilder = new();

            for (int i = 0; i < boundaries.Count; i++)
            {
                if (i == 0)
                    stringBuilder.Append(this.Text[..boundaries[i].Start]);
                else
                    stringBuilder.Append(this.Text.AsSpan(boundaries[i - 1].Finish + 1, boundaries[i].Start - boundaries[i - 1].Finish - 1));

                string hiddenDocumentElement = GetHiddenLinkDocumentElement(boundaries[i].Start + 1);
                switch (hiddenDocumentElement)
                {
                    case XmlDataConstants.CONSTRUCTORELEMENT:
                        stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", Strings.constructorVisibleTextBegin, this.GetVisibleLinkText(boundaries[i].Start + 1), Strings.constructorVisibleTextEnd));
                        break;
                    case XmlDataConstants.FUNCTIONELEMENT:
                        stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", Strings.functionVisibleTextBegin, this.GetVisibleLinkText(boundaries[i].Start + 1), Strings.functionVisibleTextEnd));
                        break;
                    case XmlDataConstants.VARIABLEELEMENT:
                        stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", Strings.variableVisibleTextBegin, this.GetVisibleLinkText(boundaries[i].Start + 1), Strings.variableVisibleTextEnd));
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{8054E7D4-BCA5-49E0-8F96-06CF34EFB0F4}");
                        throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{B2DEED96-BCEB-4F81-9BC9-6035322EDB01}"));
                }

                if (i == boundaries.Count - 1)
                    stringBuilder.Append(this.Text[(boundaries[i].Finish + 1)..]);
            }

            this.ResumeEvents(eventMask);

            return stringBuilder.ToString();
        }

        //12/2006 new method
        internal LinkBoundaries? GetBoundary(int position)
        {
            IntPtr eventMask = this.SuspendEvents();
            List<LinkBoundaries> boundaries = GetBoundaryPositions();

            for (int i = 0; i < boundaries.Count; i++)
            {
                if (position > boundaries[i].Start && position < boundaries[i].Finish)
                {
                    this.Select(position, 0);
                    this.ResumeEvents(eventMask);
                    return boundaries[i];
                }
            }
            this.ResumeEvents(eventMask);
            return null;
        }

        //12/2006 new method
        private bool InBoundary(int position, List<LinkBoundaries> boundaries)
        {
            for (int i = 0; i < boundaries.Count; i++)
            {
                if (position >= boundaries[i].Start && position <= boundaries[i].Finish)
                {
                    this.Select(position, 0);
                    return true;
                }
            }
            return false;
        }

        //12/2006 new method
        internal bool LinkInSelection()
        {
            IntPtr eventMask = this.SuspendEvents();
            int start = this.SelectionStart;
            int finish = start + this.SelectionLength - 1;

            List<LinkBoundaries> boundaries = GetBoundaryPositions();
            

            for (int i = 0; i < boundaries.Count; i++)
            {
                if ((boundaries[i].Start >= start && boundaries[i].Start <= finish) || (boundaries[i].Finish >= start && boundaries[i].Finish <= finish))
                {
                    this.ResumeEvents(eventMask);
                    return true;
                }
            }
            this.ResumeEvents(eventMask);
            return false;
        }

        //12/2006 new method
        private bool SelectionContainsLink(List<LinkBoundaries> boundaries)
        {
            int start = this.SelectionStart;
            int finish = start + this.SelectionLength - 1;

            for (int i = 0; i < boundaries.Count; i++)
            {
                if (start < boundaries[i].Start && start < boundaries[i].Finish && finish > boundaries[i].Start && finish > boundaries[i].Finish)
                {
                    return true;
                }
            }
            return false;
        }

        //12/2006 new method
        private static bool IsFinishBoundary(int position, List<LinkBoundaries> boundaries)
        {
            for (int i = 0; i < boundaries.Count; i++)
            {
                if (position == boundaries[i].Finish)
                    return true;
            }
            return false;
        }

        //12/2006 new method
        private static bool IsStartBoundary(int position, List<LinkBoundaries> boundaries)
        {
            for (int i = 0; i < boundaries.Count; i++)
            {
                if (position == boundaries[i].Start)
                    return true;
            }
            return false;
        }

        //12/2006 new method
        internal bool IsSelectionEligibleForLink()
        {
            IntPtr eventMask = this.SuspendEvents();

            int start = this.SelectionStart;
            int finish = start + this.SelectionLength - 1;
            bool eligible;
            if (this.SelectionLength == 0 && this.SelectionStart == 0)
            {
                eligible = true;
            }
            else if (this.SelectionLength == 0)
            {
                int position = this.SelectionStart;
                this.Select(position - 1, 1);
                bool beforeCharacterIsBoundary = IsSelectionBoundaryPosition();
                this.Select(position, 1);
                bool afterCharacterIsBoundary = IsSelectionBoundaryPosition();
                this.Select(position, 0);
                if (!this.SelectionProtected)
                    eligible = true;
                else if (beforeCharacterIsBoundary && afterCharacterIsBoundary)
                    eligible = true;
                else
                    eligible = false;
            }
            else
            {
                LinkBoundaries boundaryPair = new(start, finish);
                List<LinkBoundaries> boundaries = GetBoundaryPositions();
                if (boundaries.Contains(boundaryPair))
                    eligible = true;
                else if ((!InBoundary(start, boundaries) || IsStartBoundary(start, boundaries)) && (!InBoundary(finish, boundaries) || IsFinishBoundary(finish, boundaries)))
                    eligible = true;
                else if (this.SelectionProtected || SelectionContainsLink(boundaries) || InBoundary(start, boundaries) || InBoundary(finish, boundaries))
                    eligible = false;
                else
                    eligible = true;
            }
            this.ResumeEvents(eventMask);
            return eligible;
        }

        //7/2022 new method
        private void ResetNonLinkTextOnThemeChange()
        {
            IntPtr eventMask = this.SuspendEvents();
            List<LinkBoundaries> boundaries = GetBoundaryPositions();
            if (boundaries.Count == 0)
            {
                //edge case: this.Text.Length == 1
                this.Select(0, this.Text.Length);
                ResetSelectionToNormal();
                this.ResumeEvents(eventMask);
                return;
            }

            for (int i = 0; i < boundaries.Count; i++)
            {
                this.Select(boundaries[i].Start, 1);
                SetSelectionAsLinkBoundary();

                if (i == 0)
                {
                    //edge case: boundaries[i].Start == 1
                    this.Select(0, boundaries[i].Start);
                    ResetSelectionToNormal();
                }
                else
                {
                    //edge case: boundaries[i - 1].Finish == 49, boundaries[i].Start == 51
                    this.Select(boundaries[i - 1].Finish + 1, boundaries[i].Start - boundaries[i - 1].Finish - 1);
                    ResetSelectionToNormal();
                }

                if (i == boundaries.Count - 1)
                {
                    //edge case: boundaries[i].Finish == 49, Text.Length = 51
                    if (boundaries[i].Finish < this.Text.Length - 1)
                    {
                        this.Select(boundaries[i].Finish + 1, this.Text.Length - boundaries[i].Finish - 1);
                        ResetSelectionToNormal();
                    }
                }

                this.Select(boundaries[i].Finish, 1);
                SetSelectionAsLinkBoundary();
            }

            this.ResumeEvents(eventMask);
        }
        #endregion Methods

        #region EventHandlers
        //12/2006 new event handler
        protected override void OnMouseUp(MouseEventArgs e)
        {
            reverseSelection = (this.GetCharIndexFromPosition(e.Location) < charIndexOnMouseDown);
            base.OnMouseUp(e);
        }

        //12/2006 new event handler
        protected override void OnMouseDown(MouseEventArgs e)
        {
            charIndexOnMouseDown = this.GetCharIndexFromPosition(e.Location);
            base.OnMouseDown(e);
        }

        //12/2006 new event handler
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Home || e.KeyCode == Keys.PageUp)
                reverseSelection = !(this.SelectionLength < selectionLengthOnKeyDown);

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down || e.KeyCode == Keys.End || e.KeyCode == Keys.PageDown)
                reverseSelection = (this.SelectionLength < selectionLengthOnKeyDown);

            base.OnKeyUp(e);
        }

        //12/2006 new event handler
        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            bool removeFormatting = false;
            if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Control | Keys.V:
                        removeFormatting = true;
                        break;
                    case Keys.Shift | Keys.Insert:
                        removeFormatting = true;
                        break;
                    default:
                        break;
                }
            }

            if (removeFormatting && Clipboard.GetDataObject() != null)
            {
                bool dataPresent = Clipboard.GetDataObject().GetDataPresent(DataFormats.Text, true);
                if (dataPresent)
                {
                    object dataObject = Clipboard.GetDataObject().GetData(DataFormats.Text, true);
                    Clipboard.SetDataObject(dataObject);

                    if (this.denySpecialCharacters)
                    {
                        string data = dataObject.ToString()!;
                        data = data.Replace(FileConstants.DIRECTORYSEPARATOR, "&#92;");
                        data = data.Replace("{", "&#123;");
                        data = data.Replace("}", "&#125;");
                        Clipboard.SetDataObject(data);
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //12/2006 new event handler
        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.selectionLengthOnKeyDown = this.SelectionLength;

            if (e.Control && (e.KeyCode == Keys.V || e.KeyCode == Keys.C || e.KeyCode == Keys.X || e.KeyCode == Keys.Insert) && LinkInSelection())
            {
                e.Handled = true;
            }
            else if (e.Control && (e.KeyCode == Keys.V ||
                                    e.KeyCode == Keys.C ||
                                    e.KeyCode == Keys.X ||
                                    e.KeyCode == Keys.Insert ||
                                    e.KeyCode == Keys.Up ||
                                    e.KeyCode == Keys.Down ||
                                    e.KeyCode == Keys.Home ||
                                    e.KeyCode == Keys.End))
            {
            }
            else if (e.Control)
            {
                e.Handled = true;
            }

            if (e.Shift && (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Insert) && !IsSelectionEligibleForLink())
                e.Handled = true;

            if (e.Alt)
                e.Handled = true;

            if (this.denySpecialCharacters && e.Shift == false && e.KeyCode == Keys.OemPipe)//backslash
                e.SuppressKeyPress = true;

            if (this.denySpecialCharacters && e.Shift && (e.KeyCode == Keys.OemOpenBrackets || e.KeyCode == Keys.OemCloseBrackets))//curly braces
                e.SuppressKeyPress = true;

            IntPtr eventMask = this.SuspendEvents();
            if (this.SelectionLength == 0 && this.SelectionStart == 0 && this.Text.Length == 0)
            {
                ResetSelectionToNormal();
            }
            else if (this.SelectionLength == 0 && this.SelectionStart == 0)
            {
                this.Select(0, 1);
                bool firstCharacterIsBoundary = IsSelectionBoundaryPosition();
                this.Select(0, 0);
                if (firstCharacterIsBoundary)
                {
                    ResetSelectionToNormal();
                }
            }
            else if (this.SelectionLength == 0)
            {
                int position = this.SelectionStart;
                this.Select(position - 1, 1);
                bool beforeCharacterIsBoundary = IsSelectionBoundaryPosition();
                this.Select(position, 1);
                bool afterCharacterIsBoundary = IsSelectionBoundaryPosition();
                this.Select(position, 0);

                if (beforeCharacterIsBoundary && afterCharacterIsBoundary)
                {
                    ResetSelectionToNormal();
                }
            }
            this.ResumeEvents(eventMask);
            base.OnKeyDown(e);
        }

        //12/2006 new event handler
        protected override void OnTextChanged(EventArgs e)
        {
            if (suspendTextChangedEvent)
                return;

            base.OnTextChanged(e);
        }

        private void RichInputBox_Disposed(object? sender, EventArgs e)
        {
            Telerik.WinControls.ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, Telerik.WinControls.ThemeChangedEventArgs args)
        {
            this.BackColor = ForeColorUtility.GetTextBoxBackColor(args.ThemeName);
            this.ForeColor = ForeColorUtility.GetTextBoxForeColor(args.ThemeName);
            ResetNonLinkTextOnThemeChange();
        }
        #endregion EventHandlers
    }
}
