using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments
{
    internal partial class GenericArgumentName : IListBoxManageable
    {
        public GenericArgumentName(string item)
        {
            Item = item;
        }

        public string Item { get; }
        public IList<string> Errors
        {
            get
            {
                if (!GenericArgumentNameRegex().IsMatch(Item))
                    return new string[] { Strings.genericArgNameInvalid };

                return Array.Empty<string>();
            }
        }

        public override string ToString()
        {
            return Item;
        }

        public override bool Equals(object? item)
        {
            if (item == null)
                return false;
            if (this.GetType() != item.GetType())
                return false;

            if (item is not GenericArgumentName genericArgumentName)
                return false;

            return Item == genericArgumentName.Item;
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode();
        }

        [GeneratedRegex(RegularExpressions.GENERICARGUMENTNAME)]
        private static partial Regex GenericArgumentNameRegex();
    }
}
