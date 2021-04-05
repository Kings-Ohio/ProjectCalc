using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ProjectCalc.Extensions;

namespace ProjectCalc
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private enum DisplayToggle { floating, fractional };
        private DisplayToggle displayToggle = DisplayToggle.fractional;


        private StringBuilder sbInBound = new StringBuilder();

        private StringBuilder sbAccumulator = new StringBuilder();
        private char lastOperator = '=';
        private Fraction_class InBound= new Fraction_class();
        private Fraction_class Accumulator = new Fraction_class();
        private Fraction_class MemorySave = new Fraction_class();
        private Fraction_class UndoAccum = new Fraction_class();

        public MainPage()
        {
            InitializeComponent();

        }


        private void OnDigitPressed(object sender, EventArgs e)
        {
            sbInBound.Append((sender as Button).Text);
            this.FindByName<Label>("Input").Text = sbInBound.ToString();  // "awkward"
            this.FindByName<Label>("Input").FontSize =
                shrinkLabel(this.FindByName<Label>("Input"), 8, 60, sbInBound.ToString());

        }

        private void OnSpacebarPressed(object sender, EventArgs e)
        {
            sbInBound.Append(" ");
            this.FindByName<Label>("Input").Text = sbInBound.ToString();  // "awkward"

        }

        // try/catch?
        private void OnOperatorPressed(object sender, EventArgs e)
        {
            // procedurally there's no input yet, maybe better way to do this
            if (this.FindByName<Label>("Input").Text is null ||  this.FindByName<Label>("Input").Text.Trim().Length == 0)
            {
                lastOperator = (sender as Button).Text.ToCharArray()[0];
                return;
            }

            //
            if (InBound.Parse(sbInBound.ToString()) == false)
            {
                this.FindByName<Label>("Input").Text = String.Empty;   // nasty error response.
                sbInBound.Clear();
                return;
            }
            sbInBound.Clear();
            this.FindByName<Label>("Input").Text = sbInBound.ToString();

            UndoAccum.SetValueTo(Accumulator);

            switch (lastOperator)
            {
                case '+':
                    Accumulator.Add( InBound);
                    break;
                case '-':
                    Accumulator.Subtract(InBound);
                    break;
                case '/':
                    Accumulator.DivideBy(InBound);
                    break;
                case '*':
                    Accumulator.MultiplyBy(InBound);
                    break;
                case '=':
                   if (Accumulator.ToDisplayFloat("0") == "0")
                        Accumulator.SetValueTo(InBound);
                    break;
                case '%':
                    Accumulator.Modulus(InBound);
                    break;
                default:
                   //-- Accumulator.SetValueTo(InBound); 
                    break;
            };

            lastOperator = (sender as Button).Text.ToCharArray()[0];

            sbAccumulator.Clear();  // whats the best way to set stringbuilder?
           if (displayToggle == DisplayToggle.fractional)
            sbAccumulator.Append(Accumulator.ToDisplayFraction());
           else
                sbAccumulator.Append(Accumulator.ToDisplayFloat());

            this.FindByName<Label>("Result").Text = sbAccumulator.ToString();
            this.FindByName<Label>("Result").FontSize = 
                shrinkLabel( this.FindByName<Label>("Result"), 16, 30, sbAccumulator.ToString());
 
        }


        private void OnClearPressed(object sender, EventArgs e)
        {
            sbInBound.Clear();
            sbAccumulator.Clear();
            this.FindByName<Label>("Input").Text = String.Empty;
            this.FindByName<Label>("Result").Text = String.Empty;
            InBound.Clear();
            Accumulator.Clear();
            lastOperator = '=';

        }

        private void OnBackspacePressed(object sender, EventArgs e)
        {
            if (sbInBound.Length < 1) return;

            sbInBound.Remove(sbInBound.Length - 1, 1);
            this.FindByName<Label>("Input").Text = sbInBound.ToString();
        }

        /// <summary>
        /// OnFractionToggle() - Toggles display between fractional and floating point affecting the
        ///   display fields of input and result. Displays either the "/" key or the "." depending 
        ///   whether using fractions or decimals.
        /// </summary>
        /// <param name="sender"></param> 
        /// <param name="e"></param>
        /// Side Effects: 
        ///    Changes GLOBALS Fraction_class instance InBound and its display string sbInBound.
        ///    Changes GLOBALS Fraction_class instance Accumulator and display string sbAccumulator.
        ///    
        private void OnFractionToggle(object sender, EventArgs e)
        {
            string workString = this.FindByName<Button>("DotSlash").Text; 

            workString = workString == "/" ? "." : "/";

            this.FindByName<Button>("DotSlash").Text = workString;

            if (workString == ".")
            {
                displayToggle = DisplayToggle.floating;
                this.FindByName<Button>("FractionToggle").Text = "Fraction";
                this.FindByName<Button>("SpaceBar").IsVisible = false;
                InBound.Parse(this.FindByName<Label>("Input").Text);
                sbInBound.Clear();
                sbInBound.Append(InBound.ToDisplayFloat(" "));
                this.FindByName<Label>("Input").Text = sbInBound.ToString();
                this.FindByName<Label>("Input").FontSize =
                    shrinkLabel(this.FindByName<Label>("Input"), 8, 60, sbInBound.ToString());
                sbAccumulator.Clear();
                sbAccumulator.Append(Accumulator.ToDisplayFloat(" "));
                Accumulator.Parse(sbAccumulator.ToString());
                this.FindByName<Label>("Result").Text = sbAccumulator.ToString();
                this.FindByName<Label>("Result").FontSize =
                         shrinkLabel(this.FindByName<Label>("Result"), 16, 30, sbInBound.ToString());
            }
            else
            {
                displayToggle = DisplayToggle.fractional;
                this.FindByName<Button>("FractionToggle").Text = "Decimal";
                this.FindByName<Button>("SpaceBar").IsVisible = true;
                sbInBound.Clear();
                sbInBound.Append(InBound.ToDisplayFraction(" "));
                InBound.Parse(sbInBound.ToString());
                this.FindByName<Label>("Input").Text = sbInBound.ToString();
                this.FindByName<Label>("Input").FontSize =
                    shrinkLabel(this.FindByName<Label>("Input"), 8, 60, sbInBound.ToString());
                sbAccumulator.Clear();  // whats the best way to set stringbuilder?
                sbAccumulator.Append(Accumulator.ToDisplayFraction(" "));
                Accumulator.Parse(sbAccumulator.ToString());
                this.FindByName<Label>("Result").Text = sbAccumulator.ToString();
                this.FindByName<Label>("Result").FontSize =
                      shrinkLabel(this.FindByName<Label>("Result"), 16, 30, sbInBound.ToString());
            }
        }

        /// <summary>
        /// OnMemoryLoadPressed() - Initially the M+ key ready to receive data from Accumulator.
        ///                       - Stores the data until M- or MC key is hit.
        ///                       - M+ key puts Accumulator data in memory. 
        ///                       - M- moves memory to InBound, and clears memory.
        /// 
        /// </summary>
        /// <remarks>
        ///   Side effects - MemButton keytop toggles between M+ and M-.
        ///                - if initially M+ Memory is set to Accumulator and toggles to M-.
        ///                - if initially M- InBound is set to Memory, Memory is cleared, and toggles to M+.
        /// </remarks>
        private void OnMemoryLoadPressed(object sender, EventArgs e)
        {
            string keyToggle = this.FindByName<Button>("MemButton").Text;
            if (keyToggle == "M+")
            {
                MemorySave.SetValueTo(Accumulator);
                this.FindByName<Button>("MemButton").Text = "M-";
            }
            else
            {
                InBound.SetValueTo(MemorySave);
                MemorySave.Clear();
                this.FindByName<Button>("MemButton").Text = "M+";
                sbInBound.Clear();
                if (displayToggle == DisplayToggle.floating)
                    sbInBound.Append(InBound.ToDisplayFloat(" "));
                else
                    sbInBound.Append(InBound.ToDisplayFraction(" "));

                this.FindByName<Label>("Input").Text = sbInBound.ToString();
            }
        }


