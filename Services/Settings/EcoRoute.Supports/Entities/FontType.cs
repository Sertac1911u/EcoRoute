namespace EcoRoute.Settings.Entities
{
    public class FontType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // "Open Sans", "Georgia", vs.
        public string CssValue { get; set; } // "'Open Sans', sans-serif"
    }
}
