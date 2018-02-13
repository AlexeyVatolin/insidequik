using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Extensions;
using Common.Models;
using MahApps.Metro.Controls.Dialogs;
using MarketServerTest.Helpers;
using MarketServerTest.SignalR;

namespace MarketServerTest.ViewModels
{
    public class LoginViewModel : ClientBase
    {
        public event EventHandler ShowMainWindow;
        public string LoginStr { get; set; }
        public string PasswordStr { get; set; }
        private readonly IDialogCoordinator _dialogCoordinator;
        private ProgressDialogController _dialogController;

        public LoginViewModel(IDialogCoordinator dialogCoordinator)
        {
            this._dialogCoordinator = dialogCoordinator;
        }

        public ICommand ConnectToServer
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    _dialogController = await _dialogCoordinator.ShowProgressAsync(this, "Connecting...", "Please wait...");
                    _dialogController.SetIndeterminate();
                    SetHubName("LoginTestHub");
                    //var hubConnection = new HubConnection("http://localhost:8080/signalr", false)
                    //{
                    //    TraceLevel = TraceLevels.All
                    //};
                    //IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("LoginTestHub");
                    //stockTickerHubProxy.On<object>("Login", OnLogin);
                    //await hubConnection.Start();
                    bool sucsess = false;
                    try
                    {
                        var response = Login(LoginStr, PasswordStr);
                        sucsess = true;
                    }
                    catch (AggregateException ex)
                    {
                        if (ex.Message == ErrorCodes.WrongUserNameOrPassword.GetDescription())
                        {
                            ShowCancellableDialog("Ошибка", "Неверный логин или пароль");
                        }
                        else
                        {
                            ShowCancellableDialog("Ошибка", "Вознакла ошибка при подлючении к серверу");
                        }
                    }
                    catch (NullReferenceException)
                    {
                        ShowCancellableDialog("Ошибка", "Ошибка при получении данных с сервера. Попробуйте еще раз");
                    }
                    catch (Exception exception)
                    {
                        //await _dialogController.CloseAsync();
                        if (exception.Message == ErrorCodes.WrongUserNameOrPassword.GetDescription())
                        {
                            ShowCancellableDialog("Ошибка", "Неверный логин или пароль");
                        }
                    }
                    if (sucsess)
                    {
                        await _dialogController.CloseAsync();
                        await Logout();
                        ShowNewMainWindow();
                    }
                });
            }
        }

        public ICommand ConnectToQuik
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    _dialogController = await _dialogCoordinator.ShowProgressAsync(this, "Connecting...", "Please wait...");
                    _dialogController.SetIndeterminate();
                    bool isConnected = await Task.Run(QuikConnector.Connect);
                    if (isConnected)
                    {
                        await _dialogController.CloseAsync();
                        ShowNewMainWindow();
                    }
                    else
                    {
                        ShowCancellableDialog("Error", "Some error while connecting to QUIK");
                    }
                });
            }
        }

        private void ShowNewMainWindow()
        {
            ShowMainWindow?.Invoke(this, EventArgs.Empty);
        }

        private void ShowCancellableDialog(string title, string message)
        {
            _dialogController.Canceled += async (sender, args) => await _dialogController.CloseAsync();
            _dialogController.SetCancelable(true);
            _dialogController.SetProgress(0);
            _dialogController.SetTitle(title);
            _dialogController.SetMessage(message);
        }
    }
}
