//Need to add a way to reset ships health when player starts new game

using System;

/*
 * An instance of this class should be created right after the player has finalized their
 * ship placements. This instance will exist for the entire duration of the game. 
 */
public class Game
{
    /* The user interface should call this constrctor by passing it the difficulty that
     * the user chose as well as the char array that holds information on the whereabouts
     * of the user's ships. This information will be used to create a Shipset instance
     * and a Board instance for the user as well as the opponent (AI).
     */

    private static int difficulty;
    public static int DIFFICULTY { get { return difficulty; } set { difficulty = value; } }

    //Crate AI instances
    AI Opponent = new AI();

    // Crate ShipSet instances
    ShipSet UserShipSet = new ShipSet();
    ShipSet AIShipSet = new ShipSet();

    //Crate Board instances
    Board UserBoard = new Board();
    Board AIBoard = new Board();

    //empty becasue theres no need for anything to happen here.
	public Game()
	{
    }

    public void setShipSet()
    {
        //sets AI's Ships Health
        AIShipSet.ACHEALTH = 5;
        AIShipSet.BSHEALTH = 4;
        AIShipSet.SBHEALTH = 3;
        AIShipSet.DSHEALTH = 3;
        AIShipSet.PBHEALTH = 2;

        //sets User Ships Health
        UserShipSet.ACHEALTH = 5;
        UserShipSet.BSHEALTH = 4;
        UserShipSet.SBHEALTH = 3;
        UserShipSet.DSHEALTH = 3;
        UserShipSet.PBHEALTH = 2;
    }

    //this is called from main class
    //sends UserBoard to AI class so it can use it for checks
    //sets x to spot were ai attack on userboard
    //also returns aitarget for main to show on interface
    public int aiAttack()
    {
        int aitarget;

        //Gives AI Users board
        Opponent.USERBOARD = UserBoard.BOARD;

        //give AI Users ships health
        Opponent.SHIPSHEALTH = UserShipSet.getallShipsHealth();

        //gets ai attack cord
        aitarget=Opponent.AI_fire();

        //updates users shiphealth and sets x to the spot the AI it hit
        UserShipSet.updateHealth(UserBoard.fire(aitarget));

        return aitarget;
    }

    //this is called from main class to set users attack
    //also returns what is hits (char type) 
    public char userAttack(int _userAttack)
    {
        //this will update AI's ships health 
        AIShipSet.updateHealth(AIBoard.BOARD[_userAttack]);

        //returns what the AI hit to interface and put x' on AI board
        return AIBoard.fire(_userAttack);
    }

    public int checkShipHealth(char shipType)
    {
        return AIShipSet.checkHealth(shipType);
    }

    //this is called from main class to set up the user and ai boards
    public void setBoards(int difficulty, char[] userBoard)
    {
        UserBoard.BOARD = userBoard;
        Opponent.BOARD_SIZE = difficulty;
        AIBoard.BOARD = Opponent.ship_generator();
    }

    public string hasWon()
    {
        if (UserShipSet.totalHealth() == 0)
            return "AI";
        if (AIShipSet.totalHealth() == 0)
            return "User";
        else 
            return "Nobody";
    }

}
