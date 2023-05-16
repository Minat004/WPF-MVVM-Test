using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using DbwViewer.GenericCollections;
using DbwViewer.Models;
using Newtonsoft.Json;

namespace DbwViewer.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private static readonly Dictionary<string, string> areaPropertyDict = new()
    {
        { nameof(Area.Id), "Id"},
        { nameof(Area.Name), "Nazwa"},
        { nameof(Area.PrecedentElementId), "Id nadrzedny element"},
        { nameof(Area.LevelId), "Id poziom"},
        { nameof(Area.LevelName), "Nazwa poziomu"},
        { nameof(Area.IsChangeable), "Czy zmienne"}
    };
    
    public MainWindowViewModel()
    {
        AreaList = new ObservableCollection<Area>(GetAreas());

        var view = (ListCollectionView)CollectionViewSource.GetDefaultView(AreaList);

        AreaView = new ObservableCollectionView<Area>(view)
        {
            Filter = OnFilterArea,
        };

        AreaPropertyList = new ObservableCollection<string>(areaPropertyDict.Values.ToList());

        FilterSelectedItem = AreaPropertyList[1];
        OrderSelectedItem = AreaPropertyList[0];

        Directions = new ObservableCollection<ListSortDirection>
            (Enum.GetValues(typeof(ListSortDirection)).Cast<ListSortDirection>());

        DirectionSelectedItem = Directions[0];
    }
    
    [ObservableProperty]
    private string? appHeader = "GUS Dziedzinowe Bazy Wiedzy Viewer";
    
    [ObservableProperty]
    private string? searchText;

    [ObservableProperty] 
    private string orderSelectedItem;
    
    [ObservableProperty] 
    private string filterSelectedItem;
    
    [ObservableProperty] 
    private ListSortDirection directionSelectedItem;

    partial void OnOrderSelectedItemChanged(string? value)
    {
        var property = areaPropertyDict.FirstOrDefault(x => 
            x.Value == value).Key;
        
        OnOrderArea(property, DirectionSelectedItem);
    }

    partial void OnDirectionSelectedItemChanged(ListSortDirection value)
    {
        var property = areaPropertyDict.FirstOrDefault(x => 
            x.Value == OrderSelectedItem).Key;
        
        OnOrderArea(property, value);
    }

    public ObservableCollection<string> AreaPropertyList { get; set; }

    public ObservableCollection<ListSortDirection> Directions { get; set; }

    partial void OnSearchTextChanged(string? value)
    {
        AreaView.Refresh();
    }

    private ObservableCollection<Area> AreaList { get; set; }

    public ICollectionView<Area> AreaView { get; set; }

    private IEnumerable<Area> GetAreas()
    {
        using (var client = new HttpClient())
        {
            var response = client.GetAsync(@"https://api-dbw.stat.gov.pl/api/1.1.0/area/area-area?lang=pl");
            
            var responseBody = response.Result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<Area>>(responseBody.Result)!;
        }
    }

    private void OnOrderArea(string? property, ListSortDirection direction)
    {
        AreaView.SortDescriptions.Clear();
        AreaView.SortDescriptions.Add(new SortDescription(property!, direction));
        AreaView.Refresh();
    }

    private bool OnFilterArea(object o)
    {
        var area = o as Area;

        if (SearchText is null or "")
        {
            return true;
        }

        if (FilterSelectedItem == areaPropertyDict[nameof(Area.Id)])
        {
            return area!.Id.ToString().ToLower().Contains(SearchText.ToLower().Trim());
        }
        
        if (FilterSelectedItem == areaPropertyDict[nameof(Area.Name)])
        {
            return area!.Name!.ToLower().Contains(SearchText.ToLower().Trim());
        }
        
        if (FilterSelectedItem == areaPropertyDict[nameof(Area.PrecedentElementId)])
        {
            return area!.PrecedentElementId.ToString().ToLower().Contains(SearchText.ToLower().Trim());
        }
        
        if (FilterSelectedItem == areaPropertyDict[nameof(Area.LevelId)])
        {
            return area!.LevelId.ToString().ToLower().Contains(SearchText.ToLower().Trim());
        }
        
        if (FilterSelectedItem == areaPropertyDict[nameof(Area.LevelName)])
        {
            return area!.LevelName!.ToLower().Contains(SearchText.ToLower().Trim());
        }
        
        if (FilterSelectedItem == areaPropertyDict[nameof(Area.IsChangeable)])
        {
            return area!.IsChangeable.ToString().ToLower().Contains(SearchText.ToLower().Trim());
        }

        return false;
    }
}