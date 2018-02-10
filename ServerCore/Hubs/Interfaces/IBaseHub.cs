namespace ServerCore.Hubs.Interfaces
{
    public interface IBaseHub
    {
        object Login(object request);
        void Logout();
    }
}
