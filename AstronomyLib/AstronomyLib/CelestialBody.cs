namespace AstronomyLib
{
    public abstract class CelestialBody
    {
        // Only used internally
        private GeographicCoordinate Location { set; get; }

        // Set here, used by descendent classes
        protected Time Time { private set; get; }

        // Set by descendent classes, used here
        protected EquatorialCoordinate EquatorialCoordinate { set; private get; }

        // Set here, used external to library
        public HorizontalCoordinate HorizontalCoordinate { private set; get; }

        // Used externally to retain screen location
        public float ScreenX;
        public float ScreenY;

        // Called external to update HorizontalCoordinate
        public void Update(Time time, GeographicCoordinate location)
        {
            bool needsUpdate = false;

            if (!this.Time.Equals(time))
            {
                this.Time = time;
                needsUpdate = true;
                OnTimeChanged();
            }

            if (!this.Location.Equals(location))
            {
                this.Location = location;
                needsUpdate = true;
            }

            if (needsUpdate)
            {
                this.HorizontalCoordinate = 
                    HorizontalCoordinate.From(this.EquatorialCoordinate, 
                                              this.Location, this.Time);
            }
        }

        // Overridden by descendent classes to update EquatorialCoordinate
        protected virtual void OnTimeChanged()
        {
        }
    }
}
