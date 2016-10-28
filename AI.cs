using System;

public class AI
{
    private int board_size;
    public int BOARD_SIZE { get { return board_size; } set { board_size = value; } }

    private char[] ships;
    public char[] USERBOARD { get { return ships; } set { ships = value; } }

    private int[] shipsHealth;
    public int[] SHIPSHEALTH { get { return shipsHealth; } set { shipsHealth = value; } }

    static Random num_gen = new Random();

	public AI()
	{
	}

    public struct AIGlobals
    {
        public static int originalAttack;
        public static bool aiHit;
        public static int contiunedHit;
        public static bool continueDirection = false;
        public static int direction = num_gen.Next(0, 4);
        public static bool directionHit = false;
        public static bool hitCycle;
        public static int dirctionCount = 0;
        public static bool[] directionTracking = { false, false, false, false };
        public static int[] saveShipfound = new int[4];
        public static char[] saveTargetType = new char[4];
        public static int saveFoundCount = 0;
        public static char targetType;
    };

    public int AI_fire()
    {
        int health = 5;
        int fire_at = 0;
        bool mainLoop = false;
        bool continuedLoop = false;

        for (; mainLoop != true; )
        {
            if (AIGlobals.aiHit == true)//if ai finds a ship this is the searching for the rest or ship
            {
                for (; continuedLoop != true; )
                {
                    if (AIGlobals.continueDirection == false)//gets differnt direction if ai misses
                    {
                        AIGlobals.direction = getDirection();
                    }
                    switch (AIGlobals.direction)//this is to make the ai search the area around found ship cord
                    {
                        case 0://left
                            AIGlobals.directionTracking[0] = true; //this will keep ai from attacking a derection they already tryed
                            if ((AIGlobals.contiunedHit % BOARD_SIZE) == 0)// keeps ai attacking in boundary of board
                            {
                                if (AIGlobals.aiHit == true)
                                    changeDirection();
                                else
                                    getDirection();
                            }
                            else
                            {
                                fire_at = AIGlobals.contiunedHit - 1;
                                if (ships[fire_at] != 'x')
                                {
                                    AIGlobals.directionHit = true;
                                    continuedLoop = true;
                                    mainLoop = true;
                                }
                                else if (AIGlobals.directionHit == true)
                                {
                                    changeDirection();
                                }
                            }
                            break;
                        case 1://right
                            AIGlobals.directionTracking[1] = true;//this will keep ai from attacking a derection they already tryed
                            if (((AIGlobals.contiunedHit + 1) % BOARD_SIZE) == 0)// keeps ai attacking in boundary of board
                            {
                                if (AIGlobals.aiHit == true)
                                    changeDirection();
                                else
                                    getDirection();
                            }
                            else
                            {
                                fire_at = AIGlobals.contiunedHit + 1;
                                if (ships[fire_at] != 'x')
                                {
                                    AIGlobals.directionHit = true;
                                    continuedLoop = true;
                                    mainLoop = true;
                                }
                                else if (AIGlobals.directionHit == true)
                                {
                                    changeDirection();
                                }
                            }

                            break;
                        case 2://up
                            AIGlobals.directionTracking[2] = true;//this will keep ai from attacking a derection they already tryed
                            if (AIGlobals.contiunedHit < BOARD_SIZE)// keeps ai attacking in boundary of board
                            {
                                if (AIGlobals.aiHit == true)
                                    changeDirection();
                                else
                                    getDirection();
                            }
                            else
                            {
                                fire_at = AIGlobals.contiunedHit - BOARD_SIZE;
                                if (ships[fire_at] != 'x')
                                {
                                    AIGlobals.directionHit = true;
                                    continuedLoop = true;
                                    mainLoop = true;
                                }
                                else if (AIGlobals.directionHit == true)
                                {
                                    changeDirection();
                                }
                            }
                            break;
                        case 3://down
                            AIGlobals.directionTracking[3] = true;//this will keep ai from attacking a derection they already tryed
                            if (AIGlobals.contiunedHit + BOARD_SIZE >= (BOARD_SIZE * BOARD_SIZE))// keeps ai attacking in boundary of board
                            {
                                if (AIGlobals.aiHit == true)
                                    changeDirection();
                                else
                                    getDirection();
                            }
                            else
                            {
                                fire_at = AIGlobals.contiunedHit + BOARD_SIZE;
                                if (ships[fire_at] != 'x')
                                {
                                    AIGlobals.directionHit = true;
                                    continuedLoop = true;
                                    mainLoop = true;
                                }
                                else if (AIGlobals.directionHit == true)
                                {
                                    changeDirection();
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                fire_at = num_gen.Next(0, (BOARD_SIZE * BOARD_SIZE - 1));//first ai random cords from ai
                if (ships[fire_at] != 'x')//checks to see if the ai already hit the spot
                    mainLoop = true;
            }
        }

        if (ships[fire_at] != 'o')
        {
            if (AIGlobals.hitCycle != true)
            {
                AIGlobals.targetType = ships[fire_at];
                AIGlobals.originalAttack = fire_at;
            }
            if (AIGlobals.directionHit == true && ships[fire_at] == AIGlobals.targetType)
            {
                AIGlobals.continueDirection = true;
            }

            AIGlobals.hitCycle = true;
            AIGlobals.aiHit = true;

            if (ships[fire_at] != AIGlobals.targetType) //will make ai save 2nd ship found and keep attacking original
            {
                AIGlobals.saveShipfound[AIGlobals.saveFoundCount] = fire_at;
                AIGlobals.saveTargetType[AIGlobals.saveFoundCount] = ships[fire_at];
                ++AIGlobals.saveFoundCount;
                if (AIGlobals.continueDirection == true)
                {
                    changeDirection();
                }
                else
                {
                    AIGlobals.continueDirection = false;
                }
            }
            else
            {
                AIGlobals.contiunedHit = fire_at;

            }

            switch (ships[fire_at])//checks ships health
            {
                case 'a':
                    --shipsHealth[0];
                    Console.WriteLine("a Health " + shipsHealth[0]);
                    health = shipsHealth[0];
                    break;
                case 'b':
                    --shipsHealth[1];
                    Console.WriteLine("b Health " + shipsHealth[1]);
                    health = shipsHealth[1];
                    break;
                case 's':
                    --shipsHealth[2];
                    Console.WriteLine("s Health " + shipsHealth[2]);
                    health = shipsHealth[2];
                    break;
                case 'd':
                    --shipsHealth[3];
                    Console.WriteLine("d Health " + shipsHealth[3]);
                    health = shipsHealth[3];
                    break;
                case 'p':
                    --shipsHealth[4];
                    Console.WriteLine("p Health " + shipsHealth[4]);
                    health = shipsHealth[4];
                    break;
            }

            if (health == 0)
            {
                if (AIGlobals.saveFoundCount > 0)
                {
                    --AIGlobals.saveFoundCount;
                    AIGlobals.originalAttack = AIGlobals.saveShipfound[AIGlobals.saveFoundCount];
                    AIGlobals.contiunedHit = AIGlobals.saveShipfound[AIGlobals.saveFoundCount];
                    AIGlobals.targetType = AIGlobals.saveTargetType[AIGlobals.saveFoundCount];
                }
                else
                {
                    AIGlobals.hitCycle = false;
                    AIGlobals.aiHit = false;
                }
                AIGlobals.continueDirection = false;
                AIGlobals.directionHit = false;
                continuedLoop = true;
                setTracking();
            }
        }
        else
        {
            if (AIGlobals.aiHit == true)
            {
                changeDirection();
            }
        }

        return fire_at;
    }

    public void setTracking()
    {
        for (int loopnum = 0; loopnum != AIGlobals.directionTracking.Length; ++loopnum)
            AIGlobals.directionTracking[loopnum] = false;
    }

    public int getDirection()
    {
        int direction = num_gen.Next(0, 4);

        for (; AIGlobals.directionTracking[direction] != false; )
        {
            direction = num_gen.Next(0, 4);
        }

        return direction;
    }

    public void changeDirection()
    {
        switch (AIGlobals.direction)// this will switch direction for AIs next attack
        {
            case 0://left
                AIGlobals.direction = 1;
                break;
            case 1://right
                AIGlobals.direction = 0;
                break;
            case 2://up
                AIGlobals.direction = 3;
                break;
            case 3://down
                AIGlobals.direction = 2;
                break;
        }
        AIGlobals.contiunedHit = AIGlobals.originalAttack;
    }

    //Ship Generator bellow this point
    public char[] ship_generator()
    {
        char[] board = new char[100];
        int startShip = 0;
        int[] shipsSize = { 5, 4, 3, 3, 2 };
        char[] shipType = { 'a', 'b', 'd', 's', 'p' };
        int direction = 0;
        int size = 5;
        bool checkOverLap = false;

        for (int loopnum = 0; loopnum != board.Length; ++loopnum)
            board[loopnum] = 'o';

        for (int loopnum = 0; loopnum != shipsSize.Length; ++loopnum)
        {
            direction = num_gen.Next(0, 2);
            size = shipsSize[loopnum];
            startShip = num_gen.Next(0, 99);
            startShip = keepInBoard(startShip, direction, size);

            for (; ; )
            {
                checkOverLap = noOverLapping(startShip, direction, size, board);
                if (checkOverLap == false)
                {
                    startShip = num_gen.Next(0, 99);
                    startShip = keepInBoard(startShip, direction, size);
                    checkOverLap = false;
                }
                else
                    break;
            }

            for (int loopnum2 = 0; loopnum2 != size; ++loopnum2)
            {
                if (direction == 0)//horizontal
                {
                    board[startShip + loopnum2] = shipType[loopnum];
                }
                else//vertical
                {
                    board[startShip + (10 * loopnum2)] = shipType[loopnum];
                }
            }
        }
        return board;
    }

    public bool noOverLapping(int startShip, int direction, int size, char[] board)
    {
        for (int loopnum = 0; loopnum != size; ++loopnum)
        {
            switch (direction)
            {
                case 0://horizontal
                    if (board[startShip + loopnum] != 'o')
                        return false;
                    break;
                case 1://vertical
                    if (board[startShip + (10 * loopnum)] != 'o')
                        return false;
                    break;
            }
        }
        return true;
    }

    public int keepInBoard(int startShip, int direction, int size)
    {
        switch (direction)
        {
            case 0://horizontal
                if ((((startShip + size + 1) % 10) > 1) && (((startShip + size + 1) % 10) <= 5))
                {
                    startShip = startShip - ((startShip + size) % 10);
                    if (startShip < 0)//im not 100% sure why startShip goes into negative some times
                    {
                        startShip = startShip * -1;
                    }
                }
                break;
            case 1://vertical
                if (((startShip + 1) + (10 * size) > 110))
                {
                    startShip = startShip - (((startShip - (10 * size)) / 10) * 10);
                }
                break;
        }
        return startShip;
    }
}
