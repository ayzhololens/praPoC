using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;

public class mediaPlayerWindowPopulator : Singleton<mediaPlayerWindowPopulator>
{

    public Text filename;
    public Text inspector;
    public Text date;
    public Text description;
    public GameObject simpleText;

    public void populateMediaPlayerWindow(JU_databaseMan.nodeItem node, JU_databaseMan.tempComment comment)
    {
        if (node.type == 3)
        {
            if (comment.type == 0)
            {
                simpleText.GetComponent<Text>().text = comment.content;
            }
            else
            {
                if (comment.type == 1)
                {
                    filename.text = node.photos[0].path;
                }
                else if (comment.type == 2)
                {
                    filename.text = node.videos[0].path;
                }
                inspector.text = comment.user;

                date.text = comment.date;

                description.text = comment.content;
            }

        }
        else
        {
            inspector.text = node.user;

            date.text = node.date;

            description.text = node.description;
        }

        if (node.type == 0)
        {
            simpleText.GetComponent<Text>().text = node.description;
            filename.text = "Simple Text";
        }
        else if (node.type == 1)
        {
            filename.text = node.photos[0].path;
        }
        else if (node.type == 4)
        {
            filename.text = node.videos[0].path;
        }

    }
}
