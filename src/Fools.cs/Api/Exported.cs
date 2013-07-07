// Exported.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Fools.cs.Utilities;
using Fools.cs.messages;

namespace Fools.cs.Api
{
	public static class Exported
	{
		public static IEnumerable<KeyValuePair<string, Type>> messages()
		{
			return new[] {new KeyValuePair<string, Type>("write.to.log", typeof (WriteToLog))};
		}

		public static IEnumerable<KeyValuePair<string, MethodInfo>> functions()
		{
			return new[] {function("write.normal", () => WriteToLog.normal())};
		}

		public static KeyValuePair<string, MethodInfo> function([NotNull] string name,
			[NotNull] Expression<Action> method_to_export)
		{
			var exported_method = ((MethodCallExpression) method_to_export.Body).Method;
			return new KeyValuePair<string, MethodInfo>(name, exported_method);
		}

		public static KeyValuePair<string, Type> message(string name, Type message_type)
		{
			return new KeyValuePair<string, Type>(name, message_type);
		}
	}
}
