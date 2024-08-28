// Copyright 2024 © Deltares
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

module MathematicalExpressionParser.Core.ExpressionTreeTest

open MathematicalExpressionParser.Core.ExpressionTree

open FsUnit
open NUnit.Framework

[<Test>]
let ``binaryFunctions: Expected functions defined``() =
    Expression.binaryFunctions |> should haveLength 2
    Expression.binaryFunctions |> should contain ("min", Min)
    Expression.binaryFunctions |> should contain ("max", Max)


[<Test>]
let ``infixFunctions: Expected functions defined``() =
    Expression.infixFunctions |> should haveLength 4
    Expression.infixFunctions |> should contain ("+", Addition, 1)
    Expression.infixFunctions |> should contain ("-", Subtraction, 1)
    Expression.infixFunctions |> should contain ("*", Multiplication, 2)
    Expression.infixFunctions |> should contain ("/", Division, 2)


[<Test>]
let ``composedExpression: Expected results``() =
    // Setup
    let expectedOperator = InfixFunction Addition
    let expectedOperand1 = AtomicExpression ( Parameter "A" )
    let expectedOperand2 = AtomicExpression ( Parameter "B" )

    // Call
    let result = Expression.composedExpression expectedOperator 
                                               expectedOperand1
                                               expectedOperand2

    // Assert
    result.operator      |> should sameAs expectedOperator
    result.firstOperand  |> should sameAs expectedOperand1
    result.secondOperand |> should sameAs expectedOperand2
