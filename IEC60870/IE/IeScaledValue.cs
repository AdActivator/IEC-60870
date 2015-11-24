﻿using System;
using System.IO;

namespace IEC60870.IE
{
    public class IeScaledValue : IeNormalizedValue
    {
        public IeScaledValue(int value) : base(value)
        {
        }

        public IeScaledValue(BinaryReader reader) : base(reader)
        {
        }

        public override String ToString()
        {
            return "Scaled value: " + value;
        }
    }
}
