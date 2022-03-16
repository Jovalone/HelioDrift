using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    //public Character[] Characters;
    //public List<GameObject> test;
    public List<Character> Characters;

    public Dictionary<string, Character> test;

    void Start()
    {
        Debug.Log(Characters.Count);
        int i = 0;

        foreach(Character character in Characters)
        {
            character.relationships = new int[Characters.Count];
        }

        foreach(Character character in Characters)
        {
            character.CharacterNum = i;

            int j = 0;
            foreach (Character Char in Characters)
            {
                if(i == j)
                {
                    character.relationships[j] = -1;
                }
                else
                {
                    if(character.relationships[j] == 0)
                    {
                        int x = Random.Range(0, 10);

                        character.relationships[j] = x;
                        Char.relationships[i] = x;
                    }
                }
                j++;
            }

            i++;
        }
    }

}
