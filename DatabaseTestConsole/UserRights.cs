using System.Linq;

namespace DatabaseTestConsole
{
    public class UserRights
    {
        private AccountingContext _context;

        public UserRights(AccountingContext context)
        {
            _context = context;
        }

        public string LookupPassword(string userName)
        {
            var query = (from a in _context.accounts 
                         where a.username == userName 
                         select a).FirstOrDefault();

            return query.pass;
        }
    }
}
