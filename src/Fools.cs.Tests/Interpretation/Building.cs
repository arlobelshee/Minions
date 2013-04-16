using System;

namespace Fools.cs.Tests.Interpretation
{
	public class Building
	{
		public AccessBuilder access;
		private readonly Lazy<BuildingAccess> _window_access;

		public enum Access
		{
			Window,
			Door,
			MailSlot
		}

		internal Building(Interpreter interpreter)
		{
			_window_access = new Lazy<BuildingAccess>(() => new WindowAccess(this));
			interpreter.remember(this);
		}

		internal class WindowAccess : BuildingAccess
		{
			public WindowAccess(Building building) : base(building)
			{
				
			}
		}

		public BuildingAccess window_access
		{
			get {
				return _window_access;
			}
		}

		internal void destroy()
		{
		}

		public void create_string(string variable_name, string value)
		{
			throw new System.NotImplementedException();
		}

		public class AccessBuilder
		{
		}
	}
}
