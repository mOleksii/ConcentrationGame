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
        int firstFlipped, secondFlipped, thirdFlipped;
        int pairsFound = 0;

        Random random = new Random();
        Grid firstFlippedGrid = new Grid();
        Grid secondFlippedGrid = new Grid();
        Grid thirdFlippedGrid = new Grid();
        List<Button> allButtonCards = new List<Button>();
        List<Image> imagesUsed = new List<Image>();

        public MainWindow()
        {
            InitializeComponent();
            
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
            pairsFound = 0;
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

        }

        private async void CardButton_Click(object sender, RoutedEventArgs e)
        {
            int index_counter = 0;

            foreach (Button button in gameUniGrid.Children)
            {
                if (button == (Button)sender)  //and enabled?
                {
                    cardsFlippedCounter++;
                    FlipCard(index_counter);

                    break;
                }

                index_counter++;
            }

            if (cardsFlippedCounter == 2 && matchRule == 2)
            {
                await Task.Delay(1000);
                CheckIfFlippedMatch();
            }
            else if (cardsFlippedCounter == 3 && matchRule == 3)
            {
                await Task.Delay(1000);
                CheckIfFlippedMatch();
            }


            if (pairsFound == numberOfCards / matchRule)
                MessageBox.Show("You won!");

        }

        private void FlipCard(int buttonIndex)
        {
            if(cardsFlippedCounter == 1)
            {
                firstFlipped = buttonIndex;

                firstFlippedGrid = allButtonCards[firstFlipped].Content as Grid;
                firstFlippedGrid.Children[0].Visibility = Visibility.Hidden;
                firstFlippedGrid.Children[1].Visibility = Visibility.Visible;
                allButtonCards[firstFlipped].IsEnabled = false;
            }
            else if(cardsFlippedCounter == 2)
            {
                secondFlipped = buttonIndex;

                secondFlippedGrid = allButtonCards[secondFlipped].Content as Grid;
                secondFlippedGrid.Children[0].Visibility = Visibility.Hidden;
                secondFlippedGrid.Children[1].Visibility = Visibility.Visible;
                allButtonCards[secondFlipped].IsEnabled = false;

            }
            else if(cardsFlippedCounter == 3)
            {
                thirdFlipped = buttonIndex;

                thirdFlippedGrid = allButtonCards[thirdFlipped].Content as Grid;
                thirdFlippedGrid.Children[0].Visibility = Visibility.Hidden;
                thirdFlippedGrid.Children[1].Visibility = Visibility.Visible;
                allButtonCards[thirdFlipped].IsEnabled = false;
            }    
        }

        private void CheckIfFlippedMatch()
        {
            if (matchRule == 2)
            {
                if (imagesUsed[firstFlipped].Tag == imagesUsed[secondFlipped].Tag)
                {
                    pairsFound++;
                    cardsFlippedCounter = 0;
                }
                else
                {
                    firstFlippedGrid.Children[0].Visibility = Visibility.Visible;
                    firstFlippedGrid.Children[1].Visibility = Visibility.Hidden;
                    allButtonCards[firstFlipped].IsEnabled = true;

                    secondFlippedGrid.Children[0].Visibility = Visibility.Visible;
                    secondFlippedGrid.Children[1].Visibility = Visibility.Hidden;
                    allButtonCards[secondFlipped].IsEnabled = true;

                    cardsFlippedCounter = 0;
                }
            }
            else if (matchRule == 3)
            {
                if (imagesUsed[firstFlipped].Tag == imagesUsed[secondFlipped].Tag && imagesUsed[thirdFlipped].Tag == imagesUsed[firstFlipped].Tag)
                {
                    pairsFound++;
                    cardsFlippedCounter = 0;
                }
                else
                {
                    firstFlippedGrid.Children[0].Visibility = Visibility.Visible;
                    firstFlippedGrid.Children[1].Visibility = Visibility.Hidden;
                    allButtonCards[firstFlipped].IsEnabled = true;

                    secondFlippedGrid.Children[0].Visibility = Visibility.Visible;
                    secondFlippedGrid.Children[1].Visibility = Visibility.Hidden;
                    allButtonCards[secondFlipped].IsEnabled = true;


                    thirdFlippedGrid.Children[0].Visibility = Visibility.Visible;
                    thirdFlippedGrid.Children[1].Visibility = Visibility.Hidden;
                    allButtonCards[thirdFlipped].IsEnabled = true;

                    cardsFlippedCounter = 0;
                }
            }

        }

        private void CreateButtons()
        {
            for (int i = 0; i < numberOfCards; i++)
            { 
                Grid newGrid = new Grid();
                Button cardButton = new Button();
                Label label = new Label();

                label.Content = "Flip Me!";
                label.FontWeight = FontWeights.Bold;
                label.FontSize = 14;
                label.Background = Brushes.Purple; // or Transparent
                label.Opacity = 0.8;
                label.HorizontalAlignment = HorizontalAlignment.Stretch;
                label.VerticalAlignment = VerticalAlignment.Stretch;
                label.Visibility = Visibility.Visible;
                label.Tag = i.ToString();

                newGrid.Children.Insert(0,label);
                imagesUsed[i].Stretch = Stretch.UniformToFill;
                imagesUsed[i].HorizontalAlignment = HorizontalAlignment.Center;

                imagesUsed[i].Visibility = Visibility.Hidden;
                newGrid.Children.Insert(1,imagesUsed[i]);

                cardButton.Content = newGrid;
                cardButton.Click += CardButton_Click;

                allButtonCards.Add(cardButton);
                gameUniGrid.Children.Add(cardButton);
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
