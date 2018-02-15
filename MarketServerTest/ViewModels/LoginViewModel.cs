using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Extensions;
using Common.Models;
using MahApps.Metro.Controls.Dialogs;
using MarketServerTest.Helpers;
using MarketServerTest.SignalR;
using Common.Interfaces;
using Unity;
using MarketServerTest.Interfaces;

namespace MarketServerTest.ViewModels
{
    public class LoginViewModel : ClientBase
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private ProgressDialogController _dialogController;
        private readonly IUnityContainer _container;
        public event EventHandler ShowMainWindow;
        public string LoginStr { get; set; } = "admin";
        //public string PasswordStr { get; set; } = "admin";
        public string Password
        {
            get
            {
                IPasswordSupplier passwordSupplier = _container.Resolve<IPasswordSupplier>();
                return passwordSupplier.GetPassword();
            }
        } 

        public LoginViewModel(IDialogCoordinator dialogCoordinator, IUnityContainer container)
        {
            _dialogCoordinator = dialogCoordinator;
            _container = container;
        }

        public ICommand ConnectToServer
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    _dialogController = await _dialogCoordinator.ShowProgressAsync(this, "Connecting...", "Please wait...");
                    _dialogController.SetIndeterminate();
                    SetHubName("LoginHub");

                    bool success = false;
                    try
                    {
                        // var response = await Task.Run(() => Login(LoginStr, PasswordStr));
                        var response = await Task.Run(() => Login(LoginStr, Password));
                        success = true;
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
                    if (success)
                    {
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
