using UnityEngine;
using UnityEditor;

public class BatchMover : EditorWindow
{
	bool bReverseTranslate = false;
	Vector3 translate = new Vector3();

	bool bReverseRotate = false;
	Vector3 rotate = new Vector3();

	bool bReverseScale = false;
	Vector3 scale = new Vector3(1, 1, 1);

	[MenuItem("Window/Batch Mover")]
	static void Awake()
	{
		GetWindow<BatchMover>().Show();
	}

	void OnGUI()
	{
		minSize = new Vector2(226, 226);

		bReverseTranslate = EditorGUILayout.Toggle(new GUIContent("Reverse Translate", "Causes the translation to happen in reverse.\nBasically a simple method to Undo."), bReverseTranslate);
		translate = EditorGUILayout.Vector3Field("Translate Distance", translate);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("World Translate"))
		{
			BatchMove(bReverseTranslate ? -translate : translate, true);
		}
		if (GUILayout.Button("Local Translate"))
		{
			BatchMove(bReverseTranslate ? -translate : translate);
		}
		EditorGUILayout.EndHorizontal();

		bReverseRotate = EditorGUILayout.Toggle(new GUIContent("Reverse Rotation", "Causes the rotation to happen in reverse.\nBasically a simple method to Undo."), bReverseRotate);
		rotate = EditorGUILayout.Vector3Field("Rotation Amount", rotate);
		if (GUILayout.Button("Rotate"))
		{
			BatchRotate(bReverseRotate ? -rotate : rotate);
		}

		bReverseScale = EditorGUILayout.Toggle(new GUIContent("Reverse Scale Mutlipler", "Causes the scale multiplication to happen in reverse.\nBasically a simple method to Undo. Only works on Multiply."), bReverseScale);
		scale = EditorGUILayout.Vector3Field("Scale Multiplier", scale);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Multiply Scale"))
		{
			BatchScale(bReverseScale ? new Vector3(1 / scale.x, 1 / scale.y, 1 / scale.z) : scale);
		}
		if (GUILayout.Button("Set Scale"))
		{
			BatchScale(scale, true);
		}
		EditorGUILayout.EndHorizontal();
	}

	void BatchMove(Vector3 amount, bool bWorldSpace = false)
	{
		Undo.RecordObjects(Selection.transforms, "Batch Move");

		for (int i = 0; i < Selection.transforms.Length; ++i)
		{
			if (bWorldSpace)
			{
				Selection.transforms[i].Translate(amount, Space.World);
			}
			else
			{
				Selection.transforms[i].Translate(amount, Space.Self);
			}
		}
	}

	void BatchRotate(Vector3 amount)
	{
		Undo.RecordObjects(Selection.transforms, "Batch Rotate");

		for (int i = 0; i < Selection.transforms.Length; ++i)
		{
			Selection.transforms[i].Rotate(amount);
		}
	}

	void BatchScale(Vector3 amount, bool bSetScale = false)
	{
		Undo.RecordObjects(Selection.transforms, "Batch Scale");

		for (int i = 0; i < Selection.transforms.Length; ++i)
		{
			if (bSetScale)
			{
				Selection.transforms[i].localScale = amount;
			}
			else
			{
				Selection.transforms[i].localScale = new Vector3
				(
					Selection.transforms[i].localScale.x * amount.x,
					Selection.transforms[i].localScale.y * amount.y,
					Selection.transforms[i].localScale.z * amount.z
				);
			}
		}
	}
}
