using System;

namespace Covid19.Primitives
{
    [Flags]
    public enum Symptoms
    {
        None = 0,
        Fever = 1 << 0,
        Cough = 1 << 1,
        Headache = 1 << 2,
        BreathingDifficulty = 1 << 3,
        Others = 1 << 16,
    }
}
