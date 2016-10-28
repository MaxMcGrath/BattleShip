using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Media;

namespace WindowsFormsApplication1
{
    public partial class battleShipGame : Form
    {
        //start form move stuff
        //I did this because I did not want the standard form border.
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void move_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        //end form move stuff

        //Makes game object
        Game makeGame = new Game();

        //create theme object
        standardtheme standard = new standardtheme();

        //create theme object
        piratetheme pirate = new piratetheme();

        //creates new game form object
        newgame newGameSetUp = new newgame();

        //create timers
        Timer aiTimer = new Timer();
        Timer buttonTimer = new Timer();
        Timer soundTimer = new Timer();
        Timer exceptionTimer = new Timer();

        //names for grids picture box's
        public const char ATKBX = 'k';
        public const char SHPBX = 'h';

        //peg names
        public const char PEG = 'g';
        public const char PEGX = 'm';

        //ship types
        public const char AIRCRAFTCARRIER = 'a';
        public const char BATTLESHIP = 'b';
        public const char DESTROYER = 'd';
        public const char SUBMARINE = 's';
        public const char PATROLBOAT = 'p';

        //Open water
        public const char OPENWATER = 'o';

        //players
        public string AI = "AI";
        public string USER = "User";
        public string NOBODY = "Nobody";

        //ship Size
        public const int ACSIZE = 5;
        public const int BSSIZE = 4;
        public const int DSSIZE = 3;
        public const int SBSIZE = 3;
        public const int PBSIZE = 2;

        public const string STRINGNULL = "";
        
        public const string FILETYPE = ".bmp";

        //for building file image name
        public readonly string[] FILENAME = {
                "grid_", //0
                "_0",    //1
                "0",     //2
                "_",     //3
                "",      //4
                FILETYPE   //5
                };

        //image name for easy access
        public const string HIT = "grid_hit" + FILETYPE;
        public const string MISS = "grid_miss" + FILETYPE;
        public const string FAILED = "failed" + FILETYPE;
        public const string VICTORY = "victory" + FILETYPE;
        public const string ACIMAGE = "table_AC_01" + FILETYPE;
        public const string BSIMAGE = "table_BS_01" + FILETYPE;
        public const string DSIMAGE = "table_DS_01" + FILETYPE;
        public const string SBIMAGE = "table_SB_01" + FILETYPE;
        public const string PBIMAGE = "table_PB_01" + FILETYPE;
        public const string SHIPBOX = "grid_shp" + FILETYPE;
        public const string PEGXIMAGE = "pegx" + FILETYPE;
        public const string ATTACKBOX = "grid_atk" + FILETYPE;
        public const string SHIPBOXATK = "grid_shp_atk" + FILETYPE;
        public const string BACKGROUND = "background" + FILETYPE;
        public const string ATTACKBOXATK = "grid_atk_peg" + FILETYPE;
        public const string ROTATEBUTTONON = "rotatebackground02" + FILETYPE;
        public const string ROTATEBUTTONOFF = "rotatebackground01" + FILETYPE;
        public const string PANELBACKGROUND = "panalbackground" + FILETYPE;
        public const string NEXTSTAGEBUTTON = "nextbutton01" + FILETYPE;
        public const string REMOVEPEGBUTTON = "removepegbutton01" + FILETYPE;

        //sound names for easy access
        public const string SPLOOSH = "sploosh.wav";
        public const string WINSOUND = "win.wav";
        public const string GAMEOVER = "gameover.wav";
        public const string ALERTSOUND = "alert.wav";
        public const string CANNONSOUND = "cannon.wav";
        public const string NEWGAMESOUND = "gametime.wav";
        public const string SHIPPLACEMENT = "shipplacement.wav";
        public const string THEME = "theme.wav";

        //needed for theme song to play
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e){}

        //for grid setup
        public const int NORMAL = 10;
        public const int HARD = 9;
        public const int BRUTAL = 8;
        public const int PICBOXSIZE = 30;

        //I made these globals because I was not able to pass some variables.
        public static class Globals
        {
            public static int difficulty = NORMAL;
            public static int boardSize = NORMAL * NORMAL;
            public static int AttackSpot;
            public static bool makeHit = false;
            public static bool gameMode = false;
            public static bool pegPlaced = false;
            public static bool buttonTimer = false;
            public static string imgPath;
            public static string soundPath;
            public static string tagValue = STRINGNULL;
            public static string SHPPlace = STRINGNULL;
            public static bool[] rotateButton = new bool[5];
            public static char[] playerBoard = new char[100];
            public static string[] stringTheme = new string[15];
            public static string[] SHPBox = new string[2];
            public static string[] SHPType = new string[2];
        }

        public void startexceptionTimer()
        {
            exceptionTimer.Interval = 60000 * 30; //60000= 1min * it by 30 to get 30 mins
            exceptionTimer.Start();
        }

        public void resetexceptionTime()
        {
            exceptionTimer.Stop();
            exceptionTimer.Start();
        }

        public void exceptionAction(object sender, EventArgs e)
        {
            this.Close();
        }

