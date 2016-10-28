using System;

/* 
 * This class represents the ships that are in play during game play.
 * The attributes represent the health of the ships.
 */

public class ShipSet
{
    /*
     * Attribute List
     * These private variables represent the health of the ships.
     * If a ships health drops to zero the ship is sunk.
     */
    private short aircraftCarrierHealth;
    private short battleshipHealth;
    private short submarineHealth;
    private short destroyerHealth;
    private short patrolBoatHealth;

    public short ACHEALTH { get { return aircraftCarrierHealth; } set { aircraftCarrierHealth = value; } }
    public short BSHEALTH { get { return battleshipHealth; } set { battleshipHealth = value; } }
    public short SBHEALTH { get { return submarineHealth; } set { submarineHealth = value; } }
    public short DSHEALTH { get { return destroyerHealth; } set { destroyerHealth = value; } }
    public short PBHEALTH { get { return patrolBoatHealth; } set { patrolBoatHealth = value; } }

    private String error1 = "*** The ship is already sunk ***";
    //private String error2 = "*** Invalid Parameter ***";

    /*
     * Constructor
     * This constructor initializes the values of the attributes to accurately represent the starting
     * health of the ships.
     */
    public ShipSet()
    {
    }

    /*
     * Method updateHealth
     * Parameter - A char that respresents which ship has been hit.
     * This method simply decrements the health of a ship if it is hit.
     * This method does not return any value.
     * Any code that calls this method should catch ArgumentException.
     */
    public void updateHealth(char x)
    {
        switch (x)
        {
            case 'a':
                if (checkHealth('a') > 0) // Ensures that the ship is not already sunk.
                    aircraftCarrierHealth--;
                else throw new System.ArgumentException(error1);
                break;
            case 'b':
                if (checkHealth('b') > 0)
                    battleshipHealth--;
                else throw new System.ArgumentException(error1);
                break;
            case 's':
                if (checkHealth('s') > 0)
                    submarineHealth--;
                else throw new System.ArgumentException(error1);
                break;
            case 'd':
                if (checkHealth('d') > 0)
                    destroyerHealth--;
                else throw new System.ArgumentException(error1);
                break;
            case 'p':
                if (checkHealth('p') > 0)
                    patrolBoatHealth--;
                else throw new System.ArgumentException(error1);
                break;
            //removed default because 'o' 'x' is passed through here and it crashes game from that throw error thing
            //default: // If parameter is anything not expected
                //throw new System.ArgumentException(error2);
                //break;
        }
    }

    public int[] getallShipsHealth()
    {
        int[] shipsHealth = new int[5];

        shipsHealth[0] = aircraftCarrierHealth;
        shipsHealth[1] = battleshipHealth;
        shipsHealth[2] = submarineHealth;
        shipsHealth[3] = destroyerHealth;
        shipsHealth[4] = patrolBoatHealth;

        return shipsHealth;
    }

    /*
     * Parameter - A character that represents which ship's health is to be checked.
     * This method returns a short integer value that represents the indicated ship's current health.
     * Any code that calls this method should catch ArgumentException.
     */
    public int checkHealth(char x)
    {
        switch (x)
        {
            case 'a':
                return aircraftCarrierHealth;
            case 'b':
                return battleshipHealth;
            case 's':
                return submarineHealth;
            case 'd':
                return destroyerHealth;
            case 'p':
                return patrolBoatHealth;
            //default: // If parameter is anything not expected
                //throw new System.ArgumentException(error2);
        }
        return 20;
    }

    public int totalHealth() 
    {
        return (aircraftCarrierHealth + battleshipHealth + submarineHealth + destroyerHealth + patrolBoatHealth);
    }
}
