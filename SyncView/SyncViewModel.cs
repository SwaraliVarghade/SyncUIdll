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
        public ICommand BrowseDataFolderCommand { get; }
        public ICommand BrowseImageFolderCommand { get; }
        public ICommand DataSync { get; }
        public ICommand ImageSync { get; }

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

        public string ImageFolderPath
        {
            get => _syncService.ImageFolderPath;
            set { 
                _syncService.ImageFolderPath = value;
                OnPropertyChanged(nameof(ImageFolderPath)); 
            }
        }

        public string ServerHost
        {
            get => _syncService.ServerHost;
            set { 
                _syncService.ServerHost = value;
                OnPropertyChanged(nameof(ServerHost)); 
            }
        }
        public string Port
        {
            get => _syncService.HostPort;
            set { 
                _syncService.HostPort = value;
                OnPropertyChanged(nameof(Port)); 
            }
        }
        public string Username
        {
            get => _syncService.UserName;
            set { 
                _syncService.UserName = value;
                OnPropertyChanged(nameof(Username)); 
            }
        }
        public string Password
        {
            get => _syncService.Password;
            set { 
                _syncService.Password = value;
                OnPropertyChanged(nameof(Password)); 
            }
        }
        public string Path
        {
            get => _syncService.Path;
            set { 
                _syncService.Path = value;
                OnPropertyChanged(nameof(Path)); 
            }
        }

        public string FileName
        {
            get => _syncService.FileName;
            set { _syncService.FileName = value; OnPropertyChanged(); }
        }

        public string DBHost
        {
            get => _syncService.DBHost;
            set { _syncService.DBHost = value; OnPropertyChanged(); }
        }
        public string DBPort
        {
            get => _syncService.DBPort;
            set { _syncService.DBPort = value; OnPropertyChanged(); }
        }
        public string DBUsername
        {
            get => _syncService.DBUsername;
            set { _syncService.DBUsername = value; OnPropertyChanged(); }
        }
        public string DBPassword
        {
            get => _syncService.DBPassword;
            set { _syncService.DBPassword = value; OnPropertyChanged(); }
        }
        public string DBStoreCode
        {
            get => _syncService.DBStoreCode;
            set { _syncService.DBStoreCode = value; OnPropertyChanged(); }
        }
        public string DBCustomFeild
        {
            get => _syncService.DBCustomFeild;
            set { _syncService.DBCustomFeild = value; OnPropertyChanged(); }
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

        public string DataUploadType
        {
            get => _syncService.DataUploadType;
            set
            {
                if (_syncService.DataUploadType != value)
                {
                    _syncService.DataUploadType = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsFileChecked));
                    OnPropertyChanged(nameof(IsDatabaseChecked));
                    OnPropertyChanged(nameof(IsWebServiceChecked));
                }
            }
        }

        public bool IsFileChecked
        {
            get => string.Equals(DataUploadType, "File", StringComparison.OrdinalIgnoreCase);
            set
            {
                if (value)
                {
                    DataUploadType = "File";
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsDatabaseChecked));
                    OnPropertyChanged(nameof(IsWebServiceChecked));
                }
            }
        }

        public bool IsDatabaseChecked
        {
            get => string.Equals(DataUploadType, "Database", StringComparison.OrdinalIgnoreCase);
            set
            {
                if (value)
                {
                    DataUploadType = "Database";
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsFileChecked));
                    OnPropertyChanged(nameof(IsWebServiceChecked));
                }
            }
        }

        public bool IsWebServiceChecked
        {
            get => string.Equals(DataUploadType, "Server", StringComparison.OrdinalIgnoreCase) 
                || string.Equals(DataUploadType, "Webservice", StringComparison.OrdinalIgnoreCase);
            set
            {
                if (value)
                {
                    DataUploadType = "Server";
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsFileChecked));
                    OnPropertyChanged(nameof(IsDatabaseChecked));
                }
            }
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
        public string DecryptionKey 
        {
            get => _syncService.DecryptionKey;
            set { 
                _syncService.DecryptionKey = value; 
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
        public bool SubFolderOnS3
        {
            get => _syncService.SubFolderOnS3;
            set
            {
                _syncService.SubFolderOnS3 = value;
                OnPropertyChanged();
            }
        }
        public bool ReplaceImages
        {
            get => _syncService.ReplaceImages;
            set
            {
                _syncService.ReplaceImages = value;
                OnPropertyChanged();
            }
        }
        public bool PushToServer
        {
            get => _syncService.PushToServer;
            set
            {
                _syncService.PushToServer = value;
                OnPropertyChanged();
            }
        }
        public bool IsLocal
        {
            get => _syncService.IsLocal;
            set
            {
                if (_syncService.IsLocal != value)
                {
                    _syncService.IsLocal = value;
                    OnPropertyChanged();
                    if (value)
                    {
                        IsFTP = false;
                    }
                }
            }
        }
        public bool IsFTP
        {
            get => _syncService.IsFTP;
            set
            {
                if (_syncService.IsFTP != value)
                {
                    _syncService.IsFTP = value;
                    OnPropertyChanged();
                    if (value)
                    {
                        IsLocal = false;
                    }
                }
            }
        }
        public bool IsGDrive
        {
            get => _syncService.IsGDrive;
            set
            {
                if (_syncService.IsGDrive != value)
                {
                    _syncService.IsGDrive = value;
                    OnPropertyChanged();
                    if (value)
                    {
                        IsLocal = false;
                    }
                }
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
            get => !string.IsNullOrEmpty(_syncService.SelectedStore) ? _syncService.SelectedStore : _selectedStore;
            set { 
                if (_selectedStore != value || _syncService.SelectedStore != value)
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

                if (args.PropertyName == nameof(SyncService.SelectedStore))
                {
                    _selectedStore = _syncService.SelectedStore;
                    OnPropertyChanged(nameof(SelectedStore));
                }

                if (args.PropertyName == nameof(SyncService.StoreNames))
                {
                    OnPropertyChanged(nameof(StoreNames));
                    if (StoreNames != null && StoreNames.Count > 0)
                    {
                        string current = !string.IsNullOrEmpty(_syncService.SelectedStore) ? _syncService.SelectedStore : SelectedStore;
                        if (string.IsNullOrEmpty(current) || !StoreNames.Contains(current))
                        {
                            SelectedStore = StoreNames[0];
                        }
                        else
                        {
                            SelectedStore = current;
                        }
                    }
                }

                if (args.PropertyName == nameof(SyncService.DataUploadType))
                {
                    OnPropertyChanged(nameof(DataUploadType));
                    OnPropertyChanged(nameof(IsFileChecked));
                    OnPropertyChanged(nameof(IsDatabaseChecked));
                    OnPropertyChanged(nameof(IsWebServiceChecked));
                }

                if (args.PropertyName == nameof(SyncService.FileFolderPath))
                {
                    OnPropertyChanged(nameof(FileFolderPath));
                }

                if (args.PropertyName == nameof(SyncService.ImageFolderPath))
                {
                    OnPropertyChanged(nameof(ImageFolderPath));
                }

                if (args.PropertyName == nameof(SyncService.HostPort))
                {
                    OnPropertyChanged(nameof(Port));
                }
                if (args.PropertyName == nameof(SyncService.ServerHost))
                {
                    OnPropertyChanged(nameof(ServerHost));
                }

                if (args.PropertyName == nameof(SyncService.UserName))
                {
                    OnPropertyChanged(nameof(Username));
                }

                if (args.PropertyName == nameof(SyncService.Password))
                {
                    OnPropertyChanged(nameof(Password));
                }

                if (args.PropertyName == nameof(SyncService.Path))
                {
                    OnPropertyChanged(nameof(Path));
                }

                if (args.PropertyName == nameof(SyncService.IsLocal))
                {
                    OnPropertyChanged(nameof(IsLocal));
                }

                if (args.PropertyName == nameof(SyncService.IsFTP))
                {
                    OnPropertyChanged(nameof(IsFTP));
                }
                if(args.PropertyName == nameof(SyncService.IsGDrive))
                {
                    OnPropertyChanged(nameof(IsGDrive));
                }

                if (args.PropertyName == nameof(SyncService.isReplaceDataOnServer))
                {
                    OnPropertyChanged(nameof(isReplaceDataOnServer));
                }

                if (args.PropertyName == nameof(SyncService.isManualSync))
                {
                    OnPropertyChanged(nameof(isManualSync));
                }

                if (args.PropertyName == nameof(SyncService.isRFIDdecryption))
                {
                    OnPropertyChanged(nameof(isRFIDdecryption));
                }

                if (args.PropertyName == nameof(SyncService.DecryptionKey))
                {
                    OnPropertyChanged(nameof(DecryptionKey));
                }

                if (args.PropertyName == nameof(SyncService.EnableFileWatcher) || args.PropertyName == "EnableWatcher")
                {
                    OnPropertyChanged(nameof(EnableWatcher));
                }

                if (args.PropertyName == nameof(SyncService.AutoSyncData))
                {
                    OnPropertyChanged(nameof(AutoSyncData));
                }

                if (args.PropertyName == nameof(SyncService.AutoSyncImg))
                {
                    OnPropertyChanged(nameof(AutoSyncImg));
                }

                if (args.PropertyName == nameof(SyncService.SubFolderOnS3))
                {
                    OnPropertyChanged(nameof(SubFolderOnS3));
                }

                if (args.PropertyName == nameof(SyncService.ReplaceImages))
                {
                    OnPropertyChanged(nameof(ReplaceImages));
                }

                if (args.PropertyName == nameof(SyncService.PushToServer))
                {
                    OnPropertyChanged(nameof(PushToServer));
                }

                if (args.PropertyName == nameof(SyncService.FileName))
                {
                    OnPropertyChanged(nameof(FileName));
                }
                if (args.PropertyName == nameof(SyncService.DBHost))
                {
                    OnPropertyChanged(nameof(DBHost));
                }
                if (args.PropertyName == nameof(SyncService.DBUsername))
                {
                    OnPropertyChanged(nameof(DBUsername));
                }
                if (args.PropertyName == nameof(SyncService.DBPort))
                {
                    OnPropertyChanged(nameof(DBPort));
                }
                if (args.PropertyName == nameof(SyncService.DBPassword))
                {
                    OnPropertyChanged(nameof(DBPassword));
                }
                if (args.PropertyName == nameof(SyncService.DBStoreCode))
                {
                    OnPropertyChanged(nameof(DBStoreCode));
                }
                if (args.PropertyName == nameof(SyncService.DBCustomFeild))
                {
                    OnPropertyChanged(nameof(DBCustomFeild));
                }
            };

            if (!IsLocal && !IsFTP)
            {
                IsLocal = true;
            }

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
                    string current = !string.IsNullOrEmpty(_syncService.SelectedStore) ? _syncService.SelectedStore : SelectedStore;
                    SelectedStore = (!string.IsNullOrEmpty(current) && StoreNames.Contains(current)) ? current : StoreNames[0];
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

            // Browse Folder commands
            BrowseFolderCommand = new RelayCommandNew(_ => BrowseFolder());
            BrowseDataFolderCommand = BrowseFolderCommand;
            BrowseImageFolderCommand = new RelayCommandNew(_ => BrowseImageFolder());
            DataSync = new RelayCommandNew(_ => StartDataSync(apiKey));
            ImageSync = new RelayCommandNew(_ => StartImageSync(apiKey));
        }

        private void BrowseFolder()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Select a folder for data syncing";
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

        private void BrowseImageFolder()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Select a folder for image syncing";
                dialog.ShowNewFolderButton = true;
                if (!string.IsNullOrEmpty(ImageFolderPath))
                {
                    dialog.SelectedPath = ImageFolderPath;
                }

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ImageFolderPath = dialog.SelectedPath;
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

        private void StartImageSync(string apikey, string imageFolderPath = "")
        {
            _notification.ShowInfoLog($"Selected Merchant Id => {SelectedMerchantId}, Selected Store => {SelectedStore}", this.GetType());
            _syncService.ImageSyncPreProcedure(apikey, SelectedMerchantId, SelectedStore);
        }
    }
}
