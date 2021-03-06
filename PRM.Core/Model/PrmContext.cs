﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRM.Core.DB;
using PRM.Core.DB.freesql;
using Shawn.Utils;

namespace PRM.Core.Model
{
    public class PrmContext : NotifyPropertyChangedBase
    {
        protected IDb Db { get; private set; } = new FreeSqlDb();

        public GlobalData AppData { get; } = new GlobalData();
        public DbOperator DbOperator { get; private set; } = null;

        /// <summary>
        /// init db connection to a sqlite db. Do make sure sqlitePath is writable!.
        /// </summary>
        /// <param name="sqlitePath"></param>
        public EnumDbStatus InitSqliteDb(string sqlitePath)
        {
            if (!IOPermissionHelper.HasWritePermissionOnFile(sqlitePath))
            {
                return EnumDbStatus.AccessDenied;
            }

            Db.CloseConnection();
            Db.OpenConnection(DatabaseType.Sqlite, FreeSqlDb.GetConnectionStringSqlite(sqlitePath));
            DbOperator = new DbOperator(Db);
            var ret = DbOperator.CheckDbRsaStatus();
            if (ret == EnumDbStatus.OK)
                AppData.SetDbOperator(DbOperator);
            return ret;
        }

        //public void InitMysqlDb(string connectionString)
        //{
        //}
    }
}