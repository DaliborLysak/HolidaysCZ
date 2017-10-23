using System;
using System.Collections.Generic;
using System.Linq;

namespace HolidaysCZ
{
    public class Holidays
    {
        public Holidays()
        {
            var year = StartYear;
            var thisYear = DateTime.Now.Year;
            while (year <= thisYear)
            {
                YearHolidays.Add(year, GetHolidaysInYear(year));
                year++;
            }
        }

        public bool IsHoliday(DateTime date)
        {
            var year = date.Year;
            var isHoliday = false;
            if (YearHolidays.ContainsKey(year))
            {
                var holiday = YearHolidays[year].FirstOrDefault(h => (h.Date.Day == date.Day) && (h.Date.Month == date.Month));
                isHoliday = holiday != null;
            }

            return isHoliday;
        }

        public Dictionary<int, List<Holiday>> YearHolidays { get; private set; } = new Dictionary<int, List<Holiday>>();

        private const int StartYear = 2017;

        private List<Holiday> GetHolidaysInYear(int year)
        {
            var easter = GetEasterSunday(year);
            var holidays = new List<Holiday>()
            {
                new Holiday() { Date = new DateTime(year, 1, 1), Name ="Nový rok, Den obnovy samostatného českého státu" },
                GetEasterDependedDay(easter, -2, "Velký pátek" ),
                GetEasterDependedDay(easter, +1, "Velikonoční pondělí"),
                new Holiday() { Date = new DateTime(year, 5, 1), Name ="Svátek práce" },
                new Holiday() { Date = new DateTime(year, 5, 8), Name ="Den vítězství" },
                new Holiday() { Date = new DateTime(year, 7, 5), Name ="Den slovanských věrozvěstů Cyrila a Metoděje" },
                new Holiday() { Date = new DateTime(year, 7, 6), Name ="Den upálení mistra Jana Husa" },
                new Holiday() { Date = new DateTime(year, 9, 28), Name ="Den české státnosti" },
                new Holiday() { Date = new DateTime(year, 10, 28), Name ="Den vzniku samostatného československého státu" },
                new Holiday() { Date = new DateTime(year, 11,17), Name ="Den boje za svobodu a demokracii" },
                new Holiday() { Date = new DateTime(year, 12, 24), Name ="Štědrý den" },
                new Holiday() { Date = new DateTime(year, 12, 25), Name ="1. svátek vánoční" },
                new Holiday() { Date = new DateTime(year, 12, 26), Name ="2. svátek vánoční" }
            };

            return holidays;
        }

        private Holiday GetEasterDependedDay(DateTime sunday, int shift, string name)
        {
            var date = sunday.AddDays(shift);
            return new Holiday() { Date = date, Name = name };
        }

        private DateTime GetEasterSunday(int year)
        {
            int day = 0;
            int month = 0;

            int doubleDecade = year % 19;
            int centuries = year / 100;
            int x = (centuries - (centuries / 4) - ((8 * centuries + 13) / 25) + 19 * doubleDecade + 15) % 30;
            int y = x - (x / 28) * (1 - (x / 28) * (29 / (x + 1)) * ((21 - doubleDecade) / 11));

            day = y - ((year + (year / 4) + y + 2 - centuries + (centuries / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }
    }
}
