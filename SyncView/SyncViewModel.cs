using RWLinkNotificationService.Services;
using SyncBuisnessLogic.BuisnessLogic;
using SyncBuisnessLogic.Helper;
using SyncUI.Services;
using SyncUI.Helper;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SyncUI.SyncView
{
    public class SyncViewModel : SyncBaseViewModel
    {
        private readonly SyncService _syncService;
        private readonly IDialogService _dialogService;
        private readonly INotificationService _notification;

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
        public ICommand DeleteApiCommand { get; }
        public ICommand BrowseFolderCommand { get; }
        public ICommand DataSync { get; }

        private string _checkButtonImageSource = "/Assets/checkedbox.png";
        public string CheckButtonImageSource
        {
            get => _checkButtonImageSource;
            set { _checkButtonImageSource = value; OnPropertyChanged(); }
        }

        public string apiKey
        {
            get => _syncService.apiKey;
            set { 
                _syncService.apiKey = value;
                OnPropertyChanged(); 
            }
        }

        public string FileFolderPath
        {
            get => _syncService.FileFolderPath;
            set { 
                _syncService.FileFolderPath = value;
                Debug.WriteLine("Here => " + _syncService.FileFolderPath);
                OnPropertyChanged(nameof(FileFolderPath)); 
            }
        }

        public string FileName
        {
            get => _syncService.FileName;
            set { _syncService.FileName = value; OnPropertyChanged(); }
        }

        private bool _isApiKeyEnabled = false;
        public bool IsApiKeyEnabled
        {
            get => _isApiKeyEnabled;
            set { _isApiKeyEnabled = value; OnPropertyChanged(); }
        }

        public bool IsVerifyButtonEnabled
        {
            get => _syncService.IsVerifyButtonEnabled;
            set
            {
                _syncService.IsVerifyButtonEnabled = value;
                OnPropertyChanged();
            }
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

        public bool IsDropDownEnabled
        {
            get => _syncService.IsDropDownEnabled;
            set { _syncService.IsDropDownEnabled = value; OnPropertyChanged(); }
        }

        public bool isManualSync
        {
            get => _syncService.isManualSync;
            set { 
                _syncService.isManualSync = value; 
                OnPropertyChanged(); 
            }
        }

        public bool isReplaceDataOnServer
        {
            get => _syncService.isReplaceDataOnServer;
            set { 
                _syncService.isReplaceDataOnServer = value; 
                OnPropertyChanged(); 
            }
        }

        public bool isRFIDdecryption
        {
            get => _syncService.isRFIDdecryption;
            set { 
                _syncService.isRFIDdecryption = value; 
                OnPropertyChanged(); 
            }
        }

        public bool EnableWatcher
        {
            get => _syncService.EnableFileWatcher;
            set
            {
                _syncService.EnableFileWatcher = value;
                OnPropertyChanged();
            }
        }

        public bool AutoSyncData
        {
            get => _syncService.AutoSyncData;
            set
            {
                _syncService.AutoSyncData = value;
                OnPropertyChanged();
            }
        }

        public bool AutoSyncImg
        {
            get => _syncService.AutoSyncImg;
            set
            {
                _syncService.AutoSyncImg = value;
                OnPropertyChanged();
            }
        }

        public string SelectedAutoDataDelay
        {
            get => _syncService.SelectedAutoDataDelay;
            set
            {
                _syncService.SelectedAutoDataDelay = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _startDelay = new ObservableCollection<string> {"5", "10", "15", "20", "30", "60", "120", "180", "240", "480" };
        public ObservableCollection<string> StartDelay => _startDelay;

        public ObservableCollection<string> StoreNames => _syncService.StoreNames;

        public ObservableCollection<string> MerchantIDs => _syncService.MerchantIDs;

        private string _selectedMerchantId;
        public string SelectedMerchantId
        {
            get => _selectedMerchantId;
            set { 
                if (_selectedMerchantId != value)
                {
                    _selectedMerchantId = value; 
                    OnPropertyChanged(); 
                    if (!string.IsNullOrEmpty(value))
                    {
                        _syncService.LoadStoresForMerchant(value);
                        _syncService.UpdateSelectedMerchant(apiKey, value);
                    }
                    OnPropertyChanged(nameof(StoreNames));
                    if (StoreNames != null && StoreNames.Count > 0)
                    {
                        if (string.IsNullOrEmpty(SelectedStore) || !StoreNames.Contains(SelectedStore))
                        {
                            SelectedStore = StoreNames[0];
                        }
                    }
                }
            }
        }

        private string _selectedStore;
        public string SelectedStore
        {
            get => _selectedStore;
            set { 
                if (_selectedStore != value)
                {
                    _selectedStore = value; 
                    OnPropertyChanged(); 
                    if (!string.IsNullOrEmpty(value))
                    {
                        _syncService.UpdateSelectedStore(apiKey, value);
                    }
                }
            }
        }

        public SyncViewModel(INotificationService notificationService, IDialogService dialogService = null)
        {
            _dialogService = dialogService;
            _notification = notificationService;
            _syncService = SyncService.GetInstance(notificationService);

            // Subscribe to SyncService property changes so UI is immediately refreshed
            _syncService.PropertyChanged += (sender, args) =>
            {
                OnPropertyChanged(args.PropertyName);

                if (args.PropertyName == nameof(SyncService.VerifiedApiKey) ||
                    args.PropertyName == nameof(SyncService.VerifyButtonImage))
                {
                    CheckButtonImageSource = _syncService.VerifiedApiKey ? "/Assets/check-rwlink.png" : "/Assets/checkedbox.png";
                    IsDropDownEnabled = _syncService.VerifiedApiKey;
                    IsVerifyButtonEnabled = !_syncService.VerifiedApiKey;
                    OnPropertyChanged(nameof(CheckButtonImageSource));
                    OnPropertyChanged(nameof(VerifiedApiKey));
                    OnPropertyChanged(nameof(IsDropDownEnabled));
                    OnPropertyChanged(nameof(IsVerifyButtonEnabled));
                }

                if (args.PropertyName == nameof(SyncService.MerchantIDs))
                {
                    OnPropertyChanged(nameof(MerchantIDs));
                    if (MerchantIDs != null && MerchantIDs.Count > 0)
                    {
                        if (string.IsNullOrEmpty(SelectedMerchantId) || !MerchantIDs.Contains(SelectedMerchantId))
                        {
                            SelectedMerchantId = MerchantIDs[0];
                        }
                    }
                }

                if (args.PropertyName == nameof(SyncService.StoreNames))
                {
                    OnPropertyChanged(nameof(StoreNames));
                    if (StoreNames != null && StoreNames.Count > 0)
                    {
                        if (string.IsNullOrEmpty(SelectedStore) || !StoreNames.Contains(SelectedStore))
                        {
                            SelectedStore = StoreNames[0];
                        }
                    }
                }
            };

            // Check if already verified from existing settings
            if (_syncService.VerifiedApiKey)
            {
                CheckButtonImageSource = "/Assets/check-rwlink.png";
                IsDropDownEnabled = true;
                IsVerifyButtonEnabled = false;
                if (MerchantIDs != null && MerchantIDs.Count > 0)
                {
                    SelectedMerchantId = MerchantIDs[0];
                }
                if (StoreNames != null && StoreNames.Count > 0)
                {
                    SelectedStore = StoreNames[0];
                }
            }

            // Check button command
            AddApi = new RelayCommandNew(_ => CheckApiKey());
            
            // Add button command
            EnableAddModeCommand = new RelayCommandNew(_ => 
            {
                IsApiKeyEnabled = true;
                IsVerifyButtonEnabled = true;
                IsDropDownEnabled = false;
                CheckButtonImageSource = "/Assets/checkedbox.png";
            });

            // Toggle/Close button command
            DeleteApiCommand = new RelayCommandNew(_ => RemoveApiKey(apiKey));

            // Browse Folder command
            BrowseFolderCommand = new RelayCommandNew(_ => BrowseFolder());
            DataSync = new RelayCommandNew(_ => StartDataSync(apiKey));
        }

        private void BrowseFolder()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Select a folder for syncing";
                dialog.ShowNewFolderButton = true;
                if (!string.IsNullOrEmpty(FileFolderPath))
                {
                    dialog.SelectedPath = FileFolderPath;
                }

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileFolderPath = dialog.SelectedPath;
                }
            }
        }

        private async void CheckApiKey()
        {
            VerifiedApiKey = false;
            IsApiKeyEnabled = false;
            await _syncService.VerifyApiKey(apiKey);
            
            if (_syncService.VerifiedApiKey)
            {
                IsDropDownEnabled = true;
                VerifiedApiKey = true;
                CheckButtonImageSource = "/Assets/check-rwlink.png";
                IsVerifyButtonEnabled = false;

                if (MerchantIDs != null && MerchantIDs.Count > 0)
                {
                    if (string.IsNullOrEmpty(SelectedMerchantId) || !MerchantIDs.Contains(SelectedMerchantId))
                    {
                        SelectedMerchantId = MerchantIDs[0];
                    }
                    else
                    {
                        _syncService.LoadStoresForMerchant(SelectedMerchantId);
                    }
                }

                if (StoreNames != null && StoreNames.Count > 0)
                {
                    if (string.IsNullOrEmpty(SelectedStore) || !StoreNames.Contains(SelectedStore))
                    {
                        SelectedStore = StoreNames[0];
                    }
                }
            }
            else
            {
                IsDropDownEnabled = false;
                VerifiedApiKey = false;
                IsVerifyButtonEnabled = true;
                CheckButtonImageSource = "/Assets/checkedbox.png";
            }

            OnPropertyChanged(nameof(StoreNames));
            OnPropertyChanged(nameof(MerchantIDs));
            OnPropertyChanged(nameof(VerifiedApiKey));
            OnPropertyChanged(nameof(IsDropDownEnabled));
            OnPropertyChanged(nameof(CheckButtonImageSource));
            OnPropertyChanged(nameof(IsVerifyButtonEnabled));
        }

        private void StartDataSync(string apikey)
        {
            Debug.WriteLine("Selected Merchant Id => " + SelectedMerchantId);
            Debug.WriteLine("Selected Store => " + SelectedStore);
            _syncService.DataSync(apikey, SelectedMerchantId, SelectedStore);
        }

        private void RemoveApiKey(string apikey)
        {
            _notification.ShowInfoLog($"Removing {SelectedMerchantId} ", this.GetType());
            MessageBoxResult result = _dialogService != null
                ? _dialogService.ShowConfirmation($"Do you want to remove the {SelectedMerchantId} key?", "Confirm Remove")
                : MessageBox.Show("Are you sure you want to remove the API key?", "Confirm Remove", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Debug.WriteLine($"Deleting {apikey} Api key button clicked ");
                _syncService.RemoveApiKey(apikey);
            }
        }
    }
}
