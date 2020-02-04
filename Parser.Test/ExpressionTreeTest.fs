module MathematicalExpressionParser.Parser.ExpressionTreeTest

open MathematicalExpressionParser.Parser.ExpressionTree

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
