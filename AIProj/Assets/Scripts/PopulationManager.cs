using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour
{
    //create a list to hold the entire population
    List<Brain> population = new List<Brain>();
  
    //Holder for the sprite to represent our population member
    public GameObject popPrefab;

    //define the size of the total population
    public int popSize = 100;

    public int dnaLength;

    //reference to brain class
    Brain brain;

    //defines our start generation and a UI text class to display it
    public int generation = 1;
    public Text generationDisplay;

    //defines the amount thats reached the goal and a UI text class to display it
    public int goalSuccess = 0;
    public Text goalDisplay;
    public bool goalPop;

    //define placeholders for our spawn and goal areas
    public Transform spawn;
    public Transform goal;


    //how many of the pop shall be survive for next gen
    public float popSurvive = 0.3f;
    public int survivalRate = 5;

    //on start, run initialisation and create our first gen of pops
    void Start()
    {
        Init();
 
    }
    void Init()
    {
        for(int i = 0; i < popSize; i++)
        {

            //spawning our pop gameobjects using the pop prefab at the predetermined spawnpoint
            GameObject pop = Instantiate(popPrefab, spawn.position, Quaternion.identity);
            //init our pop with DNA length and goal to reach
            pop.GetComponent<Brain>().Init(new DNA(dnaLength), goal.position);
            //add init pop to the population list
            population.Add(pop.GetComponent<Brain>());
        }
    }

    void Breed()
    {
        //select members from previous pop to breed
        int selectPop = Mathf.RoundToInt(popSize * popSurvive);
        //create list of new pop
        List<Brain> newPop = new List<Brain>();
        for (int i = 0; i < selectPop; i++)
        {
            //add the fittest pop from previous gen to the new pop
            newPop.Add(fittestPop());
        }
        for (int i = 0; i < population.Count; i++)
        {
            Destroy(population[i].gameObject);
        }

        population.Clear();

        //keeps improvement by preserving the top fittness into the next generation
        for (int i = 0; i < survivalRate; i++)
        {
            GameObject pop = Instantiate(popPrefab, spawn.position, Quaternion.identity);
            pop.GetComponent<Brain>().Init(newPop[i].dna, goal.position);
            population.Add(pop.GetComponent<Brain>());
        }
        //when the population list count is lower than the predefined pop size
        while (population.Count < popSize)
        {
            for (int i = 0; i < newPop.Count; i++)
            {
                //init new pop for scene
                GameObject pop = Instantiate(popPrefab, spawn.position, Quaternion.identity);
                //selects random dna from the top 10 fittest pop
                pop.GetComponent<Brain>().Init(new DNA(newPop[i].dna, newPop[Random.Range(0, 10)].dna), goal.position);

                //add new gen to pop list
                population.Add(pop.GetComponent<Brain>());

                //breaks from the loop when requirement is met
                if (population.Count >= popSize)
                {
                    break;
                }
            }
        }
        //destroy the pop game objet at the end of its cycle to make room for new one 
        for (int i = 0; i < newPop.Count; i++)
        {
            Destroy(newPop[i].gameObject);
        }
        //increase generation counter and display accordingly on canvas
        generation++;
        generationDisplay.text = "Generation:" + generation.ToString();

        goalSuccess = 0;
        
    
    }
 
    void statCounter()
    {
        goalSuccess = goalSuccess + 1;
        goalDisplay.text = "Goal reached:" + goalSuccess.ToString();
    }

    //if no pops are alive, then run the function to breed a new generation
    private void Update()
    {
        if (!hasAlive())
        {
            Breed();
            Debug.Log("NEW GEN" + generation);
        }

        if (hasGoal())
        {
            statCounter();
        }
    }
    Brain fittestPop()
    {
        float maxFitness = float.MinValue;
        int index = 0;
        for(int i = 0; i < population.Count; i++)
        {
            //check to see if max fitness has increased within the pop 
            if(population[i].fitness > maxFitness)
            {
                //if so, set new max fitness
                maxFitness = population[i].fitness;
                index = i;
            }
        }
        Debug.Log(maxFitness);
        Brain fittest = population[index];
        //remove to prevent fitness stagnation 
        population.Remove(fittest);
        return fittest;
    }
    

    //checks if any of the current pop are alive
    bool hasAlive()
    {
        //loop through entire current pop to check if they are alive or dead
        for (int i = 0; i < population.Count; i++)
        {
            if (!population[i].dead)
            {
                return true;
            }
        }

        return false;
    }

    //checks if the goal has been reacjhed
    bool hasGoal()
    {
        for (int i = 0; i < population.Count; i++)
        {
            if (population[i].goalReached)
            {
                return true;
               // goalSuccess++;
            }

        }

        return false;
    }
}
