using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public interface DeadDrop {
		void announce([NotNull] MailMessage what_happened);
	}
}