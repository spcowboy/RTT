using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Configuration;


namespace RTT
{
    class ConfigHelper
    {
        
        //get all address
        public  Dictionary<string,string> GetAddr()
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Dictionary<string, string> addrdic = new Dictionary<string, string>();
            string[] keys = config.AppSettings.Settings.AllKeys;
            foreach (string k in keys)
            {
                string v = config.AppSettings.Settings[k].Value;
                addrdic.Add(k, v);
            }
            return addrdic;
        }
        //update address
        public  void UpdateAddr(Dictionary<string, string> addrdic)
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (KeyValuePair<string,string> k in addrdic)
            {
                config.AppSettings.Settings[k.Key].Value = k.Value;
                
            }
            config.Save(ConfigurationSaveMode.Modified);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }

        public void UpdateConfig(string key,string value)
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            
            config.Save(ConfigurationSaveMode.Modified);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
        public string GetConfig(string key)
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            
            string value = config.AppSettings.Settings[key].Value;
            
            
            return value;
        }
        //example
        private void AccessAppSettings()
        {
            //获取Configuration对象
            
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


            //根据Key读取<add>元素的Value

            string name = config.AppSettings.Settings["name"].Value;
            //写入<add>元素的Value
            config.AppSettings.Settings["name"].Value = "xieyc";
            //增加<add>元素
            config.AppSettings.Settings.Add("url", "http://www.xieyc.com");
            //删除<add>元素
            config.AppSettings.Settings.Remove("name");
            //一定要记得保存，写不带参数的config.Save()也可以
            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
