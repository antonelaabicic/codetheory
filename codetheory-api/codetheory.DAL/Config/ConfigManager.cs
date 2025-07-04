using DotNetEnv;

namespace codetheory.DAL.Config
{
    public static class ConfigManager
    {
        private static string? _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (_connectionString != null)
                {
                    return _connectionString;
                }

                var envPath = Path.Combine(AppContext.BaseDirectory, "Resources", ".env");

                if (!File.Exists(envPath))
                    throw new FileNotFoundException($".env file not found at: {envPath}");

                Env.Load(envPath);
                _connectionString = Env.GetString("PSQL_CONNECTION_STRING");

                if (string.IsNullOrWhiteSpace(_connectionString))
                    throw new InvalidOperationException("PSQL_CONNECTION_STRING not found in .env");

                return _connectionString;
            }
        }
    }
}
