Imports System.IO
Imports GrowthWare.Framework.Common

Namespace Context
    ''' <summary>
    ''' Class OutputFilterStream
    ''' </summary>
    Public Class OutputFilterStream
        Inherits Stream


        Private m_SyncLockObj = New Object
        ''' <summary>
        ''' The inner stream
        ''' </summary>
        Private ReadOnly InnerStream As Stream

        ''' <summary>
        ''' Tye copy stream
        ''' </summary>
        Private ReadOnly CopyStream As MemoryStream

        ''' <summary>
        ''' Initializes a new instance of the <see cref="OutputFilterStream" /> class.
        ''' </summary>
        ''' <param name="inner">The inner.</param>
        Sub New(ByVal inner As Stream)
            Me.InnerStream = inner
            Me.CopyStream = New MemoryStream()
        End Sub

        ''' <summary>
        ''' Reads the stream.
        ''' </summary>
        ''' <returns>System.String.</returns>
        Public Function ReadStream() As String
            SyncLock Me.m_SyncLockObj
                If Me.CopyStream.Length <= 0L OrElse Not Me.CopyStream.CanRead OrElse Not Me.CopyStream.CanSeek Then
                    Return [String].Empty
                End If

                Dim pos As Long = Me.CopyStream.Position
                Me.CopyStream.Position = 0L
                Try
                    Return New StreamReader(Me.CopyStream).ReadToEnd()
                Finally
                    Try
                        Me.CopyStream.Position = pos
                    Catch ex As ArgumentOutOfRangeException
                        Dim mLog As Logger = Logger.Instance()
                        mLog.Error(ex)
                    End Try
                End Try
            End SyncLock
        End Function

        ''' <summary>
        ''' When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        ''' </summary>
        ''' <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        ''' <returns>true if the stream supports reading; otherwise, false.</returns>
        Public Overrides ReadOnly Property CanRead As Boolean
            Get
                Return Me.InnerStream.CanRead
            End Get
        End Property

        ''' <summary>
        ''' When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        ''' </summary>
        ''' <value><c>true</c> if this instance can seek; otherwise, <c>false</c>.</value>
        ''' <returns>true if the stream supports seeking; otherwise, false.</returns>
        Public Overrides ReadOnly Property CanSeek As Boolean
            Get
                Return Me.InnerStream.CanSeek
            End Get
        End Property

        ''' <summary>
        ''' When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        ''' </summary>
        ''' <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
        ''' <returns>true if the stream supports writing; otherwise, false.</returns>
        Public Overrides ReadOnly Property CanWrite As Boolean
            Get
                Return Me.InnerStream.CanWrite
            End Get
        End Property

        ''' <summary>
        ''' When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        ''' </summary>
        Public Overrides Sub Flush()
            Me.InnerStream.Flush()
        End Sub

        ''' <summary>
        ''' When overridden in a derived class, gets the length in bytes of the stream.
        ''' </summary>
        ''' <value>The length.</value>
        ''' <returns>A long value representing the length of the stream in bytes.</returns>
        Public Overrides ReadOnly Property Length As Long
            Get
                Return Me.InnerStream.Length
            End Get
        End Property

        ''' <summary>
        ''' When overridden in a derived class, gets or sets the position within the current stream.
        ''' </summary>
        ''' <value>The position.</value>
        ''' <returns>The current position within the stream.</returns>
        Public Overrides Property Position As Long
            Get
                Return Me.InnerStream.Position
            End Get
            Set(value As Long)
                Me.CopyStream.Position = Me.InnerStream.Position = value
            End Set
        End Property

        ''' <summary>
        ''' When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        ''' </summary>
        ''' <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        ''' <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        ''' <param name="count">The maximum number of bytes to be read from the current stream.</param>
        ''' <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        Public Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer
            Return Me.InnerStream.Read(buffer, offset, count)
        End Function

        ''' <summary>
        ''' When overridden in a derived class, sets the position within the current stream.
        ''' </summary>
        ''' <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        ''' <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        ''' <returns>The new position within the current stream.</returns>
        Public Overrides Function Seek(offset As Long, origin As System.IO.SeekOrigin) As Long
            Me.CopyStream.Seek(offset, origin)
            Return Me.InnerStream.Seek(offset, origin)
        End Function

        ''' <summary>
        ''' When overridden in a derived class, sets the length of the current stream.
        ''' </summary>
        ''' <param name="value">The desired length of the current stream in bytes.</param>
        Public Overrides Sub SetLength(value As Long)
            Me.CopyStream.SetLength(value)
            Me.InnerStream.SetLength(value)
        End Sub

        ''' <summary>
        ''' When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        ''' </summary>
        ''' <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        ''' <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        ''' <param name="count">The number of bytes to be written to the current stream.</param>
        Public Overrides Sub Write(buffer() As Byte, offset As Integer, count As Integer)
            Me.CopyStream.Write(buffer, offset, count)
            Me.InnerStream.Write(buffer, offset, count)
        End Sub
    End Class
End Namespace