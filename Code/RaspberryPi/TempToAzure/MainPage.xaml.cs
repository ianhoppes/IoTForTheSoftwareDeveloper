using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TempToAzure
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Create a constant for pressure at sea level. 
        //This is based on your local sea level pressure (Unit: Hectopascal)
        const float SeaLevelPressure = 1013.25f;
        string RaspberryPiConnectionString = "<DeviceConnectionString>";
        BMP280 BMP280;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs navArgs)
        {
            Debug.WriteLine("MainPage::OnNavigatedTo");

            try
            {
                //Create a new object for our barometric sensor class
                BMP280 = new BMP280();
                //Initialize the sensor
                await BMP280.Initialize();

                float tempC = 0;
                float tempF = 0;
                float pressure = 0;
                float altitude = 0;

                while (true)
                {
                    tempC = await BMP280.ReadTemperature();
                    tempF = (float)ConvertTemp.ConvertCelsiusToFahrenheit(tempC);
                    pressure = await BMP280.ReadPreasure();
                    altitude = await BMP280.ReadAltitude(SeaLevelPressure);

                    Debug.WriteLine("Temperature: " + tempC.ToString() + " deg C");
                    Debug.WriteLine("Temperature: " + tempF.ToString() + " deg F");
                    Debug.WriteLine("Pressure: " + pressure.ToString() + " Pa");
                    Debug.WriteLine("Altitude: " + altitude.ToString() + " m");

                    await SendDataToAzure(tempF.ToString());

                    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task SendDataToAzure(string message)
        {
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(RaspberryPiConnectionString, TransportType.Http1);
            var msg = new Message(Encoding.UTF8.GetBytes(message));
            await deviceClient.SendEventAsync(msg);
        }
    }
}
