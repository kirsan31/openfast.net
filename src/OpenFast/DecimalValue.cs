/*

The contents of this file are subject to the Mozilla Public License
Version 1.1 (the "License"); you may not use this file except in
compliance with the License. You may obtain a copy of the License at
http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS"
basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
License for the specific language governing rights and limitations
under the License.

The Original Code is OpenFAST.

The Initial Developer of the Original Code is The LaSalle Technology
Group, LLC.  Portions created by Shariq Muhammad
are Copyright (C) Shariq Muhammad. All Rights Reserved.

Contributor(s): Shariq Muhammad <shariq.muhammad@gmail.com>
                Yuri Astrakhan <FirstName><LastName>@gmail.com
*/
using System;
using OpenFAST.Error;
using OpenFAST.Utility;

namespace OpenFAST
{
    public sealed class DecimalValue : NumericValue, IEquatable<DecimalValue>
    {
        public readonly int Exponent;
        public readonly long Mantissa;

        public DecimalValue(double value) : this((decimal)value)
        {
        }

        public DecimalValue(long mantissa, int exponent)
        {
            Mantissa = mantissa;
            Exponent = exponent;
        }

        public DecimalValue(decimal bigDecimal)
        {
            if (bigDecimal == 0)
            {
                Exponent = 0;
                Mantissa = 0;
                return;
            }

            int exp = Util.BigDecimalScale(bigDecimal);
            long mant = Util.BigDecimalUnScaledValue(bigDecimal);

            while (((mant % 10) == 0) && (mant != 0))
            {
                mant /= 10;
                exp -= 1;
            }

            Mantissa = mant;
            Exponent = -exp;
        }

        public override bool IsNull
        {
            get { return false; }
        }

        private long Value
        {
            get { return Mantissa * ((long)Math.Pow(10, Exponent)); }
        }

        public override NumericValue Increment()
        {
            return null;
        }

        public override NumericValue Decrement()
        {
            return null;
        }

        public override NumericValue Subtract(NumericValue subtrahend)
        {
            return new DecimalValue(decimal.Subtract(ToBigDecimal(), subtrahend.ToBigDecimal()));
        }

        public override NumericValue Add(NumericValue addend)
        {
            return new DecimalValue(decimal.Add(ToBigDecimal(), addend.ToBigDecimal()));
        }

        [Obsolete("need?")] // BUG? Do we need this?
        public string Serialize()
        {
            return ToString();
        }

        public override bool EqualsInt(int value)
        {
            return false;
        }

        public override long ToLong()
        {
            if (Exponent < 0)
            {
                Global.ErrorHandler.OnError(null, RepError.DecimalCantConvertToInt, "");
            }
            return Value;
        }

        public override int ToInt()
        {
            long v = Value;
            if (Exponent < 0 || v > int.MaxValue)
            {
                Global.ErrorHandler.OnError(null, RepError.DecimalCantConvertToInt, "");
            }
            return (int)v;
        }

        public override short ToShort()
        {
            long v = Value;
            if (Exponent < 0 || v > short.MaxValue)
            {
                Global.ErrorHandler.OnError(null, RepError.DecimalCantConvertToInt, "");
            }
            return (short)v;
        }

        public override byte ToByte()
        {
            long v = Value;
            if (Exponent < 0 || v > (byte)sbyte.MaxValue)
            {
                Global.ErrorHandler.OnError(null, RepError.DecimalCantConvertToInt, "");
            }
            return (byte)v;
        }

        public override double ToDouble()
        {
            return Mantissa * Math.Pow(10.0, Exponent);
        }

        public override decimal ToBigDecimal()
        {
            return (decimal)(Mantissa / Math.Pow(10, -Exponent));
        }

        public override string ToString()
        {
            return ToBigDecimal().ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        #region Equals (optimized for empty parent class)

        public bool Equals(DecimalValue other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other.Exponent == Exponent && other.Mantissa == Mantissa;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (!(obj is DecimalValue t))
                return false;
            return t.Exponent == Exponent && t.Mantissa == Mantissa;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Exponent * 397) ^ Mantissa.GetHashCode();
            }
        }

        #endregion
    }
}