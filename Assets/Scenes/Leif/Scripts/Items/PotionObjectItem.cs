using Unity.VisualScripting;

public class PotionObjectItem : Item
{
    [DoNotSerialize] public PotionItem itemData2;

    // public void CopyData()
    // {
    //     var asd = GetComponent<Item>();
    //     if (asd == null) return;
    //     itemData2 = (PotionItem)asd.itemData;
    //     itemData = (PotionItem)asd.itemData;
    //     DestroyImmediate(asd, true);
    // }
}