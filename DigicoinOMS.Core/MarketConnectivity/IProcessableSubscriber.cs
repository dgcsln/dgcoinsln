using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.MarketConnectivity
{
    public delegate void ProcessableReceivedEventHandler<in T>(T processableItem) where T : IProcessable;

    public interface IProcessableSubscriber<out T> where T : IProcessable
    {
        event ProcessableReceivedEventHandler<T> OnProcessableItemReceived;
    }
}
