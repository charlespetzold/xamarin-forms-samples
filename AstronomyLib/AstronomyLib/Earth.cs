namespace AstronomyLib
{
    public class Earth : Planet
    {
        static Earth()
        {
            Instance = new Earth("ear");
        }

        protected Earth(string strPlanetAbbreviation) : base(strPlanetAbbreviation)
        {
            this.Name = "Earth";
        }

        public static Earth Instance { protected set; get; }
    }
}
