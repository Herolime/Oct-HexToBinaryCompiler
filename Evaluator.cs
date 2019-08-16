using System;
using System.Collections.Generic;
using System.Linq;
using OctAndHexToBinaryCompiler.CodeAnalysis.Diagnostics;
using OctAndHexToBinaryCompiler.CodeAnalysis.SyntaxAnalysis;
using OctAndHexToBinaryCompiler.CodeInterpretation.Binding;

namespace OctAndHexToBinaryCompiler
{
    public sealed class EvaluationResult
    {
        public EvaluationResult( IEnumerable<Diagnostic> diagnostics, object value)
        {
            Diagnostics = diagnostics.ToArray();
            Value = value;  
        }

        public IReadOnlyList<Diagnostic> Diagnostics { get; }
        public object Value { get; }
    }

    internal sealed class Evaluator
   {
       private readonly BoundExpression _root;

       public Evaluator(BoundExpression Root)
       {
           _root = Root;
       }

       public object Evaluate()
       {
           return EvaluateExpression(_root);
       }

        private object EvaluateExpression(BoundExpression node)
        {
            //Binary Expression
            //Number Expression 

            if (node is BoundLiteralExpression n) 
            {
                if (n.SyntaxKind == SyntaxKind.OctToken)
                {
                    if (n.Value.GetType() == typeof(string))
                    {
                        string binary = string.Empty;
                        foreach(var numchar in (string)n.Value)  
                        {  
                            switch (numchar)  
                            { 
                            case '0': 
                                binary += "000"; 
                                break; 
                            case '1': 
                                binary += "001"; 
                                break; 
                            case '2': 
                                binary += "010"; 
                                break; 
                            case '3': 
                                binary += "011"; 
                                break; 
                            case '4': 
                                binary += "100"; 
                                break; 
                            case '5': 
                                binary += "101"; 
                                break; 
                            case '6': 
                                binary += "110"; 
                                break; 
                            case '7': 
                                binary += "111"; 
                                break; 
                            default: 
                                throw new Exception( $"Invalid Octal Digit {numchar}"); 
                            } 
                        }
                        return binary;  
                    }
                    else 
                    {
                        throw new Exception( $"Unable to parse Oct Number of type {n.Type}");
                    }
                }
                else if (n.SyntaxKind == SyntaxKind.HexToken)
                {
                    if (n.Value.GetType() == typeof(string))
                    {
                        string binary = string.Empty;
                        foreach (var numchar in (string)n.Value)
                        {
                            switch (numchar)  
                            { 
                            case '0': 
                                binary +="0000"; 
                                break; 
                            case '1': 
                                binary +="0001"; 
                                break; 
                            case '2': 
                                binary += "0010"; 
                                break; 
                            case '3': 
                                binary += "0011"; 
                                break; 
                            case '4': 
                                binary+= "0100"; 
                                break;
                            case '5': 
                                binary+= "0101"; 
                                break; 
                            case '6': 
                                binary+= "0110"; 
                                break; 
                            case '7': 
                                binary+= "0111"; 
                                break; 
                            case '8': 
                                binary+= "1000"; 
                                break; 
                            case '9': 
                                binary+= "1001"; 
                                break; 
                            case 'A': 
                                binary+= "1010"; 
                                break; 
                            case 'B': 
                                binary+= "1011"; 
                                break; 
                            case 'C': 
                                binary+= "1100"; 
                                break; 
                            case 'D': 
                                binary+= "1101"; 
                                break; 
                            case 'E': 
                            case 'e': 
                                binary+= "1110"; 
                                break; 
                            case 'F': 
                            case 'f': 
                                binary+= "1111"; 
                                break; 
                            default: 
                                throw new Exception($"Invalid hexadecimal digit {numchar}"); 
                            } 
                        }
                        return binary;
                    }
                    else
                    {
                        throw new Exception( $"Unable to parse Hex Number of type {n.Type}");
                    }
                }
                else throw new Exception( $"Unable to parse Value of type {n.Type}");
            }
            else if (node is BoundBinaryExpression b) 
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                switch (b.Op.Kind)
                {
                    case BoundBinaryOperatorKind.Addition:
                    {
                        var number_one = Convert.ToInt32((string)left, 2);
                        var number_two = Convert.ToInt32((string)right, 2);
                        return Convert.ToString(number_one + number_two, 2);
                    }
                    case BoundBinaryOperatorKind.Substraction:
                    {
                        var number_one = Convert.ToInt32((string)left, 2);
                        var number_two = Convert.ToInt32((string)right, 2);
                        return Convert.ToString(number_one - number_two, 2);
                    }
                    case BoundBinaryOperatorKind.Multiplication:
                    {
                        var number_one = Convert.ToInt32((string)left, 2);
                        var number_two = Convert.ToInt32((string)right, 2);
                        return Convert.ToString(number_one * number_two, 2);
                    }
                    default:
                        throw new Exception($"Unexpected binary operator {b.Op}");
                }
            }
            // // else if (node is BoundUnaryExpression u)
            // // {
            // //     var operand = EvaluateExpression(u.Operand);
            // //     switch (u.Op.Kind)
            // //     {
            // //         case BoundUnaryOperatorKind.Identity:
            // //             return (int)operand;
            // //         case BoundUnaryOperatorKind.Negation:
            // //             return -(int)operand;
            // //         case BoundUnaryOperatorKind.LogicalNegation:
            // //             return !(bool) operand;
            // //         default:
            // //             throw new Exception($"Unexpected Unary operator {u.Op}");
            // //     }
            // // }
            // // else if (node is BoundBinaryExpression b) 
            // // {
            // //     var left = EvaluateExpression(b.Left);
            // //     var right = EvaluateExpression(b.Right);
            // //     switch (b.Op.Kind)
            // //     {
            // //         case BoundBinaryOperatorKind.Addition:
            // //             return (int)left + (int)right;
            // //         case BoundBinaryOperatorKind.Substraction:
            // //             return (int)left - (int)right;
            // //         case BoundBinaryOperatorKind.Division:
            // //             return (int)left / (int)right;
            // //         case BoundBinaryOperatorKind.Multiplication:
            // //             return (int)left * (int)right;
            // //         case BoundBinaryOperatorKind.LogicalAnd:
            // //             return (bool)left && (bool)right;
            // //         case BoundBinaryOperatorKind.LogicalOr:
            // //             return (bool)left || (bool)right;
            // //         case BoundBinaryOperatorKind.Equals:
            // //             return Equals(left, right);
            // //         case BoundBinaryOperatorKind.NotEquals:
            // //             return !Equals(left, right);
            // //         default:
            // //             throw new Exception($"Unexpected binary operator {b.Op}");
            // //     }
            // }
            else throw new Exception($"Unexepectec node {node.Kind}");
        }
   }
}
