using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members

        /// <summary>
        /// Holds the current results of cells in the active game
        /// </summary>
        private MarkType[] mResults;

        /// <summary>
        /// True if it is player 1's turn (X)
        /// </summary>
        private bool mPlayer1Turn;
        /// <summary>
        /// True if the game has ended
        /// </summary>
        private bool mGameEnded;
        #endregion
        
        #region Constructor

        /// <summary>
        /// Default contructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }
        #endregion

        #region
        /// <summary>
        /// Start a new game and clear all values back to start
        /// </summary>
        private void NewGame()
        {
            // Create a new blank array of free cells
            mResults = new MarkType[9];

            for (var i = 0; i< mResults.Length; i++)
            {
                mResults[i] = MarkType.Free;
            }

            // Make sure Player 1 starts the game
            mPlayer1Turn = true;

            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = string.Empty;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Blue;
            });

            // Make sure that the game hasn't finished
            mGameEnded = false;

        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The button was clicked</param>
        /// <param name="e">The events of the click</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Start a new game on the click after it is finished
            if (mGameEnded)
            {
                NewGame();
                return;
            }

            // Cast the sender to button
            var button = (Button)sender;

            // Find the buttons position in the array
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);

            var index = (row * 3 )+ column;

            // Don't do anything if the cell already has a value in it
            if (mResults[index] != MarkType.Free)
                return;

            // Set the cel value based on which palyers turn it is 
            mResults[index] = mPlayer1Turn ? MarkType.Cross : MarkType.Nought;

            //Set button text to the result
            button.Content = mPlayer1Turn ? "X" : "O";

            // Change noughts to green
            if (!mPlayer1Turn)
            {
                button.Foreground = Brushes.Red;
            }
            // Toggle the players turns
            mPlayer1Turn ^= true;

            //Check for a winner

            CheckforWinner();
        }

        private void CheckforWinner()
        {
            // Check horizontal wins
            for (int row = 0; row <6; row += 3)
            {
                if (mResults[row] == MarkType.Free)
                {
                    row += 3;
                    continue;
                }
                if (mResults[row] == mResults[row +1 ] && mResults[row + 1] == mResults[row + 2])
                {
                    mGameEnded = true;
                    if (row == 0)
                        Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Green;
                    if (row == 3)
                        Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Green;
                    if (row == 6)
                        Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.Green;
                }
            }

            //Check diagonal wins
            for ( int column = 0; column < 3; column ++)
            {
                if (mResults[column] == MarkType.Free)
                {
                    column += 1;
                    continue;
                }
                if (mResults[column] == mResults[column + 3] && mResults[column + 3] == mResults[column + 6])
                {
                    mGameEnded = true;
                    if (column == 0)
                        Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Green;
                    if (column == 1)
                        Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.Green;
                    if (column == 2)
                        Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.Green;
                }
            }

            // Diagonal 1
            if (mResults[0] != MarkType.Free && mResults[0] == mResults[4] && mResults[4] == mResults[8])
            {
                mGameEnded = true;
                Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Green;
            }
            // Diagonal 2
            if (mResults[2] != MarkType.Free && mResults[2] == mResults[4] && mResults[4] == mResults[6])
            {
                mGameEnded = true;
                Button0_2.Background = Button1_1.Background = Button2_0.Background = Brushes.Green;
            }

            //// if all places ocupied
            //bool all_occupied = true;
            //for (int i =0; i< 9; i++)
            //{
            //    if (mResults[i]== MarkType.Free)
            //    {
            //        all_occupied = false;
            //        break;
            //    }
            //}

            //if (all_occupied)
            //{
            //    mGameEnded = true;
            //    Container.Background = Brushes.Yellow;
            //}

            // Check for no winner and full board
            if (!mResults.Any(f => f == MarkType.Free))
            {
                //Game ended

                //Turn all cells orange
                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    button.Background = Brushes.Orange;
                });
            }
        }
    }
}
