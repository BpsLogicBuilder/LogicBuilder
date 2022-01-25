using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Services;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class MemberAttributeReader : IMemberAttributeReader
    {
        private readonly IContextProvider _contextProvider;

        public MemberAttributeReader(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public string GetAlsoKnownAs(MemberInfo member)
        {
            foreach (object attr in member.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.ALSOKNOWNASATTRIBUTE)
                {
                    return new AlsoKnownAsAttributeReader(attr, _contextProvider.ExceptionHelper).AlsoKnownAs;
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
                    return new DomainAttributeReader(attr, _contextProvider).Domain;
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
                    return new FunctionGroupAttributeReader(attr, _contextProvider.ExceptionHelper).GetFunctionCategory();
                }
            }

            return FunctionCategories.Unknown;
        }

        public string GetFunctionSummary(MemberInfo member)
        {
            foreach (object attr in member.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.SUMMARYATTRIBUTE)
                {
                    return new SummaryAttributeReader(attr, _contextProvider.ExceptionHelper).Summary;
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
                    return new ListControlTypeAttributeReader(attr, _contextProvider.ExceptionHelper).GetListVariableInputStyle();
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
                    return new VariableControlTypeAttributeReader(attr, _contextProvider.ExceptionHelper).GetLiteralInputStyle();
                }
            }

            return LiteralVariableInputStyle.SingleLineTextBox;
        }

        public Dictionary<string, string> GetNameValueTable(MemberInfo member) 
            => member.GetCustomAttributes(true).Aggregate
            (
                new Dictionary<string, string>(), 
                (dictionary, next) =>
                {
                    if (next.GetType().FullName != AttributeConstants.NAMEVALUEATTRIBUTE)
                        return dictionary;

                    NameValueAttributeReader attributeReader = new(next, _contextProvider.ExceptionHelper);
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
                    return new VariableControlTypeAttributeReader(attr, _contextProvider.ExceptionHelper).GetObjectInputStyle();
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
                    return new CommentsAttributeReader(attr, _contextProvider.ExceptionHelper).Comments;
                }
            }

            return string.Empty;
        }

        public bool IsFunctionConfigurableFromClassHelper(MemberInfo member)
            => member.GetCustomAttributes(true)
                    .Select(attr => attr.GetType().FullName)
                    .Any(fullName => fullName.StartsWith(AttributeConstants.LOGICBUILDERATTRPREFIX));

        public bool IsVariableConfigurableFromClassHelper(MemberInfo member)
            => member.GetCustomAttributes(true)
                    .Select(attr => attr.GetType().FullName)
                    .Any(fullName => fullName.StartsWith(AttributeConstants.LOGICBUILDERATTRPREFIX));
    }
}
