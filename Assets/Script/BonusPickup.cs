using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPickup : MonoBehaviour
{


      private RubyController rubyController; // this line of code creates a variable called "rubyController" to store information about the RubyController script!

    
    // Start is called before the first frame update
    void Start()
    {
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController"); //this line of code finds the RubyController script by looking for a "RubyController" tag on Ruby
          if (rubyControllerObject != null)

        {

            rubyController = rubyControllerObject.GetComponent<RubyController>(); //and this line of code finds the rubyController and then stores it in a variable

            print ("Found the RubyConroller Script for bonusscript!");

             //this line of code is increasing Ruby's score by 1!

        }

        if (rubyController == null)

        {

            print ("Cannot find GameController Script for bonusscript!");

        }
    }


   void OnCollisionEnter2D(Collision2D bonu)
    {
        RubyController controller = bonu.gameObject.GetComponent<RubyController >();

        if (controller != null)
        {
            controller.ChangeBonus(1);
            Destroy(gameObject);
        }
    }
    }

