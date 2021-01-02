using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMO.OLP.Common.Enums
{
	[Flags]
	public enum RoleFlag
	{
		NewUser = 1,
		OldUser = 2,
		NewUserRequestReceiver = 512,
		DaqReportReceiver = 1024,
	}
}
