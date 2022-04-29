using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal abstract class ShapeValidatorUtility
    {
        protected IResultMessageBuilder _resultMessageBuilder;

        protected ShapeValidatorUtility(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors, IContextProvider contextProvider)
        {
            SourceFile = sourceFile;
            ModuleName = contextProvider.PathHelper.GetModuleName(sourceFile);
            Page = page;
            Shape = shape;
            VisioFileSource = new VisioFileSource
            (
                sourceFile, 
                page.ID, 
                page.Index, 
                shape.Master.Name, 
                shape.ID, 
                shape.Index, 
                contextProvider
            );
            ValidationErrors = validationErrors;
            _resultMessageBuilder = contextProvider.ResultMessageBuilder;
        }

        #region Methods
        internal abstract void Validate();

        protected virtual void AddValidationMessage(string message) 
            => ValidationErrors.Add(GetResultMessage(message));

        protected virtual ResultMessage GetResultMessage(string message)
            => _resultMessageBuilder.BuilderMessage(VisioFileSource, message);
        #endregion Methods

        #region Properties
        protected string ModuleName { get; }
        protected string SourceFile { get; }
        protected Page Page { get;  }
        protected Shape Shape { get; }
        protected List<ResultMessage> ValidationErrors { get; }
        protected VisioFileSource VisioFileSource { get; }
        #endregion Properties
    }
}
