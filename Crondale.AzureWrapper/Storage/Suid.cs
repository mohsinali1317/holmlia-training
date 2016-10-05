using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crondale.AzureWrapper.Storage
{
    /// <summary>
    /// Shrinking unique id.
    /// 
    /// An alternative to Guid to provide approximately shrinking id for row keys
    /// </summary>
    public class Suid
    {
        const String CHARS = "abcdefghijklmnopqrstuvwxyz0123456789";

        public String Content { get; set; }


        public override string ToString()
        {
            return Content;
        }

        public static implicit operator string(Suid suid)  // implicit digit to byte conversion operator
        {
            return suid.ToString();
        }



        private static String randomSuffix()
        {
            
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(CHARS, 4)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }

        public static Suid NewSuid()
        {

            StringBuilder suid = new StringBuilder((long.MaxValue - DateTime.Now.Ticks).ToString("D19"));
            suid.Append(randomSuffix());

            return new Suid()
            {
                Content = suid.ToString()
            };
        }


    }
}
