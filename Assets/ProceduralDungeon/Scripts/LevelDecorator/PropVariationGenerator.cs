using UnityEngine;

public class PropVariationGenerator : MonoBehaviour
{

	[ContextMenu("Generate Variation")]
	internal void GenerateVariation()
	{
		PropSelectionXor[] xorSelections = GetComponents<PropSelectionXor>();
		foreach (PropSelectionXor selection in xorSelections)
		{
			selection.GenerateVariation();
		}
		PropVariationOr[] orVariations = GetComponents <PropVariationOr>();
		foreach (PropVariationOr orVariation in orVariations)
		{
			orVariation.GenerateVariation();
		}
	}

}
