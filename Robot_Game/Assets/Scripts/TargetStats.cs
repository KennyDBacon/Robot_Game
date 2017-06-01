using System.Xml;
using System.Xml.Serialization;

public class TargetStats
{
	[XmlAttribute ("ID")]
	public string ID;

	[XmlElement ("IsActive")]
	public bool IsActive;
}
