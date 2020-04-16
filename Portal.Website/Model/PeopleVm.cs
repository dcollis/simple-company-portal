using System.Collections.Generic;
using User = Portal.Services.Models.User;

namespace Portal.Website.Model
{
    public class PeopleVm
    {
        public List<User> Users { get; }
        public string NextLink { get;  }

        public PeopleVm(List<User> users, string nextLink)
        {
            Users = users;
            NextLink = nextLink;
        }
    }
}
