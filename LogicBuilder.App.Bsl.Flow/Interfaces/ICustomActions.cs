using LogicBuilder.Attributes;

namespace LogicBuilder.App.Bsl.Flow.Interfaces
{
    public interface ICustomActions
    {
        [AlsoKnownAs("WriteToLog")]
        void WriteToLog(string message);
    }
}
