using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects
{
    internal class ConnectorObjectListBoxItem : IListBoxManageable
    {
        public ConnectorObjectListBoxItem(string text)
        {
            Text = text;
        }

        #region Properties
        public string Text { get; }

        public IList<string> Errors
        {
            get
            {
                if (!Regex.IsMatch(this.Text, RegularExpressions.FULLYQUALIFIEDCLASSNAME))
                    return new List<string> { string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, this.Text) };

                return new List<string>();
            }
        }
        #endregion Properties

        #region Methods
        public override string ToString() => this.Text;

        public override bool Equals(object? obj)
        {
            if (obj is not ConnectorObjectListBoxItem listBoxItem) 
                return false;

            return listBoxItem.Text == this.Text;
        }

        public override int GetHashCode() => this.Text.GetHashCode();
        #endregion Methods
    }
}
