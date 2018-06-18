using System;

namespace AstronomyLib
{
    public struct Time
    {
        DateTime universalTime;

        // Cached values
        DateTime dynamicalTime;
        double julianEphemerisDay;
        Angle greenwichSiderealTime;

        // Static creation methods
        public static Time GetNow()
        {
            return CreateTime(DateTime.UtcNow);
        }

        public static Time FromLocalTime(DateTime datetime)
        {
            return CreateTime(datetime.ToUniversalTime());
        }

        public static Time FromUniversalTime(DateTime datetime)
        {
            return CreateTime(datetime);
        }

        // There's obviously some number crunching involved when a Time value is first created,
        //  but it's better to do it now instead of for numerous celestial objects.
        static Time CreateTime(DateTime universalTime)
        {
            Time time = new Time();
            time.universalTime = universalTime;
            
            // Calculate dynamical time = univesal time + delta T
            //      from Meeus, pg 78 for 2000 through 2100
            double century = (universalTime.Year - 2000) / 100;
            double dt = 102 + 102 * century + 25.3 * century * century;
            dt += 0.37 * (universalTime.Year - 2100);
            TimeSpan deltat = TimeSpan.FromSeconds(dt);
            time.dynamicalTime = universalTime + deltat;

            // Calculate Julian Ephemeris Day 
            // Meeus, pgs 60-61.
            int Y = time.dynamicalTime.Year;
            int M = time.dynamicalTime.Month;
            double D = time.dynamicalTime.Day + (time.dynamicalTime.Hour +
                                           (time.dynamicalTime.Minute +
                                           (time.dynamicalTime.Second +
                                            time.dynamicalTime.Millisecond / 1000.0) / 60.0) / 60.0) / 24.0;
            if (M == 1 || M == 2)
            {
                Y -= 1;
                M += 12;
            }

            int A = Y / 100;
            int B = 2 - A + A / 4;

            time.julianEphemerisDay = Math.Floor(365.25 * (Y + 4716)) +
                                      Math.Floor(30.6001 * (M + 1)) + D + B - 1524.5;

            // Calculate Greenwich Sideral Time
            // Meeus, pg 87
            double t = 10 * time.Tau;

            time.greenwichSiderealTime = Angle.FromDegrees(280.46061837 +
                                     360.98564736629 * (time.julianEphemerisDay - 2451545.0) +
                                     0.000387933 * t * t -
                                     t * t * t / 38710000);

            time.greenwichSiderealTime.NormalizePositive();

            return time;
        }

        // Read-only properties
        public DateTime LocalTime
        {
            get { return universalTime.ToLocalTime(); }
        }

        public DateTime UniversalTime
        {
            get { return universalTime; }
        }

        public DateTime DynamicalTime
        {
            get { return dynamicalTime; }
        }

        public double Tau
        {
            get { return (JulianEphemerisDay - 2451545.0) / 365250; } 
        }

        public double JulianEphemerisDay
        {
            get { return julianEphemerisDay; }
        }

        public Angle GreenwichSiderealTime
        {
            get { return greenwichSiderealTime; }
        }

        // Operators and overrides
        public static bool operator == (Time time1, Time time2)
        {
            return time1.universalTime == time2.universalTime;
        }

        public static bool operator !=(Time time1, Time time2)
        {
            return time1.universalTime != time2.universalTime;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Time))
                return false;

            return Equals((Time)obj);
        }

        public bool Equals(Time that)
        {
            return this.universalTime.Equals(that.universalTime);
        }

        public override int GetHashCode()
        {
            return universalTime.GetHashCode();
        }
    }
}
