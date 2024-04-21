using GrowthWare.Framework.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.WebSupport.Context
{
    /// <summary>
    /// A stream which keeps an in-memory copy as it passes the bytes through
    /// </summary>
    /// <remarks>found at http://stackoverflow.com/questions/1038466/logging-raw-http-request-response-in-asp-net-mvc-iis7</remarks>
    public class OutputFilterStream : Stream
    {
        private object m_LockObj = new object();

        /// <summary>
        /// Provides access to the inner stream
        /// </summary>
        private readonly Stream InnerStream;

        /// <summary>
        /// Copies the memory stream
        /// </summary>
        private readonly MemoryStream CopyStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFilterStream" /> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        public OutputFilterStream(Stream inner)
        {
            this.InnerStream = inner;
            this.CopyStream = new MemoryStream();
        }

        /// <summary>
        /// Reads the stream.
        /// </summary>
        /// <returns>System.String.</returns>
        public string ReadStream()
        {
            lock (this.m_LockObj)
            {
                if (this.CopyStream.Length <= 0L ||
                    !this.CopyStream.CanRead ||
                    !this.CopyStream.CanSeek)
                {
                    return String.Empty;
                }

                long pos = this.CopyStream.Position;
                this.CopyStream.Position = 0L;
                try
                {
                    return new StreamReader(this.CopyStream).ReadToEnd();
                }
                finally
                {
                    try
                    {
                        this.CopyStream.Position = pos;
                    }
                    catch (ArgumentOutOfRangeException ex) 
                    {
                        Logger mLogger = Logger.Instance();
                        mLogger.Error(ex);
                    }
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        /// <returns>true if the stream supports reading; otherwise, false.</returns>
        public override bool CanRead
        {
            get { return this.InnerStream.CanRead; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <value><c>true</c> if this instance can seek; otherwise, <c>false</c>.</value>
        /// <returns>true if the stream supports seeking; otherwise, false.</returns>
        public override bool CanSeek
        {
            get { return this.InnerStream.CanSeek; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
        /// <returns>true if the stream supports writing; otherwise, false.</returns>
        public override bool CanWrite
        {
            get { return this.InnerStream.CanWrite; }
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            this.InnerStream.Flush();
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <value>The length.</value>
        /// <returns>A long value representing the length of the stream in bytes.</returns>
        public override long Length
        {
            get { return this.InnerStream.Length; }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <value>The position.</value>
        /// <returns>The current position within the stream.</returns>
        public override long Position
        {
            get { return this.InnerStream.Position; }
            set { this.CopyStream.Position = this.InnerStream.Position = value; }
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.InnerStream.Read(buffer, offset, count);
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            this.CopyStream.Seek(offset, origin);
            return this.InnerStream.Seek(offset, origin);
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            this.CopyStream.SetLength(value);
            this.InnerStream.SetLength(value);
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            this.CopyStream.Write(buffer, offset, count);
            this.InnerStream.Write(buffer, offset, count);
        }
    }
}
