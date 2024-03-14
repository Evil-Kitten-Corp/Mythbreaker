using System;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class PersistentAttribute : Attribute
{
	public readonly bool Persistent;

	public PersistentAttribute(bool persistent)
	{
		Persistent = persistent;
	}
}
