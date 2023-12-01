using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace ABIS.LogicBuilder.FlowBuilder.Exceptions
{
    /// <summary>
    /// Critical Exceptions thrown from this application
    /// </summary>
    [Serializable()]
    public class CriticalLogicBuilderException : System.Exception
    {
        public CriticalLogicBuilderException()
            : base(Strings.defaultErrorMessage)
        {
        }


        public CriticalLogicBuilderException(string logicBuilderMessage)
            : base(logicBuilderMessage)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public CriticalLogicBuilderException(string logicBuilderMessage, int id)
            : base(logicBuilderMessage)
        {
            this.id = id;
        }

        public CriticalLogicBuilderException(string logicBuilderMessage, Exception ex)
            : base(logicBuilderMessage, ex)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public CriticalLogicBuilderException(string logicBuilderMessage, Exception ex, int id)
            : base(logicBuilderMessage, ex)
        {
            this.id = id;
        }

        #region Variables
        private readonly int id;
        #endregion Variables

        #region Properties
        internal int Id
        {
            get { return id; }
        }
        #endregion Properties

        #region Methods
        [Obsolete]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{21D85BB4-FE81-4D76-BBEF-3FCDA6F70B7F}"));

            info.AddValue("Id", Id);
            base.GetObjectData(info, context);
        }
        #endregion Methods
    }
}
