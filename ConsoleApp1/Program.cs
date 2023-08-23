using Microsoft.VisualBasic;
using System;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Created 2 arrays
            int[] sudokuBoard = new int[81];
            int[,] takenNumbers = new int[81, 2];
            //function reseting Sudoku
            void resetSudoku()
            {
                for (int i = 0; i < 81; i++)
                {
                    sudokuBoard[i] = 0;
                    for (int j = 0; j < 2; j++)
                    {
                        takenNumbers[i, j] = 0;
                    }
                }
            }
            Random rnd = new Random();
            //function which generates digits into the empty sudoku board created earlier
            void generateSudokuBoard(int randomNumber = 0, int index = 0)
            {
                //we need to check if we went back to that index or if it is next index which is empty
                if (takenNumbers[index,0] == 0)
                {
                    randomNumber = rnd.Next(1, 10);
                    takenNumbers[index, 0] = randomNumber;
                }
                //when we went back
                else
                {
                    //number increasing because if we went back it means that we need to check another number,
                    //another way in making our board
                    randomNumber = takenNumbers[index, 1]++;
                    //if every digit was checked at that index then we clear this index
                    //and board from it and we go back to previous index
                    if (randomNumber == takenNumbers[index,0])
                    {
                        randomNumber = 0;
                        takenNumbers[index, 0] = 0;
                        takenNumbers[index, 1] = 0;
                        sudokuBoard[index] = 0;
                        index--;
                        generateSudokuBoard(randomNumber, index);
                        //return to stop function from going further
                        return;
                    }
                    //we operate on 1-9 digits
                    if(randomNumber > 9)
                    {
                        randomNumber = 1;
                    }
                }   
                //if our number does not fill into the board at that index
                if (checkIfFits(index, randomNumber) == false)
                {
                    //we check every digit at that index if it fills
                    do
                    {
                        randomNumber++;
                        if (randomNumber > 9)
                        {
                            randomNumber = 1;
                        }
                    }
                    while (checkIfFits(index, randomNumber) == false && randomNumber != takenNumbers[index, 0]);
                    //when loop is done we check if the number fills or not
                    if (randomNumber == takenNumbers[index,0] && checkIfFits(index, randomNumber) == false)
                    {
                        //back we go, we fill current index with 0s
                        randomNumber = 0;
                        takenNumbers[index, 0] = 0;
                        takenNumbers[index, 1] = 0;
                        sudokuBoard[index] = 0;
                        index--;
                        generateSudokuBoard(randomNumber, index);
                    }
                    //if the number fills into the board then we go further at another index
                    else if(checkIfFits(index, randomNumber) == true)
                    {
                        sudokuBoard[index] = randomNumber;
                        takenNumbers[index, 1] = randomNumber;
                        index++;
                        if (index <= 80) generateSudokuBoard(randomNumber, index);
                    }
                }
                //if it fills right in then we go further
                else
                {
                    sudokuBoard[index] = randomNumber;
                    takenNumbers[index, 1] = randomNumber;
                    index++;
                    if (index <= 80) generateSudokuBoard(randomNumber, index);
                }
            }
            //function that is checking 3 sudoku conditions to be fulfilled
            bool checkIfFits(int index, int number)
            {
                //if the number fill horizontally
                for(int i=(index/9)*9; i<((index / 9) * 9)+9; i++)
                {
                    if(number == sudokuBoard[i])
                    {
                        return false;
                    }
                }
                //if the number fill vertically
                for (int i = index % 9; i < (index % 9)+72; i=i+9)
                {
                    if (number == sudokuBoard[i])
                    {
                        return false;
                    }
                }
                //if the number fills in 3x3 box
                for (int i=((((index / 9) / 3) * 3) * 9) + (((index % 9) / 3) * 3); i<=((((index / 9) / 3) * 3) * 9) + (((index % 9) / 3) * 3) + 20; i=i + 9)
                {
                    for(int j=0; j<3; j++)
                    {
                        if (number == sudokuBoard[i+j])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            //function showing the sudoku board on the console
            void showTheBoard()
            {
                Console.WriteLine(" ------------------------- ");
                Console.Write(" | ");
                for (int i=0; i<81; i++)
                {
                    Console.Write(sudokuBoard[i] + " ");
                    if((i+1)%3 == 0)
                    {
                        Console.Write("| ");
                    }
                    if ((i + 1) % 9 == 0)
                    {
                        Console.WriteLine();
                        if ((i + 1) % 27 == 0)
                        {
                            Console.WriteLine(" ------------------------- ");
                        }
                        if(i<80) Console.Write(" | ");
                    }
                }
            }
            //function removing digits from sudoku board
            void digitRemove(int digitsToRemove = 10, int indexToRemove = 0, int removedDigit = 0)
            {
                //searching for index not already removed
                indexToRemove = rnd.Next(0, 81);
                while (sudokuBoard[indexToRemove] == 0)
                {
                    indexToRemove = rnd.Next(0, 81);
                }
                removedDigit = sudokuBoard[indexToRemove];
                sudokuBoard[indexToRemove] = 0;
                //sudoku needs to have only one unique solution
                //so we remove a digit only if our sudoku will still have one unique solution
                if (numberOfSolutions() > 1)
                {
                    sudokuBoard[indexToRemove] = removedDigit;
                    digitRemove(digitsToRemove);
                } 
                else if(digitsToRemove > 1)
                {
                    takenNumbers[indexToRemove, 0] = 0;
                    takenNumbers[indexToRemove, 1] = 0;
                    digitsToRemove--;
                    digitRemove(digitsToRemove);
                }
            }
            //function checking number of sudoku solutions
            //we iterate on every possible number combinations from 1 to 9 for every removed place in our sudoku board
            int numberOfSolutions(int solutions = 0, int index = 0, int number=1)
            {
                if(index == 0)
                {
                    while (sudokuBoard[index] != 0)
                    {
                        index++;
                    }
                }
                int nextIndex = index;
                while (number != 0)
                {
                    if (number > 9)
                    {
                        number = 0;
                        break;
                    }
                    if(checkIfFits(index, number) == true)
                    {
                        sudokuBoard[index] = number;
                        while (sudokuBoard[nextIndex] != 0)
                        {
                            nextIndex++;
                            //our index is greater than 80 it means that we reached the end and one more solution exists
                            if(nextIndex > 80)
                            {
                                sudokuBoard[index] = 0;
                                solutions++;
                                return solutions;
                            }
                        }
                        //when number fits the sudoku then we check another number
                        solutions = numberOfSolutions(solutions, nextIndex);
                    }
                    number++;
                }
                sudokuBoard[index] = 0;
                return solutions;
            }
            //writing single digit to the sudoku
            void writeSingleDigit(int number, int x, int y)
            {
                sudokuBoard[(x-1)+(9*(y-1))] = number;
            }
            //function that allows you to pick difficulty 
            int difficultyCheck()
            {
                int x = 0;
                string check = "";
                Console.Clear();
                Console.WriteLine("Hi! :)");
                Console.WriteLine("How to play:");
                Console.WriteLine("1. First pick a number that you want to write inside sudoku board");
                Console.WriteLine("2. Enter a 'x' coordinate - 'x' coordinate is a horizontal index of a place on the board where");
                Console.WriteLine("you want to write your number, this is a number starting from 1 farthest to the left");
                Console.WriteLine("and ending on 9 farthest to the right side (numbers from 1-9, from left to right)");
                Console.WriteLine("3. Enter a 'y' coordinate - 'y' coordinate is vertical index of a place on the board where");
                Console.WriteLine("you want to write your number, this is a number starting from 1 on the very top");
                Console.WriteLine("and ending on 9 at the very bottom (numbers from 1-9, from up to bottom)");
                Console.WriteLine("4. When you enter a number, x and y coordinates then this number is filled into the sudoku board");
                Console.WriteLine("5. There is a mistakes counter, if you made a mistake it is increasing but wrong number is not filled");
                Console.WriteLine("into the board");
                Console.WriteLine("6. If the sudoku is completed - it means that every number is correctly filled into the board - then you won!");
                Console.WriteLine("7. Number is filled correctly when numbers do not repeat horizontally, vertically and inside 3x3 boxes");
                Console.WriteLine("");
                Console.WriteLine("Choose your difficulty: ");
                Console.WriteLine("1 - Easy");
                Console.WriteLine("2 - Medium");
                Console.WriteLine("3 - Hard");
                check = Console.ReadLine();
                //validating if given value is a number and in range 1-9
                while (validateGivenNumber(check, "true") == false)
                {
                    check = Console.ReadLine();
                }
                while (x != 1 & x!= 2 & x!= 3)
                {
                    x = Convert.ToInt32(check);
                }
                if (x == 2) return 30;
                else if (x == 3) return 50;
                else return 10;
            }
            //number of not filled spaces inside sudoku board
            int checkEmpty()
            {
                int emptySpaces = 0;
                for (int i = 0; i < 81; i++)
                {
                    if (sudokuBoard[i] == 0) emptySpaces++;
                }
                return emptySpaces;
            }
            //function catching errors in values given by player
            bool validateGivenNumber(string number, string difficulty = "false")
            {
                int x = 0;
                if (!Int32.TryParse(number, out x))
                {
                    Console.WriteLine("Not a valid number, try again.");
                    return false;
                }
                if (difficulty == "false") 
                {
                    if (x < 1 || x > 9)
                    {
                        Console.WriteLine("Number is outside of range, please enter a number between 1-9");
                        return false;
                    }
                }
                else if(difficulty == "true")
                {
                    if (x < 1 || x > 3)
                    {
                        Console.WriteLine("Number is outside of range, please enter a number between 1-3");
                        return false;
                    }
                }
                return true;
            }
            //one function that operates other functions
            void game()
            {
                bool win = false;
                int failCount = 0;
                string check = "";
                int x = 0, y = 0, number = 0;
                //reset sudoku board -> generate new -> remove digits (filling spaces with 0s) depending on difficulty
                resetSudoku();
                generateSudokuBoard();
                digitRemove(difficultyCheck());
                while(win == false)
                {
                    Console.Clear();
                    showTheBoard();
                    Console.WriteLine("Mistakes made: {0}", failCount);
                    Console.WriteLine();
                    //validating if given values are numbers in range 1-9, storing values into variables 
                    Console.WriteLine("Choose a number to fill inside the board: ");
                    check = Console.ReadLine();
                    while(validateGivenNumber(check) == false)
                    {
                        check = Console.ReadLine();
                    }
                    number = Convert.ToInt32(check);
                    Console.WriteLine("Enter the coordinate x: ");
                    check = Console.ReadLine();
                    while (validateGivenNumber(check) == false)
                    {
                        check = Console.ReadLine();
                    }
                    x = Convert.ToInt32(check);
                    Console.WriteLine("Enter the coordinate y: ");
                    check = Console.ReadLine();
                    while (validateGivenNumber(check) == false)
                    {
                        check = Console.ReadLine();
                    }
                    y = Convert.ToInt32(check);
                    //if number fits and there is no number at that position then we can add it to the board
                    if (checkIfFits((x-1) + (9 * (y - 1)), number) && sudokuBoard[(x - 1) + (9 * (y - 1))] == 0)
                    {
                        writeSingleDigit(number, x, y);
                    }
                    else
                    {
                        Console.WriteLine("Error - number {0} does not fit into given coordinates", number);
                        Console.WriteLine("Enter any key to continue");
                        Console.ReadKey();
                        failCount++;
                    }
                    //if there is no empty space it means that we won!
                    if (checkEmpty() == 0)
                    {
                        win = true;
                        Console.Clear();
                        showTheBoard();
                        Console.WriteLine("Mistakes made: {0}", failCount);
                        Console.WriteLine("//////////////");
                        Console.WriteLine("//////////////");
                        Console.WriteLine("");
                        Console.WriteLine("YOU WON!");
                        Console.WriteLine("");
                        Console.WriteLine("//////////////");
                        Console.WriteLine("//////////////");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("Enter any key to start a new game!");
                        Console.ReadKey();
                    }
                }
                game();
            }
            game();
        }
    }
}