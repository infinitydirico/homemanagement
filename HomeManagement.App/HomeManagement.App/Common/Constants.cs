using System;
using System.Collections.Generic;
using System.Text;

namespace HomeManagement.App.Common
{
    public class Constants
    {
        public class Endpoints
        {
            public const string BASEURL = "http://homemanagement-api.azurewebsites.net/";
            public const string API = "api/";

            public class Auth
            {
                private const string AUTH = API + "auth/";
                public const string LOGIN = AUTH + "login";
                public const string LOGOUT = AUTH + "logout";
            }

            public class Accounts
            {
                private const string ACCOUNT = API + "account/";

                public const string PAGE = ACCOUNT + "paging";

                public const string AccountTopCharges = ACCOUNT + "accounttopchargers/";
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

            public class AccountMetric
            {
                public const string Metric = API + "accountmetric/";

                public const string Overalloutgoing = Metric + "overalloutgoing";

                public const string AccountsEvolution = Metric + "accountsevolution";

                public const string AccountEvolution = Metric + "accountevolution";

                public const string TotalIncome = Metric + "incomings";

                public const string TotalOutcome = Metric + "outgoings";
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
