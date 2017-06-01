using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot ("TargetStatCollection")]
public class TargetStatContainer
{
	[XmlArray ("Targets"),XmlArrayItem ("Target")]
	public TargetStats[] TargetStats;

	public static TargetStatContainer Load (string path)
	{
		var serializer = new XmlSerializer (typeof(TargetStatContainer));
		using (var stream = new FileStream (path, FileMode.Open)) {
			return serializer.Deserialize (stream) as TargetStatContainer;
		}
	}
}
