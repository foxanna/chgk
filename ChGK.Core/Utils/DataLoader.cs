using System;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.DbChGKInfo;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace ChGK.Core.Utils
{
    public class DataLoader : MvxNotifyPropertyChanged
    {
        private CancellationTokenSource _cancellationTokenSource;
        private string _error;

        private bool _hasError;
        private bool _isLoading;
        private bool _isLoadingForTheFirstTime;
        private bool _isLoadingMoreData;

        public DataLoader()
        {
            Params = Tuple.Create(false, false);
        }

        public Tuple<bool, bool> Params { get; set; }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;

                RaisePropertyChanged(() => IsLoading);
                RaisePropertyChanged(() => IsLoadingForTheFirstTime);
                RaisePropertyChanged(() => IsLoadingMoreData);
                RaisePropertyChanged(() => HasData);
            }
        }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged(() => Error);
                if (!string.IsNullOrEmpty(value))
                {
                    HasError = true;
                }
            }
        }

        public bool HasError
        {
            get { return _hasError; }
            set
            {
                _hasError = value;
                RaisePropertyChanged(() => HasError);
                RaisePropertyChanged(() => HasData);
            }
        }

        public bool HasData => !HasError && !IsLoading;

        public bool IsLoadingForTheFirstTime
        {
            set { _isLoadingForTheFirstTime = value; }
            get { return IsLoading && _isLoadingForTheFirstTime; }
        }

        public bool IsLoadingMoreData
        {
            set { _isLoadingMoreData = value; }
            get { return IsLoading && _isLoadingMoreData; }
        }

        private void RefreshCancellationTokenSource()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void CancelLoading()
        {
            RefreshCancellationTokenSource();
        }

        public async Task LoadItemsAsync(Func<CancellationToken, Task> loadDataTaskFactory)
        {
            RefreshCancellationTokenSource();

            if (IsLoading)
            {
                return;
            }

            HasError = false;
            IsLoading = true;

            try
            {
                await loadDataTaskFactory(_cancellationTokenSource.Token);
            }
            catch (NoConnectionException e)
            {
                Mvx.Trace(e.Message);
                Error = "Проверьте интернет соединение";
            }
            catch (OperationCanceledException e)
            {
                Mvx.Trace(e.Message);
            }
            catch (Exception e)
            {
                Mvx.Trace(e.Message);
                Error = "Не удалось загрузить данные\n" + e.Message + "\nПопробуйте еще раз";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void LoadItems(Action loadDataAction)
        {
            if (IsLoading)
            {
                return;
            }

            HasError = false;
            IsLoading = true;

            try
            {
                loadDataAction();
            }
            catch (Exception e)
            {
                Mvx.Trace(e.Message);
                Error = "Не удалось загрузить данные\n" + e.Message + "\nПопробуйте еще раз";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}