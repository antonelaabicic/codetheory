using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.VisualBasic;

namespace codetheory.DAL.Config
{
    public class EncryptionService
    {
        public static IEncryptionProvider GetProvider()
        {
            var key = ConfigManager.UserEncryptionKey;
            return new GenerateEncryptionProvider(key);
        }
    }
}
