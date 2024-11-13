using UnityEngine;

public class SetButtonUpgrade : MonoBehaviour
{
    void Start()
    {
        var specialButton = GetComponent<SpecialButton>();
        specialButton._onClick.AddListener(Upgrade);
    }

    public void Upgrade()
    {
        int indexInHierarchy = transform.parent.GetSiblingIndex();
        var inventory = FindFirstObjectByType<CraftItem>();
        var receipe = GetComponent<CraftReciepe>();

        if(inventory.CanUpgrade(receipe))
        {
            inventory.UpgradeOxyBulle(receipe);

            if (this.transform.parent.parent.GetChild(indexInHierarchy + 1).gameObject != null)
            {
                this.transform.parent.parent.GetChild(indexInHierarchy + 1).gameObject.SetActive(true);
            }
            this.transform.parent.gameObject.SetActive(false);
        }
    }
}
