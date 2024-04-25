using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace compoort_autodetection
{
    public partial class MainWindow : Window
    {
        public string Poort { get; set; }
        private SerialPort serialPort;
        bool geenComPoort = true;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSerialPort();
        }
        private void InitializeSerialPort()
        {
            // zoek alle mogelijke poorten
            String[] Ports = SerialPort.GetPortNames();

            while (geenComPoort == true) // blijft zoeken achter COM-poort indien geen gevonden
            {
                foreach (string poort in Ports) // overloopt alle gevonden COM-poorts
                {
                    SerialPort port = new SerialPort(poort);
                    try // opent poort en zoekt achter data
                    {
                        port.Open();
                        String indata = port.ReadExisting();
                        string trimmedData = indata.Trim();
                        if (trimmedData.IndexOf("1") >= 0) // zoeken
                        {
                            System.Windows.MessageBox.Show($"Accessing port {poort}");
                            geenComPoort = false;
                            break;
                        }
                        if (indata.Trim() != "1") // indien er niets wordt ontvangen, sluit de poort opnieuw
                        {
                            port.Close();
                        }
                    }
                    catch (Exception ex) // meld probleem bij een poort
                    {
                        System.Windows.MessageBox.Show($"Error accessing port {poort}: {ex.Message}");
                    }
                }
            }
        }
    }
}