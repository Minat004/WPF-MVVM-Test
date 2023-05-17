using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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

    partial void OnOrderSelectedItemChanged(string value)
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

    partial void OnFilterSelectedItemChanged(string value)
    {
        AreaView.Refresh();
    }

    public ObservableCollection<string> AreaPropertyList { get; set; }

    public ObservableCollection<ListSortDirection> Directions { get; set; }

    partial void OnSearchTextChanged(string? value)
    {
        AreaView.Refresh();
    }

    [ObservableProperty] 
    private ObservableCollectionView<Area> areaView = new(new List<Area>());

    partial void OnAreaViewChanged(ObservableCollectionView<Area> value)
    {
        AreaView.Refresh();
    }

    public async Task LoadAreasAsync()
    {
        AreaView = await GetAllAreasAsync();
        AreaView.Filter = OnFilterArea;
        OnOrderArea(AreaPropertyList[0], Directions[0]);
    }

    private static IEnumerable<Area> GetAreas()
    {
        using (var client = new HttpClient())
        {
            var response = client.GetAsync(@"https://api-dbw.stat.gov.pl/api/1.1.0/area/area-area?lang=pl");
            
            var responseBody = response.Result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<Area>>(responseBody.Result)!;
        }
    }
    
    private static async Task<ObservableCollectionView<Area>> GetAllAreasAsync()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(@"https://api-dbw.stat.gov.pl/api/1.1.0/area/area-area?lang=pl");
            var responseBody = await response.Content.ReadAsStringAsync();

            var list = JsonConvert.DeserializeObject<IEnumerable<Area>>(responseBody)!;

            return new ObservableCollectionView<Area>(list);
        }
    }

    private void OnOrderArea(string? property, ListSortDirection direction)
    {
        AreaView.SortDescriptions.Clear();
        AreaView.SortDescriptions.Add(new SortDescription(property!, direction));
        AreaView.Refresh();
    }

    private bool OnFilterArea(Area area)
    {
        if (SearchText is null or "")
        {
            return true;
        }

        if (FilterSelectedItem == areaPropertyDict[nameof(Area.Id)])
        {
            return area.Id.ToString().ToLower().Contains(SearchText.ToLower().Trim());
        }
        
        if (FilterSelectedItem == areaPropertyDict[nameof(Area.Name)])
        {
            return area.Name!.ToLower().Contains(SearchText.ToLower().Trim());
        }
        
        if (FilterSelectedItem == areaPropertyDict[nameof(Area.PrecedentElementId)])
        {
            return area.PrecedentElementId.ToString().ToLower().Contains(SearchText.ToLower().Trim());
        }
        
        if (FilterSelectedItem == areaPropertyDict[nameof(Area.LevelId)])
        {
            return area.LevelId.ToString().ToLower().Contains(SearchText.ToLower().Trim());
        }
        
        if (FilterSelectedItem == areaPropertyDict[nameof(Area.LevelName)])
        {
            return area.LevelName!.ToLower().Contains(SearchText.ToLower().Trim());
        }
        
        if (FilterSelectedItem == areaPropertyDict[nameof(Area.IsChangeable)])
        {
            return area.IsChangeable.ToString().ToLower().Contains(SearchText.ToLower().Trim());
        }

        return false;
    }
}