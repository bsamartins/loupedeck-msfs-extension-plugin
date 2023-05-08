namespace Loupedeck.MsfsExtensionPlugin.Helpers
{
    using System;
    using System.Diagnostics;

    public class ConvertTool
    {
        public static Int64 getToggledValue(Int64 value) => value == 0 ? 1 : 0;

        public static Int64 ApplyAdjustment(Int64 value, Int32 ticks, Int32 min, Int32 max, Int32 step, Boolean cycle = false)
        {
            Debug.WriteLine(value);
            value += ticks * step;
            if (value < min)
            {
                value = cycle ? max : min;
            }
            else if (value > max)
            {
                value = cycle ? min : max;
            }
            return value;
        }
    }
}
