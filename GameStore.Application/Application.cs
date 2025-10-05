using GameStore.Abstractions;
using GameStore.Repository;

namespace GameStore.Application
{
    public interface IApplication<T> : IDbOperation<T>
    {
    }
    public class Application<T> : IApplication<T>
    {
        private IRepository<T> _repositorio;
        public Application(IRepository<T> repositorio)
        {
            _repositorio = repositorio;
        }
        public void Delete(int id)
        {
            _repositorio.Delete(id);
        }

        public IList<T> GetAll()
        {
            return _repositorio.GetAll();
        }

        public T GetById(int id)
        {
            return _repositorio.GetById(id);
        }

        public T Save(T entity)
        {
            return _repositorio.Save(entity);
        }
    }
}