/// <summary>
///   Key" "MC" - clear memory without restoring.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void OnMemoryClearPressed(object sender, EventArgs e)
        {
            MemorySave.Clear();           
            
        }


        private void OnUndoPressed(object sender, EventArgs e)
        {
            sbInBound.Clear();
            InBound.Clear();
            this.FindByName<Label>("Input").Text = string.Empty;


            Fraction_class workFraction = new Fraction_class();
            workFraction.SetValueTo(Accumulator);

            Accumulator.SetValueTo(UndoAccum);
            sbAccumulator.Clear();
            if (displayToggle == DisplayToggle.floating)
                sbAccumulator.Append(Accumulator.ToDisplayFloat(" "));
            else
                sbAccumulator.Append(Accumulator.ToDisplayFraction(" "));

            this.FindByName<Label>("Result").Text = sbAccumulator.ToString();

            lastOperator = ' ';

            UndoAccum.SetValueTo(workFraction); // next undo will circle back 

        }

        private double shrinkLabel(Label lbl, int firstLen, int maxFont, string text)
        {
            int lblLen = lbl.Text.Length;

            if (lblLen < firstLen) return maxFont;

            if (lblLen >= firstLen ) //&& lblLen % 8 == 0)
            {
                if (lbl.FontSize >= 20)
                    return lbl.FontSize - 20;
            }
            return lbl.FontSize;
        }

    }
}
