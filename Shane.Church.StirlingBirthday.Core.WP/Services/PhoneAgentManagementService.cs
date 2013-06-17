﻿using Microsoft.Phone.Info;
using Microsoft.Phone.Scheduler;
using Shane.Church.StirlingBirthday.Core.Exceptions;
using Shane.Church.StirlingBirthday.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shane.Church.StirlingBirthday.Core.WP.Services
{
	public class PhoneAgentManagementService : IAgentManagementService
	{
		private ISettingsService _settings;
		private string _taskName = "";

		public PhoneAgentManagementService(ISettingsService settings)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");
			_settings = settings;
		}

		public void StartAgent()
		{
			// Obtain a reference to the period task, if one exists
			var periodicTask = ScheduledActionService.Find(_taskName) as PeriodicTask;

			// If the task already exists and background agents are enabled for the
			// application, you must remove the task and then add it again to update 
			// the schedule
			if (periodicTask != null)
			{
				AgentExitReason reason = periodicTask.LastExitReason;
				//				_logger.Debug("Agent Last Exited for Reason: " + reason.ToString());
				RemoveAgent();
			}

			periodicTask = new PeriodicTask(_taskName);

			// The description is required for periodic agents. This is the string that the user
			// will see in the background services Settings page on the device.
			periodicTask.Description = Resources.WPCoreResources.AgentDescription;
			periodicTask.ExpirationTime = DateTime.Now.AddDays(14);

			// Place the call to Add in a try block in case the user has disabled agents.
			try
			{
				ScheduledActionService.Add(periodicTask);
				// If debugging is enabled, use LaunchForTest to launch the agent in one minute.
#if(DEBUG_AGENT)
				ScheduledActionService.LaunchForTest(BirthdayTile.ScheduledTaskName, TimeSpan.FromSeconds(30));
#endif
			}
			catch (InvalidOperationException exception)
			{
				if (exception.Message.Contains("BNS Error: The action is disabled"))
				{
					throw new AgentManagementException(Resources.WPCoreResources.AgentsDisabledByUserError);
				}
				if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
				{
					if (!AreAgentsSupported)
					{
						throw new AgentManagementException(Resources.WPCoreResources.AgentsNotSupportedError);
					}
					else
					{
						throw new AgentManagementException(Resources.WPCoreResources.TooManyAgentsError);
					}
				}
			}
		}

		public void RemoveAgent()
		{
			try
			{
				ScheduledActionService.Remove(_taskName);
			}
			catch { }
		}

		public bool IsAgentEnabled
		{
			get
			{
				var periodicTask = ScheduledActionService.Find(_taskName) as PeriodicTask;
				return periodicTask != null;
			}
		}

		public bool AreAgentsSupported
		{
			get
			{
				try
				{
					// check the working set limit    
					var result = (Int64)DeviceExtendedProperties.GetValue("ApplicationWorkingSetLimit");
					return !(result < 94371840L);
				}
				catch (ArgumentOutOfRangeException)
				{
					// OS does not support this call => indicates a 512 MB device   
					return true;
				}
			}
		}
	}
}
