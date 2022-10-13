using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    abstract internal class ParameterNodeInfoBase
    {
        internal ParameterNodeInfoBase(ParameterInfo pInfo, IParameterAttributeReader parameterAttributeReader)
        {
            this.PInfo = pInfo;
            _parameterAttributeReader = parameterAttributeReader;
        }

        protected readonly IParameterAttributeReader _parameterAttributeReader;

        #region Properties
        internal ParameterInfo PInfo { get; }

        internal string Name => this.PInfo.Name!;/*Not obtained by using the MethodInfo.ReturnParameter*/

        /// <summary>
        /// Is the parameter optional
        /// </summary>
        internal bool IsOptional => PInfo.IsOptional;

        /// <summary>
        /// Comments
        /// </summary>
        internal string Comments => _parameterAttributeReader.GetComments(PInfo);

        abstract internal ParameterBase Parameter { get; }
        #endregion Properties
    }
}
