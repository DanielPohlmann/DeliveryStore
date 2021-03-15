using System.Threading.Tasks;

namespace Delivery.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
