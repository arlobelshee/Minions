﻿// CommandLineProgram.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Diagnostics;
using Fools.cs.Utilities;
using PowerArgs;

namespace Fools.cs.Api.CommandLineApp
{
	public class CommandLineProgram<TViewModel> where TViewModel : UniversalCommands
	{
		[NotNull] private readonly DeadDrop _app_execution;

		private CommandLineProgram([NotNull] DeadDrop app_execution)
		{
			_app_execution = app_execution;
		}

		public static void show_him_what_you_do([NotNull] MissionLocation app_execution)
		{
			var interact_with_user = NewMission.in_lab(() => new CommandLineProgram<TViewModel>(app_execution));
			interact_with_user.send_new_fool_when<DoMyBidding>()
				.and_have_it(figure_out_what_the_user_wants)
				.after_that()
				.whenever<AppAbort>(print_usage);
			app_execution.send_out_main_thread_fools_to(interact_with_user);
		}

		private static void figure_out_what_the_user_wants([NotNull] CommandLineProgram<TViewModel> lab,
			[NotNull] DoMyBidding message)
		{
			lab.figure_out_what_the_user_wants(message.args);
		}

		private static void print_usage([NotNull] CommandLineProgram<TViewModel> lab, [NotNull] AppAbort message)
		{
			lab.print_usage(message.exception, message.error_level);
		}

		private void figure_out_what_the_user_wants([NotNull] string[] args)
		{
			TViewModel user_commands;
			try
			{
				user_commands = Args.Parse<TViewModel>(args);
			}
			catch (Exception ex)
			{
				_app_execution.announce(new AppAbort(ex, ex is ArgException ? AppErrorLevel.BadCommandArgs : AppErrorLevel.Unknown));
				return;
			}
			Debug.Assert(user_commands != null, "user_commands != null");
			if (user_commands.help) _app_execution.announce(new AppAbort(null, AppErrorLevel.Ok));
			else _app_execution.announce(new AppRun<TViewModel>(user_commands));
		}

		private void print_usage([CanBeNull] Exception exception, AppErrorLevel error_level)
		{
			// ReSharper disable PossibleNullReferenceException
			ArgUsage.GetStyledUsage<TViewModel>()
				// ReSharper restore PossibleNullReferenceException
				.Write();
			if (exception != null) Console.WriteLine(exception.Message);
			_app_execution.announce(new AppQuit(error_level));
		}
	}
}
