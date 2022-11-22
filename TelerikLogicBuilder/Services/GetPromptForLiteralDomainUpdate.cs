using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class GetPromptForLiteralDomainUpdate : IGetPromptForLiteralDomainUpdate
    {
        private readonly IExceptionHelper _exceptionHelper;

        public GetPromptForLiteralDomainUpdate(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public string Get(Type domainType)
        {
            if 
            (
                new HashSet<Type> 
                {
                    typeof(char),
                    typeof(char?)
                }.Contains(domainType)
            )
            {
                return Strings.domainCharacterPrompt;
            }
            else if
            (
                new HashSet<Type>
                {
                    typeof(decimal),
                    typeof(byte),
                    typeof(short),
                    typeof(int),
                    typeof(long),
                    typeof(float),
                    typeof(double),
                    typeof(sbyte),
                    typeof(ushort),
                    typeof(uint),
                    typeof(ulong),
                    typeof(decimal?),
                    typeof(byte?),
                    typeof(short?),
                    typeof(int?),
                    typeof(long?),
                    typeof(float?),
                    typeof(double?),
                    typeof(sbyte?),
                    typeof(ushort?),
                    typeof(uint?),
                    typeof(ulong?)
                }.Contains(domainType)
            )
            {
                return Strings.domainNumberPrompt;
            }
            else if
            (
                new HashSet<Type>
                {
                    typeof(string)
                }.Contains(domainType)
            )
            {
                return Strings.domainStringPrompt;
            }
            else if
            (
                new HashSet<Type>
                {
                    typeof(bool),
                    typeof(DateTimeOffset),
                    typeof(DateOnly),
                    typeof(DateTime),
                    typeof(Date),
                    typeof(TimeSpan),
                    typeof(TimeOnly),
                    typeof(TimeOfDay),
                    typeof(Guid),
                    typeof(bool),
                    typeof(DateTimeOffset?),
                    typeof(DateOnly?),
                    typeof(DateTime?),
                    typeof(Date?),
                    typeof(TimeSpan?),
                    typeof(TimeOnly?),
                    typeof(TimeOfDay?),
                    typeof(Guid?)
                }.Contains(domainType)
            )
            {
                return Strings.domainValuePrompt;
            }
            else
                throw _exceptionHelper.CriticalException("{6094C947-DA14-4D8E-A92B-2B363455176D}");
        }
    }
}
