namespace Fools.cs.Tests.Interpretation
{
	public class FoolsLanguageApi
	{
		private readonly Interpreter _interpreter;

		internal FoolsLanguageApi(Interpreter interpreter)
		{
			_interpreter = interpreter;
		}

		public Universe create_universe()
		{
			return new Universe(_interpreter);
		}

		public Fool create_fool_in(Universe universe)
		{
			var result = new Fool(_interpreter);
			_interpreter.fool_always_requires(result, universe, Universe.Access.ReadWrite);
			_interpreter.begin_scheduling(result);
			return result;
		}
	}
}