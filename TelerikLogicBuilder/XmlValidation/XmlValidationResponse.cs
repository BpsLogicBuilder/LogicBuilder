using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation
{
    internal class XmlValidationResponse
    {
        public bool Success { get; set; }
        public ICollection<string> Errors { get; set; } = new List<string>();
    }
}
