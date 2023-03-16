using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Constants
{
    internal static class PerFontSizeConstants
    {
        public static float BoundarySize
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 20F;

                Dictionary<int, float> sizes = new()
                {
                    [ThemeCollections.NINE] = 20F,
                    [ThemeCollections.TEN] = 20F,
                    [ThemeCollections.ELEVEN] = 20F,
                    [ThemeCollections.TWELVE] = 20F,
                    [ThemeCollections.THIRTEEN] = 20F,
                    [ThemeCollections.FOURTEEN] = 20F,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int CommandButtonWidth
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 28;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 28,
                    [ThemeCollections.TEN] = 30,
                    [ThemeCollections.ELEVEN] = 33,
                    [ThemeCollections.TWELVE] = 36,
                    [ThemeCollections.THIRTEEN] = 38,
                    [ThemeCollections.FOURTEEN] = 40,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static Padding GroupBoxPadding
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return new Padding(2, 18, 2, 2);

                Dictionary<int, Padding> sizes = new()
                {
                    [ThemeCollections.NINE] = new Padding(2, 18, 2, 2),
                    [ThemeCollections.TEN] = new Padding(2, 18, 2, 2),
                    [ThemeCollections.ELEVEN] = new Padding(2, 18, 2, 2),
                    [ThemeCollections.TWELVE] = new Padding(2, 18, 2, 2),
                    [ThemeCollections.THIRTEEN] = new Padding(2, 25, 2, 2),
                    [ThemeCollections.FOURTEEN] = new Padding(2, 25, 2, 2),
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static float MultiLineHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 100F;

                Dictionary<int, float> sizes = new()
                {
                    [ThemeCollections.NINE] = 100F,
                    [ThemeCollections.TEN] = 105F,
                    [ThemeCollections.ELEVEN] = 110F,
                    [ThemeCollections.TWELVE] = 115F,
                    [ThemeCollections.THIRTEEN] = 120F,
                    [ThemeCollections.FOURTEEN] = 125F,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static float SeparatorLineHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 4F;

                Dictionary<int, float> sizes = new()
                {
                    [ThemeCollections.NINE] = 8F,
                    [ThemeCollections.TEN] = 8F,
                    [ThemeCollections.ELEVEN] = 8F,
                    [ThemeCollections.TWELVE] = 10F,
                    [ThemeCollections.THIRTEEN] = 12F,
                    [ThemeCollections.FOURTEEN] = 14F,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static float SingleLineHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 28F;

                Dictionary<int, float> sizes = new()
                {
                    [ThemeCollections.NINE] = 28F,
                    [ThemeCollections.TEN] = 30F,
                    [ThemeCollections.ELEVEN] = 33F,
                    [ThemeCollections.TWELVE] = 36F,
                    [ThemeCollections.THIRTEEN] = 38F,
                    [ThemeCollections.FOURTEEN] = 40F,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }
    }
}
