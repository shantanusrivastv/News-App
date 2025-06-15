using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Model.Helpers
{
    public static class DatetimeExtension
    {
        public static int GetCurrentAge(this DateTime dateWithTime)
        {
            var currentDate = DateTime.UtcNow;
            int age = currentDate.Year - dateWithTime.Year;

            if (currentDate < dateWithTime.AddYears(age))
            {
                age--;
            }

            return age;
        }

    }
}
