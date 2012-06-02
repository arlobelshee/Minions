using System;
using Fools.Utils;

namespace Fools.Compilation.Recognizing
{
	public interface ITransformation<in TSource, out TDest> : IObservable<TDest>, IObserver<TSource>
	{
	}

	public abstract class Transformation<TSource, TDest> : ITransformation<TSource, TDest>
	{
		private readonly ObservableMulticaster<TDest> _observers = new ObservableMulticaster<TDest>();

		public IDisposable Subscribe(IObserver<TDest> observer)
		{
			return _observers.Subscribe(observer);
		}

		public abstract void OnNext(TSource value);

		public virtual void OnError(Exception error)
		{
		}

		public virtual void OnCompleted()
		{
			_observers.NotifyDone();
		}

		protected void send_next(TDest item)
		{
			_observers.Notify(item);
		}
	}
}