        public battleShipGame()
        {
            InitializeComponent();

            aiTimer.Tick += new EventHandler(AIAttackDelay);
            buttonTimer.Tick += new EventHandler(imageDelay);
            soundTimer.Tick += new EventHandler(soundDelay);
            exceptionTimer.Tick += new EventHandler(exceptionAction);

            startexceptionTimer();// starts 30 min exception thing

            themeLoad();
            startTheme();
            game_setUp();
        }

        //plays theme song
        public void startTheme()
        {
            axWindowsMediaPlayer1.settings.setMode("loop", true);
            axWindowsMediaPlayer1.URL = Globals.soundPath + THEME;
        }

        private void themeLoad()//still not finished
        {
            string[] themeInfo = new string[2];
            string fileName = "";

            switch (0)
            {
                //Standard theme
                case 0:
                    //get theme info
                    themeInfo = standard.Theme;

                    Globals.stringTheme = standard.String;
                    Globals.imgPath = Application.StartupPath + themeInfo[0];
                    Globals.soundPath = Application.StartupPath + themeInfo[1];
                    break;

                //pirate theme **not included 
                //case 1:
                //    //get theme info
                //    themeInfo = pirate.Theme;

                //    Globals.stringTheme = pirate.String;
                //    Globals.imgPath = Application.StartupPath + themeInfo[0];
                //    Globals.soundPath = Application.StartupPath + themeInfo[1];
                //    break;
            }

            //go throught Ship panel to change each image part
            foreach (PictureBox pb in playerSHPPanel.Controls)
            {
                Globals.SHPType=breakUpName((pb.Name));

                fileName = (FILENAME[0] + Globals.SHPType[0] + FILENAME[3] + ((Convert.ToInt32(Globals.SHPType[1]) < 10) ? (FILENAME[2] + Globals.SHPType[1].ToString()) : Globals.SHPType[1].ToString()) + FILENAME[5]);

                switch (Convert.ToChar(Globals.SHPType[0]))
                {
                    case AIRCRAFTCARRIER:
                        pb.Image = Image.FromFile(Globals.imgPath + fileName);
                        break;
                    case BATTLESHIP:
                        pb.Image = Image.FromFile(Globals.imgPath + fileName);
                        break;
                    case DESTROYER:
                        pb.Image = Image.FromFile(Globals.imgPath + fileName);
                        break;
                    case SUBMARINE:
                        pb.Image = Image.FromFile(Globals.imgPath + fileName);
                        break;
                    case PATROLBOAT:
                        pb.Image = Image.FromFile(Globals.imgPath + fileName);
                        break;
                    case ATKBX:
                        pb.Image = Image.FromFile(Globals.imgPath + ATTACKBOX);
                        break;
                    case SHPBX:
                        pb.Image = Image.FromFile(Globals.imgPath + SHIPBOX);
                        break;
                }
            }

            //go throught Attack panel to change each image part
            foreach (PictureBox pb in playerATKPanel.Controls)
            {
                Globals.SHPType = breakUpName(pb.Name);

                fileName = (FILENAME[0] + Globals.SHPType[0].ToString() + FILENAME[3] + ((Convert.ToInt32(Globals.SHPType[1]) < 10) ? (FILENAME[2] + Globals.SHPType[1].ToString()) : Globals.SHPType[1].ToString()) + FILENAME[5]);

                switch (Convert.ToChar(Globals.SHPType[0]))
                {
                    case AIRCRAFTCARRIER:
                        pb.Image = Image.FromFile(Globals.imgPath + fileName);
                        break;
                    case BATTLESHIP:
                        pb.Image = Image.FromFile(Globals.imgPath + fileName);
                        break;
                    case DESTROYER:
                        pb.Image = Image.FromFile(Globals.imgPath + fileName);
                        break;
                    case SUBMARINE:
                        pb.Image = Image.FromFile(Globals.imgPath + fileName);
                        break;
                    case PATROLBOAT:
                        pb.Image = Image.FromFile(Globals.imgPath + fileName);
                        break;
                    case ATKBX:
                        pb.Image = Image.FromFile(Globals.imgPath + ATTACKBOX);
                        break;
                    case SHPBX:
                        pb.Image = Image.FromFile(Globals.imgPath + SHIPBOX);
                        break;
                }
            }

            nextStepButton.Text = Globals.stringTheme[14];
            undoPegPlaceButton.Text = Globals.stringTheme[13];
            g.Image = Image.FromFile(Globals.imgPath + PEGXIMAGE);
            a.Image = Image.FromFile(Globals.imgPath + ACIMAGE);
            b.Image = Image.FromFile(Globals.imgPath + BSIMAGE);
            d.Image = Image.FromFile(Globals.imgPath + DSIMAGE);
            s.Image = Image.FromFile(Globals.imgPath + SBIMAGE);
            p.Image = Image.FromFile(Globals.imgPath + PBIMAGE);
            this.BackgroundImage = Image.FromFile(Globals.imgPath + BACKGROUND);
            rotateACButton.Image = (Globals.rotateButton[0] == false) ? Image.FromFile(Globals.imgPath + ROTATEBUTTONOFF) : Image.FromFile(Globals.imgPath + ROTATEBUTTONON);
            rotateBSButton.Image = (Globals.rotateButton[1] == false) ? Image.FromFile(Globals.imgPath + ROTATEBUTTONOFF) : Image.FromFile(Globals.imgPath + ROTATEBUTTONON);
            rotateDSButton.Image = (Globals.rotateButton[2] == false) ? Image.FromFile(Globals.imgPath + ROTATEBUTTONOFF) : Image.FromFile(Globals.imgPath + ROTATEBUTTONON);
            rotateSBButton.Image = (Globals.rotateButton[3] == false) ? Image.FromFile(Globals.imgPath + ROTATEBUTTONOFF) : Image.FromFile(Globals.imgPath + ROTATEBUTTONON);
            rotatePBButton.Image = (Globals.rotateButton[4] == false) ? Image.FromFile(Globals.imgPath + ROTATEBUTTONOFF) : Image.FromFile(Globals.imgPath + ROTATEBUTTONON);
        }

