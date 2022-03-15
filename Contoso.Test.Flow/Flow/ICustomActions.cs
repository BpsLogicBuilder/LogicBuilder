using LogicBuilder.Attributes;
using System.Threading.Tasks;

namespace Contoso.Test.Flow
{
    public interface ICustomActions
    {
        [AlsoKnownAs("WriteToLog")]
        void WriteToLog(string message);

        [AlsoKnownAs("SetValueAync")]
        Task SetValueAync(string key, object value);
    }
}
