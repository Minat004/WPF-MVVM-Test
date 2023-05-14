using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using DbwViewer.Models;

namespace DbwViewer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        AreaData = new ObservableCollection<Area>
        {
            new()
            {
                Id = 1,
                Name = "Ceny",
                PrecedentElementId = 727,
                LevelId = 1,
                LevelName = "Dziedzina",
                IsChangeable = false
            },
            new()
            {
                Id = 4,
                Name = "Budownictwo",
                PrecedentElementId = 727,
                LevelId = 1,
                LevelName = "Dziedzina",
                IsChangeable = false
            }
        };
    }

    public string AppHeader { get; set; } = "GUS Dziedzinowe Bazy Wiedzy Viewer";

    public ObservableCollection<Area> AreaData { get; set; }
}