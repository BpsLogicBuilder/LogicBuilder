using System;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders.Factories
{
    internal class AttributeReaderFactory : IAttributeReaderFactory
    {
        private readonly Func<object, AlsoKnownAsAttributeReader> _getAlsoKnownAsAttributeReader;
        private readonly Func<object, CommentsAttributeReader> _getCommentsAttributeReader;
        private readonly Func<object, DomainAttributeReader> _getDomainAttributeReader;
        private readonly Func<object, FunctionGroupAttributeReader> _getFunctionGroupAttributeReader;
        private readonly Func<object, ListControlTypeAttributeReader> _getListControlTypeAttributeReader;
        private readonly Func<object, NameValueAttributeReader> _getNameValueAttributeReader;
        private readonly Func<object, ParameterControlTypeAttributeReader> _getParameterControlTypeAttributeReader;
        private readonly Func<object, SummaryAttributeReader> _getSummaryAttributeReader;
        private readonly Func<object, VariableControlTypeAttributeReader> _getVariableControlTypeAttributeReader;

        public AttributeReaderFactory(
            Func<object, AlsoKnownAsAttributeReader> getAlsoKnownAsAttributeReader,
            Func<object, CommentsAttributeReader> getCommentsAttributeReader,
            Func<object, DomainAttributeReader> getDomainAttributeReader,
            Func<object, FunctionGroupAttributeReader> getFunctionGroupAttributeReader,
            Func<object, ListControlTypeAttributeReader> getListControlTypeAttributeReader,
            Func<object, NameValueAttributeReader> getNameValueAttributeReader,
            Func<object, ParameterControlTypeAttributeReader> getParameterControlTypeAttributeReader,
            Func<object, SummaryAttributeReader> getSummaryAttributeReader,
            Func<object, VariableControlTypeAttributeReader> getVariableControlTypeAttributeReader)
        {
            _getAlsoKnownAsAttributeReader = getAlsoKnownAsAttributeReader;
            _getCommentsAttributeReader = getCommentsAttributeReader;
            _getDomainAttributeReader = getDomainAttributeReader;
            _getFunctionGroupAttributeReader = getFunctionGroupAttributeReader;
            _getListControlTypeAttributeReader = getListControlTypeAttributeReader;
            _getNameValueAttributeReader = getNameValueAttributeReader;
            _getParameterControlTypeAttributeReader = getParameterControlTypeAttributeReader;
            _getSummaryAttributeReader = getSummaryAttributeReader;
            _getVariableControlTypeAttributeReader = getVariableControlTypeAttributeReader;
        }

        public AlsoKnownAsAttributeReader GetAlsoKnownAsAttributeReader(object attribute)
            => _getAlsoKnownAsAttributeReader(attribute);

        public CommentsAttributeReader GetCommentsAttributeReader(object attribute)
            => _getCommentsAttributeReader(attribute);

        public DomainAttributeReader GetDomainAttributeReader(object attribute)
            => _getDomainAttributeReader(attribute);

        public FunctionGroupAttributeReader GetFunctionGroupAttributeReader(object attribute)
            => _getFunctionGroupAttributeReader(attribute);

        public ListControlTypeAttributeReader GetListControlTypeAttributeReader(object attribute)
            => _getListControlTypeAttributeReader(attribute);

        public NameValueAttributeReader GetNameValueAttributeReader(object attribute)
            => _getNameValueAttributeReader(attribute);

        public ParameterControlTypeAttributeReader GetParameterControlTypeAttributeReader(object attribute)
            => _getParameterControlTypeAttributeReader(attribute);

        public SummaryAttributeReader GetSummaryAttributeReader(object attribute)
            => _getSummaryAttributeReader(attribute);

        public VariableControlTypeAttributeReader GetVariableControlTypeAttributeReader(object attribute)
            => _getVariableControlTypeAttributeReader(attribute);
    }
}
