﻿using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class JumpShapeValidator : IJumpShapeValidator
    {
        private readonly IContextProvider _contextProvider;
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IShapeXmlHelper _shapeXmlHelper;

        public JumpShapeValidator(IContextProvider contextProvider, IJumpDataParser jumpDataParser, IShapeXmlHelper shapeXmlHelper)
        {
            _contextProvider = contextProvider;
            _jumpDataParser = jumpDataParser;
            _shapeXmlHelper = shapeXmlHelper;
        }

        public void Validate(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors)
        {
            new JumpShapeValidatorUtility
            (
                sourceFile,
                page,
                shape,
                validationErrors,
                _contextProvider,
                _jumpDataParser,
                _shapeXmlHelper
            ).Validate();
        }
    }
}