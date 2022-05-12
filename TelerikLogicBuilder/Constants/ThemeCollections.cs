using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Constants
{
    internal struct ThemeCollections
    {
		public const string Office2007Black = "Office2007Black";
		public const string Office2007Silver = "Office2007Silver";
		public const string Office2010Black = "Office2010Black";
		public const string Office2010Blue = "Office2010Blue";
		public const string Office2010Silver = "Office2010Silver";
		public const string Office2013Dark = "Office2013Dark";
		public const string Office2013Light = "Office2013Light";
		public const string Office2019Dark = "Office2019Dark";
		public const string Office2019Gray = "Office2019Gray";
		public const string Office2019Light = "Office2019Light";

		internal static readonly HashSet<string> OfficeThemes = new
		(
			new string[] 
			{
				Office2007Black,
				Office2007Silver,
				Office2010Black,
				Office2010Blue,
				Office2010Silver,
				Office2013Dark,
				Office2013Light,
				Office2019Dark,
				Office2019Gray,
				Office2019Light
			}
		);
    }
}
