namespace Extensions
{
    public static class BoolExtensions
    {
        public static float ToSign(this bool value) => value ? 1 : -1;

        public static int ToInt(this bool value) => value ? 1 : 0;
    }
}