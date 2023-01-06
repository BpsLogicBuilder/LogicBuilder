using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Parameters
{
    internal class ParametersMatcher : IParametersMatcher
    {
        private readonly ITypeHelper _typeHelper;
        private readonly IExceptionHelper _exceptionHelper;

        public ParametersMatcher(ITypeHelper typeHelper, IExceptionHelper exceptionHelper)
        {
            _typeHelper = typeHelper;
            _exceptionHelper = exceptionHelper;
        }

        public bool MatchParameters(IList<ParameterNodeInfoBase> parameterInfos, IList<ParameterBase> configuredParameters)
        {
            if (configuredParameters.Count != parameterInfos.Count) return false;

            for (int i = 0; i < parameterInfos.Count; i++)
            {
                if (configuredParameters[i].ParameterCategory != parameterInfos[i].Parameter.ParameterCategory)
                    return false;

                switch (configuredParameters[i].ParameterCategory)
                {
                    case ParameterCategory.Literal:
                        if (((LiteralParameter)configuredParameters[i]).LiteralType != ((LiteralParameterNodeInfo)parameterInfos[i]).Type)
                            return false;

                        break;
                    case ParameterCategory.Object:
                        ObjectParameter cp = (ObjectParameter)configuredParameters[i];
                        if (cp.ObjectType != _typeHelper.ToId(parameterInfos[i].PInfo.ParameterType))
                            return false;

                        break;
                    case ParameterCategory.Generic:
                        GenericParameter gp = (GenericParameter)configuredParameters[i];
                        if (gp.GenericArgumentName != parameterInfos[i].PInfo.ParameterType.Name)
                            return false;

                        break;
                    case ParameterCategory.LiteralList:
                        ListOfLiteralsParameter lol = (ListOfLiteralsParameter)configuredParameters[i];
                        if (lol.LiteralType != ((ListOfLiteralsParameterNodeInfo)parameterInfos[i]).Type)
                            return false;

                        if (!(ListTypeMatchesParameterInfo(lol.ListType, parameterInfos[i])))
                            return false;

                        break;
                    case ParameterCategory.ObjectList:
                        ListOfObjectsParameter loc = (ListOfObjectsParameter)configuredParameters[i];
                        Type? underlyingType = _typeHelper.IsValidList(parameterInfos[i].PInfo.ParameterType)
                                            ? _typeHelper.GetUndelyingTypeForValidList(parameterInfos[i].PInfo.ParameterType)
                                            : null;

                        if (underlyingType == null)
                            return false;

                        if (loc.ObjectType != _typeHelper.ToId(underlyingType))
                            return false;

                        if (!(ListTypeMatchesParameterInfo(loc.ListType, parameterInfos[i])))
                            return false;
                        break;
                    case ParameterCategory.GenericList:
                        ListOfGenericsParameter log = (ListOfGenericsParameter)configuredParameters[i];
                        Type? underlyingGenericType = _typeHelper.IsValidList(parameterInfos[i].PInfo.ParameterType)
                                            ? _typeHelper.GetUndelyingTypeForValidList(parameterInfos[i].PInfo.ParameterType)
                                            : null;

                        if (underlyingGenericType == null)
                            return false;

                        if (log.GenericArgumentName != underlyingGenericType.Name)
                            return false;

                        if (log.ListType == ListType.Array && !parameterInfos[i].PInfo.ParameterType.IsArray)
                            return false;

                        if (!(ListTypeMatchesParameterInfo(log.ListType, parameterInfos[i])))
                            return false;

                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{701F0B9B-CB2F-4AB9-83C1-E75D325135C4}");
                }
            }

            return true;
        }

        private static bool ListTypeMatchesParameterInfo(ListType listType, ParameterNodeInfoBase parameterNodeInfo)
        {
            if (listType == ListType.Array && !parameterNodeInfo.PInfo.ParameterType.IsArray)
                return false;
            if (listType == ListType.GenericList && !ParameterMatchesGenericTypeDefinition(parameterNodeInfo, typeof(List<>)))
                return false;
            if (listType == ListType.IGenericList && !ParameterMatchesGenericTypeDefinition(parameterNodeInfo, typeof(IList<>)))
                return false;
            if (listType == ListType.IGenericEnumerable && !ParameterMatchesGenericTypeDefinition(parameterNodeInfo, typeof(IEnumerable<>)))
                return false;
            if (listType == ListType.GenericCollection && !ParameterMatchesGenericTypeDefinition(parameterNodeInfo, typeof(Collection<>)))
                return false;
            if (listType == ListType.IGenericCollection && !ParameterMatchesGenericTypeDefinition(parameterNodeInfo, typeof(ICollection<>)))
                return false;

            return true;

            static bool ParameterMatchesGenericTypeDefinition(ParameterNodeInfoBase parameterNodeInfo, Type genericTypeDefinition)
                => parameterNodeInfo.PInfo.ParameterType.IsGenericType && parameterNodeInfo.PInfo.ParameterType.GetGenericTypeDefinition().Equals(genericTypeDefinition);
        }
    }
}
