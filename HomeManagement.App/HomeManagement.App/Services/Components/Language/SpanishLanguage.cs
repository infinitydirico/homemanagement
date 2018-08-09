namespace HomeManagement.App.Services.Components.Language
{
    public class SpanishLanguage : ILanguage
    {
        public Languages LanguagetType => Languages.Spanish;

        public string UsernameText => "Nombre de usuario";

        public string LanguateText => "Castellano";

        public string PasswordText => "Contraseña";

        public string LoginText => "Ingresar";

        public string OverviewText => "Vision general";

        public string AccountsText => "Cuentas";

        public string TopChargesText => "Mayores Movimientos";

        public string StatisticsText => "Estadisticas";

        public string OverallIncomeText => "Ingresos totales";

        public string OverallOutcomeText => "Egresos totales";

        public string ExpenseText => "Gasto";

        public string IncomeText => "Ingreso";

        public string PickAnAccountText => "Elige una cuenta";

        public string NewMovementText => "Nuevo movimiento";

        public string SettingsText => "Ajustes";

        public string ChangeLanguageText => "Cambiar Lenguaje";

        public string LogoutText => "Cerrar sesion";
    }
}
