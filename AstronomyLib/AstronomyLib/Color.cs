namespace AstronomyLib
{
    public struct Color
    {
        public Color(float red, float green, float blue) : this()
        {
            Red = red;
            Green = green;
            Blue = Blue;
        }

        public float Red { set; get; }

        public float Green { set; get; }

        public float Blue { set; get; }

        public static Color White { get; } = new Color(1, 1, 1);
    }
}
