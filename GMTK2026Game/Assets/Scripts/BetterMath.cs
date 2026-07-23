public static class BetterMath
{
    public static int RealMod(int value, int modulo)
    {
        return ((value % modulo) + modulo) % modulo;
    }
}