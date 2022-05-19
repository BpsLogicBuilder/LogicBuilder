using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Constants
{
    internal struct AnchorConstants
    {
        internal static AnchorStyles AnchorsLeftTopRight => AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
        internal static AnchorStyles AnchorsLeftTop => AnchorStyles.Left | AnchorStyles.Top;
        internal static AnchorStyles AnchorsLeftTopRightBottom => AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
        internal static AnchorStyles AnchorsTopRightBottom => AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
        internal static AnchorStyles AnchorsLeftRightBottom => AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        internal static AnchorStyles AnchorsLeftRight => AnchorStyles.Left | AnchorStyles.Right;
        internal static AnchorStyles AnchorsTopRight => AnchorStyles.Top | AnchorStyles.Right;
        internal static AnchorStyles AnchorsRightBottom => AnchorStyles.Bottom | AnchorStyles.Right;
        internal static AnchorStyles AnchorsLeftBottom => AnchorStyles.Left | AnchorStyles.Bottom;
        internal static AnchorStyles AnchorsLeftTopBottom => AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
    }
}
