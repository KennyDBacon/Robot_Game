using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRoot ("DifficultySettings")]
public class DifficultyContainer
{
	[XmlArray ("Settings")]
	[XmlArrayItem ("Difficulty")]
	public List<DifficultySetting> Settings;

	public static DifficultyContainer Load (string path)
	{
		var serializer = new XmlSerializer (typeof(DifficultyContainer));
		using (var stream = new FileStream (path, FileMode.Open)) {
			return serializer.Deserialize (stream) as DifficultyContainer;
		}
	}
}
