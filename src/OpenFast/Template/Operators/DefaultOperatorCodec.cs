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
using OpenFAST.Template.Types;

namespace OpenFAST.Template.Operators
{
    internal sealed class DefaultOperatorCodec : OperatorCodec
    {
        internal DefaultOperatorCodec(Operator op, FastType[] types)
            : base(op, types)
        {
        }

        public override bool DecodeNewValueNeedsPrevious
        {
            get { return false; }
        }

        public override bool DecodeEmptyValueNeedsPrevious
        {
            get { return false; }
        }

        public override ScalarValue GetValueToEncode(ScalarValue value, ScalarValue priorValue, Scalar field)
        {
            return value == null
                       ? (field.DefaultValue.IsUndefined ? null : ScalarValue.Null)
                       : (value.Equals(field.DefaultValue) ? null : value);
        }

        public override ScalarValue DecodeValue(ScalarValue newValue, ScalarValue priorValue, Scalar field)
        {
            return newValue;
        }

        public override ScalarValue DecodeEmptyValue(ScalarValue priorValue, Scalar field)
        {
            return field.DefaultValue.IsUndefined ? null : field.DefaultValue;
        }

        #region Equals

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == GetType(); //POINTP
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}