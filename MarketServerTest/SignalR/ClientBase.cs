using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Helpers;
using Common.Models;
using Inside.Common.Helpers.Extensions;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using Microsoft.AspNet.SignalR.Client;
using QuikSharp;

namespace MarketServerTest.SignalR
{
    public class ClientBase
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ClientBase));
        private AppDomain _domain;
        protected AppDomain Domain
        {
            get
            {
                if (_domain != null) return _domain;
                _domain = AppDomain.CurrentDomain;
                return _domain;
            }
        }
        public ClientBase()
        {
            ConfigureLogging();

            //Domain.ProcessExit += (s, e) => Log.Info("ProcessExit");
            Domain.UnhandledException += (s, e) => ShowError((Exception)e.ExceptionObject);
        }

        #region SignalR connection
        private IHubProxy _hub;
        protected IHubProxy Hub => GetHubProxy();

        protected virtual void RegisterCallback(IHubProxy hub)
        {
            _hub.RegisterCallback<object>(Disconnect);
        }
        private IHubProxy GetHubProxy()
        {
            if (_hub != null) return _hub;
            _hub = Connection.CreateHubProxy("TickHub");
            Log.Info("HubProxy: created");
            RegisterCallback(_hub);
            Log.Info("RegisteCallback: done");
            return _hub;
        }
        private HubConnection _connection;
        public HubConnection Connection
        {
            get
            {
                if (_connection != null) return _connection;
                var connectionString = "Url".GetConfigurationSetting();

                // create new connection
                _connection = new HubConnection(connectionString)
                {
                    // show message box in separate thread without block and reduce 300 -> 30
                    DeadlockErrorTimeout = TimeSpan.FromSeconds(300)
                };

                // write to log
                Log.InfoFormat("Connection: created\n{0}", connectionString);

                // register event handler
                _connection.Error += Connection_Error;
                _connection.StateChanged += change => Console.WriteLine(change.NewState);
                GetHubProxy();
                return _connection;
            }
            set
            {
                if (_connection == value) return;
                if (_connection != null)
                {
                    // write to log
                    Log.Info("Connection: cleanup");

                    // remove handlers
                    _connection.Error -= Connection_Error;

                    try
                    {
                        _connection.Stop();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                    try
                    {
                        _connection.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }

                if (value != null)
                {
                    Log.Info("Connection: set");
                }

                _connection = value;
                _hub = null;
            }
        }
        private void Connection_Error(Exception ex)
        {
            Log.Error("Connection error", ex);
            ProcessLogout(false);
            ShowConnectionError();
        }

        #region SignalR invoke
        protected async Task<TOutput> Invoke<TOutput>([CallerMemberName]string method = "Invoke", params object[] args)
        {
            // By default, the task ‘captures’ the current thread
            // .ConfigureAwait() change that behaviour
            // https://olitee.com/2015/01/c-async-await-common-deadlock-scenario/
            return await Hub.Invoke<TOutput>(method.Replace("Invoke", string.Empty), args).ConfigureAwait(false);
        }
        protected async Task Invoke([CallerMemberName]string method = "Invoke", params object[] args)
        {
            await Hub.Invoke(method.Replace("Invoke", string.Empty), args).ConfigureAwait(false);
        }
        protected Task LogoutInvoke()
        {
            return Invoke();
        }
        protected Task<object> LoginInvoke(LoginOptionsRequest request)
        {
            return Invoke<object>("LoginInvoke", request);
        }
        #endregion

        #endregion

        private decimal _balance;
        protected virtual decimal Balance
        {
            get { return _balance; }
            set
            {
                if (_balance == value) return;
                _balance = value;
                Log.InfoFormat("Balance: set {0}", value);
            }
        }

        private bool _isLogged;
        protected virtual bool IsLogged
        {
            get { return _isLogged; }
            set
            {
                if (_isLogged == value) return;
                _isLogged = value;
                Log.InfoFormat("IsLogged: set {0}", value);
            }
        }

        private bool _isStarted;
        protected bool IsStarted
        {
            get { return _isStarted; }
            set
            {
                if (_isStarted == value) return;
                _isStarted = value;

                Log.InfoFormat("IsStarted: set {0}", value);
            }
        }

        public bool DontShowErrors { get; set; }

        #region Start
        protected bool StartConnection()
        {
            if (Connection.ConnectionId != null)
            {
                return true;
            }

            Log.Info("Connection: start");

            try
            {
                // try start connection and wait
                if (Connection.Start().Wait(10 * 1000))
                {
                    Log.Info("Connection: estabilished");
                    return true;
                }
            }
            catch (AggregateException ex)
            {
                Log.Error(ex);
            }

            ShowConnectionError();
            return false;
        }
        public virtual LoginResponse Login(string login, string password)
        {
            if (!StartConnection())
            {
                return null;
            }

            var request = new LoginOptionsRequest
            {
                Login = login,
                //todo use md5 or sha
                Password = password
            };

            Log.InfoFormat("Login: start\n{0}", request.ToJson());

            try
            {
                var task = LoginInvoke(request);

                var response = task.Result.ToObject<LoginResponse>();
                if (response == null)
                {
                    throw new NullReferenceException("Login: LoginResponse");
                }

                Log.InfoFormat("Login: done\n{0}", response.ToJson());

                Hub.StartCallBacks();

                Balance = response.Balance;

                IsLogged = true;
                return response;
            }
            catch (Exception ex)
            {
                TryHandleException(ex);
            }
            return null;
        }
        #endregion

        #region Actions
        public event Action<object> LoggedOut;
        #endregion

        #region Logout
        protected void OnLoggedOut()
        {
            Log.Info("OnLoggedOut");
            LoggedOut?.Invoke(this);
        }

        protected virtual void OnLogoutProcessed()
        {
            Log.Info("ProcessLogout: done");
        }

        protected Task ProcessLogout(bool sendToServer = true)
        {
            if (!IsLogged)
            {
                return TaskHelper.Empty;
            }

            Log.Info("ProcessLogout: start");

            try
            {
                IsLogged = false;
                Hub.CancelCallbacks();

                if (sendToServer)
                {
                    LogoutInvoke().Wait();
                }
                OnLoggedOut();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }

            return Task.Run(() =>
            {
                Connection = null;
                OnLogoutProcessed();
            });
        }

        public Task Logout()
        {
            return ProcessLogout();
        }

        public virtual void Disconnect(object data)
        {
            ProcessLogout();
        }
        #endregion

        #region Error
        protected void ShowError(Exception exception)
        {
            Log.Error(exception.Message, exception);
            ShowError(exception.GetMessage());
        }
        protected virtual void IsDontShowErrors()
        {
            if (DontShowErrors)
            {
                Environment.Exit(0);
            }
        }
        private void ShowError(string message)
        {
            IsDontShowErrors();
            DisplayErrorText(message);
        }
        protected virtual void DisplayErrorText(string message)
        {
            Debug.WriteLine(message);
            //todo remove
            Console.WriteLine(message);
        }
        protected virtual void ShowLoggedOutError()
        {
            ShowError("The user was logged out and you need to log back in.");
        }
        protected virtual void ShowConnectionError()
        {
            ShowError("Server is not available. Please try again later.");
        }
        protected virtual void OnIgnoreError()
        {
            Environment.Exit(0);
        }
        public bool TryHandleException(Exception ex, bool showDefault)
        {
            Log.Error(ex);
            if (DontShowErrors)
            {
                OnIgnoreError();
            }
            if (IsReconnecting(ex))
            {
                return true;
            }
            if (ex.GetBaseException() is InvalidOperationException)
            {
                ShowLoggedOutError();
                return true;
            }
            var code = (ErrorCodes)ex.GetServerExceptionCode<HubException>();
            ShowError(code.GetDescription());
            switch (code)
            {
                case ErrorCodes.NotLogged:
                    ProcessLogout(false);
                    ShowLoggedOutError();
                    break;
                default:
                    if (showDefault)
                    {
                        ShowError(ex.GetMessage());
                    }
                    return false;
            }
            return true;
        }
        public bool TryHandleException(Exception ex)
        {
            return TryHandleException(ex, true);
        }
        /// <summary>
        /// Connection started reconnecting before invocation result was received.
        /// </summary>
        private static bool IsReconnecting(Exception ex)
        {
            if (!(ex is AggregateException)) return false;
            var innerException = ex.InnerException as InvalidOperationException;
            if (innerException == null || innerException.HResult != -2146233079) return false;
            Log.Info("SignalR: IsReconnecting");
            return true;
        }
        #endregion

        #region log4net
        private static void ConfigureLogging()
        {
            // get config file location
            var configFile = AppDomain.CurrentDomain.BaseDirectory + "log4net.config";

            // call log4net initialization
            log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));

            // clear log file
            ConfigureLogFile();
        }
        private static void ConfigureLogFile()
        {
            // get log4net repository
            var logRepository = (Hierarchy)LogManager.GetRepository();

            // get all log4net file appenders
            var logFileAppenders = logRepository.Root.Appenders.OfType<FileAppender>();

            // get first
            var logFileAppender = logFileAppenders.FirstOrDefault();
            if (logFileAppender == null) return;
            // try get existing log file
            var currentLogFile = logFileAppender.File;
            if (!File.Exists(currentLogFile)) return;
            try
            {
                // try to clean up previous log file entries
                File.WriteAllText(currentLogFile, string.Empty);
            }
            catch
            {
                // ignore
            }
        }
        #endregion
    }
}
