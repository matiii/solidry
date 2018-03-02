using System;

namespace Solidry.Aspects.Saga
{
    /// <summary>
    /// With aspect and error handler and retry
    /// </summary>
    public class WithSaga<TInput, TOutput>: IObservable<SagaEvent<TInput, TOutput>>
    {
        //TODO Iobservable is out maybe better is just register right Observer i.e. Observer<SagaStartProcessing

        public IDisposable Subscribe(IObserver<SagaEvent<TInput, TOutput>> observer)
        {
            throw new NotImplementedException();
        }
    }
}