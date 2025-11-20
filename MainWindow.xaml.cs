using DroneCare.Objects;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace DroneCare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //global definition of drone list and queues
        internal static Queue<Drone> regularService = new Queue<Drone>();
        internal static Queue<Drone> expressService = new Queue<Drone>();
        internal static List<Drone> finishedList = new List<Drone>();

        //runs main logic
        public MainWindow()
        {
            InitializeComponent();
        }

        //only allows numbers to be entered in specified text boxes
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //adds a new servce item to the appropriate queue
        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            string clientName = TB_ClientName.Text;
            string droneModel = TB_DroneModel.Text;
            string serviceProblem = TB_ServiceProblem.Text;
            int serviceCost = int.Parse(TB_ServiceCost.Text);
            int serviceTag = int.Parse(UD_ServiceTag.Text);

            Drone newDrone = new Drone(clientName, droneModel, serviceProblem, serviceCost, serviceTag);

            if      (RBT_Regular.IsChecked == true) regularService.Enqueue(newDrone);
            else if (RBT_Express.IsChecked == true) expressService.Enqueue(newDrone);
        }
    }
}