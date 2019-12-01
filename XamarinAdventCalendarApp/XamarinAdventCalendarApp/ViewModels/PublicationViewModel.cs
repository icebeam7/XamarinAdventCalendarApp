using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Essentials;

using XamarinAdventCalendarApp.Models;
using XamarinAdventCalendarApp.Helpers;
using XamarinAdventCalendarApp.Services;

namespace XamarinAdventCalendarApp.ViewModels
{
    public class PublicationViewModel : BaseViewModel
    {
        private ObservableCollection<Publication> _publications;

        public ObservableCollection<Publication> Publications
        {
            get => _publications;
            set
            {
                _publications = value;
                OnPropertyChanged();
            }
        }

        public ICommand ExportToExcelCommand { private set; get; }
        private ExcelService excelService;

        public PublicationViewModel()
        {
            Task.Run(async () => await LoadPublications());
            ExportToExcelCommand = new Command(async () => await ExportToExcel());
            excelService = new ExcelService();
        }

        async Task ExportToExcel()
        {
            var fileName = $"{Guid.NewGuid()}.xlsx";
            string filePath = excelService.GenerateExcel(fileName);

            var header = new List<string>() { "ID", "Date", "Publication Title", "Author" };

            var data = new ExcelData();
            data.Headers = header;

            foreach (var publication in Publications)
            {
                var row = new List<string>()
                {
                    publication.Id.ToString(),
                    publication.ScheduledDate,
                    publication.Title,
                    publication.Author
                };

                data.Values.Add(row);
            }

            excelService.InsertDataIntoSheet(filePath, "Publications", data);

            await Launcher.OpenAsync(new OpenFileRequest()
            {
                File = new ReadOnlyFile(filePath)
            });
        }

        async Task LoadPublications()
        {
            var data = await DataService<Publication>.GetData();

            foreach (var item in data)
                item.Colors = ColorPalette.GetNextColorValues();

            Publications = new ObservableCollection<Publication>(data);
        }
    }
}
