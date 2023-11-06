using bfws.Data;
using bfws.Models;
using bfws.Models.DBModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class TransactionLogger : ITransactionLogger
    {
        public TransactionType FindTypeByName(ApplicationDbContext context, string name)
        {
            try
            {
                TransactionType type = context.TransactionType.Where(t => t.Name.ToUpper().Equals(name.ToUpper())).First();
                return type;
            }
            catch(ArgumentNullException){
                return null;
            }
        }

        public Task LogTransaction(ApplicationDbContext context, ApplicationUser user, string name, object data)
        {
            TransactionType type = FindTypeByName(context, name);
            if (type == null)
            {
                throw new ArgumentException("Invalid Transaction Type");
            }
            AddTransaction(context, user, type, data);
            return Task.CompletedTask;
        }

        public Task LogTransaction(ApplicationDbContext context, string name, object data)
        {
            TransactionType type = FindTypeByName(context, name);
            if (type == null)
            {
                throw new ArgumentException("Invalid Transaction Type");
            }
            AddTransaction(context, null, type, data);
            return Task.CompletedTask;
        }

        private void AddTransaction(ApplicationDbContext context, ApplicationUser user, TransactionType type, object data)
        {
            Transaction transaction = new Transaction { TransactionType = type, ApplicationUser = user, DateOccurred = DateTime.Now, Data = JsonConvert.SerializeObject(data) };
            context.Transaction.Add(transaction);
            context.SaveChanges();
        }
    }
}
