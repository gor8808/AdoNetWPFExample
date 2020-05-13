using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace AdoNetWPFExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ConString;      
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetTableNames()
        {
            SqlConnection con = new SqlConnection(ConString);
            con.Open();
            DataTable t = con.GetSchema("Tables");
            foreach (DataRow row in t.Rows)
            {
                string tablename = (string)row[1] + "." + (string)row[2];
                TableTypeComboBox.Items.Add(tablename);
            }
            con.Close();
        }
        private void ShowTable(object sender, RoutedEventArgs e)
        {
            if (TableTypeComboBox.SelectedItem == null)
            {
                return;
            }
            SqlConnection con = new SqlConnection(ConString);
            string tableToShow = TableTypeComboBox.SelectedItem.ToString();
            string querystring = $"SELECT * FROM {tableToShow}";
            con.Open();
            SqlCommand cmd = new SqlCommand(querystring, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable(tableToShow);
            sda.Fill(dt);
            TableShowList.ItemsSource = dt.DefaultView;
            con.Close();
        }
        private void TableTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowTable(null, null);
        }
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            GetConStringPanel.Visibility = Visibility.Hidden;
            ConString = ConStringBox.Text;
            SetTableNames();
        }
    }
}
