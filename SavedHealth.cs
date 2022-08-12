using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.Fields)]
public class SavedHealth
{
    public string type;
    public int id, target_id, max_hp, num_hp;
    public bool target_bool;
    public string team;//, name, target_name;
    public SavedV3andQ position, rotation; //target_position;
    public static int counter;
    private static List<Health> links = new List<Health>();
    //под номером id в списке хранится ссылка на объект
    private static Dictionary<int, Health> targets = new Dictionary<int, Health>();
    //цели ещё без назначенного id
    private static Dictionary<int, SavedHealth> all = new Dictionary<int, SavedHealth>();
    public SavedHealth(Health health)
    {
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
        if(target_set) targets.Remove(key);
        max_hp = health.Max_hp;
        num_hp = health.Num_hp;
        team = health.team;
        position = new SavedV3andQ(health.transform.position);
        rotation = new SavedV3andQ(health.transform.rotation);
        type = health.GetType().Name;
        all.Add(id, this);
    }
    public SavedHealth() { }
    public static void Serialize()
    {
        foreach (var item in all)
        {
            string json = JsonConvert.SerializeObject(item.Value, Formatting.Indented);
            string path = Application.persistentDataPath + $"/{item.Key}.json";
            File.WriteAllText(path, json);
            SavedHealth des = JsonConvert.DeserializeObject<SavedHealth>(json);
            Debug.Log(des.id + " " + des.team);
        }
        Debug.Log(Application.persistentDataPath);
        links.Clear();
        targets.Clear();
        all.Clear();
    }
    public static void Deserialize()
    {
        int i = 0;
        string path = Application.persistentDataPath + $"/{i}.json";
        while (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavedHealth loaded = JsonConvert.DeserializeObject<SavedHealth>(json);
            all.Add(i, loaded);
            Debug.Log(loaded.type + " " + loaded.GetType());
            path = Application.persistentDataPath + $"/{++i}.json";
        }
    }
    public static void Load()
    {
        Deserialize();
        for (int i = 0; i < all.Count; i++)
        {
            var health = GameManager.Instance.Spawn(all[i].type, all[i].team,
                all[i].position.ToVector3(), all[i].rotation.ToQuaternion());
            links.Add(health);
        }
    }
    public static void RecoveryLinks()
    {
        int t;
        for (int i = 0; i < all.Count; i++)
        {
            if (all[i].target_bool)
            {
                t = all[i].target_id;
                links[i].Action(links[t]);
            }
        }
    }
}
[JsonObject(MemberSerialization.Fields)]
public class SavedV3andQ
{
    private float x, y, z, w;
    public SavedV3andQ(Vector3 v3)
    {
        x = v3.x;
        y = v3.y;
        z = v3.z;
        w = 0;
    }
    public SavedV3andQ(Quaternion q)
    {
        x = q.x;
        y = q.y;
        z = q.z;
        w = q.w;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }
}
