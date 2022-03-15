using LogicBuilder.Domain;

namespace Contoso.Domain
{
    abstract public class BaseModelClass : BaseModel
    {
        public string TypeFullName => this.GetType().AssemblyQualifiedName;
    }
}
