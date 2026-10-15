using System.Windows;

namespace SyncUI.Services
{
    public interface IDialogService
    {
        MessageBoxResult ShowConfirmation(string message, string title);
        void ShowAlert(string message, string title);
    }
}
