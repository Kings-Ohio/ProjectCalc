using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCalc.Extensions
{
    public static class Extensions
    {
        public static string ToDisplayFraction(this Fraction_class fraction, string zeroOpt = "0")
        {
            fraction.Reduce();

            if (fraction.denominator==0 || fraction.numerator == 0)
            {
                if (fraction.wholeNumber == 0)
                    return zeroOpt;
                else
                    return $"{fraction.wholeNumber}";
            }
            else  
            {
                if (fraction.wholeNumber == 0)
                    return $"{fraction.numerator}/{fraction.denominator}";
                else
                return $"{fraction.wholeNumber} { fraction.numerator}/{ fraction.denominator}";
            }        

        }


        public static string ToDisplayFloat(this Fraction_class fract, string zeroOpt="0")
        {
            string s = fract.ToFloat().ToString();

            if (s == "0")
                return zeroOpt;

            return fract.ToFloat().ToString();

        }

        // Reduce fraction by GCD. Called after all math, and before display.
        public static void Reduce(this Fraction_class fraction)
        {
            if (fraction.wholeNumber==0 && (fraction.numerator == 0 ||fraction.denominator == 0)) return;

            if (fraction.denominator == 1 )
            {
                fraction.wholeNumber += fraction.numerator;
                fraction.numerator = 0;
                fraction.denominator = 0;
                return;
            }

            if (fraction.numerator < 0 || fraction.wholeNumber < 0)
            {
                fraction.fractionSign = Sign.negative;
                fraction.numerator *= -1;
            }

            if( fraction.denominator !=0 && fraction.denominator <= fraction.numerator)
            {
                fraction.wholeNumber += fraction.numerator / fraction.denominator;
                fraction.numerator %= fraction.denominator;
                if (fraction.numerator == 0) fraction.denominator = 0;
            }

            int GCD = 0;
            for (int nextN=fraction.numerator;nextN>1;--nextN)
            {
                if (fraction.numerator % nextN == 0 &&
                    fraction.denominator % nextN ==0 )
                {
                    GCD = nextN;
                    break;
                }
            }

            if (GCD !=0)
            {
                fraction.numerator /= GCD;
                fraction.denominator /= GCD;
            }

            if (fraction.denominator == 1)
            {
                fraction.wholeNumber += fraction.numerator;
                fraction.numerator = 0;
                fraction.denominator = 0;
                return;
            }

            if (fraction.fractionSign == Sign.negative)
            {
                fraction.numerator *= -1;
                fraction.denominator *= -1;
                fraction.wholeNumber *= -1;
            }

        }

        // ToSimpleFraction adds the whole number to the fraction numerator for additive use.
        public static void ToSimpleFraction(this Fraction_class fraction)
        {
            if (fraction.numerator == 0)
            {
                fraction.numerator = fraction.wholeNumber;
                fraction.denominator = 1;
                fraction.wholeNumber = 0;
            }
            else
            {
                fraction.numerator += fraction.wholeNumber * fraction.denominator;
                fraction.wholeNumber = 0;
            }
        }


        public static double ToFloat(this Fraction_class fraction)
        {
            if (fraction.numerator == 0 || fraction.denominator == 0)
                return (double)fraction.wholeNumber;
            else
            return (double)fraction.wholeNumber + (double)fraction.numerator / fraction.denominator;
        }
    }

}
