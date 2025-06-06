namespace Hoved_Opgave_Datamatiker.Models
{
    /// <summary>
    /// Indeholder følsomme databaseoplysninger såsom forbindelsesstrenge.
    /// Bør udelukkes fra versionsstyring og håndteres med forsigtighed.
    /// </summary>
    public class DBSecret
    {
        /// <summary>
        /// Forbindelsesstreng til Azure SQL Server-databasen.
        /// Indeholder brugernavn og adgangskode.
        /// </summary>
        public static readonly string ConnectionStringSimply =
            "Data Source=alexstudieserver.database.windows.net;Initial Catalog=AlexnadersDatabase;User ID=Alex980x;Password=GruppeLilla5;Connect Timeout=60;Trust Server Certificate=True";
    }
}
