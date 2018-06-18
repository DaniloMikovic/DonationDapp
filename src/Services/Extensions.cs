using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace DonacijeDapp.Services
{
    public static class Extensions
    {
        public static DateTime UnixTimeStampToDateTime(this BigInteger unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds((double)unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
