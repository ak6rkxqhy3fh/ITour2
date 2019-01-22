using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITour.Utilities
{
    public static class MonthsOfYear
    {
        public static Dictionary<int, string> GetDictionary()
        {
            return new Dictionary<int, string>()
            {
                {1, "Январь" },
                {2, "Февраль" },
                {3, "Март" },
                {4, "Апрель" },
                {5, "Май" },
                {6, "Июнь" },
                {7, "Июль" },
                {8, "Август" },
                {9, "Сентябрь" },
                {10, "Октябрь" },
                {11, "Ноябрь" },
                {12, "Декабрь" },
                {0, "Нет" }
            };
        }
    }
}
