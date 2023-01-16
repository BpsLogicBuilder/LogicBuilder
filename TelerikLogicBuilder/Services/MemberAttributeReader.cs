using ABIS.LogicBuilder.FlowBuilder.AttributeReaders;
using ABIS.LogicBuilder.FlowBuilder.AttributeReaders.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class MemberAttributeReader : IMemberAttributeReader
    {
        private readonly IAttributeReaderFactory _attributeReaderFactory;

        public MemberAttributeReader(IAttributeReaderFactory attributeReaderFactory)
        {
            _attributeReaderFactory = attributeReaderFactory;
        }

        public string GetAlsoKnownAs(MemberInfo member)
        {
            foreach (object attr in member.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.ALSOKNOWNASATTRIBUTE)
                {
                    return _attributeReaderFactory.GetAlsoKnownAsAttributeReader(attr).AlsoKnownAs;
                }
            }

            return string.Empty;
        }

        public List<string> GetDomain(MemberInfo member)
        {
            foreach (object attr in member.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.DOMAINLOOKUPATTRIBUTE)
                {
                    return _attributeReaderFactory.GetDomainAttributeReader(attr).Domain;
                }
            }

            return new List<string>();
        }

        public FunctionCategories GetFunctionCategory(MemberInfo member)
        {
            foreach (object attr in member.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.FUNCTIONGROUPATTRIBUTE)
                {
                    return _attributeReaderFactory.GetFunctionGroupAttributeReader(attr).GetFunctionCategory();
                }
            }

            return FunctionCategories.Unknown;
        }

        public string GetSummary(MemberInfo member)
        {
            foreach (object attr in member.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.SUMMARYATTRIBUTE)
                {
                    return _attributeReaderFactory.GetSummaryAttributeReader(attr).Summary;
                }
            }

            return string.Empty;
        }

        public ListVariableInputStyle GetListInputStyle(MemberInfo member)
        {
            foreach (object attr in member.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.LISTEDITORCONTROLATTRIBUTE)
                {
                    return _attributeReaderFactory.GetListControlTypeAttributeReader(attr).GetListVariableInputStyle();
                }
            }

            return ListVariableInputStyle.HashSetForm;
        }

        public LiteralVariableInputStyle GetLiteralInputStyle(MemberInfo member)
        {
            foreach (object attr in member.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.VARIABLEEDITORCONTROLATTRIBUTE)
                {
                    return _attributeReaderFactory.GetVariableControlTypeAttributeReader(attr).GetLiteralInputStyle();
                }
            }

            return LiteralVariableInputStyle.SingleLineTextBox;
        }

        public Dictionary<string, string> GetNameValueTable(MemberInfo member) 
            => member.GetCustomAttributes(true).Aggregate
            (
                new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase), 
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

        public ObjectVariableInputStyle GetObjectInputStyle(MemberInfo member)
        {
            foreach (object attr in member.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.VARIABLEEDITORCONTROLATTRIBUTE)
                {
                    return _attributeReaderFactory.GetVariableControlTypeAttributeReader(attr).GetObjectInputStyle();
                }
            }

            return ObjectVariableInputStyle.Form;
        }

        public string GetVariableComments(MemberInfo member)
        {
            foreach (object attr in member.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.COMMENTSATTRIBUTE)
                {
                    return _attributeReaderFactory.GetCommentsAttributeReader(attr).Comments;
                }
            }

            return string.Empty;
        }

        public bool IsFunctionConfigurableFromClassHelper(MemberInfo member)
            => member.GetCustomAttributes(true)
                    .Select(attr => attr.GetType().FullName)
                    .Any(fullName => fullName != null && fullName.StartsWith(AttributeConstants.LOGICBUILDERATTRPREFIX));

        public bool IsVariableConfigurableFromClassHelper(MemberInfo member)
            => member.GetCustomAttributes(true)
                    .Select(attr => attr.GetType().FullName)
                    .Any(fullName => fullName != null && fullName.StartsWith(AttributeConstants.LOGICBUILDERATTRPREFIX));
    }
}
