﻿using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue
{
    internal class LiteralListDefaultValueItem : IListBoxManageable
    {
        private readonly ITypeHelper _typeHelper;

        public LiteralListDefaultValueItem(
            ITypeHelper typeHelper,
            string item,
            Type type)
        {
            _typeHelper = typeHelper;
            this.Item = item.Trim();
            this.type = type;
        }

        private readonly Type type;

        public string Item { get; private set; }

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

            if (obj is not LiteralListDefaultValueItem other) return false;
            return this.Item.Equals(other.Item);
        }

        public override int GetHashCode()
            => this.Item.GetHashCode();
        #endregion Methods
    }
}
