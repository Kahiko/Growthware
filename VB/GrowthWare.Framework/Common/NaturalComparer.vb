Imports System.Globalization

Namespace Common
    Public Class NaturalComparer
        Implements IComparer(Of String)
        Implements IComparer

        Private mParser1 As StringParser
        Private mParser2 As StringParser
        Private mNaturalComparerOptions As NaturalComparerOptions
        Private mSortAscending As Boolean = True

        Private Enum TokenType
            [Nothing]
            Numerical
            [String]
        End Enum


        Private Class StringParser
            Private mTokenType As TokenType
            Private mStringValue As String
            Private mNumericalValue As Decimal
            Private mIdx As Integer
            Private mSource As String
            Private mLen As Integer
            Private mCurChar As Char
            Private mNaturalComparer As NaturalComparer

            Sub New(ByVal naturalComparer As NaturalComparer)
                mNaturalComparer = naturalComparer
            End Sub

            Public Sub Init(ByVal source As String)
                If source Is Nothing Then source = String.Empty
                mSource = source
                mLen = source.Length
                mIdx = -1
                mNumericalValue = 0
                NextChar()
                NextToken()
            End Sub

            Public ReadOnly Property TokenType() As TokenType
                Get
                    Return mTokenType
                End Get
            End Property

            Public ReadOnly Property NumericalValue() As Decimal
                Get
                    If mTokenType = NaturalComparer.TokenType.Numerical Then
                        Return mNumericalValue
                    Else
                        Throw New NaturalComparerException("Internal Error: NumericalValue called on a non numerical value.")
                    End If
                End Get
            End Property

            Public ReadOnly Property StringValue() As String
                Get
                    Return mStringValue
                End Get
            End Property

            Public Sub NextToken()
                Do
                    'CharUnicodeInfo.GetUnicodeCategory 
                    If mCurChar = Nothing Then
                        mTokenType = NaturalComparer.TokenType.Nothing
                        mStringValue = Nothing
                        Exit Sub
                    ElseIf Char.IsDigit(mCurChar) Then
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
                mIdx += 1
                If mIdx >= mLen Then
                    mCurChar = Nothing
                Else
                    mCurChar = mSource(mIdx)
                End If
            End Sub

            Private Sub ParseNumericalValue()
                Dim start As Integer = mIdx
                Dim NumberDecimalSeparator As Char = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator(0)
                Dim NumberGroupSeparator As Char = NumberFormatInfo.CurrentInfo.NumberGroupSeparator(0)
                Do
                    NextChar()
                    If mCurChar = NumberDecimalSeparator Then
                        ' parse digits after the Decimal Separator
                        Do
                            NextChar()
                            If Not Char.IsDigit(mCurChar) AndAlso mCurChar <> NumberGroupSeparator Then Exit Do
                        Loop
                        Exit Do
                    Else
                        If Not Char.IsDigit(mCurChar) AndAlso mCurChar <> NumberGroupSeparator Then Exit Do
                    End If
                Loop
                mStringValue = mSource.Substring(start, mIdx - start)
                If Decimal.TryParse(mStringValue, mNumericalValue) Then
                    mTokenType = NaturalComparer.TokenType.Numerical
                Else
                    ' We probably have a too long value
                    mTokenType = NaturalComparer.TokenType.String
                End If
            End Sub

            Private Sub ParseString()
                Dim start As Integer = mIdx
                Dim roman As Boolean = (mNaturalComparer.mNaturalComparerOptions And NaturalComparerOptions.RomanNumbers) <> 0
                Dim romanValue As Integer
                Dim lastRoman As Integer = Integer.MaxValue
                Dim cptLastRoman As Integer
                Do
                    If roman Then
                        Dim thisRomanValue As Integer = RomanLetterValue(mCurChar)
                        If thisRomanValue > 0 Then
                            Dim handled As Boolean = False

                            If (thisRomanValue = 1 OrElse thisRomanValue = 10 OrElse thisRomanValue = 100) Then
                                NextChar()
                                Dim nextRomanValue As Integer = RomanLetterValue(mCurChar)
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
                    If Not Char.IsLetter(mCurChar) Then Exit Do
                Loop
                mStringValue = mSource.Substring(start, mIdx - start)
                If roman Then
                    mNumericalValue = romanValue
                    mTokenType = NaturalComparer.TokenType.Numerical
                Else
                    mTokenType = NaturalComparer.TokenType.String
                End If
            End Sub

        End Class

        Sub New(ByVal NaturalComparerOptions As NaturalComparerOptions)
            mNaturalComparerOptions = NaturalComparerOptions
            mParser1 = New StringParser(Me)
            mParser2 = New StringParser(Me)
        End Sub

        Sub New(ByVal NaturalComparerOptions As NaturalComparerOptions, ByVal Direction As NaturalComparerDirection)
            If Direction = NaturalComparerDirection.Descending Then mSortAscending = False
            mNaturalComparerOptions = NaturalComparerOptions
            mParser1 = New StringParser(Me)
            mParser2 = New StringParser(Me)
        End Sub

        Sub New()
            MyClass.New(NaturalComparerOptions.Default)
        End Sub

        Public Function Compare(ByVal string1 As String, ByVal string2 As String) As Integer Implements System.Collections.Generic.IComparer(Of String).Compare
            mParser1.Init(string1)
            mParser2.Init(string2)
            Dim result As Integer
            Do
                If mParser1.TokenType = TokenType.Numerical And mParser2.TokenType = TokenType.Numerical Then
                    ' both string1 and string2 are numerical 
                    result = Decimal.Compare(mParser1.NumericalValue, mParser2.NumericalValue)
                Else
                    result = String.Compare(mParser1.StringValue, mParser2.StringValue)
                End If
                If result <> 0 Then
                    ' if the sort direction is decending the reverse the result
                    If Not mSortAscending Then
                        If result = -1 Then result = 1 Else result = -1
                    End If
                    Return result
                Else
                    mParser1.NextToken()
                    mParser2.NextToken()
                End If
            Loop Until mParser1.TokenType = TokenType.Nothing And mParser2.TokenType = TokenType.Nothing
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

        Public Function RomanValue(ByVal string1 As String) As Integer
            mParser1.Init(string1)

            If mParser1.TokenType = TokenType.Numerical Then
                Return CInt(mParser1.NumericalValue)
            Else
                Return 0
            End If
        End Function

        Public Function IComparer_Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Return Compare(DirectCast(x, String), DirectCast(x, String))
        End Function
    End Class

    Public Enum NaturalComparerOptions
        None
        RomanNumbers
        'DecimalValues <- we could put this as an option
        'IgnoreSpaces  <- we could put this as an option
        'IgnorePunctuation <- we could put this as an option
        [Default] = None
    End Enum

    <System.Flags()> Public Enum NaturalComparerDirection
        None
        Ascending
        Descending
    End Enum

    <Serializable()>
    Public Class NaturalComparerException
        Inherits Exception

        Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub
    End Class
End Namespace
