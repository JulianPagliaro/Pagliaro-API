namespace GameStore.Abstractions
{
    public interface IDbContext<T> : IDbOperation<T> where T : class
    {

    }
}
