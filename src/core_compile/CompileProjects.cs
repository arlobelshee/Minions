// CompileProjects.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Diagnostics;
using core_compile.Messages;
using Fools.cs.Api;
using Fools.cs.Api.CommandLineApp;
using Fools.cs.Utilities;
using Simulated;

namespace core_compile
{
	public class CompileProjects
	{
		[NotNull] private readonly MissionLocation _mission_control;

		private CompileProjects([NotNull] MissionLocation mission_control)
		{
			_mission_control = mission_control;
		}

		public static void show_him_what_you_do([NotNull] MissionLocation mission_control)
		{
			var watch_for_projects_to_compile = NewMission.in_lab(() => new CompileProjects(mission_control));
			watch_for_projects_to_compile.send_new_fool_when<AppRun<CompilerUserInteractionModel>>()
				.and_have_it(start_compiling_projects)
				.after_that()
				.whenever<FoolsProjectCompileFinished>(finished_one_project)
				.whenever<FSharpCompileFinished>(report_fsharp_build_results);
			mission_control.send_out_fools_to(watch_for_projects_to_compile);
		}

		public static async void start_compiling_projects([NotNull] CompileProjects lab,
			[NotNull] AppRun<CompilerUserInteractionModel> message)
		{
			Console.WriteLine("I would be handling the command line arguments here to find all projects.");
			Console.WriteLine("#HACK - Instead I'm going to pretend it had just one project.");

			var compile_one_project = NewMission.in_lab(()=>new FoolsProjectToCompile());

			lab._mission_control.announce(new FoolsProjectFound());
	
			Console.WriteLine("I would be parsing the project file here.");
			Console.WriteLine("#HACK - Instead I'm going to pretend it was Hello world.");

			Console.WriteLine("Transliterating Fools to F#.");
			Console.WriteLine("#HACK - Instead I'm going to ignore the Fools and just emit a Hello World program in F#.");

			var file_system = FileSystem.Real();
			var compilation = await FSharpProject.hello_world(file_system);
			Debug.Assert(compilation != null, "compilation != null");
			emit_hello_world(compilation);

			Console.WriteLine("Beginning F# compile.");

			CompilationMode.debug(file_system)
				.compile(compilation)
				.prepare_mission(lab._mission_control)
				.begin();
		}

		private static void emit_hello_world([NotNull] FSharpProject compilation)
		{
			compilation.add_file("Methods.fs");
			const string transliteration_to_fsharp_results = @"module AllTheThings

let say_hello argv =
  printfn ""%A"" argv
  printfn ""hello world""
  0";
			compilation.source_root.File("Methods.fs")
				.Overwrite(transliteration_to_fsharp_results);
		}

		public static void report_fsharp_build_results([NotNull] CompileProjects lab, [NotNull] FSharpCompileFinished message)
		{
			message.compile.Dispose();
			Console.WriteLine("Underlying build completed.");
			lab._mission_control.announce(new FoolsProjectCompileFinished());
		}

		public static void finished_one_project([NotNull] CompileProjects lab, [NotNull] FoolsProjectCompileFinished message)
		{
			Console.WriteLine("#HACK - I'm pretending that there's only one project, so done now. Press ENTER to quit.");
			Console.ReadLine();
			lab._mission_control.announce(new AppQuit(AppErrorLevel.Ok));
		}
	}

	public class FoolsProjectToCompile {}

	public class FSharpCompileFinished : MailMessage
	{
		[NotNull]
		public Process compile { get; set; }

		public FSharpCompileFinished([NotNull] Process compile)
		{
			this.compile = compile;
		}
	}
}
