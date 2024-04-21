using System;
using System.Runtime.Serialization;

namespace GrowthWare.Framework
{
	/// <summary>
	/// Created to distinguish errors created in the CryptoUtility class.
	/// </summary>
	/// <remarks></remarks>
	[Serializable()]
	public class CryptoUtilityException : Exception
	{
		/// <summary>
		/// Helps separate any errors that occure within Crypto Utility
		/// </summary>
		public CryptoUtilityException() { }

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="message">string</param>
		public CryptoUtilityException(string message) : base(message)
		{

		}

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="message">string</param>
		/// <param name="innerException">Exception</param>
		public CryptoUtilityException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
