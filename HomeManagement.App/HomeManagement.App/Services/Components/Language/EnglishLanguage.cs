namespace HomeManagement.App.Services.Components.Language
{
    public class EnglishLanguage : ILanguage
    {
        public Languages LanguagetType => Languages.English;

        public string UsernameText => "Username";

        public string LanguateText => Languages.English.ToString();

        public string PasswordText => "Password";

        public string LoginText => "Login";

        public string OverviewText => "Overview";

        public string AccountsText => "Accounts";

        public string TopChargesText => "Top Charges";

        public string StatisticsText => "Statistics";

        public string OverallIncomeText => "Overall Income";

        public string OverallOutcomeText => "Overall Outcome";

        public string ExpenseText => "Expense";

        public string IncomeText => "Income";

        public string PickAnAccountText => "Pick an account";

        public string NewMovementText => "New movement";

        public string SettingsText => "Settings";

        public string ChangeLanguageText => "Change Language";

        public string LogoutText => "Logout";
    }
}
