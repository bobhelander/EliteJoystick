using System;
using System.Collections.Generic;
using System.Text;

namespace EliteJoystick.Common.Logic
{
    public class KeyCode
    {
        public string Name { get; set; }
        public byte Code { get; set; }
        public string Value { get; set; }
        public string ShiftedValue { get; set; }
        public string Description { get; set; }

        public KeyCode() { }
        public KeyCode(string name, byte code, string value, string shiftedValue, string description) 
        {
            Name = name;
            Code = code;
            Value = value;
            ShiftedValue = shiftedValue;
            Description = description;
        }

        public KeyCode Combine(KeyCode value)
        {
            Code |= value.Code;
            return this;
        }
    }
}
