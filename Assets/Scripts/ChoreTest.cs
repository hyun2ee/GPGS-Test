using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoreTest : MonoBehaviour
{
    private static ChoreTest _instance;
    public static ChoreTest instance {
        get {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType<ChoreTest>();

            if (_instance == null)
                _instance = new GameObject("Testobj").AddComponent<ChoreTest>();

            return _instance;
        }
    }

    //string testst = "LIKE_Bose<2";

    private IEnumerator Start() {
        yield return new WaitForSeconds(2);
        test();
    }

    void test() {
        int[] intarr = new int[10];
        int[] intarr2 = new int[] { 0, 1, 2, 3, 4, 5 };
        intarr = intarr2;
        Debug.Log("test - length of intarr: " + intarr.Length);
    }
}
