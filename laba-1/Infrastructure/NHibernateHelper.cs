using System;
using System.Data.OleDb;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Infrastructure
{
    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;

        public static void Configure()
        {
            sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(
                        @"Server=.; Initial Catalog=EventsDb; Integrated Security=SSPI;")
                    .ShowSql()

                )
                .Mappings(m => m.FluentMappings.Conventions.AddFromAssemblyOf<EnumConvention>())
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Infrastructure")))
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, false))
                .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return sessionFactory.OpenSession();
        }

        public static Type GetProxyType()
        {
            return typeof(NHibernate.Proxy.INHibernateProxy);
        }
    }

    public class EnumConvention : IUserTypeConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(
                x =>
                    x.Property.PropertyType.IsEnum ||
                    x.Property.PropertyType.IsGenericType && x.Property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) &&
                    x.Property.PropertyType.GetGenericArguments()[0].IsEnum);
        }

        public void Apply(IPropertyInstance instance)
        {
            instance.CustomType(instance.Property.PropertyType);
        }
    }
}
