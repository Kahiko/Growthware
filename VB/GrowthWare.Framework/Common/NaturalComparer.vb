Imports System.Globalization
Imports System.Runtime.Serialization

Namespace Common
    Public Class NaturalComparer
        Implements IComparer(Of String)
        Implements IComparer

        Private m_Parser1 As StringParser
        Private m_Parser2 As StringParser
        Private m_NaturalComparerOptions As NaturalComparerOption
        Private m_SortAscending As Boolean = True

        Private Enum TokenType
            [Nothing]
            Numerical
            [String]
        End Enum


        Private Class StringParser
            Private m_TokenType As TokenType
            Private m_StringValue As String
            Private m_NumericalValue As Decimal
            Private m_Idx As Integer
            Private m_Source As String
            Private m_Len As Integer
            Private m_CurChar As Char
            Private m_NaturalComparer As NaturalComparer

            Sub New(ByVal naturalComparer As NaturalComparer)
                m_NaturalComparer = naturalComparer
            End Sub

            Public Sub Init(ByVal source As String)
                If source Is Nothing Then source = String.Empty
                m_Source = source
                m_Len = source.Length
                m_Idx = -1
                m_NumericalValue = 0
                NextChar()
                NextToken()
            End Sub

            Public ReadOnly Property TokenType() As TokenType
                Get
                    Return m_TokenType
                End Get
            End Property

            Public ReadOnly Property NumericalValue() As Decimal
                Get
                    If m_TokenType = NaturalComparer.TokenType.Numerical Then
                        Return m_NumericalValue
                    Else
                        Throw New NaturalComparerException("Internal Error: Numeric value called on a non numerical value.")
                    End If
                End Get
            End Property

            Public ReadOnly Property StringValue() As String
                Get
                    Return m_StringValue
                End Get
            End Property

            Public Sub NextToken()
                Do
                    'CharUnicodeInfo.GetUnicodeCategory 
                    If m_CurChar = Nothing Then
                        m_TokenType = NaturalComparer.TokenType.Nothing
                        m_StringValue = Nothing
                        Exit Sub
                    ElseIf Char.IsDigit(m_CurChar) Then
                        ParseNumericalValue()
                        Exit Sub
                        'ElseIf Char.IsLetter(mCurChar) Then
                        '    ParseString()
                        '    Exit Sub
                    Else
                        'ignore this character and loop some more
                        'NextChar()
                        ParseString()
                        Exit Sub
                    End If
                Loop
            End Sub

            Private Sub NextChar()
                m_Idx += 1
                If m_Idx >= m_Len Then
                    m_CurChar = Nothing
                Else
                    m_CurChar = m_Source(m_Idx)
                End If
            End Sub

            Private Sub ParseNumericalValue()
                Dim start As Integer = m_Idx
                Dim NumberDecimalSeparator As Char = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator(0)
                Dim NumberGroupSeparator As Char = NumberFormatInfo.CurrentInfo.NumberGroupSeparator(0)
                Do
                    NextChar()
                    If m_CurChar = NumberDecimalSeparator Then
                        ' parse digits after the Decimal Separator
                        Do
                            NextChar()
                            If Not Char.IsDigit(m_CurChar) AndAlso m_CurChar <> NumberGroupSeparator Then Exit Do
                        Loop
                        Exit Do
                    Else
                        If Not Char.IsDigit(m_CurChar) AndAlso m_CurChar <> NumberGroupSeparator Then Exit Do
                    End If
                Loop
                m_StringValue = m_Source.Substring(start, m_Idx - start)
                If Decimal.TryParse(m_StringValue, m_NumericalValue) Then
                    m_TokenType = NaturalComparer.TokenType.Numerical
                Else
                    ' We probably have a too long value
                    m_TokenType = NaturalComparer.TokenType.String
                End If
            End Sub

            Private Sub ParseString()
                Dim start As Integer = m_Idx
                Dim roman As Boolean = (m_NaturalComparer.m_NaturalComparerOptions And NaturalComparerOption.RomanNumber) <> 0
                Dim romanValue As Integer
                Dim lastRoman As Integer = Integer.MaxValue
                Dim cptLastRoman As Integer
                Do
                    If roman Then
                        Dim thisRomanValue As Integer = RomanLetterValue(m_CurChar)
                        If thisRomanValue > 0 Then
                            Dim handled As Boolean = False

                            If (thisRomanValue = 1 OrElse thisRomanValue = 10 OrElse thisRomanValue = 100) Then
                                NextChar()
                                Dim nextRomanValue As Integer = RomanLetterValue(m_CurChar)
                                If nextRomanValue = thisRomanValue * 10 Or nextRomanValue = thisRomanValue * 5 Then
                                    handled = True
                                    If nextRomanValue <= lastRoman Then
                                        romanValue += nextRomanValue - thisRomanValue
                                        NextChar()
                                        lastRoman = thisRomanValue \ 10
                                        cptLastRoman = 0
                                    Else
                                        roman = False
                                    End If
                                End If
                            Else
                                NextChar()
                            End If
                            If Not handled Then
                                If thisRomanValue <= lastRoman Then
                                    romanValue += thisRomanValue
                                    If lastRoman = thisRomanValue Then
                                        cptLastRoman += 1
                                        Select Case thisRomanValue
                                            Case 1, 10, 100
                                                If cptLastRoman > 4 Then roman = False
                                            Case 5, 50, 500
                                                If cptLastRoman > 1 Then roman = False
                                        End Select
                                    Else
                                        lastRoman = thisRomanValue
                                        cptLastRoman = 1
                                    End If
                                Else
                                    roman = False
                                End If
                            End If
                        Else
                            roman = False
                        End If
                    Else
                        NextChar()
                    End If
                    If Not Char.IsLetter(m_CurChar) Then Exit Do
                Loop
                m_StringValue = m_Source.Substring(start, m_Idx - start)
                If roman Then
                    m_NumericalValue = romanValue
                    m_TokenType = NaturalComparer.TokenType.Numerical
                Else
                    m_TokenType = NaturalComparer.TokenType.String
                End If
            End Sub

        End Class

        Sub New(ByVal naturalComparerOptions As NaturalComparerOption)
            m_NaturalComparerOptions = naturalComparerOptions
            m_Parser1 = New StringParser(Me)
            m_Parser2 = New StringParser(Me)
        End Sub

        Sub New(ByVal naturalComparerOptions As NaturalComparerOption, ByVal direction As NaturalComparerDirections)
            If direction = NaturalComparerDirections.Descending Then m_SortAscending = False
            m_NaturalComparerOptions = naturalComparerOptions
            m_Parser1 = New StringParser(Me)
            m_Parser2 = New StringParser(Me)
        End Sub

        Sub New()
            MyClass.New(NaturalComparerOption.Default)
        End Sub

        ''' <summary>
        ''' Compares the x y.
        ''' </summary>
        ''' <param name="x">The string1.</param>
        ''' <param name="y">The string2.</param>
        ''' <returns>System.Int32.</returns>
        Public Function Compare(ByVal x As String, ByVal y As String) As Integer Implements System.Collections.Generic.IComparer(Of String).Compare
            m_Parser1.Init(x)
            m_Parser2.Init(y)
            Dim result As Integer
            Do
                If m_Parser1.TokenType = TokenType.Numerical And m_Parser2.TokenType = TokenType.Numerical Then
                    ' both string1 and string2 are numerical 
                    result = Decimal.Compare(m_Parser1.NumericalValue, m_Parser2.NumericalValue)
                Else
                    result = String.Compare(m_Parser1.StringValue, m_Parser2.StringValue, StringComparison.OrdinalIgnoreCase)
                End If
                If result <> 0 Then
                    ' if the sort direction is decending the reverse the result
                    If Not m_SortAscending Then
                        If result = -1 Then result = 1 Else result = -1
                    End If
                    Return result
                Else
                    m_Parser1.NextToken()
                    m_Parser2.NextToken()
                End If
            Loop Until m_Parser1.TokenType = TokenType.Nothing And m_Parser2.TokenType = TokenType.Nothing
            Return 0 'identical
        End Function

        Private Shared Function RomanLetterValue(ByVal c As Char) As Integer
            Select Case c
                Case "I"c
                    Return 1
                Case "V"c
                    Return 5
                Case "X"c
                    Return 10
                Case "L"c
                    Return 50
                Case "C"c
                    Return 100
                Case "D"c
                    Return 500
                Case "M"c
                    Return 1000
                Case Else
                    Return 0
            End Select
        End Function

        ''' <summary>
        ''' Romans the value.
        ''' </summary>
        ''' <param name="theValue">The string1.</param>
        ''' <returns>System.Int32.</returns>
        Public Function RomanValue(ByVal theValue As String) As Integer
            m_Parser1.Init(theValue)

            If m_Parser1.TokenType = TokenType.Numerical Then
                Return CInt(m_Parser1.NumericalValue)
            Else
                Return 0
            End If
        End Function

        ''' <summary>
        ''' Is the comparer_ compare.
        ''' </summary>
        ''' <param name="x">The x.</param>
        ''' <param name="y">The y.</param>
        ''' <returns>System.Int32.</returns>
        Public Function IComparerCompare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Return Compare(DirectCast(x, String), DirectCast(y, String))
        End Function
    End Class

    Public Enum NaturalComparerOption
        None
        RomanNumber
        'DecimalValues <- we could put this as an option
        'IgnoreSpaces  <- we could put this as an option
        'IgnorePunctuation <- we could put this as an option
        [Default] = None
    End Enum

    <System.Flags()> Public Enum NaturalComparerDirections
        None
        Ascending
        Descending
    End Enum

    <Serializable()>
    Public Class NaturalComparerException
        Inherits Exception

        ''' <summary>
        ''' Initializes a new instance of the <see cref="NaturalComparerException" /> class.
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Calls base method
        ''' </summary>
        ''' <param name="message">String</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Calls base method
        ''' </summary>
        ''' <param name="message">String</param>
        ''' <param name="innerException">Exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        ''' <summary>
        ''' Calls base method
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class
End Namespace
