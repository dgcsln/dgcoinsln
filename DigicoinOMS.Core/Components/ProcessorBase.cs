using System.Collections.Concurrent;
using System.Threading.Tasks;

using DigicoinOMS.Core.MarketConnectivity;
using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.Components
{
    /// <summary>
    /// Base Processor abstract class for items delivered via IProcessableSubscriberInterface.
    /// Class provides basic skeleton for receiving messages on the subscription provided.
    /// For the sake of simplicity we make the processing of an item single threaded by using
    /// the BlockCollection as an item queue. One way to provide extra parallelism would be to have multiple queues
    /// and multiple threads reading off them for processing. The only thing we would have to ensure is that Processable items
    /// that are linked together somehow (lets say Executions for a particular order) end up in the same queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ProcessorBase<T> where T : IProcessable
    {
        protected IProcessableSubscriber<T> Subscriber { get; set; }

        private readonly BlockingCollection<T> itemQueue = new BlockingCollection<T>();

        protected ProcessorBase(IProcessableSubscriber<T> processableItemSubscriber)
        {
            this.Subscriber = processableItemSubscriber;
            this.Subscriber.OnProcessableItemReceived +=Subscriber_OnProcessableItemReceived;
            
        }

        protected void Subscriber_OnProcessableItemReceived(T processableitem)
        {
            this.itemQueue.Add(processableitem);
        }

        protected void StartProcessing()
        {
            Task.Factory.StartNew(() =>
            {
                while (!this.itemQueue.IsAddingCompleted)
                {
                    var processableItem = this.itemQueue.Take();
                    this.ProcessItem(processableItem);
                }
            });    
        }

        protected abstract void ProcessItem(T processableItem);

    }
}
