namespace HomeManagement.App.Services.Components.Language
{
    public interface ILanguage
    {
        Languages LanguagetType { get; }

        string LanguateText { get; }

        string UsernameText { get; }

        string PasswordText { get; }

        string LoginText { get; }

        string OverviewText { get; }

        string AccountsText { get; }

        string TopChargesText { get; }

        string StatisticsText { get; }

        string OverallIncomeText { get; }

        string OverallOutcomeText { get; }

        string ExpenseText { get; }

        string IncomeText { get; }

        string PickAnAccountText { get; }

        string NewMovementText { get; }

        string SettingsText { get; }

        string ChangeLanguageText { get; }

        string LogoutText { get; }
    }
}