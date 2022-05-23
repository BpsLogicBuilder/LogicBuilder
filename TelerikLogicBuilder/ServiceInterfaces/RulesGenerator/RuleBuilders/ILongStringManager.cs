using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders
{
    internal interface ILongStringManager
    {
        string GetLongStringForBinary(string longString, RuntimeType platForm);
        string GetLongStringForText(string longString, RuntimeType platForm);
		string UpdateStrongNameByPlatForm(string ruleSetOrResourceString, RuntimeType platForm);
        string UpdateStrongNameToNetCore(string ruleSetOrResourceString);
    }
}
