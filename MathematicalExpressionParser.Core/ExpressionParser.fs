// Copyright 2020 © Deltares
// 
// This file is part of the Mathematical Expression Parser.
// 
//     The Mathematical Expression Parser is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Mathematical Expression Parser is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with the Mathematical Expression Parser.  If not, see <https://www.gnu.org/licenses/>

/// <summary>
/// ExpressionParser.fs defines the Domain Specific Language (DSL)
/// for the simple mathematical expressions required for the real-time
/// control (RTC).
/// The language is defined as follows:
/// * A <see cref="Parameter"/> is defined as: 
///       any identifier starting with a Capital character or '_'
///                      containing only letters, digits and '_'
/// * A <see cref="Constant"/> is defined as:
///       any float in either fixed point or scientific notation.
/// * A <see cref="BinaryFunctionType"/> is defined as:
///       <binary function identifier>( expression1, expression2 )
/// * An <see cref="InfixFunctionType"/> is defined as:
///       expression1 <infix operator> expression 2
/// </summary>
namespace MathematicalExpressionParser.Core.ExpressionParser

open FParsec
open FParsec.Pipes

open MathematicalExpressionParser.Core.ExpressionTree

module public Parser =
    let private ws = spaces
    let private str_ws s = pstring s >>. ws

    let private operatorPrecedenceParser = 
        new OperatorPrecedenceParser<ExpressionType, unit, unit>()

    let private pExpr = operatorPrecedenceParser.ExpressionParser

    let private pParameter = 
        let isParameterFirstChar c = (isLetter c && isUpper c) ||c = '_'
        let isParameterChar c = isLetter c || isDigit c || c = '_'

        ( many1Satisfy2L isParameterFirstChar isParameterChar "parameter" 
          .>> ws ) |>> Parameter

    let private pConstant = 
        ( pfloat .>> ws ) |>> Constant

    let private pBinaryFunctionExpr = 
        let strRet ((s : string), (v : BinaryFunctionType)) = stringReturn s v
        let ident = choice ( List.map strRet Expression.binaryFunctions ) |>> BinaryFunction
        %% +.ident 
        -- ( str_ws "(" ) 
        -- +.pExpr -- ( str_ws "," )
        -- +.pExpr
        -- ( str_ws ")" )
        -|> Expression.composedExpression |>> ComposedExpression

    let private  pAtomExpr =
        (pConstant <|> pParameter) |>> AtomicExpression

    let private pParenthesizedTerm = 
        between (str_ws "(") (str_ws ")")

    let private pTerm =
        choice [ pAtomExpr 
               ; pBinaryFunctionExpr
               ; (pParenthesizedTerm pExpr)
               ]
    operatorPrecedenceParser.TermParser <- pTerm

    for (ident, infixType, precedence) in Expression.infixFunctions do
        let composedInfixExpression lOperand rOperand = 
            ComposedExpression ( Expression.composedExpression ( InfixFunction infixType ) lOperand rOperand )
        operatorPrecedenceParser.AddOperator(InfixOperator(ident, ws, precedence, Associativity.Left, composedInfixExpression ))

    let private pExprComplete = ws >>. pExpr .>> eof

    /// <summary>
    /// Parse the provided string to a <see cref="ExpressionType"/>
    /// </summary>
    /// <param name="str">The string to be parsed.</param
    /// <returns>
    /// The <see cref="ExpressionType"/> corresponding with the provided
    /// <paramref name="str"/>.
    /// </returns>
    /// <exception cref="System.ArgumentException">
    /// Thrown when the provided <paramref name="str"/> cannot be parsed.
    /// </exception>
    let public parseExpression (str : string) = 
        match run pExprComplete str with
            | FParsec.CharParsers.Success(result, _, _)   -> Success result
            | FParsec.CharParsers.Failure(errorMsg, _, _) -> Failure errorMsg

