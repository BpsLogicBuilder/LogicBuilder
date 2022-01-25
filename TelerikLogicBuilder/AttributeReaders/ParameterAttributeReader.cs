using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class ParameterAttributeReader : IParameterAttributeReader
    {
        private readonly IContextProvider _contextProvider;

        public ParameterAttributeReader(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public string GetComments(ParameterInfo parameter)
        {
            foreach (object attr in parameter.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.COMMENTSATTRIBUTE)
                {
                    return new CommentsAttributeReader(attr, _contextProvider.ExceptionHelper).Comments;
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
                    return new DomainAttributeReader(attr, _contextProvider).Domain;
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
                    return new ListControlTypeAttributeReader(attr, _contextProvider.ExceptionHelper).GetListParameterInputStyle();
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
                    return new ParameterControlTypeAttributeReader(attr, _contextProvider.ExceptionHelper).GetLiteralInputStyle();
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

        public ObjectParameterInputStyle GetObjectInputStyle(ParameterInfo parameter)
        {
            foreach (object attr in parameter.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == AttributeConstants.PARAMETEREDITORCONTROLATTRIBUTE)
                {
                    return new ParameterControlTypeAttributeReader(attr, _contextProvider.ExceptionHelper).GetObjectInputStyle();
                }
            }

            return ObjectParameterInputStyle.Form;
        }
    }
}
