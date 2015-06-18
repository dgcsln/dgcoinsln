using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.Components.Validation
{
    public interface IOrderValidator
    {
        void ValidateOrder(Order clientOrder);
    }
}
