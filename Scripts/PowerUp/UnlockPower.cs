using UnityEngine;
using static CharacterSkinManager;

public class UnlockPower : MonoBehaviour
{
    public CharacterSkinManager skinManager;  
    public CharacterSkinManager.SkinType skinToUnlock;  

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

            skinManager.UnlockSkin(skinToUnlock);


            skinManager.ChangeSkin(skinToUnlock);


            Destroy(gameObject);
        }
    }
}