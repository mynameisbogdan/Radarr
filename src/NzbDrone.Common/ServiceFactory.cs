using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace NzbDrone.Common
{
    public interface IServiceFactory
    {
        T Build<T>()
            where T : class;
        IEnumerable<T> BuildAll<T>()
            where T : class;
        object Build(Type contract);
        IEnumerable<Type> GetImplementations(Type contract);
    }

    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ServiceFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public T Build<T>()
            where T : class
        {
            using var scope = _serviceScopeFactory.CreateScope();

            return scope.ServiceProvider.GetRequiredService<T>();
        }

        public IEnumerable<T> BuildAll<T>()
            where T : class
        {
            using var scope = _serviceScopeFactory.CreateScope();

            return scope.ServiceProvider.GetServices<T>().GroupBy(c => c.GetType().FullName).Select(g => g.First());
        }

        public object Build(Type contract)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            return scope.ServiceProvider.GetRequiredService(contract);
        }

        public IEnumerable<Type> GetImplementations(Type contract)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            return scope.ServiceProvider.GetServices(contract).Select(x => x.GetType());
        }
    }
}
