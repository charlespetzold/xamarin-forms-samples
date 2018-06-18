using System;
using System.IO;
using System.Reflection;

namespace AstronomyLib
{
    public class VsopCruncher
    {
        // Structure used internally to store each term.
        struct Term
        {
            public double A, B, C;

            public double Calculate(double tau)
            {
                return (A * Math.Cos(B + C * tau));
            }
        }

        // termsArray basically stores all VSOP data.
        Term[,][] termsArray = new Term[3, 6][];

        public VsopCruncher(string strPlanetAbbreviation)
        {
            Assembly assembly = GetType().Assembly;
            string filename = "AstronomyLib.Data.VSOP87D." + strPlanetAbbreviation;

            using (Stream stream = assembly.GetManifestResourceStream(filename))
            using (StreamReader reader = new StreamReader(stream))
            {
                string strLine;

                while ((strLine = reader.ReadLine()) != null)
                {
                    // variable is 1, 2, or 3 corresponding to 
                    //  L (ecliptical longitude), B (ecliptical latitude), or R (radius).
                    int variable = Int32.Parse(strLine.Substring(41, 1));

                    // terms is the number of lines that follow.
                    int terms = Int32.Parse(strLine.Substring(60, 7));

                    // power is the exponent of time.
                    int power = Int32.Parse(strLine.Substring(59, 1));

                    // Allocate a new array for all the terms that follow.
                    Term[] termArray = new Term[terms];

                    // Fill up the termArray.
                    for (int i = 0; i < terms; i++)
                    {
                        strLine = reader.ReadLine();
                        termArray[i].A = Double.Parse(strLine.Substring(79, 18));
                        termArray[i].B = Double.Parse(strLine.Substring(97, 14));
                        termArray[i].C = Double.Parse(strLine.Substring(111, 20));
                    }

                    // Save it as part of the big array.
                    termsArray[variable - 1, power] = termArray;
                }
            }
        }

        public double GetLongitude(double tau)
        {
            return Calculate(0, tau);
        }

        public double GetLatitude(double tau)
        {
            return Calculate(1, tau);
        }

        public double GetRadius(double tau)
        {
            return Calculate(2, tau);
        }

        double Calculate(int variable, double tau)
        {
            double final = 0;
            double tauPowered = 1;

            for (int power = 0; power < 6; power++)
            {
                Term[] termArray = termsArray[variable, power];

                if (termArray != null)
                {
                    double accum = 0;

                    for (int term = 0; term < termArray.Length; term++)
                        accum += termArray[term].Calculate(tau);

                    final += accum * tauPowered;
                }
                tauPowered *= tau;
            }
            return final;
        }
    }
}