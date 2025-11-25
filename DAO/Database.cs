using System;
using MySql.Data.MySqlClient;

namespace gsbMonolith.DAO
{
    /// <summary>
    /// Provides MySQL database connection management for the gsbMonolith application.
    /// <para>
    /// In <c>DEBUG</c> mode, the class uses local development credentials.
    /// In <c>RELEASE</c> mode, credentials are provided via MSBuild-injected constants.
    /// </para>
    /// </summary>
    public class Database
    {
        /// <summary>
        /// The generated connection string used to create <see cref="MySqlConnection"/> instances.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class and builds
        /// the appropriate MySQL connection string depending on the compilation mode.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        ///   <item>
        ///     <term>DEBUG</term>
        ///     <description>
        ///     Uses local MySQL credentials:
        ///     <c>localhost</c> / <c>root</c> / no password.
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <term>RELEASE</term>
        ///     <description>
        ///     Uses constants (<c>AWS_HOST</c>, <c>AWS_USER</c>, <c>AWS_PASSWORD</c>)
        ///     which are replaced during the CI/MSBuild process.
        ///     </description>
        ///   </item>
        /// </list>
        /// </remarks>
        public Database()
        {
#if DEBUG
            const string host = "localhost";
            const string user = "root";
            const string password = "";
#else
            // In Release mode, read values from MSBuild-injected constants
            string host = AWS_HOST;
            string user = AWS_USER;
            string password = AWS_PASSWORD;
#endif
            connectionString = $"server={host};database=gsbMonolith;user={user};password={password};";
        }

#if !DEBUG
        /// <summary>
        /// Placeholder value for the AWS host, replaced at build time by MSBuild.
        /// </summary>
        public const string AWS_HOST = "DEFAULT_HOST";

        /// <summary>
        /// Placeholder value for the AWS database user, replaced at build time by MSBuild.
        /// </summary>
        public const string AWS_USER = "DEFAULT_USER";

        /// <summary>
        /// Placeholder value for the AWS database password, replaced at build time by MSBuild.
        /// </summary>
        public const string AWS_PASSWORD = "DEFAULT_PASSWORD";
#endif

        /// <summary>
        /// Creates a new <see cref="MySqlConnection"/> using the generated connection string.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="MySqlConnection"/> configured with the internal connection string.
        /// </returns>
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}