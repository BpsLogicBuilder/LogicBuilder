namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders.Factories
{
    internal interface IAttributeReaderFactory
    {
        AlsoKnownAsAttributeReader GetAlsoKnownAsAttributeReader(object attribute);
        CommentsAttributeReader GetCommentsAttributeReader(object attribute);
        DomainAttributeReader GetDomainAttributeReader(object attribute);
        FunctionGroupAttributeReader GetFunctionGroupAttributeReader(object attribute);
        ListControlTypeAttributeReader GetListControlTypeAttributeReader(object attribute);
        NameValueAttributeReader GetNameValueAttributeReader(object attribute);
        ParameterControlTypeAttributeReader GetParameterControlTypeAttributeReader(object attribute);
        SummaryAttributeReader GetSummaryAttributeReader(object attribute);
        VariableControlTypeAttributeReader GetVariableControlTypeAttributeReader(object attribute);
    }
}
