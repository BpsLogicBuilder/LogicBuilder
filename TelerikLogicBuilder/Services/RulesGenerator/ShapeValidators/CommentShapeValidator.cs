﻿using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class CommentShapeValidator : ICommentShapeValidator
    {
        private readonly IContextProvider _contextProvider;
        private readonly IShapeHelper _shapeHelper;

        public CommentShapeValidator(IContextProvider contextProvider, IShapeHelper shapeHelper)
        {
            _contextProvider = contextProvider;
            _shapeHelper = shapeHelper;
        }

        public void Validate(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors)
        {
            new CommentShapeValidatorUtility
            (
                sourceFile,
                page,
                shape,
                validationErrors,
                _contextProvider,
                _shapeHelper
            ).Validate();
        }
    }
}
