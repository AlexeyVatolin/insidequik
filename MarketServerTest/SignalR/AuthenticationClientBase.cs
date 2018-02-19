using System;
using System.Threading.Tasks;
using Common.Helpers;

namespace MarketServerTest.SignalR
{
    public class AuthenticationClientBase : ClientBase
    {
        protected async Task LoginAsync(string username, string password)
        {
            SetHubName("AuthenticationHub");
            StartConnection();

            Log.InfoFormat("Login: start for {0}", username);

            try
            {
                var response = await LoginInvoke(username, password);

                if (response == null)
                {
                    throw new NullReferenceException("Login: LoginResponse");
                }

                Log.InfoFormat("Login: done");

                Hub.StartCallBacks();

                Balance = response.Balance;
                SessionId = response.SessionId;

                IsLogged = true;
            }
            catch (Exception ex)
            {
                TryHandleException(ex);
            }
        }

        protected Task LogoutAsync()
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

                LogoutInvoke().Wait();
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

        protected virtual void OnLogoutProcessed()
        {
            Log.Info("ProcessLogout: done");
        }

        protected void OnLoggedOut()
        {
            Log.Info("OnLoggedOut");
        }

    }
}
