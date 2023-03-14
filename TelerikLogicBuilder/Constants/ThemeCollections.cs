using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Constants
{
    internal struct ThemeCollections
    {
        private const string defaultNamespace = "ABIS.LogicBuilder.FlowBuilder";
        public static readonly string Office2019Dark10_PackageResource = $"{defaultNamespace}.{Office2019Dark10}.tssp";
        public static readonly string Office2019Dark11_PackageResource = $"{defaultNamespace}.{Office2019Dark11}.tssp";
        public static readonly string Office2019Dark12_PackageResource = $"{defaultNamespace}.{Office2019Dark12}.tssp";
        public static readonly string Office2019Dark14_PackageResource = $"{defaultNamespace}.{Office2019Dark14}.tssp";
        public static readonly string Office2019Gray10_PackageResource = $"{defaultNamespace}.{Office2019Gray10}.tssp";
        public static readonly string Office2019Gray11_PackageResource = $"{defaultNamespace}.{Office2019Gray11}.tssp";
        public static readonly string Office2019Gray12_PackageResource = $"{defaultNamespace}.{Office2019Gray12}.tssp";
        public static readonly string Office2019Gray14_PackageResource = $"{defaultNamespace}.{Office2019Gray14}.tssp";
        public static readonly string Office2019Light10_PackageResource = $"{defaultNamespace}.{Office2019Light10}.tssp";
        public static readonly string Office2019Light11_PackageResource = $"{defaultNamespace}.{Office2019Light11}.tssp";
        public static readonly string Office2019Light12_PackageResource = $"{defaultNamespace}.{Office2019Light12}.tssp";
        public static readonly string Office2019Light14_PackageResource = $"{defaultNamespace}.{Office2019Light14}.tssp";

        public const string ControlDefault = "ControlDefault";
		public const string Office2019Dark = "Office2019Dark";
        public const string Office2019Dark10 = "Office2019Dark10";
        public const string Office2019Dark11 = "Office2019Dark11";
        public const string Office2019Dark12 = "Office2019Dark12";
        public const string Office2019Dark14 = "Office2019Dark14";
        public const string Office2019Gray = "Office2019Gray";
        public const string Office2019Gray10 = "Office2019Gray10";
        public const string Office2019Gray11 = "Office2019Gray11";
        public const string Office2019Gray12 = "Office2019Gray12";
        public const string Office2019Gray14 = "Office2019Gray14";
        public const string Office2019Light = "Office2019Light";
        public const string Office2019Light10 = "Office2019Light10";
        public const string Office2019Light11 = "Office2019Light11";
        public const string Office2019Light12 = "Office2019Light12";
        public const string Office2019Light14 = "Office2019Light14";

        public const string Dark = "Dark";
        public const string Gray = "Gray";
        public const string Light = "Light";

        public const int NINE = 9;
        public const int TEN = 10;
        public const int ELEVEN = 11;
        public const int TWELVE = 12;
        public const int FOURTEEN = 14;

        public static readonly ThemeSelector Dark09 = new(Dark, NINE);
        public static readonly ThemeSelector Dark10 = new(Dark, TEN);
        public static readonly ThemeSelector Dark11 = new(Dark, ELEVEN);
        public static readonly ThemeSelector Dark12 = new(Dark, TWELVE);
        public static readonly ThemeSelector Dark14 = new(Dark, FOURTEEN);
        public static readonly ThemeSelector Gray09 = new(Gray, NINE);
        public static readonly ThemeSelector Gray10 = new(Gray, TEN);
        public static readonly ThemeSelector Gray11 = new(Gray, ELEVEN);
        public static readonly ThemeSelector Gray12 = new(Gray, TWELVE);
        public static readonly ThemeSelector Gray14 = new(Gray, FOURTEEN);
        public static readonly ThemeSelector Light09 = new(Light, NINE);
        public static readonly ThemeSelector Light10 = new(Light, TEN);
        public static readonly ThemeSelector Light11 = new(Light, ELEVEN);
        public static readonly ThemeSelector Light12 = new(Light, TWELVE);
        public static readonly ThemeSelector Light14 = new(Light, FOURTEEN);

        public static readonly HashSet<string> ThemeNames = new
		(
			new string[] 
			{
				ControlDefault,
                Office2019Dark,
                Office2019Dark10,
                Office2019Dark11,
                Office2019Dark12,
                Office2019Dark14,
                Office2019Gray,
                Office2019Gray10,
                Office2019Gray11,
                Office2019Gray12,
                Office2019Gray14,
                Office2019Light,
                Office2019Light10,
                Office2019Light11,
                Office2019Light12,
                Office2019Light14
            }
		);

        public static readonly IDictionary<ThemeSelector, string> SelectorToTheme = new Dictionary<ThemeSelector, string>
        {
            [Dark09] = Office2019Dark,
            [Dark10] = Office2019Dark10,
            [Dark11] = Office2019Dark11,
            [Dark12] = Office2019Dark12,
            [Dark14] = Office2019Dark14,
            [Gray09] = Office2019Gray,
            [Gray10] = Office2019Gray10,
            [Gray11] = Office2019Gray11,
            [Gray12] = Office2019Gray12,
            [Gray14] = Office2019Gray14,
            [Light09] = Office2019Light,
            [Light10] = Office2019Light10,
            [Light11] = Office2019Light11,
            [Light12] = Office2019Light12,
            [Light14] = Office2019Light14
        };

        public static readonly IDictionary<string, ThemeSelector> ThemeToSelector = new Dictionary<string, ThemeSelector>
        {
            [Office2019Dark] = Dark09,
            [Office2019Dark10] = Dark10,
            [Office2019Dark11] = Dark11,
            [Office2019Dark12] = Dark12,
            [Office2019Dark14] = Dark14,
            [Office2019Gray] = Gray09,
            [Office2019Gray10] = Gray10,
            [Office2019Gray11] = Gray11,
            [Office2019Gray12] = Gray12,
            [Office2019Gray14] = Gray14,
            [Office2019Light] = Light09,
            [Office2019Light10] = Light10,
            [Office2019Light11] = Light11,
            [Office2019Light12] = Light12,
            [Office2019Light14] = Light14
        };
    }
}
