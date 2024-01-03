namespace Extensions
{
    public static class BoolExtensions
    {
        public static float ToSign(this bool value) => value ? 1 : -1;
    }
}