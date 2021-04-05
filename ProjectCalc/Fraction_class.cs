using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ProjectCalc.Extensions;

namespace ProjectCalc
{
    public enum Sign { positive, negative };  
    public class Fraction_class
    {
        public Sign fractionSign { get; set; }

        public int wholeNumber;
        public int numerator;
        public int denominator;

        public Fraction_class()
        {
            Clear();
            //fractionSign = Sign.positive;
            //wholeNumber = 0;
            //numerator = 0;
            //denominator = 0;
        }

        

        public  Fraction_class( string input)
        {
            Parse(input);  // bool return unhandled
        }

        public bool Parse(string input)
        {
            if(string.IsNullOrEmpty(input)) return false;

            if (input.Contains("."))
                return ParseDecimal(input);
            else
                return ParseFraction(input);
        }

        private bool ParseDecimal( string input)
        {
            if (input.Trim().StartsWith("."))
                input = input.Replace(".", "0.");
            string[] stringSplit = input.Trim().Split('.');

            try
            {
                if (stringSplit.Length > 0)
                    this.wholeNumber = Int32.Parse(stringSplit[0].Trim());

                if (stringSplit.Length > 1)
                {
                    this.numerator = Int32.Parse(stringSplit[1].Trim());
                    double orderMag = stringSplit[1].Trim().Length;
                    this.denominator = (int)Math.Pow(10.0, orderMag);
                    this.Reduce();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }


        private bool ParseFraction(string input)
        {
            Int32 work_int = 0;

            if (string.IsNullOrWhiteSpace(input))
            {
                Clear();
                return true;
            }

            this.Clear();
            string[] stringSplit = input.Trim().Split(' ');
            string[] fracSplit = null;
            if (stringSplit.Length > 1 || stringSplit[0].Contains("/"))     // May begin with fraction n/m or whole number
                fracSplit = stringSplit[stringSplit.Length - 1].Split('/');   // " 1   1/2": account for too many spaces before fraction part.

            if (stringSplit[0].Contains("/"))
                this.wholeNumber = 0;
            else
            if (stringSplit.Length > 0)
                if (Int32.TryParse(stringSplit[0], out work_int))   // Use Int64.Parse?
                    this.wholeNumber = work_int;

            if (!(fracSplit is null) && fracSplit.Length > 1)
            {
                int n = 0;
                int m = 0;
                if (Int32.TryParse(fracSplit[0], out n) &&
                    (Int32.TryParse(fracSplit[1], out m)))
                {
                    this.numerator = n;
                    this.denominator = m;
                }
                else
                    return false;
            }

            return true;
        }


        public void Clear()
        {
            fractionSign = Sign.positive;
            wholeNumber = 0;
            numerator = 0;
            denominator = 0;
        }

        internal void SetValueTo(Fraction_class inBound)
        {
            fractionSign = inBound.fractionSign;
            wholeNumber = inBound.wholeNumber;
            numerator = inBound.numerator;
            denominator = inBound.denominator;
        }

        internal void Add(Fraction_class inBound)
        {
            inBound.ToSimpleFraction();
            this.ToSimpleFraction();
           
            this.numerator =
                (inBound.numerator * this.denominator) + (this.numerator * inBound.denominator);
            this.denominator =
                (inBound.denominator * this.denominator);

             this.Reduce();            
        }

       // internal void MultiplyBy(Fraction_class inBound)
            public void MultiplyBy(Fraction_class inBound)
        {
            inBound.ToSimpleFraction();
            this.ToSimpleFraction();

            this.numerator *= inBound.numerator;
            this.denominator *= inBound.denominator;

            fractionSign = wholeNumber < 0 || numerator < 0 ? Sign.negative : Sign.positive;

            this.Reduce();
        }

        //  internal void Subtract(Fraction_class inBound)
        public void Subtract(Fraction_class inBound)
        {
            inBound.ToSimpleFraction();
            this.ToSimpleFraction();

            this.numerator =
               (this.numerator * inBound.denominator) - (inBound.numerator * this.denominator) ;
            this.denominator =
                (inBound.denominator * this.denominator);

            fractionSign = wholeNumber < 0 || numerator < 0 ? Sign.negative : Sign.positive;

            this.Reduce();
        }

        //internal void DivideBy(Fraction_class inBound)
        public void DivideBy(Fraction_class inBound)
        {
            inBound.ToSimpleFraction();
            this.ToSimpleFraction();

            this.denominator = inBound.numerator * this.denominator;
            this.numerator = inBound.denominator * this.numerator;

            fractionSign = wholeNumber < 0 || numerator < 0 ? Sign.negative : Sign.positive;

            this.Reduce();
        }

        public void Modulus(Fraction_class inbound)
        {
            try
            {
                double dbl = this.ToFloat() % inbound.ToFloat();
                this.Parse(dbl.ToString());
            }
            catch( Exception e0)
            {
                //

            }
        }

    }
}
