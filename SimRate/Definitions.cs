using System.Runtime.InteropServices;

namespace SimRate
{
    public enum  DEFINITIONS
    {
        Struct1
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct Struct1
    {
        public float SimRate;
    };
}
