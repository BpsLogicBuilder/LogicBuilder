using ABIS.LogicBuilder.FlowBuilder.AttributeReaders;
using ABIS.LogicBuilder.FlowBuilder.AttributeReaders.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ParameterAttributeReader : IParameterAttributeReader
    {
        private readonly IAttributeReaderFactory _attributeReaderFactory;

        public ParameterAttributeReader(IAttributeReaderFactory attributeReaderFactory)
        {
            _attributeReaderFactory = attributeReaderFactory;
        }

        public string GetComments(ParameterInfo parameter)
        {
            foreach (object attr in parameter.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.COMMENTSATTRIBUTE)
                {
                    return _attributeReaderFactory.GetCommentsAttributeReader(attr).Comments;
                }
            }

            return string.Empty;
        }

        public List<string> GetDomain(ParameterInfo parameter)
        {
            foreach (object attr in parameter.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.DOMAINLOOKUPATTRIBUTE)
                {
                    return _attributeReaderFactory.GetDomainAttributeReader(attr).Domain;
                }
            }

            return new List<string>();
        }

        public ListParameterInputStyle GetListInputStyle(ParameterInfo parameter)
        {
            foreach (object attr in parameter.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.LISTEDITORCONTROLATTRIBUTE)
                {
                    return _attributeReaderFactory.GetListControlTypeAttributeReader(attr).GetListParameterInputStyle();
                }
            }

            return ListParameterInputStyle.HashSetForm;
        }

        public LiteralParameterInputStyle GetLiteralInputStyle(ParameterInfo parameter)
        {
            foreach (object attr in parameter.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.PARAMETEREDITORCONTROLATTRIBUTE)
                {
                    return _attributeReaderFactory.GetParameterControlTypeAttributeReader(attr).GetLiteralInputStyle();
                }
            }

            return LiteralParameterInputStyle.SingleLineTextBox;
        }

        public Dictionary<string, string> GetNameValueTable(ParameterInfo parameter)
            => parameter.GetCustomAttributes(true).Aggregate
            (
                new Dictionary<string, string>(),
                (dictionary, next) =>
                {
                    if (next.GetType().FullName != AttributeConstants.NAMEVALUEATTRIBUTE)
                        return dictionary;

                    NameValueAttributeReader attributeReader = _attributeReaderFactory.GetNameValueAttributeReader(next);
                    if (attributeReader != null)
                    {
                        var pair = attributeReader.GetNameValuePair();
                        if (!pair.Equals(default(KeyValuePair<string, string>)))
                        {
                            dictionary.Add(pair.Key, pair.Value);
                        }
                    }

                    return dictionary;
                }
            );

        public ObjectParameterInputStyle GetObjectInputStyle(ParameterInfo parameter)
        {
            foreach (object attr in parameter.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.PARAMETEREDITORCONTROLATTRIBUTE)
                {
                    return _attributeReaderFactory.GetParameterControlTypeAttributeReader(attr).GetObjectInputStyle();
                }
            }

            return ObjectParameterInputStyle.Form;
        }
    }
}
