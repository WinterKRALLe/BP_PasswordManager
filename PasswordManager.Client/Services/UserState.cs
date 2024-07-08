namespace PasswordManager.Client.Services;

public class UserState
{
    private string _username;
    public string Username 
    { 
        get => _username; 
        set
        {
            _username = value;
            NotifyStateChanged();
        }
    }

    public event Action OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}