namespace HomeManagement.App.Common
{
    public class Constants
    {
        public class Endpoints
        {
            //public const string BASEURL = "http://localhost:60424/";
            //public const string BASEURL = "http://homemanagement-api.azurewebsites.net/";
            public const string BASEURL = "http://206.189.239.38:5100/";
            public const string API = "api/";

            public class Auth
            {
                private const string AUTH = API + "authentication/";
                public const string LOGIN = AUTH + "signin";
                public const string LOGOUT = AUTH + "signout";

                public const string REGISTER = API + "register";
            }

            public class Accounts
            {
                public const string ACCOUNT = API + "account/";

                public const string PAGE = ACCOUNT + "paging";

                public const string AccountTopTransactions = "toptransactions";
                
                public const string TotalIncome = ACCOUNT + "incomes";

                public const string TotalOutcome = ACCOUNT + "outcomes";

                public const string Overalloutgoing = "overall";

                public const string AccountsEvolution = ACCOUNT + "accountsevolution";

                public const string AccountEvolution = "accountevolution";
            }

            public class Transaction
            {
                public const string TRANSACTION = API + "transactions/";

                public const string PAGE = TRANSACTION + "paging";

                public const string BY_ACCOUNT_AND_DATE = TRANSACTION + "by/date/{0}/{1}/account/{2}";
            }

            public class Category
            {
                public const string CATEGORY = API + "category";
            }

            public class Currency
            {
                public const string CURRENCY = API + "currency";
            }

            public class Notifications
            {
                public const string Notification = API + "Notification";
            }

            public class Images
            {
                public const string Image = API + "Images";
            }
        }

        public class Keys
        {
            public const string UserKey = "user_storage_key";
            public const string UserIdKey = "user_id_storage_key";
            public const string CategoriesKey = "categories_storage_key";
        }

        public class Messages
        {
            public const string UpdateOnAppearing  = "UpdateOnAppearing";
            public const string Logout = "Logout";
        }
    }
}
