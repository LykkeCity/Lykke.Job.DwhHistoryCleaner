using System.Threading.Tasks;

namespace Lykke.Job.DwhHistoryCleaner.Domain.Services
{
    public interface IAccountHistoryCleaner
    {
        Task CleanHistoryAsync();
    }
}
