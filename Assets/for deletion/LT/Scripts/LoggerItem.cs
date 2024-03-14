using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoggerItem : MonoBehaviour
{
	[Header("[Logger Item]")]
	public TMP_Text itemText;

	public Image itemImage;

	public void SetItem(string itemName, Sprite itemSprite)
	{
		itemText.text = itemName;
		itemImage.sprite = itemSprite;
	}
}
