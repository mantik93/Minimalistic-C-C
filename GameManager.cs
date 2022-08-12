using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> BlueTeam, GreenTeam, RedTeam, YellowTeam;
    //0 танк, 1 кран, 2 турель, 3 фабрика.
    private string meTeam, enemyTeam;
    private int mePos, enemyPos;
    private Vector3[] startpos = new Vector3[4] { new Vector3(-30, 0, 30),
        new Vector3(30, 0, 30), new Vector3(-30, 0, -30), new Vector3(30, 0, -30)};
    //new Vector3[4] { new Vector3(-60, 0, 60),
    //new Vector3(60, 0, 60), new Vector3(-60, 0, -60), new Vector3(60, 0, -60) };
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
/*            var arr = GameObject.FindObjectsOfType<Health>();
            foreach (var item in arr)
            {
                new SavedHealth(item);
            }
            SavedHealth.Serialize();*/
            var arr = GameObject.FindObjectsOfType<Unit>();
            foreach (var item in arr)
            {
                new SavedUnit(item);
            }
            SavedHealth.Serialize();

        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            SavedHealth.Load();
            StartCoroutine(nameof(RecoveryLinks));
        }
    }
    public void NewGame(string me_color, string enemy_color, int me_corner, int enemy_corner)
    {
        SceneManager.LoadSceneAsync("Battlefield");
        meTeam = me_color;
        enemyTeam = enemy_color;
        mePos = me_corner;
        enemyPos = enemy_corner;
        SceneManager.sceneLoaded += NewGame;
    }
    private void NewGame(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= NewGame;
        Debug.Log(scene.name);
        SpawnStartTeam(SetTeam(meTeam), mePos);
        SpawnStartTeam(SetTeam(enemyTeam), enemyPos);
        //GameObject.FindObjectOfType<Light>().intensity++;
    }

    List<GameObject> SetTeam(string color)
    {
        switch (color)
        {
            case "Blue":
                return BlueTeam;
            case "Green":
                return GreenTeam;
            case "Red":
                return RedTeam;
            case "Yellow":
                return YellowTeam;
            default:
                Debug.Log("ошибка при выборе цвета");
                return null;
        }
    }
    void SpawnStartTeam(List<GameObject> team, int corner)
    {
        Instantiate(team[3], startpos[corner], team[3].transform.rotation);
        Vector3 pos = startpos[corner] + new Vector3(0, 0.75f, 5);
        Instantiate(team[0], pos, team[0].transform.rotation);
        pos = startpos[corner] + new Vector3(0, 0.75f, 10);
        Instantiate(team[0], pos, team[0].transform.rotation);
        pos = startpos[corner] + new Vector3(5, 1, 5);
        Instantiate(team[1], pos, team[1].transform.rotation);
    }
    int SetType(string typeName)
    {
        switch (typeName)
        {
            case "Tank":
                return 0;
            case "Crane":
                return 1;
            case "Turret":
                return 2;
            case "Factory":
                return 3;
            default:
                Debug.Log("ошибка при выборе цвета");
                return 55;
        }
    }
    public Health Spawn(string type, string color, Vector3 pos, Quaternion rot)
    {
        int t = SetType(type);
        List<GameObject> team = SetTeam(color);
        var obj = Instantiate(team[t], pos, rot);
        return obj.GetComponent<Health>();
    }
    IEnumerator RecoveryLinks()
    {
        yield return null;
        SavedHealth.RecoveryLinks();
        yield break;
    }
}
