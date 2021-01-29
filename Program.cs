using System;
using System.IO;
using System.Threading;

namespace Project3
{
    class Program
    {

        public struct Piece
        {
            public string type;
            public string color;
        }
        //This variable, the kind of Piece, is created for game board 
        static public Piece[,] CHESS_BOARD = new Piece[8, 8];
        static public bool Isfirst_selection = false;
        static public int row_selected;
        static public int column_selected;
        static public string temp_motion = "";
        static public bool en_passant = false;
        static public bool promotion = false;
        static public bool Castling = false;
        static public bool[] Cast_check_M = {false,false,false};
        static public bool[] Cast_check_K = {false, false, false};
        static public string transforming_game;
        static public int mode;
        //It holds the name of text played on at the moment
        static public string name_game_up = "";
        //This path contains names of all game played 
        static public string path = "names_of_games.txt";
        static public int number_writing = 1;
        static public int set_cursor = 0;
        static public int cursorx_hint = 70;
        static public int cursory_hint = 17;
        //This variable decides the which user has turn
        static public int which_user = 0;
        static public int cursorx;
        static public int cursory;
        static public int cursorx_writing = 45;
        static public int cursory_writing = 1;

        //This function creates the pieces
        static Piece Generate(string color, string type)
        {
            Piece p = new Piece();
            p.color = color;
            p.type = type;
            return p;
        }
        static int Menu()
        {
            Console.Clear();
            //Console.SetWindowSize(97, 17);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("                                                                                         _:_");
            Console.WriteLine("                                                                                        '-.-'");
            Console.WriteLine("                                                                              ()       __.'.__");
            Console.WriteLine("                                                                           .-:--:-.   |_______|");
            Console.WriteLine(@"                                                                   ()       \____/     \=====/");
            Console.WriteLine(@"                                                                   /\       {====}      )___(");
            Console.WriteLine(@"                     (\=,                                         //\\       )__(      /_____\");
            Console.WriteLine(@"   __     |'-'-'|   //  .\                                       (    )     /____\      |   |");
            Console.WriteLine(@"  /  \    |_____|  (( \_  \                                       )__(       |  |       |   |");
            Console.WriteLine(@"  \__/     |===|    ))  `\_)                                     /____\      |  |       |   |");
            Console.WriteLine(@" /____\    |   |   (/     \                                       |  |       |  |       |   |");
            Console.WriteLine(@"  |  |     |   |    |_.- '|                                       |  |       |  |       |   |");
            Console.WriteLine(@"  |__|     )___(     )___(                                       /____\     /____\     /_____\");
            Console.WriteLine(@" (====)   (=====)   (=====)                                     (======)   (======)   (=======)");
            Console.WriteLine(@" }===={   }====={   }====={                                     }======{   }======{   }======={");
            Console.WriteLine(@"(______) (_______) (_______)                                   (________) (________) (_________)");
            int cx = 4, cy = 7;
            ConsoleKeyInfo cki;
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t\t\t\t|------------------------|");
            Console.WriteLine("\t\t\t\t|                        |");
            Console.WriteLine("\t\t\t\t|     DEU-CENG CHESS     |");
            Console.WriteLine("\t\t\t\t|        MAIN MENU       |");
            Console.WriteLine("\t\t\t\t|                        |");
            Console.WriteLine("\t\t\t\t|        Play Mode       |");
            Console.WriteLine("\t\t\t\t|                        |");
            Console.WriteLine("\t\t\t\t|        Demo Mode       |");
            Console.WriteLine("\t\t\t\t|                        |");
            Console.WriteLine("\t\t\t\t|       Intructions      |");
            Console.WriteLine("\t\t\t\t|                        |");
            Console.WriteLine("\t\t\t\t|          Exit          |");
            Console.WriteLine("\t\t\t\t|                        |");
            Console.WriteLine("\t\t\t\t|------------------------|");
            while (true)
            {
                if (cy == 7) Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(41, 7);
                Console.WriteLine("Play Mode      ");
                if (cy == 9) Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(41, 9);
                Console.WriteLine("Demo Mode     ");
                if (cy == 11) Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(40, 11);
                Console.WriteLine("Intructions    ");
                if (cy == 13) Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(43, 13);
                Console.WriteLine("Exit        ");
                Console.SetCursorPosition(cx + 32, cy);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(">");
                Console.SetCursorPosition(cx + 50, cy);
                Console.Write("<");
                Console.SetCursorPosition(cx + 32, cy);
                cki = Console.ReadKey();
                Console.SetCursorPosition(cx + 32, cy);
                Console.Write(" ");
                if (cki.Key == ConsoleKey.UpArrow && cy > 7) cy -= 2;
                else if (cki.Key == ConsoleKey.DownArrow && cy < 13) cy += 2;
                else if (cki.Key == ConsoleKey.Enter) break;
            }
            Console.ResetColor();
            int x = (cy - 5) / 2;
            return x;
        }        //This function is called every time when needing 
        //User selectes a position on the console, the function converts and returns it as mathmateically
        static int[] Position()
        {
            int[] positions = new int[2];
            bool founded = false;
            while (!founded)
            {
                bool transfering_demo = false;
                Writing_board();
                Console.SetCursorPosition(cursorx, cursory);
                ConsoleKeyInfo cki;
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.RightArrow && cursorx < 27) cursorx += 3;
                else if (cki.Key == ConsoleKey.LeftArrow && cursorx > 6) cursorx -= 3;
                else if (cki.Key == ConsoleKey.UpArrow && cursory > 2) cursory -= 2;
                else if (cki.Key == ConsoleKey.DownArrow && cursory < 16) cursory += 2;
                else if (cki.Key == ConsoleKey.H) Hint();
                else if (cki.Key == ConsoleKey.D) transfering_demo=true;
                else if (cki.Key == ConsoleKey.Spacebar)
                {
                    for (int i = 0; i < CHESS_BOARD.GetLength(0); i++)
                        for (int j = 0; j < CHESS_BOARD.GetLength(1); j++)
                            if (cursorx == 6 + (3 * j) &&
                                cursory == 2 + (2 * i))
                            {
                                positions[0] = i;
                                positions[1] = j;
                                founded = true;
                                Console.SetCursorPosition(50, 6);
                            }
                }
                if (transfering_demo == true)
                {
                    transforming_game = name_game_up;
                    mode = 2;
                    Removing_game();
                    Starting_game();
                    Demo();
                    mode = 1;
                    Table_continued(transforming_game);
                    Background();
                    transfering_demo = false;
                }
            }
            return positions;
        }
        //This is an important function to count the positions a piece can go
        //Used many times for many purpose
        static int[,] Control(int row, int column)
        {
            int index = 0;
            int[,] control = new int[2, 28];
            for (int i = 0; i < control.GetLength(0); i++)
                for (int j = 0; j < control.GetLength(1); j++)
                    control[i, j] = -1;
            for (int i = 0; i < CHESS_BOARD.GetLength(0); i++)
            {
                for (int j = 0; j < CHESS_BOARD.GetLength(1); j++)
                {
                    if (CHESS_BOARD[row, column].color == "M" && CHESS_BOARD[i, j].color == "M") continue;
                    else if (CHESS_BOARD[row, column].color == "K" && CHESS_BOARD[i, j].color == "K") continue;
                    if (i == row && j == column) continue;
                    bool control_can = Calculating_score(row, column, i, j, true);
                    if (control_can == true)
                    {
                        control[0, index] = i;
                        control[1, index] = j;
                        index++;
                    }
                }
            }
            return control;
        }
        //This function gives the program a dynamic visuality
        //Called after every changing on the game and it shows the canging to the user
        static void Writing_board()
        {
            for (int i = 0; i < CHESS_BOARD.GetLength(0); i++)
                for (int j = 0; j < CHESS_BOARD.GetLength(1); j++)
                {
                    bool is_black = false;
                    int cursorx = 6 + 3 * j;
                    int cursory = 2 + 2 * i;
                    Console.SetCursorPosition(cursorx, cursory);
                    switch (CHESS_BOARD[i, j].color)
                    {
                        case "K":
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case "M":
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                    }

                    if ((j % 2 == 1 && i % 2 == 0) || (j % 2 == 0 && i % 2 == 1))
                    {
                        for (int k = -1; k < 1; k++)
                        {
                            for (int l = -1; l < 2; l++)
                            {
                                Console.SetCursorPosition(cursorx + l, cursory + k);
                                Console.BackgroundColor = ConsoleColor.Black;
                                is_black = true;
                                Console.WriteLine(" ");
                            }
                        }
                    }
                    else
                    {
                        for (int k = -1; k < 1; k++)
                        {
                            for (int l = -1; l < 2; l++)
                            {
                                Console.SetCursorPosition(cursorx + l, cursory + k);
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.WriteLine(" ");
                            }
                        }
                    }
                    if (Isfirst_selection == true)
                    {
                        int[,] can_go = Control(row_selected, column_selected);
                        for (int k = 0; k < can_go.GetLength(1); k++)
                        {
                            if (can_go[0, k] == -1)
                                break;

                            if (can_go[0, k] == i && can_go[1, k] == j)
                            {
                                for (int l = -1; l < 1; l++)
                                {
                                    for (int m = -1; m < 2; m++)
                                    {
                                        Console.SetCursorPosition(cursorx + m, cursory + l);
                                        if (is_black == true)
                                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                                        else
                                            Console.BackgroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine(" ");
                                    }
                                }
                            }
                        }
                    }
                    Console.SetCursorPosition(cursorx, cursory);
                    Console.WriteLine(CHESS_BOARD[i, j].type);

                }
            Console.ResetColor();

        }
        //This function does a selection every time when called for piece
        static int[] First_selection()
        {
            int[] First_selection = new int[2];
            while (!Isfirst_selection)
            {
                First_selection = Position();
                row_selected = First_selection[0];
                column_selected = First_selection[1];
                if ((which_user == 0 && CHESS_BOARD[row_selected, column_selected].color == "K")
                || (which_user == 1 && CHESS_BOARD[row_selected, column_selected].color == "M"))
                {
                    Console.SetCursorPosition(65, 12);
                    Console.WriteLine("You chose the other user's piece");
                    Thread.Sleep(2000);
                    Console.SetCursorPosition(65, 12);
                    Console.WriteLine("                                 ");
                }
                else if (CHESS_BOARD[row_selected, column_selected].type == "")
                {
                    Console.SetCursorPosition(65, 12);
                    Console.WriteLine("You select an empty place!");
                    Console.SetCursorPosition(65, 12);
                    Thread.Sleep(2000);
                    Console.WriteLine("                            ");
                }
                else
                    Isfirst_selection = true;
            }
            return First_selection;
        }
        //This function does a selection every time when called for place(may a piece or sth)
        static int[] Second_selection()
        {
            bool Issecond_selection = false;
            int[] second_selection = new int[2];
            while (!Issecond_selection)
            {
                second_selection = Position();
                int row_selected = second_selection[0];
                int column_selected = second_selection[1];
                if ((which_user == 0 && CHESS_BOARD[row_selected,column_selected].color == "K")
                    || (which_user == 1 && CHESS_BOARD[row_selected, column_selected].color == "M")
                    || (CHESS_BOARD[row_selected, column_selected].type == ""))
                {
                    Issecond_selection = true; break;
                }
            }
            return second_selection;
        }
        //For the notation of the game, when occuring the case which has same chance to write for different piece 
        static string Checking_same(int row_selected, int column_selected,int row_selected_2, int column_selected_2)
        {
            string same_movement = "";
            if (CHESS_BOARD[row_selected, column_selected].type == "N" || CHESS_BOARD[row_selected, column_selected].type == "R"
                || CHESS_BOARD[row_selected, column_selected].type == "Q" || CHESS_BOARD[row_selected, column_selected].type == "B")
                for (int i = 0; i < CHESS_BOARD.GetLength(0); i++)
                    for (int j = 0; j < CHESS_BOARD.GetLength(1); j++)
                    {
                        if (row_selected == i && column_selected == j) continue;
                        if (CHESS_BOARD[row_selected, column_selected].type == CHESS_BOARD[i, j].type
                            && CHESS_BOARD[row_selected, column_selected].color == CHESS_BOARD[i, j].color)
                        {
                            int [,] intersection = Control(i, j);
                            for (int k = 0; k < intersection.GetLength(1); k++)
                            {
                                if (intersection[0, k] == -1)
                                    break;
                                if (intersection[0, k] == row_selected_2 && intersection[1, k] == column_selected_2)
                                {
                                    if (column_selected != j)
                                        same_movement = Convert.ToString((char)(97+column_selected));
                                    else
                                        same_movement = Convert.ToString(8 - row_selected);
                                    break;
                                }
                            }
                        }
                    }
            return same_movement;
        }
        //To change the position of a piece selected by the user
        static void Changing_really(int row_selected, int column_selected, int row_selected_2, int column_selected_2)
        {
            string temp_piece = CHESS_BOARD[row_selected, column_selected].type;
            CHESS_BOARD[row_selected, column_selected].type = "";
            string temp_color = CHESS_BOARD[row_selected, column_selected].color;
            CHESS_BOARD[row_selected, column_selected].color = "B";
            //Now, assigning new piece with its color
            CHESS_BOARD[row_selected_2, column_selected_2].type = temp_piece;
            CHESS_BOARD[row_selected_2, column_selected_2].color = temp_color;
        }
        //The difference between the function above is that this function does another job, writing to text by calling
        static void Changing(int row_selected, int column_selected, int row_selected_2, int column_selected_2, bool isControl)
        {
            //For deleting the area of threat
            for (int i = 0; i < 36; i++)
            {
                Console.SetCursorPosition(60+i, 6);
                Console.WriteLine(" ");
            }
            //For deleting the area of hint
            for (int i = 0; i < 16; i++)
            {
                Console.SetCursorPosition(70+i, 16);
                Console.WriteLine(" ");
            }
            if (isControl == false)
                Writing(row_selected, column_selected, row_selected_2, column_selected_2, false);
            //Firstly taking chars and deleting old places
            Changing_really(row_selected, column_selected, row_selected_2, column_selected_2);

        }
        //Thanks to this function, user can continue the game played before
        static void Table_continued(string text_name)
        {
            int[] position_1 = new int[4];
            int[] position_2 = new int[4];
            Removing_game();
            StreamReader f_reading = File.OpenText(text_name);
            do
            {
                string[] operation = f_reading.ReadLine().Split(' ');
                position_1 = Reading_text(operation[0], "M");
                Changing_really(position_1[0], position_1[1], position_1[2], position_1[3]);
                Castling = false;
                which_user = 1;
                if (operation.Length == 1)
                    break;
                position_2 = Reading_text(operation[1], "K");
                Changing_really(position_2[0], position_2[1], position_2[2], position_2[3]);
                which_user = 0;
                Castling = false;
            } while (!f_reading.EndOfStream);
            f_reading.Close();
        }
        //This function writes the lasevery motion in the game by depending the rules of notation
        static void Writing_txt(string changing)
        {
            StreamWriter file_writing = File.AppendText(name_game_up);
            if (which_user == 0)
                file_writing.Write(changing);
            else
                file_writing.WriteLine(" " + changing);
            file_writing.Close();
        }
        static void Background()
        {
            Console.ResetColor();
            for (int i = 8; i > 0; i--)
            {
                if (i == 8)
                    Console.WriteLine("   +--------------------------+");
                Console.WriteLine("   |                          |");
                Console.WriteLine(" " + i + " |                          |");
                if (i == 1)
                    Console.WriteLine("   +--------------------------+");
            }
            Console.WriteLine("      a  b  c  d  e  f  g  h");
            Console.WriteLine("   Please press H for hint");
            Console.WriteLine("   Please press D for demo mode");
        }
        //This function contains the demo mode
        static void Demo()
        {
            mode = 2;
            Removing_game();
            Background();
            Writing_board();
            
            StreamReader File_reading_final = File.OpenText(name_game_up);
            do
            {
                string color;
                if (which_user == 0)
                    color = "M";
                else
                    color = "K";
                string line = File_reading_final.ReadLine();
                string[] operations = line.Split(' ');
                int[] Converted = Reading_text(operations[0], color);
                Changing(Converted[0], Converted[1], Converted[2], Converted[3], false);

                Writing_board();
                if (which_user == 0)
                {
                    color = "K";
                    number_writing++;
                    which_user = 1;
                }
                else
                {
                    which_user = 0;
                    color = "M";
                }
                Thread.Sleep(1000);
                if (operations.Length == 1)
                    break;
                Converted = Reading_text(operations[1], color);
                Changing(Converted[0], Converted[1], Converted[2], Converted[3], false);
                Writing_board();
                if (which_user == 0)
                {
                    number_writing++;
                    which_user = 1;
                }
                else
                    which_user = 0;
                Thread.Sleep(1000);
            } while (!File_reading_final.EndOfStream);
            File_reading_final.Close();
        }
        //This function is a sub-function of the function named reading_text
        //It helps other function to find the index of places selected 
        static int[] Reading_text_control(string piece, char row, char column, string color)
        {
            bool checking_contain = false;
            int[] outcoming = new int[4];
            int out_column = Convert.ToInt16(column) - 97;
            int out_row = 8 - Convert.ToInt32(Convert.ToString(row));
            for (int i = 0; i < CHESS_BOARD.GetLength(0); i++)
            {
                for (int j = 0; j < CHESS_BOARD.GetLength(1); j++)
                {
                    if (CHESS_BOARD[i, j].type == piece && CHESS_BOARD[i, j].color == color)
                    {
                        int[,] can_go = Control(i, j);
                        for (int k = 0; k < can_go.GetLength(1); k++)
                        {
                            checking_contain = false;
                            if (can_go[0, k] == -1)
                                break;

                            if (can_go[0, k] == out_row &&
                                can_go[1, k] == out_column)
                            {
                                checking_contain = true;
                                outcoming[0] = i;
                                outcoming[1] = j;
                                outcoming[2] = out_row;
                                outcoming[3] = out_column;
                                break;
                            }
                        }
                    }
                    if (checking_contain == true) break;
                }
                if (checking_contain == true) break;
            }
            return outcoming;
        }
        //This function converts the strings to indexes by reading text file
        static int[] Reading_text(string coming,string color)
        {//exd6e.p.
            en_passant = false;
            promotion = false;
            bool castling_local = coming.Contains("O");
            int[] outcoming = new int[4];
            int length = coming.Length;
            //Firstly checking containg x
            bool contain_x = coming.Contains("x");
            if (coming[length - 1] == '+')
                length--;
            if (contain_x == true)
            {
                if (length == 5)
                {//Nbxd2 - N1xf3 - exd8Q
                    if (Char.IsUpper(coming[coming.Length - 1]) == true)
                    {
                        outcoming = Reading_text_control("P", coming[3], coming[2], color);
                        outcoming[1] = Convert.ToInt32(coming[0]) - 97;
                        promotion = true;
                        CHESS_BOARD[outcoming[0], outcoming[1]].type = Convert.ToString(coming[coming.Length - 1]);
                    }
                    else
                    {
                        outcoming = Reading_text_control(Convert.ToString(coming[0]), coming[4], coming[3], color);
                        if (Convert.ToInt32(coming[1]) > 96 && Convert.ToInt32(coming[1]) < 104)
                            outcoming[1] = Convert.ToInt32(coming[1]) - 97;
                        else
                            outcoming[0] = 8 - Convert.ToInt32(Convert.ToString(coming[1]));
                    }


                }
                else if (Char.IsUpper(coming[0]) == true)//Qxd4
                    outcoming = Reading_text_control(Convert.ToString(coming[0]), coming[3], coming[2], color);
                else if (length == 8)
                {
                    //exd6e.p.
                    int which_user_check = 0;
                    if (which_user == 0)
                        which_user_check = -1;
                    else
                        which_user_check = 1;

                    int row = 9 - Convert.ToInt32(Convert.ToString(coming[3]));
                    int column = Convert.ToInt32(coming[2]) - 97;
                    Changing_really(row, column, row + which_user_check, column);
                    outcoming = Reading_text_control("P", coming[3], coming[2], color);
                    outcoming[1] = Convert.ToInt32(coming[0]) - 97;
                    en_passant = true;
                }
                else
                {//exd4
                    outcoming = Reading_text_control("P", coming[3], coming[2], color);
                    outcoming[1] = Convert.ToInt32(coming[0]) - 97;
                }
            }
            else if (Castling == false)
            {
                if (length == 3)
                {//Nf5 , f5N c1R
                    if (Char.IsUpper(coming[length - 1]) == true)
                    {
                        outcoming = Reading_text_control("P", coming[1], coming[0], color);
                        promotion = true;
                        CHESS_BOARD[outcoming[0], outcoming[1]].type = Convert.ToString(coming[length - 1]);
                    }
                    else
                        outcoming = Reading_text_control(Convert.ToString(coming[0]), coming[2], coming[1], color);
                }
                else if (length==4)
                {//Nbd2 
                    outcoming = Reading_text_control(Convert.ToString(coming[0]), coming[3], coming[2], color);
                    if (Convert.ToInt32(coming[1]) > 96 && Convert.ToInt32(coming[1]) < 104)
                        outcoming[1] = Convert.ToInt32(coming[1]) - 97;
                    else
                        outcoming[0] = 8 - Convert.ToInt32(Convert.ToString(coming[1]));

                }
                else if (length == 2)
                    outcoming = Reading_text_control("P", coming[1], coming[0], color);
            }
            if (castling_local == true)
            {
                int which_user_row = 0;
                if (which_user == 0)
                    which_user_row = 7;
                Castling = true;
                if (length==5)
                { 
                    outcoming[0] = which_user_row;
                    outcoming[1] = 4;
                    outcoming[2] = outcoming[0];
                    outcoming[3] = 2;
                    Changing_really(which_user_row,0,which_user_row,3);
                }
                else
                {
                    outcoming[0] = which_user_row;
                    outcoming[1] = 4;
                    outcoming[2] = outcoming[0];
                    outcoming[3] = 6;
                    Changing_really(which_user_row, 7, which_user_row, 5);

                }
            }
            return outcoming;
        }
        //This function finds the notation of the motion
        static void Writing(int row_selected, int column_selected, int row_selected_2, int column_selected_2, bool hint)
        { 
            int column_from = 97 + column_selected;
            int column_to = 97 + column_selected_2;
            string destination = Convert.ToString((char)column_to) + Convert.ToString(8 - row_selected_2);
            string same_movement = Checking_same(row_selected, column_selected, row_selected_2, column_selected_2);
            if (CHESS_BOARD[row_selected_2, column_selected_2].color != "B")
            {
                if (CHESS_BOARD[row_selected, column_selected].type == "P")
                    destination = Convert.ToString((char)column_from + "x" + destination);
                else
                    destination = CHESS_BOARD[row_selected, column_selected].type + same_movement + "x" + destination;
            }
            else
            {
                if (CHESS_BOARD[row_selected, column_selected].type != "P")
                    destination = CHESS_BOARD[row_selected, column_selected].type + same_movement + destination;
            }
            if (promotion == true)
            {
                if (destination.Contains('x') == false)
                {//Qd8 d8Q
                    destination += destination[0];
                    destination = destination.Remove(0,1);
                }
                else
                {//Qxd8Q exd8Q
                    destination += destination[0];
                    destination = destination.Remove(0,1);
                    destination = Convert.ToString((char)column_from)+destination;
                }
                promotion = false;
            }
            if (en_passant == true)
            {
                destination += "e.p.";
                en_passant = false;
            }
            if (Castling == true)
            {
                if (column_selected_2 == 6)
                    destination = "O-O";
                else
                    destination = "O-O-O";
                Castling = false;
            }
            if (which_user == 0 && hint == false)
            {
                cursorx_writing -= 11;
                cursory_writing++;
            }
            else if (which_user == 1 && hint == false)
                cursorx_writing += 11;
            if (hint == false)
                Console.SetCursorPosition(cursorx_writing, cursory_writing);
            else
                Console.SetCursorPosition(cursorx_hint, cursory_hint + set_cursor);

            //Changing for the case of threat
            if (hint == false)
            {
                Changing_really(row_selected, column_selected, row_selected_2, column_selected_2);
                bool check = Threat(row_selected_2, column_selected_2, "K");
                if (check == false)
                    check = Threat(row_selected_2, column_selected_2, "M");

                if (check == true)
                    destination += "+";

                Changing_really(row_selected_2, column_selected_2, row_selected, column_selected);
            }
            
            
            
            
            Console.ForegroundColor = ConsoleColor.White;
            if (hint == true)
                Console.Write($"{number_writing} ");
            if (which_user == 0)
            {
                if (hint == false)
                    Console.Write($"{number_writing} ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(destination);
                if (mode == 1)
                    Writing_txt(destination);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(destination);
                if (mode == 1)
                    Writing_txt(destination);
            }
        }
        //This function checks whether the motion is acceptable 
        static bool King(int row_selected, int column_selected, int row_selected_2, int column_selected_2)
        {
            bool continue_game = false;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (row_selected - 1 + i == row_selected_2 && column_selected - 1 + j == column_selected_2)
                        continue_game = true;
            return continue_game;
        }
        //This function checks whether the motion is acceptable 
        static bool RookBisQueen(int row_selected, int column_selected, int row_selected_2, int column_selected_2)
        {
            //Firstly checking Bishop
            bool like_bishop = false;
            int for_row = row_selected - row_selected_2;
            int for_column = column_selected - column_selected_2;
            if (for_row == for_column || for_row == for_column * -1)
                like_bishop = true;
            if (like_bishop == true)
            {
                int for_row_final = for_row / Math.Abs(for_row);
                int for_column_final = for_column / Math.Abs(for_column);
                //if it's bishop, checking area of movement
                for (int i = 1; i < Math.Abs(for_row) + 1; i++)
                {
                    if (i != Math.Abs(for_row) && CHESS_BOARD[row_selected - (for_row_final * i), column_selected - (for_column_final * i)].type != "")
                    {
                        like_bishop = false;
                        break;
                    }
                    else if (i == Math.Abs(for_row) && (CHESS_BOARD[row_selected - (for_row_final * i), column_selected - (for_column_final * i)].type != "")
                        || (CHESS_BOARD[row_selected - (for_row_final * i), column_selected - (for_column_final * i)].color == "K" && which_user == 0)
                        || (CHESS_BOARD[row_selected - (for_row_final * i), column_selected - (for_column_final * i)].color == "M" && which_user == 1))
                        like_bishop = true;
                }
            }
            //Secondly, checking R
            bool check_R = false;
            if (row_selected != row_selected_2 && column_selected == column_selected_2)
                check_R = true;
            else if (column_selected != column_selected_2 && row_selected == row_selected_2)
                check_R = true;
            //if it's R, checking area of movement
            for_row = row_selected_2 - row_selected;
            for_column = column_selected_2 - column_selected;
            if (check_R == true)
            {
                int for_row_final_R = 0;
                int for_column_final_R = 0;
                if (for_row != 0)
                    for_row_final_R = for_row / Math.Abs(for_row);
                else
                    for_column_final_R = for_column / Math.Abs(for_column);
                for (int i = 1; i < Math.Abs(for_row + for_column) + 1; i++)
                    if (i != Math.Abs(for_row + for_column) && CHESS_BOARD[row_selected + (for_row_final_R * i), column_selected + (for_column_final_R * i)].type != "")
                    {
                        check_R = false;
                        break;
                    }
                    else if (i == Math.Abs(for_row + for_column) &&
                        (CHESS_BOARD[row_selected + (for_row_final_R * i), column_selected + (for_column_final_R * i)].color == "K" && which_user == 0)
                        || CHESS_BOARD[row_selected + (for_row_final_R * i), column_selected + (for_column_final_R * i)].color == "M" && which_user == 1)
                        check_R = true;
            }
            if (CHESS_BOARD[row_selected, column_selected].type == "B" && like_bishop == true)
                return true;
            else if (CHESS_BOARD[row_selected, column_selected].type == "R" && check_R == true)
                return true;
            else if ((check_R == true || like_bishop == true) && CHESS_BOARD[row_selected, column_selected].type == "Q")
                return true;
            else
                return false;
        }
        //This function checks whether the motion is acceptable 
        static bool Knight(int row_selected, int column_selected, int row_selected_2, int column_selected_2)
        {
            bool checking_knight = false;
            //Firstly, checking Column
            int column_K = column_selected - column_selected_2;
            //Now checking row
            int row_K = row_selected - row_selected_2;
            row_K = Math.Abs(row_K);
            column_K = Math.Abs(column_K);
            if ((row_K == 1 && column_K == 2) || (row_K == 2 && column_K == 1))
                checking_knight = true;
            return checking_knight;
        }
        //This function checks whether the motion is acceptable 
        static bool Pawn(int row_selected, int column_selected, int row_selected_2, int column_selected_2)
        {
            bool checking_pawn = false;
            if (CHESS_BOARD[row_selected, column_selected].color == "M")
            { 
                if (row_selected - 1 == row_selected_2 && column_selected == column_selected_2 && CHESS_BOARD[row_selected_2, column_selected_2].type == ""
                    || (row_selected == 6 && row_selected - 2 == row_selected_2
                    && column_selected == column_selected_2 && CHESS_BOARD[row_selected_2, column_selected_2].color == "B"))
                    checking_pawn = true;

                if (row_selected - 1 == row_selected_2)
                {
                    if (column_selected > 0 && column_selected < 7)
                    {
                        if (column_selected + 1 == column_selected_2 || column_selected - 1 == column_selected_2)
                        {
                            if (CHESS_BOARD[row_selected_2, column_selected_2].color == "K")
                            {
                                checking_pawn = true;
                            }
                        }
                    }

                    else if (column_selected == 0 || column_selected == 7)
                    {
                        if (column_selected + 1 == column_selected_2 || column_selected - 1 == column_selected_2)
                        {
                            if (CHESS_BOARD[row_selected_2, column_selected_2].color == "K")
                            {
                                checking_pawn = true;
                            }
                        }

                    }
                }

            }
            else
            {
                if (row_selected + 1 == row_selected_2 && column_selected == column_selected_2 && CHESS_BOARD[row_selected_2, column_selected_2].type == ""
                    || (row_selected == 1 && row_selected + 2 == row_selected_2
                    && column_selected == column_selected_2 && CHESS_BOARD[row_selected_2, column_selected_2].color == "B"))
                    checking_pawn = true;
                if (row_selected + 1 == row_selected_2)
                {
                    if (column_selected > 0 && column_selected < 7)
                    {
                        if (column_selected + 1 == column_selected_2 || column_selected - 1 == column_selected_2)
                        {
                            if (CHESS_BOARD[row_selected_2, column_selected_2].color == "M")
                            {
                                checking_pawn = true;
                            }
                        }
                    }

                    else if (column_selected == 0||column_selected==7)
                    {
                        if (column_selected + 1 == column_selected_2 || column_selected - 1 == column_selected_2)
                        {
                            if (CHESS_BOARD[row_selected_2, column_selected_2].color == "M")
                            {
                                checking_pawn = true;
                            }
                        }
                    }
                }


            }
            return checking_pawn;
        }
        //This function is called after the user decides the motion, checks whether is acceptable
        static bool Calculating_score(int row_selected, int column_selected, int row_selected_2, int column_selected_2, bool iscontrol)
        {
            bool continue_game = false;
            switch (CHESS_BOARD[row_selected, column_selected].type)
            {
                case "P":
                    continue_game = Pawn(row_selected, column_selected, row_selected_2, column_selected_2);
                    if (continue_game == true && iscontrol == false)
                        promotion = Promotion(row_selected, column_selected, row_selected_2, column_selected_2);
                    break;
                case "K":
                    continue_game = King(row_selected, column_selected, row_selected_2, column_selected_2);
                    break;
                case "Q":
                case "B":
                case "R":
                    continue_game = RookBisQueen(row_selected, column_selected, row_selected_2, column_selected_2);
                    break;
                case "N":
                    continue_game = Knight(row_selected, column_selected, row_selected_2, column_selected_2);
                    break;
            }
            if (continue_game == true && iscontrol == false)
                Changing(row_selected, column_selected, row_selected_2, column_selected_2, false);
            if (continue_game == true) return true;
            else return false;

        }
        //This function finds the position of king when called
        //It helps to the functions playing role about the game-over
        static int[] Position_king(string color)
        {
            int[] position = new int[2];
            position[0] = -1;
            position[1] = -1;
            for (int i = 0; i < CHESS_BOARD.GetLength(0); i++)
                for (int j = 0; j < CHESS_BOARD.GetLength(1); j++)
                    if (CHESS_BOARD[i, j].type == "K" && CHESS_BOARD[i, j].color != color)
                    {
                        position[0] = i;
                        position[1] = j;
                        break;
                    }
            return position;
        }
        //This function checks there is a threat
        static bool Threat(int i, int j, string color)
        {
            bool threat = false;
            int[] position_king = Position_king(color);
            int[,] can_go = Control(i, j);
            for (int k = 0; k < can_go.GetLength(1); k++)
            {
                if (can_go[0, k] == -1)
                    break;
                if (can_go[0, k] == position_king[0]
                    && can_go[1, k] == position_king[1])
                {
                    threat = true;
                    break;
                }
            }
            return threat;
        }
        //After decided there is a threat, this function checks the user can deal with it
        static bool Still_exist_threat(int row, int column, int threat_row, int threat_col, string color)
        {
            bool stil_exist_threat = true;
            int[,] can_go = Control(row, column);
            for (int i = 0; i < can_go.GetLength(1); i++)
            {
                if (can_go[0, i] == -1)
                    break;
                int row_run = can_go[0, i];
                int col_run = can_go[1, i];
                string type_run = CHESS_BOARD[row_run, col_run].type;
                string color_run = CHESS_BOARD[row_run, col_run].color;
                Changing(row, column, row_run, col_run, true);
                bool exist_threat = Threat(threat_row, threat_col, color);
                Changing(row_run, col_run, row, column, true);
                CHESS_BOARD[row_run, col_run].type = type_run;
                CHESS_BOARD[row_run, col_run].color = color_run;
                if (exist_threat == false)
                {
                    stil_exist_threat = false;
                    break;
                }
                else
                    stil_exist_threat = true;
            }
            return stil_exist_threat;
        }
        //This function checks there is still a threat after changing by the still_exist_threat function 
        static bool Running_threat(int threat_row, int threat_col, string color)
        {
            int coun_all_piece = 0;
            int defeat = 0;
            for (int i = 0; i < CHESS_BOARD.GetLength(0); i++)
                for (int j = 0; j < CHESS_BOARD.GetLength(1); j++)
                    if (CHESS_BOARD[i, j].type != "." && CHESS_BOARD[i, j].color != color)
                    {
                        coun_all_piece++;
                        bool still_threat = Still_exist_threat(i, j, threat_row, threat_col, color);
                        if (still_threat == true)
                            defeat++;
                    }
            if (coun_all_piece == defeat) return true;
            else return false;
        }
        //This is the main function of checking game-over
        static int Exist_threat(string color)
        {
            int state_thread = 0;
            bool run_threat = false;
            for (int i = 0; i < CHESS_BOARD.GetLength(0); i++)
                for (int j = 0; j < CHESS_BOARD.GetLength(1); j++)
                    if (CHESS_BOARD[i, j].color == color)
                    {
                        bool threat = Threat(i, j, color);
                        if (threat == true)
                        {
                            state_thread = 1;
                            run_threat = Running_threat(i, j, color);
                        }
                    }
            if (run_threat == true)
                state_thread = 2;
            return state_thread;
        }
        //This function gives the user some hints
        static void Hint()
        {
            //To avoid to write to txt
            mode = 3;
            set_cursor = 0;
            Console.SetCursorPosition(70, 16);
            Console.WriteLine("-----HINTS-----");
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 6; j++)
                {
                    Console.SetCursorPosition(70 + i, 17 + j);
                    Console.WriteLine(" ");
                }
            for (int i = 0; i < CHESS_BOARD.GetLength(0); i++)
            {
                for (int j = 0; j < CHESS_BOARD.GetLength(1); j++)
                {
                    if (which_user == 0 && CHESS_BOARD[i, j].color == "M")
                    {
                        int[,] can_go = Control(i, j);
                        for (int k = 0; k < can_go.GetLength(1); k++)
                        {
                            if (can_go[0, k] == -1)
                                break;
                            int can_go_row = can_go[0, k];
                            int can_go_column = can_go[1, k];
                            if (CHESS_BOARD[can_go_row, can_go_column].color == "K")
                            {
                                set_cursor++;
                                Writing(i, j, can_go_row, can_go_column, true);
                            }
                        }
                    }
                    if (which_user == 1 && CHESS_BOARD[i, j].color == "K")
                    {
                        int[,] can_go = Control(i, j);
                        for (int k = 0; k < can_go.GetLength(1); k++)
                        {
                            if (can_go[0, k] == -1)
                                break;
                            int can_go_row = can_go[0, k];
                            int can_go_column = can_go[1, k];
                            if (CHESS_BOARD[can_go_row, can_go_column].color == "M")
                            {
                                set_cursor++;
                                Console.SetCursorPosition(cursorx_hint, cursory_hint + set_cursor);
                                Writing(i, j, can_go_row, can_go_column, true);
                            }
                        }
                    }
                }
            }
            mode = 1;
        }
        //This function creates a text, its name is determined by the user
        static void New_game()
        {
            mode = 1;
            string game_names = "";
            Console.Clear();
            Console.ResetColor();
            Console.Write("Please enter the name of first user ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("(Blue): ");
            game_names += Console.ReadLine();
            Console.Clear();
            Console.ResetColor();
            Console.Write("Please enter the name of second user ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("(Red): ");
            game_names += Console.ReadLine()+".txt";
            Console.ResetColor();
            StreamWriter file_names = File.AppendText(path);
            file_names.WriteLine(game_names);
            file_names.Close();
            name_game_up = game_names;
        }
        static bool Choice()
        {
            bool returning = false;
            int cx = 4, cy = 7;
            ConsoleKeyInfo cki;
            Console.SetCursorPosition(0, 2);
            Console.WriteLine("\t\t\t\t|--------------------------------|");
            Console.WriteLine("\t\t\t\t|                                |");
            Console.WriteLine("\t\t\t\t|                                |");
            Console.WriteLine("\t\t\t\t|        Create a new game       |");
            Console.WriteLine("\t\t\t\t|                                |");
            Console.WriteLine("\t\t\t\t|         Continue a game        |");
            Console.WriteLine("\t\t\t\t|                                |");
            Console.WriteLine("\t\t\t\t|                                |");
            Console.WriteLine("\t\t\t\t|--------------------------------|");

            while (true)
            {
                if (cy == 5) Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(41, 5);
                Console.WriteLine("Create a new game      ");
                if (cy == 7) Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(41, 7);
                Console.WriteLine("Continue a game     ");
                Console.SetCursorPosition(cx + 32, cy);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(">");
                Console.SetCursorPosition(cx + 55, cy);
                Console.Write("<");
                cki = Console.ReadKey();
                Console.SetCursorPosition(cx + 32, cy);
                Console.Write(" ");
                if (cki.Key == ConsoleKey.UpArrow && cy > 5) cy -= 2;
                else if (cki.Key == ConsoleKey.DownArrow && cy < 7) cy += 2;
                else if (cki.Key == ConsoleKey.Enter)
                {
                    if (cy == 5)
                    {
                        returning = true;
                    }
                    else
                        returning = false;
                    break;
                }

            }    
            return returning;

        }
        //This function remove all of the variables in the class, starts the game as its first
        static void Removing_game()
        {
            Console.Clear();
            number_writing = 1;
            set_cursor = 0;
            cursorx_hint = 70;
            cursory_hint = 17;
            which_user = 0;
            cursorx_writing = 45;
            cursory_writing = 1;

            for (int i = 2; i < 6; i++)
                for (int j = 0; j < CHESS_BOARD.GetLength(1); j++)
                    CHESS_BOARD[i, j] = Generate("B", "");

            int pawn_row = 1;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < CHESS_BOARD.GetLength(1); j++)
                {
                    if (pawn_row == 1)
                        CHESS_BOARD[pawn_row, j] = Generate("K", "P");
                    else
                        CHESS_BOARD[pawn_row, j] = Generate("M", "P");
                }
                pawn_row = 6;
            }

            int color_other = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    string color = "";
                    if (color_other == 0)
                        color = "K";
                    else if (color_other == 7)
                        color = "M";

                    if (j == 0)
                    {
                        CHESS_BOARD[color_other, j] = Generate(color, "R");
                        CHESS_BOARD[color_other, j + 7] = Generate(color, "R");
                    }
                    else if (j == 1)
                    {
                        CHESS_BOARD[color_other, j] = Generate(color, "N");
                        CHESS_BOARD[color_other, j + 5] = Generate(color, "N");
                    }
                    else if (j == 2)
                    {
                        CHESS_BOARD[color_other, j] = Generate(color, "B");
                        CHESS_BOARD[color_other, j + 3] = Generate(color, "B");
                    }
                    else if (j == 3)
                        CHESS_BOARD[color_other, j] = Generate(color, "Q");
                    else
                        CHESS_BOARD[color_other, j] = Generate(color, "K");
                }
                color_other= 7;
            }

        }
        //Checking the special cases
        static bool En_passant_checking(int row_selected, int column_selected, int row_selected_2, int column_selected_2)
        {
            bool en_passant_checking = false;
            int which_user_check = 0;
            if (which_user == 0)
                which_user_check = -1;
            else
                which_user_check = 1;

            string last_motion = temp_motion;
            int[] last_motion_array = new int[4];
            for (int i = 0; i < last_motion.Length; i++)
                last_motion_array[i] = Convert.ToInt32(Convert.ToString(last_motion[i]));

            if (row_selected + which_user_check == row_selected_2)
                if (column_selected + 1 == column_selected_2 || column_selected - 1 == column_selected_2)
                    en_passant_checking = true;

            if (en_passant_checking == false)
                return false;
            else
                en_passant_checking = false;

            if (last_motion_array[3] == column_selected_2 && row_selected == last_motion_array[2])
            {
                if (CHESS_BOARD[last_motion_array[2], last_motion_array[3]].type == "P")
                {
                    Changing_really(last_motion_array[2], last_motion_array[3], row_selected_2, column_selected_2);
                    en_passant = true;
                    en_passant_checking = true;
                    Changing(row_selected, column_selected, row_selected_2, column_selected_2, false);
                    return true;
                }
                else
                    return false;
            }

            return en_passant_checking;
        }
        static bool Promotion(int row_selected, int column_selected, int row_selected_2, int column_selected_2)
        {
            if (row_selected_2 == 7 || row_selected_2 == 0)
            {
                Console.SetCursorPosition(0, 25);
                Console.WriteLine("1-) Q\n2-) B\n3-) N\n4-) R\nPlease enter your choice: (like Q) ");
                string choice_type = Console.ReadLine();
                Console.SetCursorPosition(0, 25);
                Console.WriteLine("     \n     \n     \n     \n                                                \n ");
                CHESS_BOARD[row_selected, column_selected].type = Convert.ToString(choice_type.ToUpper());
                return true;
            }
            else
                return false;

        }
        static bool Starting_game()
        {
            Console.Clear();
            Console.ResetColor();
            int linenum = 0;
            if (File.Exists(path))
            {
                int index = 0;
                StreamReader file_names = File.OpenText(path);
                do
                {
                    file_names.ReadLine();
                    linenum++;
                } while (!file_names.EndOfStream);
                file_names.Close();
                string[] names_files = new string[linenum];
                StreamReader file_names_reading = File.OpenText(path);
                do
                {
                    names_files[index] = file_names_reading.ReadLine();
                    index++;
                } while (!file_names_reading.EndOfStream);
                file_names_reading.Close();
                if (names_files[0]=="")
                {
                    return false;
                }
                for (int i = 0; i < names_files.Length; i++)
                {
                    Console.WriteLine($"{i+1}-) {names_files[i]}");
                }
                Console.Write("Please enter the index of your game for demo mode: ");
                int choice= Convert.ToInt32(Console.ReadLine());
                name_game_up = names_files[choice - 1];
                return true;
            }
            else
            {
                FileStream fs = File.Create(path);
                fs.Close(); 
                return false;
            }
        }
        static bool Check_cast_final(int row_selected, int column_selected, int row_selected_2, int column_selected_2)
        {
            int which_user_row = 0;
            if (which_user == 0)
                which_user_row = 7;

            //firs condition
            bool check_cast = false;
            if (row_selected == row_selected_2 && Math.Abs(column_selected - column_selected_2) == 2)
                check_cast = true;
            if (check_cast == false)
                return false;


            //Second condition
            if (column_selected_2==6)
            {
                for (int i = column_selected+1; i < 7; i++)
                    if (CHESS_BOARD[which_user_row, i].type != "")
                        check_cast = false;
            }
            else
            {
                for (int i = column_selected - 1; i>0; i--)
                    if (CHESS_BOARD[which_user_row, i].type != "")
                        check_cast = false;
            }
            if (check_cast == false)
                return false;
            //Third condition
            if (column_selected_2 == 6&& which_user == 0)
            {
                if (Cast_check_M[1] == false && Cast_check_M[2] == false)
                    check_cast = true;
                else
                    check_cast = false;
            }
            else if (column_selected_2 == 6 && which_user == 1)
            {
                if (Cast_check_K[1] == false && Cast_check_K[2] == false)
                    check_cast = true;
                else
                    check_cast = false;
            }
            else if (column_selected_2 == 2 && which_user == 1)
            {
                if (Cast_check_K[1] == false && Cast_check_K[0] == false)
                    check_cast = true;
                else
                    check_cast = false;
            }
            else if (column_selected_2 == 2 && which_user == 0)
            {
                if (Cast_check_M[1] == false && Cast_check_M[0] == false)
                    check_cast = true;
                else
                    check_cast = false;
            }
            //Operation will be done if bool is true
            if (check_cast == true)
            {
                if (column_selected_2==6)
                    Changing_really(which_user_row, 7, which_user_row, 5);
                else
                    Changing_really(which_user_row, 0, which_user_row, 3);

                Castling = true;
                Changing(row_selected, column_selected, row_selected_2, column_selected_2,false);
            }
            return check_cast;
        }
        static void Check_Cast_move(int row_selected, int column_selected, int row_selected_2, int column_selected_2)
        {
            if (which_user == 0)
            {
                if (CHESS_BOARD[row_selected,column_selected].type=="K")
                {
                    Cast_check_M[1] = true;
                }
                else
                {
                    if (column_selected==0)
                    {
                        Cast_check_M[0] = true;
                    }
                    else if (column_selected == 7)
                    {
                        Cast_check_M[2] = true;
                    }
                }

            }
            else
            {
                if (CHESS_BOARD[row_selected, column_selected].type == "K")
                {
                    Cast_check_K[1] = true;
                }
                else
                {
                    if (column_selected == 0)
                    {
                        Cast_check_K[0] = true;
                    }
                    else if (column_selected == 7)
                    {
                        Cast_check_K[2] = true;
                    }
                }
            }




        }
        //This function manages the all job of checking whether the game is over
        static bool Game_over()
        {
            Writing_board();
            bool Is_game_over = false;
            int checking_K = Exist_threat("K");
            if (checking_K == 2)
            {
                Console.SetCursorPosition(60, 6);
                Console.WriteLine("! ! ! THE END ! ! !");
                Console.SetCursorPosition(60, 7);
                Console.WriteLine("   Red Player Win   ");
                Console.SetCursorPosition(55, 9);
                Console.WriteLine("Please enter to go main menu");
                Console.ReadLine();
            }
            else if (checking_K == 1)
            {
                Console.SetCursorPosition(60, 6);
                Console.WriteLine("! ! ! Blue Player is in danger ! ! !");
            }
            int checking_M = Exist_threat("M");
            if (checking_M == 2)
            {
                Console.SetCursorPosition(60, 6);
                Console.WriteLine("! ! ! THE END ! ! !");
                Console.SetCursorPosition(55, 9);
                Console.WriteLine("  Blue Player Win");
                Console.SetCursorPosition(60, 8);
                Console.WriteLine("Please enter to go main menu");
                Console.ReadLine();
            }
            else if (checking_M == 1)
            {
                Console.SetCursorPosition(60, 6);
                Console.WriteLine("! ! ! Red Player is in danger ! ! !");

            }
            if (checking_K == 2 || checking_M == 2)
                Is_game_over = true;
            if (which_user == 0)
                which_user = 1;
            else
            {
                number_writing++;
                which_user = 0;
            }

            return Is_game_over;
        }
        static bool Game_mode()
        {
            int row_selected = -1;
            int column_selected = -1;
            int row_selected_2 = -1;
            int column_selected_2 = -1;
            bool Is_game_over = false;
            Removing_game();
            bool choice = Choice();
            if (choice == true)
                New_game();
            else
            {
                bool check = Starting_game();
                if (check == false)
                {
                    Console.WriteLine("There is nothing! Please enter to go menu..");
                    Console.ReadLine();
                    New_game();
                }
                else
                    Table_continued(name_game_up);
            }
            Console.ResetColor();
            Console.Clear();
            Background();
            while (!Is_game_over)
            {
                int can_going = 0;
                while (can_going == 0)
                {
                    int[] first_selection = First_selection();
                    row_selected = first_selection[0];
                    column_selected = first_selection[1];
                    int[,] control = Control(row_selected, column_selected);

                    for (int i = 0; i < control.GetLength(1); i++)
                        if (control[0, i] != -1)
                            can_going++;
                    if (can_going == 0)
                    {
                        Isfirst_selection = false;
                        Console.SetCursorPosition(55, 12);
                        Console.WriteLine("Every place is filled, can not go anywhere");
                        Thread.Sleep(2000);
                        Console.SetCursorPosition(55, 12);
                        Console.WriteLine("                                           ");
                    }
                    else
                        break;

                }
                bool IsTrueMovement = false;
                while (!IsTrueMovement)
                {
                    int[] second_selection = Second_selection();
                    row_selected_2 = second_selection[0];
                    column_selected_2 = second_selection[1];
                    IsTrueMovement = Calculating_score(row_selected, column_selected, row_selected_2, column_selected_2, false);
                    if (IsTrueMovement == false && CHESS_BOARD[row_selected, column_selected].type == "P" && CHESS_BOARD[row_selected_2, column_selected_2].type == "")
                    {
                        IsTrueMovement = En_passant_checking(row_selected, column_selected, row_selected_2, column_selected_2);
                    }
                    if (CHESS_BOARD[row_selected, column_selected].type == "K" && IsTrueMovement == false)
                    {
                        IsTrueMovement = Check_cast_final(row_selected, column_selected, row_selected_2, column_selected_2);
                    }
                    if (IsTrueMovement == false)
                    {
                        Console.SetCursorPosition(55, 12);
                        Console.WriteLine("This piece can not go, it has not such a this movement");
                        Thread.Sleep(2000);
                        Console.SetCursorPosition(55, 12);
                        Console.WriteLine("                                                       ");
                    }
                    else
                    {
                        Isfirst_selection = false;
                        break;
                    }
                }
                if (CHESS_BOARD[row_selected, column_selected].type == "R" || CHESS_BOARD[row_selected, column_selected].type == "K")
                    Check_Cast_move(row_selected, column_selected, row_selected_2, column_selected_2);

                temp_motion = Convert.ToString(row_selected) + Convert.ToString(column_selected);
                temp_motion += Convert.ToString(row_selected_2) + Convert.ToString(column_selected_2);
                Is_game_over = Game_over();
            }
            return Is_game_over;
        }
        //All of the project is managed in this function
        static void Main(string[] args)
        {
            Console.Title = "CHESS - DEU - CENG";
            while (true)
            {
                int choice = Menu();
                if (choice == 1)
                {
                    Game_mode();
                }
                else if (choice == 2)
                {
                    bool check = Starting_game();
                    if (check == false)
                        Console.WriteLine("There is nothing, please enter to go menu");
                    else
                        Demo();
                }
                else if (choice == 3)
                {
                    Console.Clear();
                    Console.WriteLine("The king moves exactly one square horizontally, vertically, or diagonally. A special move with the king known as castling is allowed only once per player, per game (see below).\n"
                        + "A rook moves any number of vacant squares horizontally or vertically.It also is moved when castling.\n"
                        + "A bishop moves any number of vacant squares diagonally.\n"
                        + "The queen moves any number of vacant squares horizontally, vertically, or diagonally.\n"
                        + "A knight moves to the nearest square not on the same rank, file, or diagonal. (This can be thought of as moving two squares horizontally then one square vertically, or moving one square horizontally then two squares vertically—i.e. in an L pattern.) \n"
                        + "The knight is not blocked by other pieces it jumps to the new location.\n"
                        + "\nPawns have the most complex rules of movement:\n"
                        + "A pawn moves straight forward one square, if that square is vacant.If it has not yet moved, a pawn also has the option of moving two squares straight forward, provided both squares are vacant.Pawns cannot move backwards.\n"
                        + "Pawns are the only pieces that capture differently from how they move.A pawn can capture an enemy piece on either of the two squares diagonally in front of the pawn(but cannot move to those squares if they are vacant).\n"
                        + "The pawn is also involved in the two special moves en passant and promotion\n");
                    Console.WriteLine("Please enter to go main menu...");
                    Console.ReadLine();
                }
                else if (choice == 4)
                    break;
                else
                    Console.WriteLine("Please enter a valid choice!");

            }

        }
    }
}