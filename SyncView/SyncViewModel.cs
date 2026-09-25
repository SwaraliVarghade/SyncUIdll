using System;
using SyncBuisnessLogic.BuisnessLogic;
using SyncBuisnessLogic.Helper;
using SyncBuisnessLogic.Model;
using SyncUI.Helper;
using System.Diagnostics;
using System.Windows.Input;
using RWLinkNotificationService.Services;
using System.Collections.ObjectModel;

namespace SyncUI.SyncView
{
    public class SyncViewModel : SyncBaseViewModel
    {
        private readonly SyncService _syncService;

        private bool _verifiedApi;
        public bool VerifiedApiKey
        {
            get => _syncService.VerifiedApiKey;
            set
            {
                _syncService.VerifiedApiKey = value;
                OnPropertyChanged(nameof(VerifiedApiKey));
            }
        }

        public ICommand AddApi { get; }
        public ICommand EnableAddModeCommand { get; }
        public ICommand CancelAddModeCommand { get; }
        public ICommand BrowseFolderCommand { get; }

        private string _folderPath = string.Empty;
        public string FolderPath
        {
            get => _folderPath;
            set { _folderPath = value; OnPropertyChanged(); }
        }

        private string _checkButtonImageSource = "/Assets/checkedbox.png";
        public string CheckButtonImageSource
        {
            get => _checkButtonImageSource;
            set { _checkButtonImageSource = value; OnPropertyChanged(); }
        }

        public string apiKey
        {
            get => _syncService.apiKey;
            set { _syncService.apiKey = value; OnPropertyChanged(); }
        }

        private bool _isApiKeyEnabled = false;
        public bool IsApiKeyEnabled
        {
            get => _isApiKeyEnabled;
            set { _isApiKeyEnabled = value; OnPropertyChanged(); }
        }

        private bool _isFileChecked = true;
        public bool IsFileChecked
        {
            get => _isFileChecked;
            set { _isFileChecked = value; OnPropertyChanged(); }
        }

        private bool _isDatabaseChecked = false;
        public bool IsDatabaseChecked
        {
            get => _isDatabaseChecked;
            set { _isDatabaseChecked = value; OnPropertyChanged(); }
        }

        private bool _isWebServiceChecked = false;
        public bool IsWebServiceChecked
        {
            get => _isWebServiceChecked;
            set { _isWebServiceChecked = value; OnPropertyChanged(); }
        }

        //private bool _isDropDownEnabled = true;
        public bool IsDropDownEnabled
        {
            get => _syncService.IsDropDownEnabled;
            set { _syncService.IsDropDownEnabled = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> StoreNames
        {
            get => _syncService.StoreNames;
        }
        public ObservableCollection<string> MerchantIDs
        {
            get => _syncService.MerchantIDs;
        }

        public SyncViewModel(INotificationService notificationService)
        {
            _syncService = SyncService.GetInstance(notificationService);//Creating object if it is not get created in UI dll.
            if(apiKey != null)
            {
                CheckApiKey();
            }
            
            // Check button command
            AddApi = new RelayCommandNew(_ => CheckApiKey());
            
            // Add button command
            EnableAddModeCommand = new RelayCommandNew(_ => 
            {
                IsApiKeyEnabled = true;
                IsDropDownEnabled = false;
                CheckButtonImageSource = "/Assets/checkedbox.png";
            });

            // Toggle/Close button command
            CancelAddModeCommand = new RelayCommandNew(_ => 
            {
                // Go to original state
                IsApiKeyEnabled = false;
                IsDropDownEnabled = false;
                VerifiedApiKey = false;
            });

            // Browse Folder command
            BrowseFolderCommand = new RelayCommandNew(_ => BrowseFolder());
        }

        private void BrowseFolder()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Select a folder for syncing";
                dialog.ShowNewFolderButton = true;
                if (!string.IsNullOrEmpty(FolderPath))
                {
                    dialog.SelectedPath = FolderPath;
                }

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FolderPath = dialog.SelectedPath;
                }
            }
        }

        private async void CheckApiKey()
        {
            VerifiedApiKey = false;
            IsApiKeyEnabled = false;
            await _syncService.VerifyApiKey(apiKey);
            
            CheckButtonImageSource = "/Assets/check-rwlink.png";
            
            if (StoreNames != null && StoreNames.Count > 0)
            {
                IsDropDownEnabled = true;
            }
            else
            {
                IsDropDownEnabled = false;
            }
            OnPropertyChanged(nameof(StoreNames));
            OnPropertyChanged(nameof(MerchantIDs));
            OnPropertyChanged(nameof(VerifiedApiKey));
        }
    }
}