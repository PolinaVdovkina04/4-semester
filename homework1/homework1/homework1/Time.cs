using System;
using System.Reflection;

namespace TimeStruct
{
    public struct Time
    {
        const int maxValue = 59;
        const int secondsInMinute = 60;
        const int secondsInHour = 3600;

        int hours;
        public int Hours
        {
            get => hours;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Значение должно быть неотрицательным");

                hours = value;
            }
        }

        int minutes;
        public int Minutes
        {
            get => minutes;
            set
            {
                if (value < 0 || value > maxValue)
                    throw new ArgumentException("Значение должно быть неотрицательным и не более 59");

                minutes = value;
            }
        }

        int seconds;
        public int Seconds
        {
            get => seconds;
            set
            {
                if (value < 0 || value > maxValue)
                    throw new ArgumentException("Значение должно быть неотрицательным и не более 59");

                seconds = value;
            }
        }

        public int DurationInSeconds
        {
            get => (hours* secondsInMinute* secondsInMinute)+(minutes* secondsInMinute)+seconds;
        }

        public Time(int hours, int minutes, int seconds) : this()
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        public override string ToString()
        {
            if (Hours < 10)
            {
                if (Minutes < 10)
                {
                    if (seconds < 10)
                        return $"0{Hours}:0{Minutes}:0{Seconds}";
                    return $"0{Hours}:0{Minutes}:{Seconds}";
                }
                return $"0{Hours}:{Minutes}:{Seconds}";
            }
            else if (Minutes < 10)
            {
                if (Seconds < 10)
                    return $"{Hours}:0{Minutes}:0{Seconds}";
                return $"{Hours}:0{Minutes}:{Seconds}";
            }
            else if (seconds < 10)
                return $"{Hours}:{Minutes}:0{Seconds}";
            else
                return $"{Hours}:{Minutes}:{Seconds}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Time)
                return DurationInSeconds == ((Time)obj).DurationInSeconds;

            throw new ArgumentException("Объект для сравнения не является временем");
        }

        public override int GetHashCode() => DurationInSeconds.GetHashCode();

        public static bool operator ==(Time x, Time y) => x.Equals(y);
        public static bool operator !=(Time x, Time y) => !x.Equals(y);
        public static bool operator >(Time x, Time y) => x.DurationInSeconds > y.DurationInSeconds;
        public static bool operator <(Time x, Time y) => x.DurationInSeconds < y.DurationInSeconds;
        public static bool operator >=(Time x, Time y) => x.DurationInSeconds >= y.DurationInSeconds;
        public static bool operator <=(Time x, Time y) => x.DurationInSeconds <= y.DurationInSeconds;

        public static Time operator +(Time x, Time y) =>
            GetTimeByValueInseconds(x.DurationInSeconds + y.DurationInSeconds);

        public static Time operator -(Time x, Time y) 
        {
            if (x.DurationInSeconds < y.DurationInSeconds)
                throw new ArgumentException("Уменьшаемое не может быть меньше вычитаемого");
            return GetTimeByValueInseconds(x.DurationInSeconds - y.DurationInSeconds);
        }

        public static Time operator *(Time x, double k)
        {
            if (k < 0)
                throw new ArgumentException("Необходимо неотрицательное действительное число");
            return GetTimeByValueInseconds((int)Math.Round(k * x.DurationInSeconds));
        }

        private static Time GetTimeByValueInseconds(int val)
        {
            int seconds = Math.Abs(val);
            int hours = Math.Sign(val) * (seconds / secondsInHour);
            seconds %= secondsInHour;
            int minutes = seconds / secondsInMinute;
            seconds %= secondsInMinute;

            return new Time(hours, minutes, seconds);
        }
    }
}
