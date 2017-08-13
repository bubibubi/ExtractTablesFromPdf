namespace BuildTablesFromPdf.Engine
{
    public struct Color
    {
        public static readonly Color White = new Color(1, 1, 1);

        public Color(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public readonly float R;
        public readonly float G;
        public readonly float B;

        public bool IsWhite()
        {
            return R > .95 && G > .95 && B > .95;
        }
    }
}