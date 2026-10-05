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
            set { _syncService.FileFolderPath = value;
                Debug.WriteLine("Here => " + _syncService.FileFolderPath);
                    OnPropertyChanged(FileFolderPath); }
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
        public ObservableCollection<string> StartDelay
        {
            get => _startDelay;
        }
        public ObservableCollection<string> StoreNames
        {
            get => _syncService.StoreNames;
        }
        public ObservableCollection<string> MerchantIDs
        {
            get => _syncService.MerchantIDs;
        }
        private string _selectedMerchantId;
        public string SelectedMerchantId
        {
            get => _selectedMerchantId;
            set { 
                _selectedMerchantId = value;
                OnPropertyChanged(); 
            }
        }
        private string _selectedStore;
        public string SelectedStore
        {
            get => _selectedStore;
            set { 
                _selectedStore = value;
                OnPropertyChanged(); 
            }
        }

        public SyncViewModel(INotificationService notificationService)
        {
            _syncService = SyncService.GetInstance(notificationService);//Creating object if it is not get created in UI dll, Only one instance of SyncService will be created and used in both UI and BuisnessLogic dlls
            if (apiKey != null)
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
            if (MerchantIDs != null && MerchantIDs.Count > 0)
                SelectedMerchantId = MerchantIDs[0];

            if (StoreNames != null && StoreNames.Count > 0)
                SelectedStore = StoreNames[0];
        }

        private void StartDataSync(string apikey)
        {
            Debug.WriteLine("Selected Merchant Id => "+SelectedMerchantId);
            Debug.WriteLine("Selected Store => " + SelectedStore);
            _syncService.DataSync(apikey, SelectedMerchantId, SelectedStore);
        }
    }
}