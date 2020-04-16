using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Graph;
using Portal.Services.GraphServices;
using User = Portal.Services.Models.User;

namespace Portal.Services
{
    public class PeopleService
    {
        private readonly AzureAuthenticationProvider _authenticationProvider;

        private const string FILTER_STR =
            "givenName,businessPhones,mobilePhone,jobTitle,officeLocation,OnPremisesDistinguishedName,displayName,userPrincipalName,surname,accountEnabled,companyName";

        public PeopleService(AzureAuthenticationProvider authenticationProvider)
        {
            _authenticationProvider = authenticationProvider;
        }

        public async Task<(List<User> users, string nextLink)> GetPeople(string filter = null)
        {
            GraphServiceClient client = new GraphServiceClient(_authenticationProvider);
            var request = client.Users.Request().Select(FILTER_STR);
            if (!string.IsNullOrEmpty(filter))
            {
                request = request.Filter($"startswith(GivenName, '{filter}')");
            }
            var graphUsers = await request.GetAsync();
            var users =  GetPeople(graphUsers);
            var nextLink = GetNextLink(graphUsers);
            return (users, nextLink);
        }

        public async Task<(List<User> users, string nextLink)> GetMorePeople(string url)
        {
            GraphServiceClient client = new GraphServiceClient(_authenticationProvider);

            // get next page
            IGraphServiceUsersCollectionPage page = new GraphServiceUsersCollectionPage();
            page.InitializeNextPageRequest(client, url);
            var graphUsers = await page.NextPageRequest.GetAsync();

            // get data
            var users = GetPeople(graphUsers);
            var nextLink = GetNextLink(graphUsers);
            return (users, nextLink);
        }

        private List<User> GetPeople(IGraphServiceUsersCollectionPage graphUsers)
        {
            var usersTmp = graphUsers.Where(u => !string.IsNullOrEmpty(u.Surname)
                                              && !u.DisplayName.EndsWith("LOCAL ADMIN")
                                              && !u.UserPrincipalName.Contains("_adm@")
                                              && CheckUser(u)
                                              && u.AccountEnabled == true).ToList();
                var users = usersTmp.Select(u =>
            {
                var directNumber = "";
                var mobileNumber = "";
                if (u.BusinessPhones != null && u.BusinessPhones.Any())
                {
                    directNumber = $"{u.BusinessPhones.First()}";
                }

                if (!string.IsNullOrEmpty(u.MobilePhone))
                {
                    mobileNumber += $"{u.MobilePhone}";
                }

                return new User()
                {
                    Forename = u.GivenName,
                    Surname = u.Surname,
                    EmailAddress = u.UserPrincipalName.ToLower(),
                    DirectPhone = directNumber,
                    MobilePhone = mobileNumber,
                    JobTitle = u.JobTitle,
                    Location = u.OfficeLocation,
                    CompanyName = u.CompanyName
                };
            }).ToList();

            return users;
        }

        private bool CheckUser(Microsoft.Graph.User user)
        {
            return user.AdditionalData["onPremisesDistinguishedName"] is string onPremisesDistinguishedName 
                   && (onPremisesDistinguishedName.EndsWith("OU=Staff,DC=DUETGROUP,DC=NET")
                   || (onPremisesDistinguishedName.EndsWith("OU=Staff,DC=meritcapital,DC=be") 
                       && !onPremisesDistinguishedName.EndsWith("OU=Shared Accounts,OU=Staff,DC=meritcapital,DC=be")
                       && !onPremisesDistinguishedName.EndsWith("OU=Vendors,OU=Staff,DC=meritcapital,DC=be")));
        }

        private string GetNextLink(IGraphServiceUsersCollectionPage graphUsers)
        {
            var gusers = (GraphServiceUsersCollectionPage)graphUsers;
            var nextLink = (gusers.AdditionalData.ContainsKey("@odata.nextLink")) ? gusers.AdditionalData["@odata.nextLink"] as string : "";
            return nextLink;
        }
    }
}
