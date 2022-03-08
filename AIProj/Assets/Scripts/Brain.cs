using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brain : MonoBehaviour
{
    //vars to denote traits of the pops
    public float speed;
    public int path;

    //public reference to dna
    public DNA dna;

    //bools to handle initialisation and measuring fitness of pops
    bool init = false;
    public bool dead = false;
    bool mazeCollision = false;
    public bool goalReached = false;

    //vectors to be assigned in the inspector which allow pops to know where to spawn and where to head as their goal
    Vector2 goal;
    Vector2 nextPoint;

    //Initialise our traits and goals
    public void Init(DNA newDNA, Vector2 _goal)
    {
        dna = newDNA;
        goal = _goal;
        nextPoint = transform.position;
        init = true;
    }


    //This deals with the state of the population member and what to do next based upon that state
    private void Update()
    {
        //check that the pop member has been initialised AND is not dead in the scene to save unneccesary processing
        if (init && !dead)
        {
            //if the pathing trait is equal to the gene count(no more capacity to progress this gen) OR if close enough to goal, indivdual pop may die
            if(path == dna.genes.Count || Vector2.Distance(transform.position, goal) < 0.5f)
            {
                dead = true;
            }
            //if the pops position has reached their defined next point position
            if((Vector2)transform.position == nextPoint)
            {
                //then reset the next point to a new position in the 2d space with the addition of the path trait 
                nextPoint = (Vector2)transform.position + dna.genes[path];
                //add another point to the path trait as they have reached their goal
                path++;
            }
            else
            {
                //if they are not dead, not reached their target next point, or not reached the goal then keep the pop moving towards their nextpoint goal
                transform.position = Vector2.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);
            }
        }

    }

    //Defines the fitness function for each population member & returns it
    public float fitness
    {
        get
        {
            //get distance between the pop game obj and the goal
            float dist = Vector2.Distance(transform.position, goal);
            //prevent errors by setting a small offset if reached goal
            if(dist == 0)
            {
                dist = 0.0001f;
            }
            //fitness calculated by denoting that the close to the goal, the higher the fitness THEN MULTIPLIED by the maze collision fitness factor, if hit wall - * by 0.5, if avoided wall * by 1
            return (60/dist)*(mazeCollision ? 0.5f : 1f);
        }
    }

    //checks if pops have collided with the maze and adjusts their qualities accordingly
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if the player has collided with the maze
        if(collision.gameObject.tag == "Maze")
        {
            dead = true;
            mazeCollision = true;
        }

        //checks if the pop member has reached the goal
        if (collision.gameObject.tag == "Goal")
        {
            goalReached = true;
         }
    }

   
   
}

