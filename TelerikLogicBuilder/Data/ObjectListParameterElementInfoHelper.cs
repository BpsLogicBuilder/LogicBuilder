﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ObjectListParameterElementInfoHelper : IObjectListParameterElementInfoHelper
    {
        private readonly IEnumHelper _enumHelper;

        public ObjectListParameterElementInfoHelper(
            IEnumHelper enumHelper)
        {
            _enumHelper = enumHelper;
        }

        public ObjectListParameterElementInfo GetDefaultObjectListElementInfo()
            => new
            (
                _enumHelper.GetTypeDescription
                (
                    ListType.GenericList,
                    MiscellaneousConstants.DEFAULT_OBJECT_TYPE
                ),
                ListType.GenericList,
                MiscellaneousConstants.DEFAULT_OBJECT_TYPE,
                ListParameterInputStyle.ListForm, 
                string.Empty
            );

        public ObjectListParameterElementInfo GetObjectListElementInfo(ListOfObjectsParameter objectListParameter)
            => new
            (
                objectListParameter.Name,
                objectListParameter.ListType,
                objectListParameter.ObjectType,
                objectListParameter.Control,
                objectListParameter.Comments,
                objectListParameter
            );

        public ObjectListParameterElementInfo GetObjectListElementInfo(ObjectListData listData)
            => new
            (
                _enumHelper.GetTypeDescription
                (
                    listData.ListType,
                    listData.ObjectType
                ),
                listData.ListType,
                listData.ObjectType,
                ListParameterInputStyle.ListForm,
                string.Empty
            );
    }
}
