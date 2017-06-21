using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

[XmlRoot ("TargetStatCollection")]
public class TargetStatContainer
{
	[XmlArray ("Targets"),XmlArrayItem ("Target")]
	public List<TargetStat> TargetStats;

	public static TargetStatContainer Load (string path)
	{
		var serializer = new XmlSerializer (typeof(TargetStatContainer));
		using (var stream = new FileStream (path, FileMode.Open)) {
			return serializer.Deserialize (stream) as TargetStatContainer;
		}
	}
}
