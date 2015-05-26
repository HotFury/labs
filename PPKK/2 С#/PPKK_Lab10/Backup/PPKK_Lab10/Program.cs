// Лекционный курс: ППКК | Лектор: Евгений Александрович Лобода (доц. каф. "Выч.техн.и прог." НТУ"ХПИ")
// ЗАДАНИЕ: вывести на экран и в файл перечень графических режимов, поддерживаемых текущим гафич. адаптером.
// Программу писать на языке C# для среды NetFramework v.2.0 (средствами компилятора MSVS2005)
// =====================================================================================================================
// Пример выполнения задания:
// =====================================================================================================================
using System;
using System.IO;  //для записи в файл
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

class TestForm : Form
{
    static void Main()
    {
        StreamWriter ToFile = new StreamWriter("MyGAdapterModes.doc", true);//true == Append
        foreach (AdapterInformation adapterInfo in Manager.Adapters)
           {Console.WriteLine("Adapter: \n\tDriver: {0}\n\t" +
                              "Description: {1}\n\tDeviceName: {2}\n",
                               adapterInfo.Information.DriverName,
                               adapterInfo.Information.Description,
                               adapterInfo.Information.DeviceName);
            ToFile.WriteLine("Adapter: \n\tDriver: {0}\n\t" +
                             "Description: {1}\n\tDeviceName: {2}\n",
                              adapterInfo.Information.DriverName,
                              adapterInfo.Information.Description,
                              adapterInfo.Information.DeviceName);
           }
        foreach (DisplayMode mode in
                 Manager.Adapters[0].SupportedDisplayModes)
               //Manager.Adapters[0].SupportedDisplayModes[Format.X8R8G8B8])//R5G6B5,X1R5G5B5,X4R4G4B4 
           {Console.WriteLine("{0}x{1}, {2} Hz, Format: {3}",
                    mode.Width, mode.Height, mode.RefreshRate, mode.Format);
            ToFile.WriteLine("{0}x{1}, {2} Hz, Format: {3}",
                    mode.Width, mode.Height, mode.RefreshRate, mode.Format);
           }
        ToFile.Close();
        Console.ReadLine();
    }
}
  }
}
