using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain
{
    internal class LiteralDomainItem : IListBoxManageable
    {
        private readonly ITypeHelper _typeHelper;

        public LiteralDomainItem(
            ITypeHelper typeHelper,
            string item,
            Type type)
        {
            _typeHelper = typeHelper;
            this.Item = item.Trim();
            this.type = type;
        }

        private readonly Type type;

        public string Item { get; }

        public IList<string> Errors
        {
            get
            {
                if (this.type != typeof(string) && !_typeHelper.TryParse(this.Item, this.type, out object? _))
                    return new List<string> { string.Format(CultureInfo.CurrentCulture, Strings.notValidTypeFormat, this.Item, type.Name) };

                return new List<string>();
            }
        }

        #region Methods
        public override string ToString()
            => this.Item.Length == 0 ? Strings.emptyStringVisibleText : this.Item;

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;

            if (obj is not LiteralDomainItem other) return false;
            return this.Item == other.Item;
        }

        public override int GetHashCode()
            => this.Item.GetHashCode();
        #endregion Methods
    }
}
