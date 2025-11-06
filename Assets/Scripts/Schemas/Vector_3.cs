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

public partial class Vector_3 : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve]
#endif
public Vector_3() { }
	[Type(0, "number")]
	public float x = default(float);

	[Type(1, "number")]
	public float y = default(float);

	[Type(2, "number")]
	public float z = default(float);
}

