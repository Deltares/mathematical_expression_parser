// Copyright 2025 � Deltares
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

namespace MathematicalExpressionParser.Core.ExpressionTree

/// <summary>
/// <see cref="InfixFunctionType"/> defines the possible infix functions.
/// </summary>
type InfixFunctionType =
    | Addition
    | Subtraction
    | Multiplication
    | Division

/// <summary>
/// <see cref="BinaryFunctionType"/> defines the possible binary function types.
/// </summary>
type BinaryFunctionType =
    | Min
    | Max

/// <summary>
/// <see cref="ExpressionOperatorType"/> defines the possible expression 
/// operators.
/// </summary>
type ExpressionOperatorType =
    | InfixFunction  of InfixFunctionType
    | BinaryFunction of BinaryFunctionType

/// <summary>
/// <see cref="AtomicExpressionType"/> defines the possible atomic expressions,
/// i.e. the expressions that cannot be divided further.
/// </summary>
type AtomicExpressionType = 
    | Constant of float
    | Parameter of string

/// <summary>
/// <see cref="ComposedExpressionType"/> defines the record describing a single
/// composed operation, as an operator and two operands.
/// </summary>
type ComposedExpressionType = 
    { operator      : ExpressionOperatorType
    ; firstOperand  : ExpressionType
    ; secondOperand : ExpressionType
    }
/// <summary>
/// <see cref="ExpressionType"/> defines the possible expressions.
/// </summary>
and ExpressionType =
    | AtomicExpression  of AtomicExpressionType
    | ComposedExpression of ComposedExpressionType

/// <summary>
/// <see cref="ParseResult"/> defines the possible results when parsing an 
/// expression.
/// </summary>
type ParserResult = 
    | Success of ExpressionType
    | Failure of string

module internal Expression =
    /// <summary>
    /// The set of binary function identifiers and their corresponding 
    /// <see cref="BinaryFunctionType"/>.
    /// </summary>
    let binaryFunctions = 
        [ ("min", Min)
        ; ("max", Max)
        ]

    /// <summary>
    /// The set of infix functions, their corresponding 
    /// <see cref="InfixFunctionType"/>, and their precedence value.
    /// </summary>
    let infixFunctions = 
        [ ("+", Addition,       1)
        ; ("-", Subtraction,    1)
        ; ("*", Multiplication, 2)
        ; ("/", Division,       2)
        ]

    /// <summary>
    /// Construct a new <see cref="ComposedExpressionType"/> with the 
    /// given parameters.
    /// </summary>
    /// <param name="operator">The operator</param>
    /// <param name="firstOperand">The first operand of this expression.</param>
    /// <param name="secondOperand">The second operand of this expression.</param>
    /// <returns>
    /// A new <see cref="ComposedExpressionType"/> with the given parameters.
    /// </returns>
    let composedExpression ( operator      : ExpressionOperatorType ) 
                           ( firstOperand  : ExpressionType ) 
                           ( secondOperand : ExpressionType )  =
        { operator = operator
        ; firstOperand = firstOperand
        ; secondOperand = secondOperand
        }
