using LogicBuilder.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Contoso.Contexts
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class EntityConfigurationHandler(DbContext context)
    {

        #region Properties
        protected DbContext Context { get; private set; } = context;
        #endregion Properties

        #region Methods
        public virtual void Configure(ModelBuilder modelBuilder)
        {
            foreach (Type propertyType in this.Context.GetType()
                .GetProperties()
                .Select(property => property.PropertyType)
                .Where(t => t.Name == "DbSet`1"))
            {
                Type modelType = propertyType.GetGenericArguments()[0];
                if (!typeof(BaseData).IsAssignableFrom(modelType))
                    continue;

                modelBuilder.Entity(modelType).Ignore(nameof(BaseData.EntityState));
            }

            Type interfaceType = typeof(Configuations.ITableConfiguration);
            interfaceType.Assembly.GetTypes().Where(p => interfaceType.IsAssignableFrom(p)
                                && !p.IsAbstract
                                && !p.IsGenericTypeDefinition
                                && !p.IsInterface).ToList().ForEach(t =>
                                {
                                    MethodInfo mi = t.GetMethod(nameof(Configuations.ITableConfiguration.Configure))!;//ITableConfiguration implements Configure
                                    mi.Invoke(Activator.CreateInstance(t), [modelBuilder]);
                                });
        }
        #endregion Methods
    }
}
