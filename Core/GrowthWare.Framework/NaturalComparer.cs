using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace GrowthWare.Framework
{
    /// <summary>
    /// Class NaturalComparer
    /// </summary>
    /// <remarks>
    /// Originally from https://www.codeproject.com/articles/22978/implementing-the-net-icomparer-interface-to-get-a
    /// </remarks>
    public class NaturalComparer : IComparer<string>, IComparer
    {
        private StringParser m_Parser1;
        private StringParser m_Parser2;
        private NaturalComparerOption m_NaturalComparerOptions;
        private bool m_SortAscending = true;

        private enum TokenType
        {
            Nothing,
            Numerical,
            String
        }

        private class StringParser
        {
            private TokenType m_TokenType;
            private string m_StringValue;
            private decimal m_NumericalValue;
            private int m_Idx;
            private string m_Source;
            private int m_Len;
            private char m_CurChar;
            private NaturalComparer m_NaturalComparer;

            /// <summary>
            /// Initializes a new instance of the <see cref="StringParser" /> class.
            /// </summary>
            /// <param name="naturalComparer">The natural comparer.</param>
            public StringParser(NaturalComparer naturalComparer)
            {
                m_NaturalComparer = naturalComparer;
            }

            /// <summary>
            /// Inits the specified source.
            /// </summary>
            /// <param name="source">The source.</param>
            public void Init(string source)
            {
                if (source == null) source = string.Empty;
                m_Source = source;
                m_Len = source.Length;
                m_Idx = -1;
                m_NumericalValue = 0;
                NextChar();
                NextToken();
            }

            /// <summary>
            /// Gets the type of the token.
            /// </summary>
            /// <value>The type of the token.</value>
            public TokenType TokenType
            {
                get { return m_TokenType; }
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
                    if (m_TokenType == NaturalComparer.TokenType.Numerical)
                    {
                        return m_NumericalValue;
                    }
                    else
                    {
                        throw new NaturalComparerException("Internal Error: Numeric value called on a non numerical value.");
                    }
                }
            }

            /// <summary>
            /// Gets the string value.
            /// </summary>
            /// <value>The string value.</value>
            public string StringValue
            {
                get { return m_StringValue; }
            }

            /// <summary>
            /// Nexts the token.
            /// </summary>
            public void NextToken()
            {
                if (m_CurChar == 0)
                {
                    m_TokenType = NaturalComparer.TokenType.Nothing;
                    m_StringValue = null;
                    return;
                }
                else if (char.IsDigit(m_CurChar))
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
                m_Idx += 1;
                if (m_Idx >= m_Len)
                {
                    m_CurChar = '\0';
                }
                else
                {
                    m_CurChar = m_Source[m_Idx];
                }
            }

            private void ParseNumericalValue()
            {
                int start = m_Idx;
                char NumberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
                char NumberGroupSeparator = NumberFormatInfo.CurrentInfo.NumberGroupSeparator[0];
                do
                {
                    NextChar();
                    if (m_CurChar == NumberDecimalSeparator)
                    {
                        // parse digits after the Decimal Separator
                        do
                        {
                            NextChar();
                            if (!char.IsDigit(m_CurChar) && m_CurChar != NumberGroupSeparator) break; // TODO: might not be correct. Was : Exit Do

                        }
                        while (true);
                        break; // TODO: might not be correct. Was : Exit Do
                    }
                    else
                    {
                        if (!char.IsDigit(m_CurChar) && m_CurChar != NumberGroupSeparator) break; // TODO: might not be correct. Was : Exit Do

                    }
                }
                while (true);
                m_StringValue = m_Source.Substring(start, m_Idx - start);
                if (decimal.TryParse(m_StringValue, out m_NumericalValue))
                {
                    m_TokenType = NaturalComparer.TokenType.Numerical;
                }
                else
                {
                    // We probably have a too long value
                    m_TokenType = NaturalComparer.TokenType.String;
                }
            }

            private void ParseString()
            {
                int start = m_Idx;
                bool roman = (m_NaturalComparer.m_NaturalComparerOptions & NaturalComparerOption.RomanNumber) != 0;
                int romanValue = 0;
                int lastRoman = int.MaxValue;
                int cptLastRoman = 0;
                do
                {
                    if (roman)
                    {
                        int thisRomanValue = NaturalComparer.RomanLetterValue(m_CurChar);
                        if (thisRomanValue > 0)
                        {
                            bool handled = false;

                            if ((thisRomanValue == 1 || thisRomanValue == 10 || thisRomanValue == 100))
                            {
                                NextChar();
                                int nextRomanValue = NaturalComparer.RomanLetterValue(m_CurChar);
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
                    if (!char.IsLetter(m_CurChar)) break; // TODO: might not be correct. Was : Exit Do

                }
                while (true);
                m_StringValue = m_Source.Substring(start, m_Idx - start);
                if (roman)
                {
                    m_NumericalValue = romanValue;
                    m_TokenType = NaturalComparer.TokenType.Numerical;
                }
                else
                {
                    m_TokenType = NaturalComparer.TokenType.String;
                }
            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalComparer" /> class.
        /// </summary>
        /// <param name="naturalComparerOptions">The natural comparer options.</param>
        public NaturalComparer(NaturalComparerOption naturalComparerOptions)
        {
            m_NaturalComparerOptions = naturalComparerOptions;
            m_Parser1 = new StringParser(this);
            m_Parser2 = new StringParser(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalComparer" /> class.
        /// </summary>
        /// <param name="naturalComparerOptions">The natural comparer options.</param>
        /// <param name="direction">The direction.</param>
        public NaturalComparer(NaturalComparerOption naturalComparerOptions, NaturalComparerDirections direction)
        {
            if (direction == NaturalComparerDirections.Descending) m_SortAscending = false;
            m_NaturalComparerOptions = naturalComparerOptions;
            m_Parser1 = new StringParser(this);
            m_Parser2 = new StringParser(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalComparer" /> class.
        /// </summary>
        public NaturalComparer()
            : this(NaturalComparerOption.Default)
        {
        }

        /// <summary>
        /// Compares the specified x and y.
        /// </summary>
        /// <param name="x">The string1.</param>
        /// <param name="y">The string2.</param>
        /// <returns>System.Int32.</returns>
        public int Compare(string x, string y)
        {
            m_Parser1.Init(x);
            m_Parser2.Init(y);
            int result = 0;
            do
            {
                if (m_Parser1.TokenType == TokenType.Numerical & m_Parser2.TokenType == TokenType.Numerical)
                {
                    // both string1 and string2 are numerical 
                    result = decimal.Compare(m_Parser1.NumericalValue, m_Parser2.NumericalValue);
                }
                else
                {
                    result = string.Compare(m_Parser1.StringValue, m_Parser2.StringValue, StringComparison.OrdinalIgnoreCase);
                }
                if (result != 0)
                {
                    // if the sort direction is decending the reverse the result
                    if (!m_SortAscending)
                    {
                        if (result == -1) result = 1; else result = -1;
                    }
                    return result;
                }
                else
                {
                    m_Parser1.NextToken();
                    m_Parser2.NextToken();
                }
            }
            while (!(m_Parser1.TokenType == TokenType.Nothing & m_Parser2.TokenType == TokenType.Nothing));
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
        /// <param name="theValue">The string1.</param>
        /// <returns>System.Int32.</returns>
        public int RomanValue(string theValue)
        {
            m_Parser1.Init(theValue);

            if (m_Parser1.TokenType == TokenType.Numerical)
            {
                return (int)m_Parser1.NumericalValue;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Is the comparer_ compare.
        /// </summary>
        /// <param name="theValue1">The x.</param>
        /// <param name="theValue2">The y.</param>
        /// <returns>System.Int32.</returns>
        public int IComparerCompare(object theValue1, object theValue2)
        {
            return Compare((string)theValue1, (string)theValue2);
        }

        int System.Collections.IComparer.Compare(object x, object y)
        {
            return IComparerCompare(x, y);
        }
    }

    /// <summary>
    /// Enum NaturalComparerOptions
    /// </summary>
    public enum NaturalComparerOption
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// RomanNumbers
        /// </summary>
        RomanNumber,

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
    public enum NaturalComparerDirections
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
        /// Created to distinguish errors created in the NaturalComparer class.
        /// </summary>
        public NaturalComparerException() { }

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="message">string</param>
		public NaturalComparerException(string message):base(message)
		{
			
		}

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="message">string</param>
		/// <param name="innerException">Exception</param>
		public NaturalComparerException(string message, Exception innerException):base(message, innerException)
		{
		
		}

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
        protected NaturalComparerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
		{
			// Implement type-specific serialization constructor logic.
		}
    }
}
