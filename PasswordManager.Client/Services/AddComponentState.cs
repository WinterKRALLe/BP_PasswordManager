namespace PasswordManager.Client.Services;

public class AddComponentState<T>
{
    public bool IsComponentOpen { get; set; }
    
    public void SetComponentOpen(bool value)
    {
        IsComponentOpen = value;
        NotifyStateChanged();
    }
    
    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}