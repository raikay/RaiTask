using Quartz.Impl.AdoJobStore;
using Quartz.Impl.AdoJobStore.Common;
using RaiTask.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaiTask.Repository.Implement
{

    public class JobRepositoryFactory
    {
        public static IJobRepository CreateRepositorie(string driverDelegateType, IDbProvider dbProvider)
        {

            if (driverDelegateType == typeof(SQLiteDelegate).AssemblyQualifiedName)
            {
                return new JobRepositorySQLite(dbProvider);
            }
            //else if (driverDelegateType == typeof(MySQLDelegate).AssemblyQualifiedName)
            //{
            //    return new JobRepositoryMySql(dbProvider);
            //}
            //else if (driverDelegateType == typeof(PostgreSQLDelegate).AssemblyQualifiedName)
            //{
            //    return new JobRepositoryPostgreSQL(dbProvider);
            //}
            //else if (driverDelegateType == typeof(OracleDelegate).AssemblyQualifiedName)
            //{
            //    return new JobRepositoryOracle(dbProvider);
            //}
            else
            {
                return null;
            }
        }
    }
}
