﻿using DotNetEnv;

namespace codetheory.DAL.Config
{
    public static class ConfigManager
    {
        private static bool _loaded = false;

        private static void EnsureEnvLoaded()
        {
#if DEBUG
            if (_loaded) return;

            var envPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "codetheory.DAL", "Resources", ".env"));
            if (!File.Exists(envPath))
                throw new FileNotFoundException($".env file not found at: {envPath}");

            Env.Load(envPath);
            _loaded = true;
#endif
        }

        private static string GetRequiredEnv(string key, int? requiredLength = null)
        {
            EnsureEnvLoaded();
            var value = Environment.GetEnvironmentVariable(key);

#if DEBUG
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Env.GetString(key);
            }
#endif

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"{key} not found in environment.");
            }

            if (requiredLength.HasValue && value.Length != requiredLength.Value)
            {
                throw new InvalidOperationException($"{key} must be exactly {requiredLength.Value} characters.");
            }

            return value;
        }

        public static string ConnectionString => GetRequiredEnv("PSQL_CONNECTION_STRING");
        public static string UserEncryptionKey => GetRequiredEnv("USER_ENCRYPTION_KEY", 32);
        public static string SupabaseUrl => GetRequiredEnv("SUPABASE_URL");
        public static string SupabaseBucket => GetRequiredEnv("SUPABASE_BUCKET");
        public static string SupabaseServiceRoleKey => GetRequiredEnv("SUPABASE_SERVICE_ROLE_KEY");

        public static string SupabasePublicBaseUrl => $"{SupabaseUrl}/storage/v1/object/public/{SupabaseBucket}";
        public static string DefaultImagePath => $"{SupabasePublicBaseUrl}//neutral_profile.png";

        public static string SupabaseUploadUrl(string fileName) => $"{SupabaseUrl}/storage/v1/object/{SupabaseBucket}/{fileName}";
        public static string SupabaseDeleteUrl(string fileName) => $"{SupabaseUrl}/storage/v1/object/{SupabaseBucket}/{fileName}";
        public static string SupabasePublicUrl(string fileName) => $"{SupabasePublicBaseUrl}/{fileName}";

        public static string JwtSecret => GetRequiredEnv("JWT_SECRET", 32);
    }
}
