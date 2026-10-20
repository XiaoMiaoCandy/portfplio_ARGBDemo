using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    //切换场景的目标名
    public string sceneToLoad;
    public Animator fadeAnim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //玩家进入，加载目标场景
            //SceneManager.LoadScene(sceneToLoad);
            fadeAnim.Play("FadeToWhite");
            StartCoroutine(DelayFade());
           
        }
    }
    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneToLoad);
    }

}
