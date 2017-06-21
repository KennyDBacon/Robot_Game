using System.Xml;
using System.Xml.Serialization;

public class TargetStat
{
	/// <summary>
	/// ID of Target.
	/// </summary>
	[XmlAttribute ("ID")]
	public int ID;

	/// <summary>
	/// Name of Target.
	/// </summary>
	[XmlElement ("Label")]
	public string Label;

	/// <summary>
	/// Type of Target.
	/// </summary>
	[XmlElement ("Type")]
	public string Type;

	/// <summary>
	/// If Target is active in game.
	/// </summary>
	[XmlElement ("IsActive")]
	public bool IsActive;

	/// <summary>
	/// Index reference for receiving data.
	/// </summary>
	[XmlElement ("StatusIndex")]
	public int StatusIndex;

	/// <summary>
	/// Index reference for sending command.
	/// </summary>
	[XmlElement ("CommandIndex")]
	public int CommandIndex;

	/// <summary>
	/// The position of the Target in game.
	/// </summary>
	[XmlElement ("PositionIndex")]
	public int PositionIndex;
}
