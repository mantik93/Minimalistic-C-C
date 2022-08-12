using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private string me_color, enemy_color;
    private int me_corner, enemy_corner;
    public Button[] buttons = new Button[12];
    private Text[] b_texts = new Text[12];
    private void Start()
    {
        for (int i = 0; i < 12; i++)
        {
            b_texts[i] = buttons[i].GetComponentInChildren<Text>();
        }
    }
    public void SelectButton(int b)
    {
        if (b < 4)
        {
            for (int i = 0; i < 4; i++)
            {
                b_texts[i].text = "";
            }
            for (int i = 4; i < 8; i++)
            {
                buttons[i].interactable = true;
            }
            buttons[b + 4].interactable = false;
        }
        else if (b < 8)
        {
            for (int i = 4; i < 8; i++)
            {
                b_texts[i].text = "";
            }
        }
        else if (b < 12)
        {
            for (int i = 8; i < 12; i++)
            {
                b_texts[i].text = "";
            }
        }
        b_texts[b].text = "+";
    }
    public void SetMeColor(string c) 
    {
        me_color = c;
    }
    public void SetEnemyColor(string c)
    {
        enemy_color = c;
    }
    public void SetCorners(int c)
    {
        me_corner = c;
        enemy_corner = Random.Range(0, 4);
        if (enemy_corner == me_corner)
        {
            if (enemy_corner == 4) enemy_corner = 0;
            else enemy_corner++;
        }
    }
    public void StartGame()
    {
        if (me_color != null && enemy_color != null && me_color != enemy_color)
            GameManager.Instance.NewGame(me_color, enemy_color, me_corner, enemy_corner);
    }
}
