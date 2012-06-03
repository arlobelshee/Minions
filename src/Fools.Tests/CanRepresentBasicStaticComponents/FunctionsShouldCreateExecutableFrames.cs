﻿using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Fools.Model;
using Fools.Model.Execution;
using Fools.Model.Static;
using NUnit.Framework;

namespace Fools.Tests.CanRepresentBasicStaticComponents
{
	[TestFixture]
	public class FunctionsShouldCreateExecutableFrames
	{
		[Test]
		public void function_body_should_support_local_variables()
		{
			var test_subject = new FunctionBuilder("right");
			test_subject.add_local(typeof(int)).add_local(typeof(string));
			test_subject.build().frame.Should().Be(
				new StackFrame(
					new[]
					{
						typeof(int), typeof(string)
					},
					new Type[]
					{
					},
					new Type[]
					{
					}));
		}

		[Test]
		public void functions_should_always_emit_functions_of_the_same_signature()
		{
			var test_subject = new FunctionBuilder("function_name").build();
			var compiled_method = test_subject.emit();

			compiled_method.Name.Should().Be("function_name");
			compiled_method.ReturnType.Should().Be(typeof(FunctionCall));
			compiled_method.GetParameters().Select(p => p.Name).Should().Equal(
				new object[]
				{
					"locals", "args", "return_bindings", "return_continuation"
				});
			compiled_method.GetParameters().Select(p => p.ParameterType).Should().Equal(
				new object[]
				{
					typeof(StackFrame), typeof(VariableGroup), typeof(VariableGroup), typeof(FunctionCall)
				});
			compiled_method.Attributes.Should().Be(MethodAttributes.Static | MethodAttributes.Public);
		}

		[Test, Ignore("Will not finish implementing until I've got some wrappers for the compiler infrastructure.")]
		public void stack_frame_should_build_type_with_locals_args_and_return_bindings()
		{
			var test_subject = new StackFrame(
				new[]
				{
					typeof(int), typeof(string)
				},
				new[]
				{
					typeof(double), typeof(Guid)
				},
				new[]
				{
					typeof(float), typeof(bool)
				});
			test_subject.emit();
		}
	}
}