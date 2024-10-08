﻿using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Constants
{
    internal static class PerFontSizeConstants
    {
        public static Size AddNewFileIconSize
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return new Size(76, 64);

                Dictionary<int, Size> sizes = new()
                {
                    [ThemeCollections.NINE] = new Size(76, 64),
                    [ThemeCollections.TEN] = new Size(76, 64),
                    [ThemeCollections.ELEVEN] = new Size(76, 64),
                    [ThemeCollections.TWELVE] = new Size(80, 67),
                    [ThemeCollections.THIRTEEN] = new Size(95, 70),
                    [ThemeCollections.FOURTEEN] = new Size(95, 70),
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static Padding AddUpdateItemGroupBoxPadding
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return new Padding(18, 24, 1, 12);

                Dictionary<int, Padding> sizes = new()
                {
                    [ThemeCollections.NINE] = new Padding(18, 24, 1, 9),
                    [ThemeCollections.TEN] = new Padding(18, 24, 1, 9),
                    [ThemeCollections.ELEVEN] = new Padding(18, 24, 1, 9),
                    [ThemeCollections.TWELVE] = new Padding(18, 26, 1, 12),
                    [ThemeCollections.THIRTEEN] = new Padding(18, 30, 1, 12),
                    [ThemeCollections.FOURTEEN] = new Padding(18, 30, 1, 12),
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int ApplicationGroupBoxHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 63;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 63,
                    [ThemeCollections.TEN] = 66,
                    [ThemeCollections.ELEVEN] = 69,
                    [ThemeCollections.TWELVE] = 75,
                    [ThemeCollections.THIRTEEN] = 81,
                    [ThemeCollections.FOURTEEN] = 87,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static Padding ApplicationGroupBoxPadding
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return new Padding(18, 24, 18, 12);

                Dictionary<int, Padding> sizes = new()
                {
                    [ThemeCollections.NINE] = new Padding(18, 24, 18, 12),
                    [ThemeCollections.TEN] = new Padding(18, 24, 18, 12),
                    [ThemeCollections.ELEVEN] = new Padding(18, 24, 18, 12),
                    [ThemeCollections.TWELVE] = new Padding(18, 26, 18, 15),
                    [ThemeCollections.THIRTEEN] = new Padding(18, 30, 18, 15),
                    [ThemeCollections.FOURTEEN] = new Padding(18, 30, 18, 15),
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static float BottomPanelHeight => (5 * SeparatorLineHeight) + (4 * SingleLineHeight);

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
                    return 35;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 35,
                    [ThemeCollections.TEN] = 38,
                    [ThemeCollections.ELEVEN] = 41,
                    [ThemeCollections.TWELVE] = 43,
                    [ThemeCollections.THIRTEEN] = 45,
                    [ThemeCollections.FOURTEEN] = 47,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int ConditionsRadioButtonPanelWidth
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 100;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 100,
                    [ThemeCollections.TEN] = 100,
                    [ThemeCollections.ELEVEN] = 100,
                    [ThemeCollections.TWELVE] = 120,
                    [ThemeCollections.THIRTEEN] = 120,
                    [ThemeCollections.FOURTEEN] = 120,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static Padding DropDownListControlPadding
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return new Padding(4, 5, 0, 0);

                Dictionary<int, Padding> sizes = new()
                {
                    [ThemeCollections.NINE] = new Padding(4, 5, 0, 0),
                    [ThemeCollections.TEN] = new Padding(4, 5, 0, 0),
                    [ThemeCollections.ELEVEN] = new Padding(4, 5, 0, 0),
                    [ThemeCollections.TWELVE] = new Padding(6, 6, 0, 0),
                    [ThemeCollections.THIRTEEN] = new Padding(6, 6, 1, 1),
                    [ThemeCollections.FOURTEEN] = new Padding(6, 6, 1, 1),
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
                    [ThemeCollections.NINE] = new Padding(2, 21, 2, 2),
                    [ThemeCollections.TEN] = new Padding(2, 21, 2, 2),
                    [ThemeCollections.ELEVEN] = new Padding(2, 21, 2, 2),
                    [ThemeCollections.TWELVE] = new Padding(2, 22, 2, 2),
                    [ThemeCollections.THIRTEEN] = new Padding(2, 29, 2, 2),
                    [ThemeCollections.FOURTEEN] = new Padding(2, 29, 2, 2),
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int FindInFilesFormMinimumHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 545;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 545,
                    [ThemeCollections.TEN] = 570,
                    [ThemeCollections.ELEVEN] = 582,
                    [ThemeCollections.TWELVE] = 610,
                    [ThemeCollections.THIRTEEN] = 618,
                    [ThemeCollections.FOURTEEN] = 626,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int FindInShapeOrCellFormMinimumHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 654;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 654,
                    [ThemeCollections.TEN] = 666,
                    [ThemeCollections.ELEVEN] = 678,
                    [ThemeCollections.TWELVE] = 690,
                    [ThemeCollections.THIRTEEN] = 690,
                    [ThemeCollections.FOURTEEN] = 690,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int FindReplaceInShapeOrCellFormMinimumHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 754;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 754,
                    [ThemeCollections.TEN] = 766,
                    [ThemeCollections.ELEVEN] = 778,
                    [ThemeCollections.TWELVE] = 790,
                    [ThemeCollections.THIRTEEN] = 790,
                    [ThemeCollections.FOURTEEN] = 790,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int FindShapeOrCellFormMinimumHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 270;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 270,
                    [ThemeCollections.TEN] = 300,
                    [ThemeCollections.ELEVEN] = 330,
                    [ThemeCollections.TWELVE] = 360,
                    [ThemeCollections.THIRTEEN] = 390,
                    [ThemeCollections.FOURTEEN] = 420,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static Padding ImageLabelPadding
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return new Padding(4, 6, 0, 0);

                Dictionary<int, Padding> sizes = new()
                {
                    [ThemeCollections.NINE] = new Padding(4, 7, 0, 0),
                    [ThemeCollections.TEN] = new Padding(4, 7, 0, 0),
                    [ThemeCollections.ELEVEN] = new Padding(4, 7, 0, 0),
                    [ThemeCollections.TWELVE] = new Padding(6, 8, 0, 0),
                    [ThemeCollections.THIRTEEN] = new Padding(6, 8, 0, 0),
                    [ThemeCollections.FOURTEEN] = new Padding(6, 8, 0, 0),
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static Padding InputControlPadding
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return new Padding(4, 6, 0, 0);

                Dictionary<int, Padding> sizes = new()
                {
                    [ThemeCollections.NINE] = new Padding(4, 7, 0, 0),
                    [ThemeCollections.TEN] = new Padding(4, 7, 0, 0),
                    [ThemeCollections.ELEVEN] = new Padding(4, 7, 0, 0),
                    [ThemeCollections.TWELVE] = new Padding(6, 8, 0, 0),
                    [ThemeCollections.THIRTEEN] = new Padding(6, 8, 0, 0),
                    [ThemeCollections.FOURTEEN] = new Padding(6, 8, 0, 0),
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int MultiLineAddUpdateItemGroupBoxHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 136;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 136,
                    [ThemeCollections.TEN] = 141,
                    [ThemeCollections.ELEVEN] = 148,
                    [ThemeCollections.TWELVE] = 155,
                    [ThemeCollections.THIRTEEN] = 165,
                    [ThemeCollections.FOURTEEN] = 175,
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

        public static int MultiLineTextGroupBoxHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 136;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 136,
                    [ThemeCollections.TEN] = 141,
                    [ThemeCollections.ELEVEN] = 148,
                    [ThemeCollections.TWELVE] = 155,
                    [ThemeCollections.THIRTEEN] = 165,
                    [ThemeCollections.FOURTEEN] = 175,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int OkCancelButtonPanelWidth
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 160;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 160,
                    [ThemeCollections.TEN] = 163,
                    [ThemeCollections.ELEVEN] = 166,
                    [ThemeCollections.TWELVE] = 176,
                    [ThemeCollections.THIRTEEN] = 181,
                    [ThemeCollections.FOURTEEN] = 185,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static Padding ParentGroupBoxPadding
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return new Padding(15, 18, 15, 2);

                Dictionary<int, Padding> sizes = new()
                {
                    [ThemeCollections.NINE] = new Padding(15, 21, 15, 2),
                    [ThemeCollections.TEN] = new Padding(15, 21, 15, 2),
                    [ThemeCollections.ELEVEN] = new Padding(15, 21, 15, 2),
                    [ThemeCollections.TWELVE] = new Padding(15, 22, 15, 2),
                    [ThemeCollections.THIRTEEN] = new Padding(15, 29, 15, 2),
                    [ThemeCollections.FOURTEEN] = new Padding(15, 29, 15, 2),
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static Padding SelectConfiguredItemGroupBoxPadding
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return new Padding(12, 24, 12, 12);

                Dictionary<int, Padding> sizes = new()
                {
                    [ThemeCollections.NINE] = new Padding(12, 24, 12, 12),
                    [ThemeCollections.TEN] = new Padding(12, 24, 12, 12),
                    [ThemeCollections.ELEVEN] = new Padding(12, 24, 12, 12),
                    [ThemeCollections.TWELVE] = new Padding(12, 26, 12, 15),
                    [ThemeCollections.THIRTEEN] = new Padding(12, 30, 12, 15),
                    [ThemeCollections.FOURTEEN] = new Padding(12, 30, 12, 15),
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static Padding SelectVariableCellPadding
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return new Padding(0, 0, 10, 0);

                Dictionary<int, Padding> sizes = new()
                {
                    [ThemeCollections.NINE] = new Padding(0, 0, 10, 0),
                    [ThemeCollections.TEN] = new Padding(0, 0, 10, 0),
                    [ThemeCollections.ELEVEN] = new Padding(0, 0, 10, 0),
                    [ThemeCollections.TWELVE] = new Padding(0, 0, 12, 0),
                    [ThemeCollections.THIRTEEN] = new Padding(0, 0, 12, 0),
                    [ThemeCollections.FOURTEEN] = new Padding(0, 0, 12, 0)
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
                    [ThemeCollections.THIRTEEN] = 10F,
                    [ThemeCollections.FOURTEEN] = 10F,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static float SingleLineHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 27F;

                Dictionary<int, float> sizes = new()
                {
                    [ThemeCollections.NINE] = 27F,
                    [ThemeCollections.TEN] = 30F,
                    [ThemeCollections.ELEVEN] = 33F,
                    [ThemeCollections.TWELVE] = 35F,
                    [ThemeCollections.THIRTEEN] = 37F,
                    [ThemeCollections.FOURTEEN] =39F,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int SingleRowGroupBoxHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 63;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 63,
                    [ThemeCollections.TEN] = 66,
                    [ThemeCollections.ELEVEN] = 69,
                    [ThemeCollections.TWELVE] = 75,
                    [ThemeCollections.THIRTEEN] = 81,
                    [ThemeCollections.FOURTEEN] = 83,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static Padding SingleRowGroupBoxPadding
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return new Padding(18, 24, 18, 12);

                Dictionary<int, Padding> sizes = new()
                {
                    [ThemeCollections.NINE] = new Padding(18, 24, 18, 12),
                    [ThemeCollections.TEN] = new Padding(18, 24, 18, 12),
                    [ThemeCollections.ELEVEN] = new Padding(18, 24, 18, 12),
                    [ThemeCollections.TWELVE] = new Padding(18, 26, 18, 15),
                    [ThemeCollections.THIRTEEN] = new Padding(18, 30, 18, 15),
                    [ThemeCollections.FOURTEEN] = new Padding(18, 30, 18, 15),
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int ThreeRowGroupBoxHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 172;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 172,
                    [ThemeCollections.TEN] = 181,
                    [ThemeCollections.ELEVEN] = 190,
                    [ThemeCollections.TWELVE] = 205,
                    [ThemeCollections.THIRTEEN] = 222,
                    [ThemeCollections.FOURTEEN] = 234,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int TitleBarHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 24;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 24,
                    [ThemeCollections.TEN] = 27,
                    [ThemeCollections.ELEVEN] = 30,
                    [ThemeCollections.TWELVE] = 32,
                    [ThemeCollections.THIRTEEN] = 33,
                    [ThemeCollections.FOURTEEN] = 33,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }

        public static int TwoRowGroupBoxHeight
        {
            get
            {
                if (!ThemeCollections.FontSizes.Contains(Properties.Settings.Default.fontSize))
                    return 137;

                Dictionary<int, int> sizes = new()
                {
                    [ThemeCollections.NINE] = 137,
                    [ThemeCollections.TEN] = 143,
                    [ThemeCollections.ELEVEN] = 149,
                    [ThemeCollections.TWELVE] = 160,
                    [ThemeCollections.THIRTEEN] = 175,
                    [ThemeCollections.FOURTEEN] = 185,
                };

                return sizes[Properties.Settings.Default.fontSize];
            }
        }
    }
}
