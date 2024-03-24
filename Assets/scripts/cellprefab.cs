using UnityEngine;

public class CellBehavior : MonoBehaviour
{
    private Color originalColor;
    private Renderer rend;
    public GameObject cancercell;
    void Start()
    {
        cancercell.gameObject.tag = "Untagged";
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        
        rend.material.color = Color.yellow;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            rend.material.color = originalColor;
            cancercell.gameObject.tag = "CancerCell";
        }
    }

}
