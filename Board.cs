using System;

/* This class is the representation of the physical board. A character array is used, each element
 * representing a coordinate of the board. Certain characters will represent certain conditions of 
 * the coordinate.
 * 'a'  Part of an Aircraft carrier is located on this position.
 * 'b'  Part of a Battleship is located on this position.
 * 's'  Part of a Submarine is located on this position.
 * 'd'  Part of a Destroyer is located on this position.
 * 'p'  Part of a Patrol boat is located on this position.
 * 'o'  Open water is located at this position (no ships).
 * 'x'  This position has already been fired upon.
 */
public class Board
{
    /* Attribute list
     * "board" is the char array that represents the physical game board.
     * The String attributes are returned to inform the user of what was in the position 
     * they fired at.
     */
    private char[] board;
    public char[] BOARD { get { return board; } set { board = value; } }


    /* Constructor
     * Initializes the char array to the correct size and fills it with the fillBoard method
     */
	public Board()
	{        
	}

    /* Parameter - This method takes an int value that corresponds to the element in the character
     * array that is being fired upon.
     * This method modifieds the elements of the array to show that the position has been fired at
     * and returns a string that tells the user if they hit a ship or not.
     */
    public char fire(int k) 
    {
        switch (board[k]) 
        {
            case 'a': // Aircraft carrier here
                board[k] = 'x';
                return 'a';
            case 'b': // Battleship here
                board[k] = 'x';
                return 'b';
            case 's': // Submarine here
                board[k] = 'x';
                return 's';
            case 'd': // Destroyer here
                board[k] = 'x';
                return 'd';
            case 'p': // Patrol boat here
                board[k] = 'x';
                return 'p';
            case 'o': // Open water here
                board[k] = 'x';
                return 'o';
            case 'x': // The position has already been fired at
                return 'x';
            default: // If parameter is anything not expected
                throw new System.ArgumentException("Invalid Parameter");
        }
    }
}
