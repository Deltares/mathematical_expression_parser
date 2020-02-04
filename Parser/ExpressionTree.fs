namespace MathematicalExpressionParser.Parser.ExpressionTree

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
