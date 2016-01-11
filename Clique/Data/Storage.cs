﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Clique.Data
{
    public class Storage
    {
        public async Task<string> ReadTextFileAsync(string path)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.GetFileAsync(path);
            return await FileIO.ReadTextAsync(file);
        }

        public async void WriteTotextFileAsync(string fileName, string contents)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, contents);
        }

        public void SaveSettings(string key, string contents)
        {
            ApplicationData.Current.LocalSettings.Values[key] = contents;
        }

        public string LoadSettings(string key)
        {

            var SettingIsSet = 0;

            if(ApplicationData.Current.LocalSettings.Values.Keys.Contains(key))
            {
                SettingIsSet = 1;
            }
            if(SettingIsSet == 1)
            {
                var settings = ApplicationData.Current.LocalSettings;
                return settings.Values[key].ToString();
            }
            else
            {
                string contents = "Null";
                SaveSettings(key, contents);

                var settings = ApplicationData.Current.LocalSettings;
                return settings.Values[key].ToString();
            }
        }
        public void SaveSettingsInContainer(string user, string key, string contents)
        {
            var localSetting = ApplicationData.Current.LocalSettings;

            localSetting.CreateContainer(user, ApplicationDataCreateDisposition.Always);

            if (localSetting.Containers.ContainsKey(user))
            {
                localSetting.Containers[user].Values[key] = contents;
            }
        }
    }

}
