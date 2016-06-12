using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrowthWare.Framework.Interfaces
{
	public interface IMessageProfile
	{
		string Body
		{
			get;
			set;
		}
		string Title
		{
			get;
			set;
		}
		bool FormatAsHTML
		{
			get;
			set;
		}
		void FormatBody();
		string Tags(string Seporator);
	}

}
