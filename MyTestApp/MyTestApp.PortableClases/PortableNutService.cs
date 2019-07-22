using Nut;

namespace MyTestApp.PortableClases
{
    public static class PortableNutService
    {
        public static string ToText(this int number)
        {
            return number.ToText("es");
        }
    }
}
