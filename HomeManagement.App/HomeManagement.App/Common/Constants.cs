namespace HomeManagement.App.Common
{
    public class Constants
    {
        public class Endpoints
        {
            public const string BASEURL = "http://localhost:60424/";
            //public const string BASEURL = "http://homemanagement-api.azurewebsites.net/";
            public const string API = "api/";

            public class Auth
            {
                private const string AUTH = API + "authentication/";
                public const string LOGIN = AUTH + "signin";
                public const string LOGOUT = AUTH + "signout";
            }

            public class Accounts
            {
                public const string ACCOUNT = API + "account/";

                public const string PAGE = ACCOUNT + "paging";

                public const string AccountTopCharges = "/topcharges/";
                
                public const string TotalIncome = ACCOUNT + "incomes";

                public const string TotalOutcome = ACCOUNT + "outcomes";

                public const string Overalloutgoing = "overall";

                public const string AccountsEvolution = ACCOUNT + "accountsevolution";

                public const string AccountEvolution = "accountevolution";

                public const string Metric = "chartbychargetype";
            }

            public class Charge
            {
                public const string CHARGE = API + "charge/";

                public const string PAGE = CHARGE + "paging/";
            }

            public class Category
            {
                public const string CATEGORY = API + "category";
            }
        }

        public class Keys
        {
            public const string UserKey = "user_storage_key";
            public const string UserIdKey = "user_id_storage_key";
            public const string CategoriesKey = "categories_storage_key";
        }
    }
}