        //start a new game
        private void newGameButton_Click(object sender, EventArgs e)
        {
            //opens newgame forum to set difficulty
            newGameSetUp.ShowDialog();

            resetexceptionTime(); //resets 30 min exception thing

            if (newGameSetUp.difficulty != -1)
            {
                //sets difficulty
                Globals.difficulty = newGameSetUp.difficulty;

                game_setUp();
                themeLoad();
            }
        }

        //sets up boards
        private void game_setUp()
        {
            int Xcord = 0;
            int Ycord = 0;
            char boxName;
            string imageFileName;

            //stops timer if some one starts new game with it runing
            buttonTimer.Stop();
            nextStepButton.BackgroundImage = null;

            //sets BackgroundImages to empty
            playerSHPPanel.BackgroundImage = null;
            playerATKPanel.BackgroundImage = null;
            undoPegPlaceButton.BackgroundImage = null;

            //Sets the ships health
            makeGame.setShipSet();

            //sets playerBoard to empty
            for (int loopnum = 0; loopnum != Globals.playerBoard.Length; ++loopnum)
                Globals.playerBoard[loopnum] = OPENWATER;

            //resets info for new game
            Globals.gameMode = false;
            Globals.pegPlaced = false;
            Globals.AttackSpot = -1;

            //destroys all PBs in panels if there is any, so new grid can be made
            destroy_Grid();

            //sets all rotation buttons to false
            for (int loopnum = 0; loopnum != Globals.rotateButton.Length; ++loopnum)
                Globals.rotateButton[loopnum] = false;

            //sets action label to say place ships
            playerActionLabel.Text = Globals.stringTheme[0];

            //make board for ATKPANEL
            boxName = ATKBX;
            imageFileName = ATTACKBOX;
            Globals.boardSize = NORMAL * NORMAL;
            createPBox(Xcord, Ycord, boxName, imageFileName);

            //SHPPANEL's boards size based on difficulty
            switch(Globals.difficulty)
            {
	            case NORMAL:
        	        Ycord = 0;
        	        Xcord = 0;
        	        Globals.boardSize = NORMAL * NORMAL;
		            break;

	            case HARD:
		            Xcord = PICBOXSIZE / 2;
		            Ycord = PICBOXSIZE / 2;
		            Globals.boardSize = HARD * HARD;
		            break;

	            case BRUTAL:
        	        Ycord = PICBOXSIZE;
		            Xcord = PICBOXSIZE;
 		            Globals.boardSize = BRUTAL * BRUTAL;
		            break;
            }

            //make board for SHPPANEL
            boxName = SHPBX;
            imageFileName = SHIPBOX;
            createPBox(Xcord, Ycord, boxName, imageFileName);
        }

        //creats picturebox for both boards
        public void createPBox(int Xcord, int Ycord, char boxName, string imageFileName)
        {
            for (int loopnum = 0; loopnum != Globals.boardSize; ++loopnum)
            {
                //sets up picturebox info
                PictureBox pBox = new PictureBox()
                {
                    AllowDrop = true,
                    Size = new Size(PICBOXSIZE, PICBOXSIZE),
                    Location = new Point(Xcord, Ycord),
                    Name = boxName + loopnum.ToString(),
                    Tag = boxName + loopnum.ToString(),
                    Image = Image.FromFile(Globals.imgPath + imageFileName)
                };

                //these set up the events to allow drag and drop
                pBox.DragDrop += new DragEventHandler(Label_DragDrop);
                pBox.DragEnter += new DragEventHandler(Label_DragEnter);
                pBox.MouseDown += new MouseEventHandler(Label_MouseDown);

                Xcord += PICBOXSIZE;

                switch (boxName)
                {
                    //for attack panel 
                    case ATKBX:
                        playerATKPanel.Controls.Add(pBox);
                        if ((loopnum + 1) % NORMAL == 0)
                        {
                            Ycord += PICBOXSIZE;
                            Xcord = 0;
                        }
                        break;

                    //for ship panel
                    case SHPBX:
                        playerSHPPanel.Controls.Add(pBox);
                        if ((loopnum + 1) % NORMAL == 0 && Globals.difficulty == NORMAL)
                        {
                            Ycord += PICBOXSIZE;
                            Xcord = 0;
                        }
                        if ((loopnum + 1) % HARD == 0 && Globals.difficulty == HARD)
                        {
                            Ycord += PICBOXSIZE;
                            Xcord = PICBOXSIZE / 2;
                        }
                        if ((loopnum + 1) % BRUTAL == 0 && Globals.difficulty == BRUTAL)
                        {
                            Ycord += PICBOXSIZE;
                            Xcord = PICBOXSIZE;
                        }
                        break;
                }
            }
        }

