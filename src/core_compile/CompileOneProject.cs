// CompileOneProject.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using core_compile.Messages;
using Fools.cs.Api;
using Fools.cs.Utilities;
using Simulated;

namespace core_compile
{
	public class CompileOneProject
	{
		[NotNull, UsedImplicitly] private readonly MissionLocation _mission_control;

		private CompileOneProject([NotNull] MissionLocation mission_control)
		{
			_mission_control = mission_control;
		}

		public static void show_him_what_you_do([NotNull] MissionLocation mission_control)
		{
			var compile_a_project_when_found = NewMission.in_lab(() => new CompileOneProject(mission_control));
			compile_a_project_when_found.send_new_fool_when<FoolsProjectFound>()
				.and_have_it(start_compiling)
				.after_that()
				.whenever<FSharpCompileFinished>(report_fsharp_build_results);

			mission_control.send_out_fools_to(compile_a_project_when_found);
		}

		public static async void start_compiling([NotNull] CompileOneProject lab, [NotNull] FoolsProjectFound message)
		{
			Console.WriteLine("I would be parsing the project file here.");
			Console.WriteLine("#HACK - Instead I'm going to pretend it was Hello world.");

			Console.WriteLine("Transliterating Fools to F#.");
			Console.WriteLine("#HACK - Instead I'm going to ignore the Fools and just emit a Hello World program in F#.");

			var file_system = FileSystem.Real();
			var tmp = await file_system.TempDirectory;
			var compilation = await FSharpProject.command_line_program(tmp.Dir("hello_world"));
			Debug.Assert(compilation != null, "compilation != null");
			await emit_hello_world(compilation);

			Console.WriteLine("Beginning F# compile.");

			CompilationMode.debug(file_system)
				.compile(compilation)
				.prepare_mission(lab._mission_control)
				.begin();
		}

		private static async Task emit_hello_world([NotNull] FSharpProject compilation)
		{
			compilation.add_file("Methods.fs");
			const string transliteration_to_fsharp_results = @"module AllTheThings

let say_hello argv =
  printfn ""%A"" argv
  printfn ""hello world""
  0";
			await compilation.source_root.File("Methods.fs")
				.Overwrite(transliteration_to_fsharp_results);
		}

		public static void report_fsharp_build_results([NotNull] CompileOneProject lab,
			[NotNull] FSharpCompileFinished message)
		{
			message.compile.Dispose();
			Console.WriteLine("Underlying build completed.");
			lab._mission_control.announce(new FoolsProjectCompileFinished());
		}
	}
}
