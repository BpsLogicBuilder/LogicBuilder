namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal record ThemeSelector
    {
        public ThemeSelector(string colorTheme, int fontSize)
        {
            ColorTheme = colorTheme;
            FontSize = fontSize;
        }

        public string ColorTheme { get; }
        public int FontSize { get; }
    }
}