        //destroys picturebox so new game can be started, also used to show who won images
        public void destroy_Grid()
        {
            if (makeGame.hasWon() != USER)
                for (int loopnum = playerATKPanel.Controls.Count - 1; loopnum >= 0; loopnum--)
                    if (playerATKPanel.Controls[loopnum] is PictureBox)
                        playerATKPanel.Controls[loopnum].Dispose();
            if (makeGame.hasWon() != AI)
                for (int loopnum = playerSHPPanel.Controls.Count - 1; loopnum >= 0; loopnum--)
                    if (playerSHPPanel.Controls[loopnum] is PictureBox)
                        playerSHPPanel.Controls[loopnum].Dispose();
        }

        //Start drop and drag stuff
        //this is to get info on what is going to be droped
        private void Label_MouseDown(object sender, MouseEventArgs e)
        {
            //needed for drag stuff
            PictureBox lblPic = (PictureBox)sender;

            //this gets the ship type that is going to be droped
            if(Globals.pegPlaced==false)
                Globals.tagValue = ((PictureBox)sender).Name;

            //needed for drag stuff
            lblPic.DoDragDrop(lblPic.Image, DragDropEffects.Copy);

            resetexceptionTime(); //resets 30 min exception thing
        }

        //Placing ships on PlayerShipPanel
        private void Label_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Bitmap)))
            {
                //needed for peg drop
                e.Effect = DragDropEffects.Copy;

                //this gets the info for where the ship is being droped
                if (Globals.pegPlaced == false)
                    Globals.SHPPlace = ((PictureBox)sender).Name;

                //sets ships onto board
                if (Globals.gameMode == false)
                {
                    string fileName = STRINGNULL;
                    string[] pbSHPBox = new string[2];

                    Globals.SHPBox = breakUpName(Globals.SHPPlace);
                    Globals.SHPType = breakUpName(Globals.tagValue);

                    int shipSize = getShipSize(Convert.ToChar(Globals.SHPType[0]));

                    bool direction = (Convert.ToChar(Globals.SHPType[0]) == AIRCRAFTCARRIER) ? ((Globals.rotateButton[0] == true) ? true : false) :
                                     (Convert.ToChar(Globals.SHPType[0]) == BATTLESHIP) ? ((Globals.rotateButton[1] == true) ? true : false) :
                                     (Convert.ToChar(Globals.SHPType[0]) == DESTROYER) ? ((Globals.rotateButton[2] == true) ? true : false) :
                                     (Convert.ToChar(Globals.SHPType[0]) == SUBMARINE) ? ((Globals.rotateButton[3] == true) ? true : false) :
                                     ((Globals.rotateButton[4] == true) ? true : false);

                    //returns picturebox to original blank box
                    if (Globals.SHPBox[0] != Globals.SHPType[0] && Convert.ToChar(Globals.SHPBox[0]) == SHPBX)
                        foreach (PictureBox pb in playerSHPPanel.Controls)
                        {
                            pbSHPBox = breakUpName(pb.Name);

                            if (pbSHPBox[0] == Globals.SHPType[0])
                            {
                                pb.Name = pb.Tag.ToString();
                                pb.Image = Image.FromFile(Globals.imgPath + SHIPBOX);
                            }
                        }

                    //nudges the ship if needed.
                    Globals.SHPBox = nudgeSystem(shipSize, Globals.SHPBox, Globals.SHPType, direction);

                    //assigns ship to board
                    if (Convert.ToChar(Globals.SHPBox[0]) == SHPBX && checkShipLenght(shipSize, Globals.SHPBox, direction) == 0)
                        try
                        {
                            for (int loopnum = 0; loopnum != shipSize; ++loopnum)
                            {
                                fileName = (direction == false) ? (FILENAME[0] + Convert.ToChar(Globals.SHPType[0]) + FILENAME[1] + loopnum + FILENAME[5]) : (FILENAME[0] + Convert.ToChar(Globals.SHPType[0]) + FILENAME[3] + (((loopnum + shipSize) < 10) ? FILENAME[2] : FILENAME[4]) + (loopnum + shipSize) + FILENAME[5]);
                                ((PictureBox)playerSHPPanel.Controls[Globals.SHPBox[0] + (Convert.ToInt32(Globals.SHPBox[1]) + ((direction == false) ? (loopnum * Globals.difficulty) : (loopnum))).ToString()]).Image = Image.FromFile(Globals.imgPath + fileName);
                                ((PictureBox)playerSHPPanel.Controls[Globals.SHPBox[0] + (Convert.ToInt32(Globals.SHPBox[1]) + ((direction == false) ? (loopnum * Globals.difficulty) : (loopnum))).ToString()]).Name = Globals.SHPType[0] + ((direction == false) ? loopnum : (loopnum + shipSize));
                            }
                        }
                        catch
                        {
                            //do nothing *meaning not let game crash :P
                        }
                }
            }
            else
                e.Effect = DragDropEffects.None;
        }

        //This is for peg drop, and plays shipplacement sound
        private void Label_DragDrop(object sender, DragEventArgs e)
        {
            Globals.SHPBox = breakUpName(Globals.SHPPlace);
            Globals.SHPType = breakUpName(Globals.tagValue);

            //plays ship placement sound
            if (Globals.gameMode == false && Globals.SHPBox[0] != Globals.SHPType[0] && Globals.SHPType[0] != PEG.ToString())
                playSound(Globals.soundPath + SHIPPLACEMENT);

            //starts button animation to show player is done placeing ships
            if (Globals.gameMode == false && checkTotalShips() == 0)
                buttonAnim();

            //this sets image, name, sets pegPlace to true so player can only drop one peg, and saves attack spot for later use
            if (Globals.SHPBox[0] == ATKBX.ToString() && Globals.SHPType[0] == PEG.ToString() && Globals.gameMode == true && Globals.pegPlaced == false && aiTimer.Enabled == false && soundTimer.Enabled == false && makeGame.hasWon() == NOBODY)
            {
                ((PictureBox)playerATKPanel.Controls[Globals.SHPBox[0] + Globals.SHPBox[1]]).Image = Image.FromFile(Globals.imgPath + ATTACKBOXATK);
                ((PictureBox)playerATKPanel.Controls[Globals.SHPBox[0] + Globals.SHPBox[1]]).Tag = Globals.SHPType[0];

                Globals.AttackSpot = Convert.ToInt32(Globals.SHPBox[1]);
                Globals.pegPlaced = true;

                //starts flashing button
                buttonAnim();
            }
        }

        //this moves the ship around so it stays on board and in bounds of the board
        public string[] nudgeSystem(int shipLength, string[] SHPBox, string[] SHPType, bool direction)
        {
            int numberOfShiftsNeeded = checkShipLenght(shipLength, SHPBox, direction);

            switch (direction)
            {
                //this is vertical nudge
                case false:
                    if (numberOfShiftsNeeded > 0)
                        SHPBox[1] = (Convert.ToInt32(SHPBox[1]) - ((shipLength - numberOfShiftsNeeded)* Globals.difficulty)).ToString();

                    if (Convert.ToInt32(SHPBox[1]) + ((shipLength) * Globals.difficulty) >= (Globals.boardSize + Globals.difficulty))
                        SHPBox[1] = (Convert.ToInt32(SHPBox[1]) - ((shipLength - numberOfShiftsNeeded) * Globals.difficulty)).ToString();
                    break;

                //this is horizontal nudge
                case true:
                    if (numberOfShiftsNeeded>0)
                        SHPBox[1] = (Convert.ToInt32(SHPBox[1]) -(shipLength - numberOfShiftsNeeded)).ToString();

                    if ((Convert.ToInt32(SHPBox[1]) + shipLength) % Globals.difficulty > 0 && (Convert.ToInt32(SHPBox[1]) + shipLength) % Globals.difficulty <= (shipLength - 1))
                        SHPBox[1] = (Convert.ToInt32(SHPBox[1]) - ((Convert.ToInt32(SHPBox[1]) + shipLength) % Globals.difficulty)).ToString();
                    break;
            }
            return SHPBox;
        }

        //codeing check, this is a brute force check
        public int checkShipLenght(int shipLength, string[] SHPBox, bool direction)
        {
            string boxName = STRINGNULL;
            int loopnum = 0;
            //if every thing looks good it will return no nudges needed
            try
            {
                switch (direction)
                {
                    //this is vertical check
                    case false:
                        for (loopnum = 0; loopnum != shipLength; ++loopnum)
                            boxName = ((PictureBox)playerSHPPanel.Controls[SHPBox[0] + (Convert.ToInt32(SHPBox[1]) + (loopnum * Globals.difficulty)).ToString()]).Name;
                        break;

                    //this is horizontal check
                    case true:
                        for (loopnum = 0; loopnum != shipLength; ++loopnum)
                            boxName = ((PictureBox)playerSHPPanel.Controls[SHPBox[0] + (Convert.ToInt32(SHPBox[1]) + loopnum).ToString()]).Name;
                        break;
                }
                return 0;
            }
            //if errors (IE can not find next PictureBox) it will return loopnum for nudge
            catch
            {
                return loopnum;
            }
        }
        //End drop and drag stuff

        //once this button is hit it will do 1 of two things
        //when game mode is false
        //1st it check to make sure the player has added all ships, and it will also make sure i didnt screw up... :P
        //then it will send array of players ships to board class *still not coded...
        //when game mode is true
        //this is were the player drops pegs onto board
        private void nextStepButton_Click(object sender, EventArgs e)
        {
            resetexceptionTime(); //resets 30 min exception thing

            //next step button has two stages 1st Ship placement, 2nd Attacking
            switch(Globals.gameMode)
            {
                //Ship placement stage
	            case false:

                    //this is to make sure all 20 ship and ship parts are there.
                    if (checkTotalShips() != 0)
                        playerActionLabel.Text = Globals.stringTheme[1];
                    else
                    {
                        Globals.gameMode = true;

                        //stops button animation
                        buttonTimer.Stop();
                        nextStepButton.BackgroundImage = null;

                        playSound(Globals.soundPath + NEWGAMESOUND);

                        //builds player board
                        foreach (PictureBox pb in playerSHPPanel.Controls)
                        {
                            Globals.SHPBox = breakUpName((pb.Tag).ToString());
                            Globals.SHPType = breakUpName(pb.Name);

                            Globals.playerBoard[Convert.ToInt32(Globals.SHPBox[1])] = (Convert.ToChar(Globals.SHPType[0]) == AIRCRAFTCARRIER) ? AIRCRAFTCARRIER :
                                                                                      (Convert.ToChar(Globals.SHPType[0]) == BATTLESHIP) ? BATTLESHIP :
                                                                                      (Convert.ToChar(Globals.SHPType[0]) == DESTROYER) ? DESTROYER :
                                                                                      (Convert.ToChar(Globals.SHPType[0]) == SUBMARINE) ? SUBMARINE :
                                                                                      (Convert.ToChar(Globals.SHPType[0]) == PATROLBOAT) ? PATROLBOAT : OPENWATER;
                        }

                        //changes labels to say attack and drag peg
                        nextStepButton.Text = Globals.stringTheme[2];
                        playerActionLabel.Text = Globals.stringTheme[3];

                        //sends players board to Game class
                        makeGame.setBoards(Globals.difficulty, Globals.playerBoard);
                    }
		            break;

                //Attacking stage
	            case true:
                    //This checks to see if the user placed a peg
                    if (Globals.pegPlaced == false && makeGame.hasWon() == NOBODY)
                        playerActionLabel.Text = Globals.stringTheme[3];
                    else if (makeGame.hasWon() == NOBODY)
                    {
                        //pass's user attack cord to set the spot the user hits to 'x' on the aiboard in game class
                        char player_hit = makeGame.userAttack(Globals.AttackSpot);

                        bool userHit = (player_hit != OPENWATER) ? true : false;

                        int shipSize = getShipSize(player_hit);

                        int stringnum = (player_hit == AIRCRAFTCARRIER) ? 5 :
                                        (player_hit == BATTLESHIP) ? 6 :
                                        (player_hit == DESTROYER) ? 7 :
                                        (player_hit == SUBMARINE) ? 8 :
                                        (player_hit == PATROLBOAT) ? 9 : 11;

                        Globals.makeHit = (player_hit != OPENWATER) ? true : false;
                        fireSound();

                        //changes label to say hit or miss
                        playerActionLabel.Text = (player_hit != OPENWATER) ? Globals.stringTheme[16] : Globals.stringTheme[11];

                        //change the image to hit or miss
                        ((PictureBox)playerATKPanel.Controls[Globals.SHPBox[0] + Globals.SHPBox[1]]).Image = Image.FromFile(Globals.imgPath + ((player_hit != OPENWATER) ? HIT : MISS));
                        ((PictureBox)playerATKPanel.Controls[Globals.SHPBox[0] + Globals.SHPBox[1]]).Name = player_hit.ToString() + Globals.SHPBox[1];

                        //if ship has sunk change images to show ship and tell user what ship was sunk
                        if (makeGame.checkShipHealth(player_hit) == 0)
                        {
                            playerActionLabel.Text = Globals.stringTheme[18] + Globals.stringTheme[stringnum];
                            changeAiShipImages(player_hit, shipSize, player_hit);
                        }

                        //this checks to see if the user wins
                        if (makeGame.hasWon() == USER)
                        {
                            playSound(Globals.soundPath + WINSOUND);

                            //this tell player if they won.
                            playerActionLabel.Text = Globals.stringTheme[17];

                            //destroy all SHPPanel Picktureboxs and add victory image
                            destroy_Grid();

                            //sets SHPPanel image to win
                            playerSHPPanel.BackgroundImage = Image.FromFile(Globals.imgPath + VICTORY);
                        }

                        //sets game info so player can attack again
                        Globals.AttackSpot = -1;
                        Globals.pegPlaced = false;
                        buttonTimer.Stop();
                        nextStepButton.BackgroundImage = null;
                        undoPegPlaceButton.BackgroundImage = null;

                        aiAttack();
                    }
		            break;
            }
        }

        //change images of ship when its sunk
        public void changeAiShipImages(char player_hit, int shipSize, char shipType)
        {
            string[] shipCords = new string[shipSize];
            string[] shipNameParts;
            int loopnum = 0;
            bool shipDirection=false; //starts out assuming ship is veritcal
            string fileName;

            //gets info to find out ships direction
            foreach (PictureBox pb in playerATKPanel.Controls)
            {
                shipNameParts=breakUpName(pb.Name);
                if (shipNameParts[0] == player_hit.ToString())
                {
                    shipCords[loopnum] = shipNameParts[1];
                    ++loopnum;
                }
            }

            //find out if ship is horizontal
            if ((Convert.ToInt32(shipCords[1]) - Convert.ToInt32(shipCords[0])) == 1)
                shipDirection = true;

            loopnum = 0;

            //change images
            foreach (PictureBox pb in playerATKPanel.Controls)
            {
                shipNameParts = breakUpName(pb.Name);
                if (shipNameParts[0] == player_hit.ToString())
                {
                    fileName = (FILENAME[0] + shipType + ((((shipDirection == false) ? (shipSize * 2 + loopnum) : (shipSize * 2 + loopnum + shipSize)) > 9) ? FILENAME[3] : FILENAME[1]) + ((shipDirection == false) ? (shipSize * 2 + loopnum) : (shipSize * 2 + loopnum + shipSize)) + FILENAME[5]);
                    pb.Image = Image.FromFile(Globals.imgPath + fileName);
                    ++loopnum;
                }
            }
        }

        //sets timer and calls AIAttackDelay
        public void aiAttack()
        {
            aiTimer.Interval = 3500; 
            aiTimer.Start();
        }

        private void AIAttackDelay(object sender, EventArgs e)
        {
            string fileName = STRINGNULL;
            int shipSize = 0;
            int aiTarget= makeGame.aiAttack();

            foreach (PictureBox pb in playerSHPPanel.Controls)
            {
                Globals.SHPBox = breakUpName(pb.Tag.ToString());
                Globals.SHPType = breakUpName(pb.Name);

                if (Convert.ToInt32(Globals.SHPBox[1]) == aiTarget)
                {
                    Globals.makeHit = (Globals.SHPType[0] != SHPBX.ToString()) ? true : false;
                    fireSound();

                    if (Convert.ToChar(Globals.SHPType[0]) != SHPBX)
                    {

                        shipSize = (getShipSize(Convert.ToChar(Globals.SHPType[0])) * 2);

                        fileName = (FILENAME[0] + Globals.SHPType[0] + FILENAME[3] + ((((Convert.ToInt32(Globals.SHPType[1]) + shipSize)) < 10) ? FILENAME[2] : FILENAME[4]) + ((Convert.ToInt32(Globals.SHPType[1]) + shipSize).ToString()) + FILENAME[5]);

                        playerActionLabel.Text = Globals.stringTheme[4] + ((Convert.ToChar(Globals.SHPType[0]) == AIRCRAFTCARRIER) ? Globals.stringTheme[5] :
                                                                          (Convert.ToChar(Globals.SHPType[0]) == BATTLESHIP) ? Globals.stringTheme[6] :
                                                                          (Convert.ToChar(Globals.SHPType[0]) == DESTROYER) ? Globals.stringTheme[7] :
                                                                          (Convert.ToChar(Globals.SHPType[0]) == SUBMARINE) ? Globals.stringTheme[8] :
                                                                          Globals.stringTheme[9]);
                    }
                    else
                    {
                        fileName = MISS;

                        //tells user the ai hit nothing
                        playerActionLabel.Text = Globals.stringTheme[10];
                    }
                    try
                    {
                        pb.Image = Image.FromFile(Globals.imgPath + fileName);
                        pb.Name = (Globals.SHPType[0] + (Convert.ToInt32(Globals.SHPType[1]) + shipSize).ToString()); 
                    }
                    catch
                    {
                        //do nothing *meaning not let game crash :P
                    }
                }
            }

            //this checks to see if the AI wins
            if (makeGame.hasWon() == AI)
            {
                playSound(Globals.soundPath + GAMEOVER);

                //this tell player if they lost.
                playerActionLabel.Text = Globals.stringTheme[19];

                //destroy all ATKPanel Picktureboxs
                destroy_Grid();

                //show failed image
                playerATKPanel.BackgroundImage = Image.FromFile(Globals.imgPath + FAILED);
            }
            //stops ai
            aiTimer.Stop();
        }

        //sets time legnth, and calls imageDelay
        public void buttonAnim()
        {
            buttonTimer.Interval = 500;
            buttonTimer.Start();
        }

        private void imageDelay(object sender, EventArgs e)
        {
            if (Globals.gameMode == true)
            {
                undoPegPlaceButton.BackgroundImage = (Globals.buttonTimer == false) ? Image.FromFile(Globals.imgPath + REMOVEPEGBUTTON) : null;
            }

            //adds or removes image on time interval to simulate flashing image
            nextStepButton.BackgroundImage = (Globals.buttonTimer == false) ? Image.FromFile(Globals.imgPath + NEXTSTAGEBUTTON) : null;
            Globals.buttonTimer = (Globals.buttonTimer == false) ? true : false;
        }

        //sets time legnth, and calls soundDelay
        public void fireSound()
        {
            playSound(Globals.soundPath + CANNONSOUND);
            soundTimer.Interval = 1500;
            soundTimer.Start();
        }

        private void soundDelay(object sender, EventArgs e)
        {
            if (makeGame.hasWon() == NOBODY)
                playSound(Globals.soundPath + ((Globals.makeHit == true) ? ALERTSOUND : SPLOOSH));
            Globals.makeHit = false;
            soundTimer.Stop();
        }

        //returns how many ships are left to be placed
        public int checkTotalShips()
        {
            int total = 0;

            //this will go through all picturebox's in the panel and find all ship parts to assign into array
            foreach (PictureBox pb in playerSHPPanel.Controls)
            {
                Globals.SHPBox = breakUpName((pb.Name).ToString());
                if (Convert.ToChar(Globals.SHPBox[0]) != SHPBX)
                    ++total;
            }
            return (ACSIZE + BSSIZE + DSSIZE + SBSIZE + PBSIZE) - total;
        }

        //returns ship size
        public int getShipSize(char shipType)
        {
            return (shipType == AIRCRAFTCARRIER) ? ACSIZE :
                   (shipType == BATTLESHIP) ? BSSIZE :
                   (shipType == DESTROYER) ? DSSIZE :
                   (shipType == SUBMARINE) ? SBSIZE : PBSIZE;
        }

        //breaks up names so they can be used 
        private string[] breakUpName(string BoxName)
        {
            string[] BoxNameParts = new string[2];

            for (int loopnum1 = 0; loopnum1 != BoxName.Length; ++loopnum1)
            {
                if (Char.IsNumber(Convert.ToChar(BoxName[loopnum1])) == false)
                    BoxNameParts[0] += BoxName[loopnum1];
                if (Char.IsNumber(Convert.ToChar(BoxName[loopnum1])) == true)
                    BoxNameParts[1] += BoxName[loopnum1];
            }
            return BoxNameParts;
        }

        //play sound
        public void playSound(string soundName)
        {
            SoundPlayer sound = new SoundPlayer(soundName);
            sound.Play();
        }

        //removes players last placed peg
        private void undoPegPlaceButton_Click(object sender, EventArgs e)
        {
            resetexceptionTime(); //resets 30 min exception thing

            if (makeGame.hasWon() == NOBODY)
            {
                buttonTimer.Stop();
                nextStepButton.BackgroundImage = null;
                undoPegPlaceButton.BackgroundImage = null;
                Globals.SHPBox = breakUpName(Globals.SHPPlace);
                try
                {
                    ((PictureBox)playerATKPanel.Controls[Globals.SHPBox[0].ToString() + Globals.SHPBox[1].ToString()]).Image = Image.FromFile(Globals.imgPath + ATTACKBOX);
                    Globals.pegPlaced = false;
                }
                catch
                {
                    //do nothing *meaning not let game crash :P
                }
            }
        }

        //Rotates AircraftCarrier
        private void rotateACButton_Click(object sender, EventArgs e)
        {
            resetexceptionTime(); //resets 30 min exception thing

            Globals.rotateButton[0] = (Globals.rotateButton[0] == false) ? true : false;
            rotateACButton.Image = (Globals.rotateButton[0] == false) ? Image.FromFile(Globals.imgPath + ROTATEBUTTONOFF) : Image.FromFile(Globals.imgPath + ROTATEBUTTONON);
        }

        //Rotates Battleship
        private void rotateBSButton_Click(object sender, EventArgs e)
        {
            resetexceptionTime(); //resets 30 min exception thing

            Globals.rotateButton[1] = (Globals.rotateButton[1] == false) ? true : false;
            rotateBSButton.Image = (Globals.rotateButton[1] == false) ? Image.FromFile(Globals.imgPath + ROTATEBUTTONOFF) : Image.FromFile(Globals.imgPath + ROTATEBUTTONON);
        }

        //Rotates Destroyer
        private void rotateDSButton_Click(object sender, EventArgs e)
        {
            resetexceptionTime(); //resets 30 min exception thing

            Globals.rotateButton[2] = (Globals.rotateButton[2] == false) ? true : false;
            rotateDSButton.Image = (Globals.rotateButton[2] == false) ? Image.FromFile(Globals.imgPath + ROTATEBUTTONOFF) : Image.FromFile(Globals.imgPath + ROTATEBUTTONON);
        }

        //Rotates Submarine
        private void rotateSBButton_Click(object sender, EventArgs e)
        {
            resetexceptionTime(); //resets 30 min exception thing

            Globals.rotateButton[3] = (Globals.rotateButton[3] == false) ? true : false;
            rotateSBButton.Image = (Globals.rotateButton[3] == false) ? Image.FromFile(Globals.imgPath + ROTATEBUTTONOFF) : Image.FromFile(Globals.imgPath + ROTATEBUTTONON);
        }

        //Rotates Patrolboat
        private void rotatePBButton_Click(object sender, EventArgs e)
        {
            resetexceptionTime(); //resets 30 min exception thing

            Globals.rotateButton[4] = (Globals.rotateButton[4] == false) ? true : false;
            rotatePBButton.Image = (Globals.rotateButton[4] == false) ? Image.FromFile(Globals.imgPath + ROTATEBUTTONOFF) : Image.FromFile(Globals.imgPath + ROTATEBUTTONON);
        }

        //load theme button *Removed*
        //private void setThemeButton_Click(object sender, EventArgs e)
        //{
        //    resetexceptionTime(); //resets 30 min exception thing

        //    themeLoad();
       // }

        //minimize window to system try
        private void toTrayButton_Click(object sender, EventArgs e)
        {
            resetexceptionTime(); //resets 30 min exception thing

            this.WindowState = FormWindowState.Minimized;
        }

        //close game
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
