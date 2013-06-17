﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shane.Church.StirlingBirthday.Core.Services
{
	public interface IAgentManagementService
	{
		void StartAgent();
		void RemoveAgent();

		bool IsAgentEnabled { get; }
		bool AreAgentsSupported { get; }
	}
}
