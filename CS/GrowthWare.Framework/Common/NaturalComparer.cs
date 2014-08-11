using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace GrowthWare.Framework.Common
{
    /// <summary>
    /// Class NaturalComparer
    /// </summary>
    public class NaturalComparer : IComparer<string>, IComparer
    {

        private StringParser mParser1;
        private StringParser mParser2;
        private NaturalComparerOptions mNaturalComparerOptions;
        private bool mSortAscending = true;

        private enum TokenType
        {
            Nothing,
            Numerical,
            String
        }

        private class StringParser
        {
            private TokenType mTokenType;
            private string mStringValue;
            private decimal mNumericalValue;
            private int mIdx;
            private string mSource;
            private int mLen;
            private char mCurChar;
            private NaturalComparer mNaturalComparer;

            /// <summary>
            /// Initializes a new instance of the <see cref="StringParser" /> class.
            /// </summary>
            /// <param name="naturalComparer">The natural comparer.</param>
            public StringParser(NaturalComparer naturalComparer)
            {
                mNaturalComparer = naturalComparer;
            }

            /// <summary>
            /// Inits the specified source.
            /// </summary>
            /// <param name="source">The source.</param>
            public void Init(string source)
            {
                if (source == null) source = string.Empty;
                mSource = source;
                mLen = source.Length;
                mIdx = -1;
                mNumericalValue = 0;
                NextChar();
                NextToken();
            }

            /// <summary>
            /// Gets the type of the token.
            /// </summary>
            /// <value>The type of the token.</value>
            public TokenType TokenType
            {
                get { return mTokenType; }
            }

            /// <summary>
            /// Gets the numerical value.
            /// </summary>
            /// <value>The numerical value.</value>
            /// <exception cref="GrowthWare.Framework.Common.NaturalComparerException"></exception>
            public decimal NumericalValue
            {
                get
                {
                    if (mTokenType == NaturalComparer.TokenType.Numerical)
                    {
                        return mNumericalValue;
                    }
                    else
                    {
                        throw new NaturalComparerException("Internal Error: NumericalValue called on a non numerical value.");
                    }
                }
            }

            /// <summary>
            /// Gets the string value.
            /// </summary>
            /// <value>The string value.</value>
            public string StringValue
            {
                get { return mStringValue; }
            }

            /// <summary>
            /// Nexts the token.
            /// </summary>
            public void NextToken()
            {
                if (mCurChar == 0)
                {
                    mTokenType = NaturalComparer.TokenType.Nothing;
                    mStringValue = null;
                    return;
                }
                else if (char.IsDigit(mCurChar))
                {
                    ParseNumericalValue();
                    return;
                }
                else
                {
                    //ignore this character and loop some more
                    //NextChar()
                    ParseString();
                    return;
                }
            }

            private void NextChar()
            {
                mIdx += 1;
                if (mIdx >= mLen)
                {
                    mCurChar = '\0';
                }
                else
                {
                    mCurChar = mSource[mIdx];
                }
            }

            private void ParseNumericalValue()
            {
                int start = mIdx;
                char NumberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
                char NumberGroupSeparator = NumberFormatInfo.CurrentInfo.NumberGroupSeparator[0];
                do
                {
                    NextChar();
                    if (mCurChar == NumberDecimalSeparator)
                    {
                        // parse digits after the Decimal Separator
                        do
                        {
                            NextChar();
                            if (!char.IsDigit(mCurChar) && mCurChar != NumberGroupSeparator) break; // TODO: might not be correct. Was : Exit Do

                        }
                        while (true);
                        break; // TODO: might not be correct. Was : Exit Do
                    }
                    else
                    {
                        if (!char.IsDigit(mCurChar) && mCurChar != NumberGroupSeparator) break; // TODO: might not be correct. Was : Exit Do

                    }
                }
                while (true);
                mStringValue = mSource.Substring(start, mIdx - start);
                if (decimal.TryParse(mStringValue, out mNumericalValue))
                {
                    mTokenType = NaturalComparer.TokenType.Numerical;
                }
                else
                {
                    // We probably have a too long value
                    mTokenType = NaturalComparer.TokenType.String;
                }
            }

            private void ParseString()
            {
                int start = mIdx;
                bool roman = (mNaturalComparer.mNaturalComparerOptions & NaturalComparerOptions.RomanNumbers) != 0;
                int romanValue = 0;
                int lastRoman = int.MaxValue;
                int cptLastRoman = 0;
                do
                {
                    if (roman)
                    {
                        int thisRomanValue = NaturalComparer.RomanLetterValue(mCurChar);
                        if (thisRomanValue > 0)
                        {
                            bool handled = false;

                            if ((thisRomanValue == 1 || thisRomanValue == 10 || thisRomanValue == 100))
                            {
                                NextChar();
                                int nextRomanValue = NaturalComparer.RomanLetterValue(mCurChar);
                                if (nextRomanValue == thisRomanValue * 10 | nextRomanValue == thisRomanValue * 5)
                                {
                                    handled = true;
                                    if (nextRomanValue <= lastRoman)
                                    {
                                        romanValue += nextRomanValue - thisRomanValue;
                                        NextChar();
                                        lastRoman = thisRomanValue / 10;
                                        cptLastRoman = 0;
                                    }
                                    else
                                    {
                                        roman = false;
                                    }
                                }
                            }
                            else
                            {
                                NextChar();
                            }
                            if (!handled)
                            {
                                if (thisRomanValue <= lastRoman)
                                {
                                    romanValue += thisRomanValue;
                                    if (lastRoman == thisRomanValue)
                                    {
                                        cptLastRoman += 1;
                                        switch (thisRomanValue)
                                        {
                                            case 1:
                                            case 10:
                                            case 100:
                                                if (cptLastRoman > 4) roman = false;
                                                break;
                                            case 5:
                                            case 50:
                                            case 500:
                                                if (cptLastRoman > 1) roman = false;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        lastRoman = thisRomanValue;
                                        cptLastRoman = 1;
                                    }
                                }
                                else
                                {
                                    roman = false;
                                }
                            }
                        }
                        else
                        {
                            roman = false;
                        }
                    }
                    else
                    {
                        NextChar();
                    }
                    if (!char.IsLetter(mCurChar)) break; // TODO: might not be correct. Was : Exit Do

                }
                while (true);
                mStringValue = mSource.Substring(start, mIdx - start);
                if (roman)
                {
                    mNumericalValue = romanValue;
                    mTokenType = NaturalComparer.TokenType.Numerical;
                }
                else
                {
                    mTokenType = NaturalComparer.TokenType.String;
                }
            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalComparer" /> class.
        /// </summary>
        /// <param name="NaturalComparerOptions">The natural comparer options.</param>
        public NaturalComparer(NaturalComparerOptions NaturalComparerOptions)
        {
            mNaturalComparerOptions = NaturalComparerOptions;
            mParser1 = new StringParser(this);
            mParser2 = new StringParser(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalComparer" /> class.
        /// </summary>
        /// <param name="NaturalComparerOptions">The natural comparer options.</param>
        /// <param name="Direction">The direction.</param>
        public NaturalComparer(NaturalComparerOptions NaturalComparerOptions, NaturalComparerDirection Direction)
        {
            if (Direction == NaturalComparerDirection.Descending) mSortAscending = false;
            mNaturalComparerOptions = NaturalComparerOptions;
            mParser1 = new StringParser(this);
            mParser2 = new StringParser(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalComparer" /> class.
        /// </summary>
        public NaturalComparer()
            : this(NaturalComparerOptions.Default)
        {
        }

        /// <summary>
        /// Compares the specified string1.
        /// </summary>
        /// <param name="string1">The string1.</param>
        /// <param name="string2">The string2.</param>
        /// <returns>System.Int32.</returns>
        public int Compare(string string1, string string2)
        {
            mParser1.Init(string1);
            mParser2.Init(string2);
            int result = 0;
            do
            {
                if (mParser1.TokenType == TokenType.Numerical & mParser2.TokenType == TokenType.Numerical)
                {
                    // both string1 and string2 are numerical 
                    result = decimal.Compare(mParser1.NumericalValue, mParser2.NumericalValue);
                }
                else
                {
                    result = string.Compare(mParser1.StringValue, mParser2.StringValue);
                }
                if (result != 0)
                {
                    // if the sort direction is decending the reverse the result
                    if (!mSortAscending)
                    {
                        if (result == -1) result = 1; else result = -1;
                    }
                    return result;
                }
                else
                {
                    mParser1.NextToken();
                    mParser2.NextToken();
                }
            }
            while (!(mParser1.TokenType == TokenType.Nothing & mParser2.TokenType == TokenType.Nothing));
            return 0;
            //identical
        }

        private static int RomanLetterValue(char c)
        {
            switch (c)
            {
                case 'I':
                    return 1;
                case 'V':
                    return 5;
                case 'X':
                    return 10;
                case 'L':
                    return 50;
                case 'C':
                    return 100;
                case 'D':
                    return 500;
                case 'M':
                    return 1000;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Romans the value.
        /// </summary>
        /// <param name="string1">The string1.</param>
        /// <returns>System.Int32.</returns>
        public int RomanValue(string string1)
        {
            mParser1.Init(string1);

            if (mParser1.TokenType == TokenType.Numerical)
            {
                return (int)mParser1.NumericalValue;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Is the comparer_ compare.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>System.Int32.</returns>
        public int IComparer_Compare(object x, object y)
        {
            return Compare((string)x, (string)x);
        }

        int System.Collections.IComparer.Compare(object x, object y)
        {
            return IComparer_Compare(x, y);
        }
    }

    /// <summary>
    /// Enum NaturalComparerOptions
    /// </summary>
    public enum NaturalComparerOptions
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// RomanNumbers
        /// </summary>
        RomanNumbers,

        //DecimalValues <- we could put this as an option
        //IgnoreSpaces  <- we could put this as an option
        //IgnorePunctuation <- we could put this as an option

        /// <summary>
        /// Default
        /// </summary>
        Default = None
    }

    /// <summary>
    /// Enum NaturalComparerDirection
    /// </summary>
    [System.Flags()]
    public enum NaturalComparerDirection
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Ascending
        /// </summary>
        Ascending,
        /// <summary>
        /// Descending
        /// </summary>
        Descending
    }

    /// <summary>
    /// Class NaturalComparerException
    /// </summary>
    [Serializable]
    public class NaturalComparerException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalComparerException" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public NaturalComparerException(string msg)
            : base(msg)
        {
        }
    }
}
