using System;
using System.IO;
using System.Threading.Tasks;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Util;

namespace ItemObserveApp.Models.Repository
{
    public class FileUserRepository : IUserRepository
    {
        private const string FileName = "UserSetting.txt";

        public FileUserRepository()
        {
        }

        public async Task<UserSetting> GetUserSettingAsync()
        {
            try
            {

                // ファイルを取得する
                var text = await FileExtensions.LoadFileStringAsync(FileName);
                var splits = text.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                var usr = new UserSetting();
                foreach (var line in splits)
                {
                    var data = line.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (data.Length >= 2)
                    {
                        if (data[0] == "UserID")
                        {
                            usr.UserID = data[1];
                        }
                        else if (data[0] == "Password")
                        {
                            usr.Password = data[1];
                        }
                        else if (data[0] == "Token")
                        {
                            usr.Token = data[1];
                        }
                    }
                }
                return usr;
            }
            catch (Exception)
            {
                return new UserSetting();
            }
        }

        public async Task PutUserSettingAsync(UserSetting target)
        {
            var txt = "UserID," + target.UserID;
            txt = txt + Environment.NewLine + "Password," + target.Password;
            txt = txt + Environment.NewLine + "Token," + target.Token;
            // テキストをファイルに書き込む
            await txt.SaveToLocalFolderAsync(FileName);
        }
    }
}
