export class ServiceConstants {
    //localhost
    public static readonly apiUrl: string = "http://localhost:60424/api/";
    //localhost Docker
    //public static readonly apiUrl: string = "http://localhost:32776/api/";
    //azureweb api
    //public static readonly apiUrl: string = "https://homemanagement-api.azurewebsites.net/api/";
    //digital ocean api
    //public static readonly apiUrl: string = "http://206.189.239.38:5100/api/";
    //digital ocean api
    //public static readonly apiUrl: string = "http://206.189.239.38:5101/api/";


    public static readonly storageApiUrl: string = "http://localhost:5500/api/";

    public static readonly apiAuth: string = ServiceConstants.apiUrl + 'authentication/';

    public static readonly apiUser: string = ServiceConstants.apiUrl + 'user';

    public static readonly apiAccount: string = ServiceConstants.apiUrl + 'account';

    public static readonly apiAccountMetric: string = ServiceConstants.apiUrl + 'accountmetric';    

    public static readonly apiCharge: string = ServiceConstants.apiUrl + 'transactions';

    public static readonly deleteAllCharges: string = ServiceConstants.apiCharge + "/deleteall";

    public static readonly apiCategory: string = ServiceConstants.apiUrl + 'category';

    public static readonly activeCategories: string = ServiceConstants.apiCategory + '/active';

    public static readonly apiTax: string = ServiceConstants.apiUrl + 'tax';

    public static readonly apiReminder: string = ServiceConstants.apiUrl + 'reminder';

    public static readonly apiNotification: string = ServiceConstants.apiUrl + 'notification';

    public static readonly apiCurrency: string = ServiceConstants.apiUrl + 'currency';

    public static readonly apiPreferences: string = ServiceConstants.apiUrl + 'preferences';

    public static readonly apiStorage: string = ServiceConstants.storageApiUrl + 'storage';

    public static readonly apiMonthlyExpense: string = ServiceConstants.apiUrl + 'MonthlyExpenses';

    public static readonly apiImages: string = ServiceConstants.apiUrl + 'Images';

    public static readonly apiPasswordManagement: string = ServiceConstants.apiUrl + 'PasswordManagement';
}