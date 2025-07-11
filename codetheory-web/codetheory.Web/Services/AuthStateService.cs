namespace codetheory.Web.Services
{
    public class AuthStateService
    {
        public bool IsLoggedIn { get; private set; }
        public string? Role { get; private set; }

        public event Action? OnChange;

        public void SetUser(string? role, bool isLoggedIn)
        {
            Role = role;
            IsLoggedIn = isLoggedIn;
            NotifyStateChanged();
        }

        public void Logout()
        {
            Role = null;
            IsLoggedIn = false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
