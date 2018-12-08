
namespace Laserbeam.UI.HR.Models
{
    public class SamlAttribute
    {
        public readonly string Name;
        public readonly string Value;
        public SamlAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}