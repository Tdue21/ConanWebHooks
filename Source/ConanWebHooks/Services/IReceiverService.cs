namespace ConanWebHooks.Services;

public interface IReceiverService<in T> where T : class
{
    Task ReceiveData(T data);
}
