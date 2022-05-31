using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class TableValidator : ITableValidator
    {
        private readonly ICellHelper _cellHelper;
        private readonly ICellXmlHelper _cellXmlHelper;
        private readonly IConditionsElementValidator _conditionsElementValidator;
        private readonly IContextProvider _contextProvider;
        private readonly IFunctionsElementValidator _functionsElementValidator;
        private readonly IPriorityDataParser _priorityDataParser;

        public TableValidator(ICellHelper cellHelper, ICellXmlHelper cellXmlHelper, IConditionsElementValidator conditionsElementValidator, IContextProvider contextProvider, IFunctionsElementValidator functionsElementValidator, IPriorityDataParser priorityDataParser)
        {
            _cellHelper = cellHelper;
            _cellXmlHelper = cellXmlHelper;
            _conditionsElementValidator = conditionsElementValidator;
            _contextProvider = contextProvider;
            _functionsElementValidator = functionsElementValidator;
            _priorityDataParser = priorityDataParser;
        }

        public Task<IList<ResultMessage>> Validate(string sourceFile, DataSet dataSet, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource) 
            => new TableValidatorUtility
            (
                sourceFile,
                dataSet,
                application,
                progress,
                cancellationTokenSource,
                _contextProvider,
                _conditionsElementValidator,
                _cellHelper,
                _cellXmlHelper,
                _functionsElementValidator,
                _priorityDataParser
            ).Validate();
    }
}
