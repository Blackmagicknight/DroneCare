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
            //uses regex to only allow numbers
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //returns the value if the priority radio group
        private string GetServicePriority()
        {
            if      (RBT_Regular.IsChecked == true) return "Regular";
            else if (RBT_Express.IsChecked == true) return "Express";

            else return "Unknown";
        }

        //adds a new servce item to the appropriate queue
        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            //retrieves user input from text boxes  
            string clientName = TB_ClientName.Text;
            string droneModel = TB_DroneModel.Text;
            string serviceProblem = TB_ServiceProblem.Text;
            int serviceCost = int.Parse(TB_ServiceCost.Text);
            int serviceTag = int.Parse(UD_ServiceTag.Text);

            //creates new drone object
            Drone newDrone = new Drone(clientName, droneModel, serviceProblem, serviceCost, serviceTag);

            //adds new drone to the appropriate queue based on service type selected
            switch (GetServicePriority())
            {
                case "Regular":
                    regularService.Enqueue(newDrone);
                    break;
                case "Express":
                    //adds 15% to cost for express service
                    newDrone.serviceCost *= 1.15;
                    expressService.Enqueue(newDrone);
                    break;
            }
        }
    }
}