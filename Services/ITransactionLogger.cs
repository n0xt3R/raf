using bfws.Data;
using bfws.Models;
using bfws.Models.DBModels;
using System.Threading.Tasks;

namespace bfws.Services
{
    public interface ITransactionLogger
    {
        /// <summary>
        /// Attempts to find a TransactionType in the database by name; returns null if not found.
        /// </summary>
        TransactionType FindTypeByName(ApplicationDbContext context, string name);
        Task LogTransaction(ApplicationDbContext context, ApplicationUser user, string name, object data);
        Task LogTransaction(ApplicationDbContext context, string name, object data);
    }
}
