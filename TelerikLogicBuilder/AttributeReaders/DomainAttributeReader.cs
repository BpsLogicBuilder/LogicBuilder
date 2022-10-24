using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using LogicBuilder.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class DomainAttributeReader : AttributeReader
    {
        private readonly object attribute;
        private readonly IStringHelper _stringHelper;

        public DomainAttributeReader(
            IExceptionHelper exceptionHelper,
            IStringHelper stringHelper,
            object attribute) : base(exceptionHelper)
        {
            this.attribute = attribute;
            _stringHelper = stringHelper;
        }

        internal List<string> Domain
        {
            get
            {
                string domainList = GetString(this.attribute, nameof(DomainAttribute.DomainList));
                return string.IsNullOrEmpty(domainList)
                    ? new List<string>()
                    : _stringHelper.SplitWithQuoteQualifier(domainList, new string[] { ",", ";" }).ToList();
            }
        }
    }
}
