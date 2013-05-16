using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpintronicsGUI
{
	interface GenericPacket
	{
		byte Command
		{
			get;
			set;
		}
		byte PayloadLength
		{
			get;
			set;
		}
		byte[] Payload
		{
			get;
			set;
		}
		byte Xor
		{
			get;
			set;
		}

		void ComputeXor();
	}
}
