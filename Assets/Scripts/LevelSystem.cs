using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    [SerializeField]
    private int level = 1;

    public int levelExp = 0;

    public int levelExpMax => (level+1)*10;

    public Text levelText;
    public Text expText;
    public ProgressBar progressBar;

    // Start is called before the first frame update
    void Start()
    {
        expText.text = $"{levelExp}/{levelExpMax}";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (progressBar != null)
        {
            progressBar.current = levelExp;
            progressBar.maximum = levelExpMax;
        }
        if(levelExp >= levelExpMax)
        {
            levelExp = levelExp > levelExpMax ? levelExp-levelExpMax : 0;
            level += 1;
            levelText.text = $"Level: {level}";
            expText.text = $"{levelExp}/{levelExpMax}";
        }
    }
}
