using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;


namespace macierze
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Mother MOTHER;
        private TextBox[][] gridUserInputs;

        //funkcja, ktora odpowiada za klikniecie przycisku Browse, obsluga plikow
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = "Text documents (.txt;.xlsx;.csv)|*.txt;*.xlsx;*.csv";

            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                fileInfoLabel.Content = openFileDlg.FileName;
            }


            string format = openFileDlg.FileName.Substring(openFileDlg.FileName.IndexOf("."));
            this.MOTHER = new Mother(getMotherFromFile(openFileDlg.FileName, format));
            generateGridWithMother(this.MOTHER.mother);

            fileInfoLabel.BorderBrush = Brushes.Green;
        }

        //funkcja, ktora generuje grida z gotowego macierza podanego w argumencie
        private void generateGridWithMother(string[][] mother)
        {
            Grid grid = new Grid();

            for (int i = 0; i < mother.Length; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                grid.RowDefinitions.Add(rowDef);

                ColumnDefinition colDef = new ColumnDefinition();
                grid.ColumnDefinitions.Add(colDef);

                
            }

            for (int i = 0; i < mother.Length; i++)
            {
                for (int j = 0; j < mother[i].Length; j++)
                {
                    Label output = new Label();
                    output.Content = mother[i][j];
                    output.HorizontalContentAlignment = HorizontalAlignment.Center;
                    output.VerticalContentAlignment = VerticalAlignment.Center;

                    Grid.SetRow(output, i);
                    Grid.SetColumn(output, j);

                    grid.Children.Add(output);
                }
            }

            
            motherContainer.Content = grid;
        }

        //funkcja, ktora generuje grida z pustymi TextBox do wpisania dla uzytkownika
        private void generateGridWithInputs(int size)
        {
            Grid grid = new Grid();
            grid.ShowGridLines = true;
            TextBox[][] array = new TextBox[size][]; 

            for (int i = 0; i < size; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                grid.RowDefinitions.Add(rowDef);

                ColumnDefinition colDef = new ColumnDefinition();
                grid.ColumnDefinitions.Add(colDef);

                array[i] = new TextBox[size];


            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    TextBox input = new TextBox();
                    input.Text = "";
                    input.HorizontalContentAlignment = HorizontalAlignment.Center;
                    input.VerticalContentAlignment = VerticalAlignment.Center;

                    Grid.SetRow(input, i);
                    Grid.SetColumn(input, j);

                    array[i][j] = input;

                    grid.Children.Add(input);
                }
            }

            this.gridUserInputs = array;
            motherContainer.Content = grid;
            saveButton.Visibility = Visibility.Visible;
        }

        private string[][] getMotherFromFile(string path, string format)
        {
            
            if(format == ".txt")
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                string[][] mother = new string[lines.Length][];

                for (int i = 0; i < mother.Length; i++)
                    mother[i] = lines[i].Split(" ");

                return mother;

            }
            else if(format == ".csv")
            {
                
                using (var reader = new StreamReader(path))
                {
                    var content = reader.ReadToEnd();
                    string[] lines = content.Split("\n");
                    int size = lines[0].Split(";").Length;
                    string[][] mother = new string[size][];

                    for (int i = 0; i < mother.Length; i++)
                        mother[i] = lines[i].Split(";");

                    return mother;

                }


            }
            else
            {
                string[][] empty = new string[3][];
                return empty;
            }

            







        }

        

       


        private void motherSizeInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void motherSizeInput_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        //funkcja, ktora reaguje na zmiane wielkosci macierza w comboboxie
        private void sizeOfMother_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)sizeOfMother_ComboBox.SelectedItem;
            int size = Convert.ToInt32(typeItem.Content);

            generateGridWithInputs(size);
        }


        //funkcja, ktora pobiera grida od uzytkownika i zapisuje go do maceirza globalnego MOTHER 
        private void saveMotherFromUserInput(object sender, RoutedEventArgs e)
        {
            Grid loadedGrid = (Grid)motherContainer.Content;

            ComboBoxItem typeItem = (ComboBoxItem)sizeOfMother_ComboBox.SelectedItem;
            int sizeFromUser = Convert.ToInt32(typeItem.Content);

            int size = loadedGrid.Children.Count / sizeFromUser;
            string[][] mother = new string[size][];

            
            
            for (int i = 0; i < mother.Length; i++)
            {
                mother[i] = new string[size];
                for (int j = 0; j < mother[i].Length; j++)
                    mother[i][j] = this.gridUserInputs[i][j].Text; 
            }

            //przypisanie do globalnego macierza
            this.MOTHER = new Mother(mother);
        }

        //funkcja, ktora po nacisnieciu przycisku oblicza przekatne itp.
        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int row = Convert.ToInt32(rowInput.Text);
                int column = Convert.ToInt32(ColumnInput.Text);

                if(row > this.MOTHER.size && column > this.MOTHER.size)
                {
                    resultContainer.Content = "Column and row is out of range!";
                }
                else if (row > this.MOTHER.size)
                {
                    resultContainer.Content = "Row is out of range!";
                }else if(column > this.MOTHER.size)
                {
                    resultContainer.Content = "Column is out of range!";
                }
                else
                {
                    string result = $"Sum of main diagonal: {this.MOTHER.getSumOfMainDiagonal()}\n" +
                    $"Sum of {row} row: {this.MOTHER.getSumOfRow(row)}\n" +
                    $"Sum of {column} column: {this.MOTHER.getSumOfColumn(column)}";

                    resultContainer.Content = result;
                }

            } catch(Exception exec)
            {
                resultContainer.Content = "You need to pass mother from file or\n manually.";
            }
        }

    }
}
