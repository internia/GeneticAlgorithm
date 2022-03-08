using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    //a list of vectors holding all genes for our pops
    public List<Vector2> genes = new List<Vector2>();

    //constructor
    public DNA(int dnaLength = 50)
    {
        //for every DNA genome
        for(int i = 0; i < dnaLength; i++)
        {
            //add a random value to the list of genes
            genes.Add(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
        }
    }

    //pass through here to get child DNA
    public DNA(DNA p1, DNA p2, float mutation=0.1f)
    {
        //for every DNA genome
        for (int i = 0; i < p1.genes.Count; i++)
        {
            float mutationRate = Random.Range(0.0f, 1.0f);

            //if the mututation rate is lower than the actual mutations
            if (mutationRate < mutation)
            {
                //add a new random mutation
                genes.Add(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
            }
            else
            {   //take gene from either parent 1 or parent 2
                int chance = Random.Range(0, 2);
                if(chance == 0)
                {
                    genes.Add(p1.genes[i]);
                }
                else
                {
                    genes.Add(p2.genes[i]);
                }
            }
         }
    }
    
}
