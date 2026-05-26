namespace SemilleroGR3.Helpers
{
    public static class Constants
    {
        // ==========================================
        // CONFIGURACIÓN DE LA API REST
        // ==========================================
        // NOTA: Ajusta el puerto (ej. 7001) y la IP según donde esté corriendo tu backend en .NET.
        // Si pruebas en el emulador de Android apuntando a tu PC local, usa 10.0.2.2 en lugar de localhost.

        public const string LocalhostUrl = "https://localhost:7001/api/";
        public const string AndroidEmulatorUrl = "https://10.0.2.2:7001/api/";

        public static string BaseApiUrl
        {
            get
            {
#if ANDROID
                return AndroidEmulatorUrl;
#else
                return LocalhostUrl;
#endif
            }
        }

        // ==========================================
        // CLAVES DE ALMACENAMIENTO (SecureStorage / Preferences)
        // ==========================================
        public const string AuthTokenKey = "jwt_token";
        public const string UserRoleKey = "user_role"; // Administrador, Docente, Familia
        public const string UserIdKey = "user_id";

        // Clave para la persistencia offline (Cache Memory / Preferences)
        public const string CacheEvaluacionesKey = "cache_evaluaciones_";
    }
}