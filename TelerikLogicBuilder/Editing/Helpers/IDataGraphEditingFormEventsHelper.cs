﻿namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal interface IDataGraphEditingFormEventsHelper
    {
        void RequestDocumentUpdate(IEditingControl editingControl);
        void Setup();
    }
}
