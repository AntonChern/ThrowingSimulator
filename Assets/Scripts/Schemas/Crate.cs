// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 3.0.66
// 

using Colyseus.Schema;
#if UNITY_5_3_OR_NEWER
using UnityEngine.Scripting;
#endif

public partial class Crate : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve]
#endif
public Crate() { }
	[Type(0, "string")]
	public string author = default(string);

	[Type(1, "string")]
	public string owner = default(string);

	[Type(2, "ref", typeof(Vector_3))]
	public Vector_3 position = null;

	[Type(3, "ref", typeof(Vector_4))]
	public Vector_4 rotation = null;

	[Type(4, "number")]
	public float scale = default(float);
}

