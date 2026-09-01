using LogicBuilder.App.Bsl.Flow.Interfaces;
using LogicBuilder.Attributes;

namespace LogicBuilder.App.Bsl.Flow
{
    public static class CustomActionUtils
    {
        [AlsoKnownAs("WriteToLog")]
        public static void WriteToLog(ICustomActions customActions, string message)
        {
            customActions.WriteToLog(message);
        }
    }
}
