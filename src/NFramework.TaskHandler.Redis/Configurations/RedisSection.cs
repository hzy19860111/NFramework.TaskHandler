using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Redis.Configurations
{
    public class RedisSection : ConfigurationSection
    {
        [ConfigurationProperty("RedisDBs")]
        [ConfigurationCollection(typeof(RedisDB), AddItemName = "RedisDB")]
        public RedisDBCollection RedisDBs
        {
            get
            { return (RedisDBCollection)this["RedisDBs"]; }
            set
            { this["RedisDBs"] = value; }
        }

        public RedisDB GetRedisDB(string dbName)
        {
            return this.RedisDBs[dbName];
        }

        public RedisDB GetRedisDBByAppName(string appName)
        {
            var redisApp = GetRedisApp(appName);
            if (redisApp == null) return null;

            return GetRedisDB(redisApp.RedisDB);
        }

        [ConfigurationProperty("RedisApps")]
        [ConfigurationCollection(typeof(RedisApp), AddItemName = "RedisApp")]
        public RedisAppCollection RedisApps
        {
            get
            { return (RedisAppCollection)this["RedisApps"]; }
            set
            { this["RedisApps"] = value; }
        }

        public RedisApp GetRedisApp(string appName)
        {
            return RedisApps[appName];
        }
    }
    public class RedisDB : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["Name"].ToString();
            }
            set
            {
                this["Name"] = value;
            }
        }

        [ConfigurationProperty("ConnectionString", IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                return this["ConnectionString"].ToString();
            }
            set
            {
                this["ConnectionString"] = value;
            }
        }
    }
    public class RedisDBCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RedisDB();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RedisDB)element).Name;
        }

        public new RedisDB this[string dbName]
        {
            get { return (RedisDB)BaseGet(dbName); }
        }
    }

    public class RedisApp : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["Name"].ToString();
            }
            set
            {
                this["Name"] = value;
            }
        }

        [ConfigurationProperty("RedisDB", IsRequired = true)]
        public string RedisDB
        {
            get
            {
                return this["RedisDB"].ToString();
            }
            set
            {
                this["RedisDB"] = value;
            }
        }

        [ConfigurationProperty("DatabaseIndex", DefaultValue = -1)]
        public int DatabaseIndex
        {
            get
            {
                return (int)this["DatabaseIndex"];
            }
            set
            {
                this["DatabaseIndex"] = value;
            }
        }
    }
    public class RedisAppCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RedisApp();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RedisApp)element).Name;
        }

        public new RedisApp this[string appName]
        {
            get { return (RedisApp)BaseGet(appName); }
        }
    }
}
