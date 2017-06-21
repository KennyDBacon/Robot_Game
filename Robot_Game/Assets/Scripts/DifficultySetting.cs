using System.Collections.Generic;
using System.Xml.Serialization;

public class DifficultySetting
{
	[XmlAttribute ("ID")]
	public string ID;

	[XmlElement ("SpawnIntensity")]
	public float SpawnIntensity;

	[XmlElement ("MinSpawnInterval")]
	public float MinSpawnInterval;

	[XmlElement ("MaxSpawnInterval")]
	public float MaxSpawnInterval;

	[XmlElement ("ST_AttackInterval")]
	public float ST_AttackInterval;

	[XmlElement ("RA_AttackInterval")]
	public float RA_AttackInterval;
}
