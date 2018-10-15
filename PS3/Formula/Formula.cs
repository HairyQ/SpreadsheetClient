// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Normalizer delegate for transforming variables given certain parameters
        /// </summary>
        private Func<string, string> normalizer;

        /// <summary>
        /// Validator delegate for determining whether or not a given variable is a
        /// "valid" variable, as determined by the user
        /// </summary>
        private Func<string, bool> validator;

        /// <summary>
        /// Stack containing the values of doubles and variables in the formula
        /// </summary>
        private Stack<double> values;

        /// <summary>
        /// Stack containing operators given in the formula represented as strings
        /// </summary>
        private Stack<string> operators;

        /// <summary>
        /// Normalized string representing the given formula
        /// </summary>
        private string expression;

        /// <summary>
        /// Currently calculated value of the expression so far
        /// </summary>
        private double currVal;

        /// <summary>
        /// Placeholder FormulaError for passing an error from a helper method
        /// </summary>
        private FormulaError fE;

        /// <summary>
        /// Placeholder FormulaError for passing an error from a helper method
        /// </summary>
        private FormulaError gibberish = new FormulaError("vjaspv-023572395-=409uds256632iabfuiaw");

        /// <summary>
        /// Running list of all variables involved in the formula
        /// </summary>
        private ArrayList variables;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
            values = new Stack<double>();
            operators = new Stack<string>();
            fE = gibberish;
            variables = new ArrayList();

            normalizer = s => s;
            validator = s => true;

            FormulaConstructorMethod(formula, normalizer, validator);
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            values = new Stack<double>();
            operators = new Stack<string>();
            fE = gibberish;
            variables = new ArrayList();

            if (normalize == null)
            {
                normalizer = s => s;
                validator = isValid;
            } else if (isValid == null)
            {
                normalizer = normalize;
                validator = s => true;
            } else
            {
                normalizer = normalize;
                validator = isValid;
            }

            FormulaConstructorMethod(formula, normalizer, validator);
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            try
            {
                operators.Clear();
                values.Clear();

                if (expression == "")
                    return new FormulaError("Formula cannot be empty");

                foreach (string token in GetTokens(expression))
                {
                    //Regex for finding variables
                    if (Regex.IsMatch(token, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
                    {
                        currVal = lookup(normalizer(token));
                        VariableDoubleInstructions(currVal);
                        if (!fE.Reason.Equals(gibberish.Reason))
                        {
                            return fE;
                        }
                    }
                    //Regex for finding just doubles (and ints)
                    else if (Regex.IsMatch(token, @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?") || Regex.IsMatch(token, @"(\s?[0-9]{1,}\s?)"))
                    {
                        double.TryParse(token, out currVal);
                        VariableDoubleInstructions(currVal);

                        if (!fE.Equals(gibberish)) //Had to use this to pass a FormulaError from a helper method - 
                        {                          //gibberish should never be returned, just represents whether or 
                            return fE;             //not a Formula error was returned from VariableDoubleInstructions()
                        }                          //based on whether or not gibberish has changed
                    }
                    //Regex for finding '+' and '-' operators
                    else if (Regex.IsMatch(token, @"(\s?\+|\-\s?)"))
                    {
                        if (operators.Count > 0 && operators.Peek().Equals("+"))
                        {
                            if (values.Count < 2)
                            {
                                return new FormulaError("Operators must have two operands");
                            }
                            operators.Pop();
                            values.Push(values.Pop() + values.Pop());
                            operators.Push(token);
                        }
                        else if (operators.Count > 0 && operators.Peek().Equals("-"))
                        {
                            if (values.Count < 2)
                            {
                                return new FormulaError("Operators must have two operands");
                            }
                            operators.Pop();
                            double placeHolder = values.Pop();
                            values.Push(values.Pop() - placeHolder);
                            operators.Push(token);
                        }
                        else // '+' or '-' is not at the top of operators stack
                            operators.Push(Regex.IsMatch(token, @"(\s?\+\s?)") ? "+" : "-");

                    }
                    //Regex for finding '*' and '/' operators
                    else if (Regex.IsMatch(token, @"(\s?\*|\/\s?)"))
                    {
                        operators.Push(Regex.IsMatch(token, @"(\s?\*\s?)") ? "*" : "/");
                    }
                    //Regex for finding '(' left parentheses
                    else if (Regex.IsMatch(token, @"(\s?\(\s?)"))
                    {
                        operators.Push("(");
                    }
                    //Regex for finding ')' right parentheses
                    else if (Regex.IsMatch(token, @"(\s?\)\s?)"))
                    {
                        if (!operators.Contains("("))
                        {
                            return new FormulaError("Mismatched Parentheses");
                        }

                        if (operators.Peek().Equals("+"))
                        {
                            if (values.Count < 2)
                            {
                                return new FormulaError("Operators must have two operands");
                            }
                            operators.Pop();
                            values.Push(values.Pop() + values.Pop());
                        }
                        else if (operators.Peek().Equals("-"))
                        {
                            if (values.Count < 2)
                            {
                                return new FormulaError("Operators must have two operands");
                            }
                            operators.Pop();
                            double placeHolder = values.Pop();
                            values.Push(values.Pop() - placeHolder);
                        }

                        //Next operator should be '(' - pop it.
                        operators.Pop();

                        if (operators.Count > 0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))
                        {
                            VariableDoubleInstructions(values.Pop());
                        }
                    }

                }
                //Final token has been processed - time to take final steps
                if (operators.Count.Equals(0))
                {
                    if (!values.Count.Equals(1))
                    {
                        return new FormulaError("Expression contains too many or too few integers and variables");
                    }
                    double retVal = values.Pop();
                    return retVal;
                }
                else if (operators.Count > 0 && operators.Peek().Equals("+"))
                {
                    if (!values.Count.Equals(2))
                    {
                        return new FormulaError("Expression contains too many or too few integers and variables");
                    }
                    if (operators.Count >= 2)
                    {
                        return new FormulaError("Expression contains too many operators");
                    }
                    double retVal = values.Pop() + values.Pop();

                    return retVal;
                }
                else if (operators.Count > 0 && operators.Peek().Equals("-"))
                {
                    if (!values.Count.Equals(2))
                    {
                        return new FormulaError("Expression contains too many or too few integers and variables");
                    }
                    double placeHolder = values.Pop();
                    double retVal = values.Pop() - placeHolder;

                    return retVal;
                }
                else
                {
                    if (operators.Count > 0)
                    {
                        return new FormulaError("Expression contains too many operators per operand");
                    }
                    return new FormulaError("Expression contains too few operators or has other syntax problems");
                }
            } catch (ArgumentException e)
            {
                return new FormulaError();
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            foreach (string variable in variables)
            {
                yield return variable;
            }
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return expression;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            try
            {
                return GetHashCode() == obj.GetHashCode();
            } catch (NullReferenceException e)
            {
                return false;
            }
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if ((object)f1 == null && (object)f2 == null)
                return true;
            if ((object)f1 == null || (object)f2 == null)
                return false;

            return f1.GetHashCode() == f2.GetHashCode();
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if ((object)f1 == null && (object)f2 == null)
                return false;
            if ((object)f1 == null || (object)f2 == null)
                return true;

                return f1.GetHashCode() != f2.GetHashCode();
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return expression.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        /// <summary>
        /// Integer and Variable tokens use the same instructions, so to avoid redundant code, I included this function.
        /// 
        /// </summary>
        /// <param name="currVal">Integer or Variable Value</param>
        private void VariableDoubleInstructions(double currVal)
        {
            //Exception case: The stack is empty, and can't be "peeked"
            if (operators.Count > 0 && operators.Peek().Equals("*"))
            {
                operators.Pop();
                currVal *= values.Pop();
                values.Push(currVal);
                
            }
            else if (operators.Count > 0 && operators.Peek().Equals("/"))
            {
                operators.Pop();
                double placeHolder = values.Pop();
                currVal = placeHolder / currVal;
                if (double.IsInfinity(currVal))
                {
                    fE = new FormulaError("Cannot divide by 0");
                } else { 
                    values.Push(currVal);
                }
            }
            else
            {
                values.Push(currVal);
            }
        }

        /// <summary>
        /// Helps the formula constructor create a string representation (without spaces)
        /// of the formula, and builds the Arraylist of variables within the formula
        /// </summary>
        /// <param name="formula">String passed to formula</param>
        /// <param name="normalizer">Normalizer delegate</param>
        /// <param name="validator">Validator delegate</param>
        private void FormulaConstructorMethod(string formula, Func<string, string> normalizer, Func<string, bool> validator)
        {
            StringBuilder finalString = new StringBuilder();

            int tokenCount = 0;
            int numOperators = 0;
            int numOperands = 0;
            int numParenth = 0;

            foreach (string token in GetTokens(formula))
            {
                tokenCount++;

                if (Regex.IsMatch(token, @"^\d+") || token.Contains('.'))  
                {                                                          
                    double.TryParse(token, out double currDouble);
                    string s = currDouble.ToString();
                    numOperands++;

                    finalString.Append(currDouble.ToString());
                }
                else if (Regex.IsMatch(token, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
                {
                    if (!validator(normalizer(token)))
                    {
                        throw new FormulaFormatException("Formula contains invalid variable");
                    }
                    numOperands++;

                    if (!variables.Contains(normalizer(token)))
                    {
                        variables.Add(normalizer(token));
                    }
                    finalString.Append(normalizer(token).ToString());
                }
                else
                {
                    if (Regex.IsMatch(token, @"\+|\-|\*|\/"))
                    {
                        numOperators++;
                    } else if (Regex.IsMatch(token, @"\("))
                    {
                        numParenth++;
                    } else if (Regex.IsMatch(token, @"\)"))
                    {
                        numParenth--;
                    }
                    finalString.Append(token.ToString());
                }
            }

            if (numParenth != 0)
            {
                throw new FormulaFormatException("Formula contains mismatched parentheses");
            }

            if (tokenCount == 0)
            {
                throw new FormulaFormatException("Formula cannot be empty");
            }

            if (numOperators >= numOperands)
            {
                throw new FormulaFormatException("Operators cannot be greater than or equal to operands");
            }

            if ((numOperands - numOperators) > 1)
            {
                throw new FormulaFormatException("Too few operators per operand");
            }

            expression = finalString.ToString();
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}
