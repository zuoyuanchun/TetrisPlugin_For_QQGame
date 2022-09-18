using System;
using Microsoft.Win32;

namespace QQGameTool
{
    internal class RegU
    {
       public RegU()
        {
            //获取默认注册表主键位置
            name = DirCheck.ReadRegInfo();
            try
            {
                if (!ExistSubKey())
                {
                    CreateSubKey();
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 创建注册表子键
        /// </summary>
        public void CreateSubKey()//创建注册表子键
        {
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey registryKey = localMachine.CreateSubKey("SOFTWARE\\" + name);
            localMachine.Close();
            registryKey.Close();
        }
        /// <summary>
        /// 判断子键是否存在
        /// </summary>
        /// <returns>存在true,不存在false</returns>
        public bool ExistSubKey()//注册表子键存在
        {
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey registryKey = localMachine.OpenSubKey("SOFTWARE\\" + name, true);
            localMachine.Close();
            if (registryKey == null)
            {
                return false;
            }
            registryKey.Close();
            return registryKey != null;
        }
        /// <summary>
        /// 删除注册表子键
        /// </summary>
        public void DeletedSubKey()//删除注册表子类
        {
            RegistryKey localMachine = Registry.LocalMachine;
            localMachine.DeleteSubKey("SOFTWARE\\" + name, true);
            localMachine.Close();
        }
        /// <summary>
        /// 写入注册表键值
        /// </summary>
        /// <param name="key">要写入的键名</param>
        /// <param name="value">要写入的键值</param>
        public void SetValue(string key, string value)
        {
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey registryKey = localMachine.OpenSubKey("SOFTWARE\\" + name, true);
            registryKey.SetValue(key, value, RegistryValueKind.String);
            localMachine.Close();
            registryKey.Close();
        }
        /// <summary>
        /// 获取注册表键值
        /// </summary>
        /// <param name="key">要读取的键名</param>
        /// <returns>返回读取到的键值(string),无键值则反回空值(string.Empty)</returns>
        public string getValue(string key)//读取注册表键值
        {
            string result;
            try
            {
                RegistryKey localMachine = Registry.LocalMachine;
                RegistryKey registryKey = localMachine.OpenSubKey("SOFTWARE\\" + name, true);
                object value = registryKey.GetValue(key);
                string text = ((value == null) ? null : value.ToString());
                localMachine.Close();
                registryKey.Close();
                result = text;
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 删除注册表键值
        /// </summary>
        /// <param name="key">要删除的键名</param>
        public void deleteValue(string key)//删除注册表键值
        {
            try
            {
                RegistryKey localMachine = Registry.LocalMachine;
                RegistryKey registryKey = localMachine.OpenSubKey("SOFTWARE\\" + name, true);
                registryKey.DeleteValue(key, true);
                localMachine.Close();
                registryKey.Close();
            }
            catch
            {
            }
        }
        /// <summary>
        /// 判断注册表主键下的子键是否存在
        /// </summary>
        /// <param name="sKeyName">子键名(类似于目录)</param>
        /// <returns>存在true,不存在false</returns>
        public bool IsRegistryKeyExist(string sKeyName)
        {
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey registryKey = localMachine.OpenSubKey("SOFTWARE");
            string[] subKeyNames = registryKey.GetSubKeyNames();//获取主键下所有子类的名称
            foreach (string a in subKeyNames)
            {
                if (a == sKeyName)
                {
                    localMachine.Close();
                    registryKey.Close();
                    return true;
                }
            }
            localMachine.Close();
            registryKey.Close();
            return false;
        }
        /// <summary>
        /// 判断键名称是否存在
        /// </summary>
        /// <param name="sValueName">键名</param>
        /// <returns>存在true,不存在false</returns>
        public bool IsRegistryValueNameExist(string sValueName)
        {
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey registryKey = localMachine.OpenSubKey("SOFTWARE\\" + name);
            string[] valueNames = registryKey.GetValueNames();//获取子键中所有的键名称
            foreach (string a in valueNames)
            {
                if (a == sValueName)
                {
                    localMachine.Close();
                    registryKey.Close();
                    return true;
                }
            }
            localMachine.Close();
            registryKey.Close();
            return false;
        }

        public string name;
    }
}
