using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Factories
{
    internal class ResultMessageHelperFactory : IResultMessageHelperFactory
    {
        private readonly Func<string, Page, Shape, List<ResultMessage>, IResultMessageHelper> _getResultMessageHelper;

        public ResultMessageHelperFactory(Func<string, Page, Shape, List<ResultMessage>, IResultMessageHelper> getResultMessageHelper)
        {
            _getResultMessageHelper = getResultMessageHelper;
        }

        public IResultMessageHelper GetResultMessageHelper(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors)
            => _getResultMessageHelper(sourceFile, page, shape, validationErrors);
    }
}
