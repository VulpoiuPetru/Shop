using FirstProject.Helpers;
using FirstProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstProject.ViewModel
{
    public class MainWindowVM
    {
        private ICommand openWindowCommand;
        public ICommand OpenWindowCommand
        {
            get
            {
                if (openWindowCommand == null)
                {
                    openWindowCommand = new RelayCommand(OpenWindow);
                }
                return openWindowCommand;
            }
        }
        public void OpenWindow(object obj)
        {
            string nr = obj as string;
            switch (nr)
            {
                case "1":
                    AdaugareElemView adaugareElemView = new AdaugareElemView();
                    adaugareElemView.ShowDialog();
                    break;
                case "2":
                    ManageriereElemView manageriereElemView = new ManageriereElemView();
                    manageriereElemView.ShowDialog();
                    break;
                case "3":
                    ReceptiaView receptiaView = new ReceptiaView();
                    receptiaView.ShowDialog();
                    break;
                case "4":
                    VanzareView vanzareView = new VanzareView();
                    vanzareView.ShowDialog();
                    break;
                case "5":
                    GenerareQRCodeBarcodeView generareQRCodeBarcodeView = new GenerareQRCodeBarcodeView();
                    generareQRCodeBarcodeView.ShowDialog();
                    break;

            }
        }
    }
}
