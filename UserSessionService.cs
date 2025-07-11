public class UserSessionService
{
    public string? CurrentUsername { get; private set; }

    public void SetUser(string username) => CurrentUsername = username;
    public void ClearUser() => CurrentUsername = null;

    public bool IsLoggedIn => !string.IsNullOrEmpty(CurrentUsername);
}
