using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.Fields)]
public class Test
{
    public int i;
    public string s;
    public int id, target_id, max_hp, num_hp;
    public bool target_bool;
    public string team;//, name, target_name;
    public SavedV3andQ position, rotation; //target_position;
    public static int counter;
    private static List<Health> links = new List<Health>();
    //под номером id в списке хранится ссылка на объект
    private static Dictionary<int, Health> targets = new Dictionary<int, Health>();
    //цели ещё без назначенного id
    private static Dictionary<int, Test> all = new Dictionary<int, Test>();
    public Test() { }
    public Test(Health health)
    {
        i = 111;
        s = "тест успешен";

        id = counter;
        counter++;
        links.Add(health);
        target_bool = health.Target != null;
        if (target_bool)
        {
            target_id = links.IndexOf(health.Target);
            if (target_id < 0)
                targets.Add(id, health.Target);
        }
        int key = -1;
        bool target_set = false;
        foreach (var item in targets)
        {
            if (item.Value == health)
            {
                all[item.Key].target_id = id;
                key = item.Key;
                target_set = true;
            }
        }
        if (target_set) targets.Remove(key);
        max_hp = health.Max_hp;
        num_hp = health.Num_hp;
        team = health.team;
        position = new SavedV3andQ(health.transform.position);
        rotation = new SavedV3andQ(health.transform.rotation);
        all.Add(id, this);
        //this.SerializeTest();
    }

    public void SerializeTest()
    {
        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        Test test = JsonConvert.DeserializeObject<Test>(json);
        Debug.Log(test.i + " " + test.s);
    }
    public static void SerializeAllTest()
    {
        foreach (var item in all)
        {
            //item
        }
    }
}
