using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors
{
    internal class Constructor
    {
        public Constructor(string Name, string TypeName, List<ParameterBase> Parameters, List<string> genericArguments, string Summary)
        {
            this.Name = Name;
            this.TypeName = TypeName;
            this.Parameters = Parameters;
            this.GenericArguments = genericArguments;
            this.Summary = Summary;
        }

        #region Properties
        /// <summary>
        /// Unique name for this constructor
        /// </summary>
        internal string Name { get; private set; }

        /// <summary>
        /// Fully Qualified class for for this constructor
        /// </summary>
        internal string TypeName { get; private set; }

        /// <summary>
        /// Parameters for this constructor
        /// </summary>
        internal List<ParameterBase> Parameters { get; private set; }

        /// <summary>
        /// Generic Arguments
        /// </summary>
        internal List<string> GenericArguments { get; private set; }

        /// <summary>
        /// Has Generic Arguments
        /// </summary>
        internal bool HasGenericArguments => GenericArguments.Count > 0;

        /// <summary>
        /// Comments about this constructor
        /// </summary>
        internal string Summary { get; private set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.constructorToStringFormat,
                this.Name,
                this.TypeName,
                string.Join(string.Concat(MiscellaneousConstants.COMMASTRING, " "), this.Parameters.Select(p => p.ToString())));
        }
        #endregion Properties
    }
}
