using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using habytee.Client.Services;
using habytee.Client.Core;
using habytee.Interconnection.Models;

namespace habytee.Client.ViewModels;

public class AddHabitViewModel : BaseViewModel
{
    private readonly NavigationManager navigationManager;
    
    private readonly ApiService apiService;
    private readonly MessageService messageService;
    private readonly IJSRuntime iJSRuntime;
    
    public AddHabitNameViewModel AddHabitNameViewModel { get; set; }
    public AddHabitReasonViewModel AddHabitReasonViewModel { get; set; }
    public AddHabitDaysViewModel AddHabitDaysViewModel { get; set; }
    public AddHabitAlarmViewModel AddHabitAlarmViewModel { get; set; }
    public AddHabitEarningsViewModel AddHabitEarningsViewModel { get; set; }
    public BaseViewModel SelectedAddHabitViewModel
    {
        get => addHabitViewModels[SelectedIndex];
        set
        {
            addHabitViewModels[SelectedIndex] = value;
        }
    }
    public RelayCommand Back { get; set; }
    public RelayCommand Next { get; set; }
    private ObservableCollection<BaseViewModel> addHabitViewModels = new ObservableCollection<BaseViewModel>();

    private int selectedIndex = 0;
    public int SelectedIndex
    {
        get => selectedIndex;
        set
        {
            selectedIndex = value;
            OnPropertyChanged();
        }
    }

    private bool allowSendToApi = true;
    public bool AllowSendToApi
    {
        get => allowSendToApi;
        set
        {
            allowSendToApi = value;
            OnPropertyChanged();
        }
    }

    private async Task<bool> RequestNotificationPermission()
    {
        var permission = await iJSRuntime.InvokeAsync<string>("requestNotificationPermission");
        if (permission == "granted")
        {
            return true;
        }

        return false;
    }

    public string NextButtonText => SelectedIndex == 4 ? "Finish" : "Next";

    public bool CurrentWeekAWeek { get; set; }

    public AddHabitViewModel(NavigationManager navigationManager, ApiService apiService, MessageService messageService, IJSRuntime iJSRuntime)
    {
        this.navigationManager = navigationManager;
        this.apiService = apiService;
        this.messageService = messageService;
        this.iJSRuntime = iJSRuntime;

        AddHabitNameViewModel = new AddHabitNameViewModel(this);
        AddHabitReasonViewModel = new AddHabitReasonViewModel(this);
        AddHabitDaysViewModel = new AddHabitDaysViewModel(this);
        AddHabitAlarmViewModel = new AddHabitAlarmViewModel(this);
        AddHabitEarningsViewModel = new AddHabitEarningsViewModel(this);

        addHabitViewModels.Add(AddHabitNameViewModel);
        addHabitViewModels.Add(AddHabitReasonViewModel);
        addHabitViewModels.Add(AddHabitDaysViewModel);
        addHabitViewModels.Add(AddHabitAlarmViewModel);
        addHabitViewModels.Add(AddHabitEarningsViewModel);

        SelectedIndex = 0;

        Back = new RelayCommand(
            execute: () =>
            {
                if (selectedIndex > 0)
                {
                    selectedIndex--;
                    OnPropertyChanged(nameof(SelectedIndex));
                }
            },
            canExecute: () => SelectedIndex <= 0
        );

        Next = new RelayCommand(
            execute: async () =>
            {
                SelectedAddHabitViewModel.OnFormSubmitRequested();
                if(!addHabitViewModels[SelectedIndex].IsValid)
                {
                    return;
                }

                if(!AllowSendToApi)
                {
                    return;
                }

                if(SelectedIndex == 3)
                {
                    if(AddHabitAlarmViewModel.SetAlarm)
                    {
                        bool permissions = await RequestNotificationPermission();
                        if(!permissions)
                        {
                            return;
                        }
                    }
                }

                if (SelectedIndex < 4)
                {
                    selectedIndex++;
                    OnPropertyChanged(nameof(SelectedIndex));
                }
                else
                {
                    AllowSendToApi = false;
                    
                    var success = await apiService.CreateHabitAsync(
                        new Habit{
                            Name = AddHabitNameViewModel.Name,
                            Reason = AddHabitReasonViewModel.Reason,
                            ABBoth = AddHabitDaysViewModel.ABActivated,
                            AWeekDays = AddHabitDaysViewModel.AWeek.ToList(),
                            BWeekDays = AddHabitDaysViewModel.BWeek.ToList(),
                            Alarm = AddHabitAlarmViewModel.AlarmTime,
                            Earnings = AddHabitEarningsViewModel.Earnings
                        }
                    );
                    if(success)
                    {
                        navigationManager?.NavigateTo("/");
                        AllowSendToApi = true;
                        messageService.SendMessage("Habit successfully added!");
                    }
                }
            },
            canExecute: () => SelectedIndex >= 5 && AllowSendToApi
        );

        Calendar cal = CultureInfo.CurrentCulture.Calendar;
        int weekOfYear =  cal.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        if(weekOfYear % 2 != 0)
        {
            CurrentWeekAWeek = true;
        }
        else
        {
            CurrentWeekAWeek = false;
        }
    }
}
