// Copyright 2025 © Deltares
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

module MathematicalExpressionParser.Core.ExpressionParserTest

open MathematicalExpressionParser.Core.ExpressionTree
open MathematicalExpressionParser.Core.ExpressionParser

open FsUnit
open NUnit.Framework

[<TestFixture>]
type ParseExpressionTest() =
    static let paramExpr str = AtomicExpression ( Parameter str )
    static let constExpr d = AtomicExpression ( Constant d )

    static member parseExpressionParameterCorrectData = 
        [| 
            [| ("Parameter",    Success <| ( paramExpr "Parameter" )) |]
            [| ("Parameter012", Success <| ( paramExpr "Parameter012" )) |]
            [| ("Param3t3r",    Success <| ( paramExpr "Param3t3r" )) |]
            [| ("_Param",       Success <| ( paramExpr "_Param" )) |]
            [| ("_Param3t3r",   Success <| ( paramExpr "_Param3t3r" )) |]
            [| ("A",            Success <| ( paramExpr "A" )) |]
            [| ("ABBCab",       Success <| ( paramExpr "ABBCab" )) |]
            [| ("ABB_Cab",      Success <| ( paramExpr "ABB_Cab" )) |]
        |]

    static member parseExpressionConstantCorrectData = 
        [| 
            [| ("1.0",     Success <| ( constExpr 1.0 )) |]
            [| ("0.55",    Success <| ( constExpr 0.55 )) |]
            [| ("-0.55",   Success <| ( constExpr -0.55 )) |]
            [| ("1E-3",    Success <| ( constExpr 0.001 )) |]
            [| ("1.0E-3",  Success <| ( constExpr 0.001 )) |]
            [| ("1.57E-3", Success <| ( constExpr 0.00157 )) |]
            [| ("1.57E3",  Success <| ( constExpr 1570.0 )) |]
            [| ("-1.57E3", Success <| ( constExpr -1570.0 )) |]
        |]

    static member parseExpressionBinaryFunctionCorrectData = 
        [| 
            [| ("min(A,B)",     Success <| ComposedExpression (Expression.composedExpression ( BinaryFunction Min ) ( paramExpr "A" ) ( paramExpr "B" ))) |]
            [| ("min( A , B )", Success <| ComposedExpression (Expression.composedExpression ( BinaryFunction Min ) ( paramExpr "A" ) ( paramExpr "B" ))) |]
            [| ("max(A,B)",     Success <| ComposedExpression (Expression.composedExpression ( BinaryFunction Max ) ( paramExpr "A" ) ( paramExpr "B" ))) |]
            [| ("max( A , B )", Success <| ComposedExpression (Expression.composedExpression ( BinaryFunction Max ) ( paramExpr "A" ) ( paramExpr "B" ))) |]
            [| ("max( min( A, B) , min( C, D) )", Success <| ComposedExpression (Expression.composedExpression ( BinaryFunction Max ) 
                                                                                                               ( ComposedExpression ( Expression.composedExpression ( BinaryFunction Min ) ( paramExpr "A" ) ( paramExpr "B" ))) 
                                                                                                               ( ComposedExpression ( Expression.composedExpression ( BinaryFunction Min ) ( paramExpr "C" ) ( paramExpr "D" ))) ))|]
            [| ("max( A, min( C, D) )", Success <| ComposedExpression (Expression.composedExpression ( BinaryFunction Max ) 
                                                                                                     ( paramExpr "A" )
                                                                                                     ( ComposedExpression ( Expression.composedExpression ( BinaryFunction Min ) ( paramExpr "C" ) ( paramExpr "D" )))))|]
            [| ("max( min( A, B) ,  C)", Success <| ComposedExpression (Expression.composedExpression ( BinaryFunction Max ) 
                                                                       ( ComposedExpression ( Expression.composedExpression ( BinaryFunction Min ) ( paramExpr "A" ) ( paramExpr "B" ))) 
                                                                       ( paramExpr "C" ))) |]
        |]

    static member parseExpressionInfixFunctionCorrectData = 
        [| 
            [| ("A*B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Multiplication) ( paramExpr "A") ( paramExpr "B" ))) |]
            [| (" A * B ", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Multiplication) ( paramExpr "A") ( paramExpr "B" ))) |]
            [| ("A/B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Division) ( paramExpr "A") ( paramExpr "B" ))) |]
            [| (" A / B ", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Division) ( paramExpr "A") ( paramExpr "B" ))) |]
            [| ("A+B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Addition) ( paramExpr "A") ( paramExpr "B" ))) |]
            [| (" A + B ", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Addition) ( paramExpr "A") ( paramExpr "B" ))) |]
            [| ("A-B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Subtraction) ( paramExpr "A") ( paramExpr "B" ))) |]
            [| (" A - B ", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Subtraction) ( paramExpr "A") ( paramExpr "B" ))) |]
        |]

    static member parseExpressionAssociativityCorrectData =
        [|
            [| ("A*B+C", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Addition) 
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Multiplication) ( paramExpr "A") ( paramExpr "B" )))
                                                                                      (paramExpr "C"))) |]
            [| ("A*B-C", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Subtraction) 
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Multiplication) ( paramExpr "A") ( paramExpr "B" )))
                                                                                      (paramExpr "C"))) |]
            [| ("A/B+C", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Addition) 
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Division) ( paramExpr "A") ( paramExpr "B" )))
                                                                                      (paramExpr "C"))) |]
            [| ("A/B-C", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Subtraction) 
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Division) ( paramExpr "A") ( paramExpr "B" )))
                                                                                      (paramExpr "C"))) |]
            [| ("C+A*B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Addition) 
                                                                                      (paramExpr "C")
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Multiplication) ( paramExpr "A") ( paramExpr "B" ))))) |]
            [| ("C-A*B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Subtraction) 
                                                                                      (paramExpr "C")
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Multiplication) ( paramExpr "A") ( paramExpr "B" ))))) |]
            [| ("C+A/B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Addition) 
                                                                                      (paramExpr "C")
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Division) ( paramExpr "A") ( paramExpr "B" ))))) |]
            [| ("C-A/B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Subtraction) 
                                                                                      (paramExpr "C")
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Division) ( paramExpr "A") ( paramExpr "B" ))))) |]
        |]

    static member parseExpressionParenthesesCorrectData =
        [|
            [| ("A*(B+C)", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Multiplication) 
                                                                                      (paramExpr "A")
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Addition) ( paramExpr "B") ( paramExpr "C" ))))) |]
            [| ("A*(B-C)", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Multiplication) 
                                                                                      (paramExpr "A")
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Subtraction) ( paramExpr "B") ( paramExpr "C" ))))) |]
            [| ("A/(B+C)", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Division) 
                                                                                      (paramExpr "A")
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Addition) ( paramExpr "B") ( paramExpr "C" ))))) |]
            [| ("A/(B-C)", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Division) 
                                                                                      (paramExpr "A")
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Subtraction) ( paramExpr "B") ( paramExpr "C" ))))) |]
            [| ("(C+A)*B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Multiplication) 
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Addition) ( paramExpr "C") ( paramExpr "A" )))
                                                                                      (paramExpr "B"))) |]
            [| ("(C-A)*B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Multiplication) 
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Subtraction) ( paramExpr "C") ( paramExpr "A" )))
                                                                                      (paramExpr "B"))) |]
            [| ("(C+A)/B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Division) 
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Addition) ( paramExpr "C") ( paramExpr "A" )))
                                                                                      (paramExpr "B"))) |]
            [| ("(C-A)/B", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Division) 
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Subtraction) ( paramExpr "C") ( paramExpr "A" )))
                                                                                      (paramExpr "B"))) |]
            [| ("((C)-(A))/(B)", Success <| ComposedExpression (Expression.composedExpression (InfixFunction Division) 
                                                                                      (ComposedExpression (Expression.composedExpression (InfixFunction Subtraction) ( paramExpr "C") ( paramExpr "A" )))
                                                                                      (paramExpr "B"))) |]
        |]

    [<TestCaseSource("parseExpressionParameterCorrectData");
      TestCaseSource("parseExpressionConstantCorrectData");
      TestCaseSource("parseExpressionBinaryFunctionCorrectData");
      TestCaseSource("parseExpressionInfixFunctionCorrectData");
      TestCaseSource("parseExpressionAssociativityCorrectData");
      TestCaseSource("parseExpressionParenthesesCorrectData")>]
    member x.``parseExpression: Correct results`` (data : string * ParserResult) =
        let inputString, expectedResult = data in
        ( Parser.parseExpression inputString ) |> should equal expectedResult

    [<Test>]
    [<TestCase("")>]
    [<TestCase("A B")>]
    [<TestCase("min A, B")>]
    [<TestCase("max A, B")>]
    [<TestCase("func(A, B)")>]
    [<TestCase("min(A, B, C)")>]
    [<TestCase("min(A)")>]
    [<TestCase("A * ( B")>]
    [<TestCase("A * B )")>]
    member x.``parseExpression: Failure to parse`` (inputString : string) =
        match Parser.parseExpression inputString with
        | Failure _ -> Assert.Pass()
        | Success _ -> Assert.Fail("Expected Failure but got Success")
