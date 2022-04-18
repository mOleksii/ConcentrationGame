using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConcentrationGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int NUMBER_OF_DIFFERENT_CARDS = 17;
        int gameDifficulty = 0; // 1 - easy, 2 - medium, 3 - hard
        int matchRule = 0;
        int numberOfCards = 0;
        int cardsFlippedCounter = 0;
        int firstFlipped, secondFlipped;
        int pairsFound = 0;

        Random random = new Random();
        List<Button> allButtonCards = new List<Button>();
        List<Image> imagesUsed = new List<Image>();
        Label label = new Label();

        public MainWindow()
        {
            InitializeComponent();
            label.Content = "Flip Me!";
            label.FontWeight = FontWeights.Bold;
            label.FontSize = 14;
            label.Background = Brushes.Purple; // or Transparent
            label.Opacity = 0.8;
            label.Visibility = Visibility.Visible;
        }

        private void DifficultyLevelButtons_Checked(object sender, RoutedEventArgs e)
        {
            gameUniGrid.Children.Clear();

            if (easyButton.IsChecked == true)
                gameDifficulty = 1;
            else if (mediumButton.IsChecked == true)
                gameDifficulty = 2;
            else if (hardButton.IsChecked == true)
                gameDifficulty = 3;

            if (matchRule != 0 && gameDifficulty != 0)
                GameStart();

        }

        private void MatchRuleButtons_Checked(object sender, RoutedEventArgs e)
        {
            gameUniGrid.Children.Clear();

            if (matchTwoButton.IsChecked == true)
                matchRule = 2;
            else if (matchThreeButton.IsChecked== true)
                matchRule = 3;

            if (matchRule != 0 && gameDifficulty != 0)
                GameStart();
        }

        private void GameStart()
        {
            imagesUsed.Clear();
            allButtonCards.Clear();
            GameBoardSetup();
        }

        private void GameBoardSetup()
        {
            imagesUsed.Clear();
            numberOfCards = 0;

            SetGameDifficultyAndRule();

            LoadNeededImages();

            CreateButtons();

            //allCards.Clear();
                   
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            int index_counter = 0;

            foreach (Button button in gameUniGrid.Children)
            {
                if(button == (Button)sender)  //and enabled?
                {
                    cardsFlippedCounter++;
                    CheckIfFlippedMatch(index_counter); //maybe change to picture first, and then check?
                    
                        
                    break;
                }

                index_counter++;
            }

            if (pairsFound == numberOfCards / matchRule)
                MessageBox.Show("You won!");

        }

        private void CheckIfFlippedMatch(int buttonIndex)
        {
            if(cardsFlippedCounter == 1)
            {
                firstFlipped = buttonIndex;

                Grid n = allButtonCards[firstFlipped].Content as Grid;
                n.Children[0].Visibility = Visibility.Visible;
                allButtonCards[firstFlipped].Content = imagesUsed[firstFlipped];
               // allButtonCards[firstFlipped].Content = Stretch.UniformToFill;
                allButtonCards[firstFlipped].IsEnabled = false;

                gameUniGrid.Children[firstFlipped] = allButtonCards[firstFlipped];
                    
            }
            else
            {
                secondFlipped = buttonIndex;

                allButtonCards[secondFlipped].Content = imagesUsed[secondFlipped];
                allButtonCards[secondFlipped].IsEnabled = false;
                gameUniGrid.Children[secondFlipped] = allButtonCards[secondFlipped];

                if (imagesUsed[firstFlipped].Tag == imagesUsed[secondFlipped].Tag)
                {
                    pairsFound++;
                    cardsFlippedCounter = 0;
                }
                else
                {
                    //use a label instead?
                    //or make a function that changes specific button to an OG button (again maybe label or fix the content one)
                    //Check why the image is not showing

                    /*allButtonCards[firstFlipped].Content = "Flip Me!";
                    allButtonCards[firstFlipped].FontWeight = FontWeights.Bold;
                    allButtonCards[firstFlipped].FontSize = 14;
                    allButtonCards[firstFlipped].Background = Brushes.Purple; // or Transparent
                    allButtonCards[firstFlipped].Opacity = 0.8;
                    allButtonCards[firstFlipped].IsEnabled = true;

                    allButtonCards[secondFlipped].Content = "Flip Me!";
                    allButtonCards[secondFlipped].FontWeight = FontWeights.Bold;
                    allButtonCards[secondFlipped].FontSize = 14;
                    allButtonCards[secondFlipped].Background = Brushes.Purple; // or Transparent
                    allButtonCards[secondFlipped].Opacity = 0.8;
                    allButtonCards[secondFlipped].IsEnabled = true;
                    */
                    allButtonCards[firstFlipped].IsEnabled = true;
                    imagesUsed[firstFlipped].Visibility = Visibility.Visible;
                    allButtonCards[secondFlipped].IsEnabled = true;
                    allButtonCards[secondFlipped].Visibility = Visibility.Visible;
                    gameUniGrid.Children[firstFlipped] = allButtonCards[firstFlipped];
                    gameUniGrid.Children[secondFlipped] = allButtonCards[secondFlipped];
                    cardsFlippedCounter = 0;
                }

            }

        }
     
        private void CreateButtons()
        {
            /* for (int i = 0; i < numberOfCards; i++)
             {
                 Button cardButton = new Button();

                 cardButton.Content = "Flip Me!";
                 cardButton.FontWeight = FontWeights.Bold;
                 cardButton.FontSize = 14;
                 cardButton.Background = Brushes.Purple; // or Transparent
                 cardButton.Opacity = 0.8;
                 cardButton.Click += CardButton_Click;
                 allButtonCards.Add(cardButton);

                 gameUniGrid.Children.Add(cardButton);
             }
            */

            for (int i = 0; i < numberOfCards; i++)
            { 
                Grid newGrid = new Grid();
                Button cardButton = new Button();

                newGrid.Children.Insert(0,label);
                imagesUsed[i].Stretch = Stretch.UniformToFill;
                imagesUsed[i].Visibility = Visibility.Hidden;
                newGrid.Children.Insert(0,imagesUsed[i]);

                cardButton.Content = newGrid;
                cardButton.Click += CardButton_Click;

                allButtonCards.Add(cardButton);
                gameUniGrid.Children.Add(cardButton);
                //newGrid.Children.Clear();
            }
        }

        private void SetGameDifficultyAndRule()
        {
            switch (gameDifficulty)
            {
                case 1:
                    numberOfCards = 5;  //10 or 15
                    break;
                case 2:
                    numberOfCards = 11; //22 or 33
                    break;
                case 3:
                    numberOfCards = 16; //32 or 48
                    break;
                default:
                    numberOfCards = 5;
                    break;
            }

            numberOfCards *= matchRule;
        }

        private void LoadNeededImages()
        {
            for (int i = 0; i < numberOfCards / matchRule; i++)
            {
                for (int j = 1; j <= matchRule; ++j)
                {
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri($"/Images/{i + 1}.jpg", UriKind.RelativeOrAbsolute));
                    image.Tag = i.ToString();

                    int rnd_index = random.Next(0, imagesUsed.Count + 1);
                    imagesUsed.Insert(rnd_index, image);
                }
            }
        }

    }
}
